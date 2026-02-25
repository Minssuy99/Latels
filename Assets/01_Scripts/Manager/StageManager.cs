using System;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.InputSystem;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private ClearDirector clearDirector;

    public event Action OnStageClear;

    public PlayerInput PlayerInput => playerInput;
    private PlayerInput playerInput;
    private GameObject playerObj;

    private GameObject map;
    private ClearPlace clearPlace;
    private Transform playerSpawn;
    private Area[] areas;

    private StageData stageData;
    private int clearAreaCount;
    private int totalAreaCount;

    protected override void Awake()
    {
        base.Awake();
        LoadStage();
    }

    private void OnAreaCleared()
    {
        clearAreaCount++;
        if (clearAreaCount >= totalAreaCount)
        {
            OnStageClear?.Invoke();
        }
    }

    private void LoadStage()
    {
        stageData = GameManager.Instance.stageData;
        SpawnMap();
        SpawnBattleCharacters();
        SpawnDisplayCharacters();
        SpawnEnemies();
        InitAreas(areas);
    }

    private void SpawnMap()
    {
        map = Instantiate(stageData.mapPrefab);
        map.GetComponent<NavMeshSurface>().BuildNavMesh();
        clearPlace = map.GetComponentInChildren<ClearPlace>();
        clearDirector.SetCameraPoint(clearPlace.cameraPoint, clearPlace.CameraEndPoint);
        Camera.main.fieldOfView = 60f;
        playerSpawn = map.transform.Find("PlayerSpawn");
    }

    private void SpawnBattleCharacters()
    {
        CharacterData[] slots = GameManager.Instance.characterSlots;
        Transform battleRoot = new GameObject("캐릭터").transform;

        playerObj = Instantiate(slots[0].prefab, playerSpawn.position, playerSpawn.rotation);
        playerObj.GetComponent<CharacterSetup>().SetRole(CharacterRole.Main, slots[0]);
        InGameUIManager.Instance.SetPlayer(playerObj.GetComponent<PlayerStateManager>());
        TimeManager.Instance.SetAnimator(playerObj.GetComponent<Animator>());
        Camera.main.GetComponent<FollowCamera>().SetPlayer(playerObj.transform);
        playerInput = playerObj.GetComponent<PlayerInput>();
        playerObj.transform.SetParent(battleRoot, true);
        playerObj.name = $"메인: {slots[0].charName}";
        playerObj.tag = "Player";

        for (int i = 1; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            GameObject supportObj = Instantiate(slots[i].prefab, battleRoot, true);
            supportObj.name = $"지원: {slots[i].charName}";
            supportObj.GetComponent<CharacterSetup>().SetRole(CharacterRole.Support, slots[i]);
            playerObj.GetComponent<SupportSkill>().SetSupport(i - 1, supportObj);
            supportObj.SetActive(false);
        }
    }

    private void SpawnDisplayCharacters()
    {
        CharacterData[] slots = GameManager.Instance.characterSlots;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            Instantiate(slots[i].prefab, clearPlace.characterPosition[i]).GetComponent<CharacterSetup>().SetRole(CharacterRole.Display, slots[i]);
        }
    }

    private void SpawnEnemies()
    {
        areas = map.GetComponentsInChildren<Area>();
        Transform enemyRoot = new GameObject("몬스터").transform;

        foreach (Area area in areas)
        {
            foreach (SpawnPoint point in area.spawnPoints)
            {
                GameObject enemyObj = Instantiate(point.Data.prefab, point.transform.position, point.transform.rotation);
                enemyObj.transform.SetParent(enemyRoot, true);
                EnemyStateManager enemy = enemyObj.GetComponent<EnemyStateManager>();
                enemy.SetData(point.Data);
                enemy.gameObject.name = point.Data.EnemyName;
                area.AddEnemy(enemy);
                if (point.IsBoss)
                {
                    enemy.gameObject.name = $"보스: {point.Data.EnemyName}";
                    area.SetBoss(enemy);
                }

                enemy.SetPlayer(playerObj);
            }
        }
    }

    private void InitAreas(Area[] areas)
    {
        totalAreaCount = areas.Length;

        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].OnCleared += OnAreaCleared;
        }
    }
}