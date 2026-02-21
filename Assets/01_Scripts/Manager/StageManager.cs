using System;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.InputSystem;

public class StageManager : Singleton<StageManager>
{
    // 인스펙터
    [SerializeField] private StageClear stageClear;

    // 이벤트
    public event Action OnStageClear;

    // 플레이어
    public PlayerInput PlayerInput => playerInput;
    private PlayerInput playerInput;
    private GameObject playerObj;

    // 맵
    private GameObject map;
    private ClearPlace clearPlace;
    private Transform playerSpawn;
    private Area[] areas;

    // 스테이지
    private StageData stageData;
    private int clearAreaCount = 0;
    private int totalAreaCount = 0;

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
            Debug.Log("스테이지 클리어!");
            OnStageClear?.Invoke();
        }
    }

    private void LoadStage()
    {
        stageData = GameManager.Instance.stageData;
        SpawnMap();
        SpawnCharacters();
        SpawnEnemies();
        InitAreas(areas);
    }

    private void SpawnMap()
    {
        map = Instantiate(stageData.mapPrefab);
        map.GetComponent<NavMeshSurface>().BuildNavMesh();
        clearPlace = map.GetComponentInChildren<ClearPlace>();
        stageClear.SetCameraPoint(clearPlace.cameraPoint, clearPlace.CameraEndPoint);
        playerSpawn = map.transform.Find("PlayerSpawn");
    }

    private void SpawnCharacters()
    {
        CharacterData[] slots = GameManager.Instance.characterSlots;

        playerObj = Instantiate(slots[0].Prefab, playerSpawn.position, playerSpawn.rotation);
        playerObj.GetComponent<CharacterSetup>().SetRole(CharacterRole.Main, slots[0]);
        playerObj.name = $"메인 : {slots[0].charName}";
        playerObj.tag = "Player";
        Instantiate(slots[0].Prefab, clearPlace.characterPosition[0]).GetComponent<CharacterSetup>().SetRole(CharacterRole.Display, slots[0]);

        InGameUIManager.Instance.SetPlayer(playerObj.GetComponent<PlayerStateManager>());
        playerInput = playerObj.GetComponent<PlayerInput>();

        for (int i = 1; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            Instantiate(slots[i].Prefab, clearPlace.characterPosition[i]).GetComponent<CharacterSetup>().SetRole(CharacterRole.Display, slots[i]);
            GameObject supportObj = Instantiate(slots[i].Prefab);
            supportObj.name = $"지원 : {slots[i].charName}";
            supportObj.GetComponent<CharacterSetup>().SetRole(CharacterRole.Support, slots[i]);
            playerObj.GetComponent<SupportSkill>().SetSupport(i - 1, supportObj);
            supportObj.SetActive(false);
        }
    }

    private void SpawnEnemies()
    {
        areas = map.GetComponentsInChildren<Area>();

        for (int i = 0; i < areas.Length; i++)
        {
            GameObject enemy;
            AreaSpawnData spawnData = stageData.areas[i];

            for (int j = 0; j < areas[i].spawnPoints.Length; j++)
            {
                enemy = Instantiate(spawnData.enemyPrefab, areas[i].spawnPoints[j].position, Quaternion.identity);
                EnemyStateManager enemyStateManager = enemy.GetComponent<EnemyStateManager>();
                areas[i].AddEnemy(enemyStateManager);
            }

            if (spawnData.bossPrefab != null)
            {
                enemy = Instantiate(spawnData.bossPrefab, areas[i].bossSpawnPoint.position, Quaternion.identity);

                EnemyStateManager boss = enemy.GetComponent<EnemyStateManager>();
                areas[i].AddEnemy(boss);
                areas[i].SetBoss(enemy.GetComponent<EnemyAttack>());
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