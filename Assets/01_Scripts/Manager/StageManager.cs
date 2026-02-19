using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private StageClear stageClear;
    public event Action OnStageClear;
    public PlayerInput PlayerInput => playerInput;
    private PlayerInput playerInput;
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

    private void Init(Area[] areas)
    {
        totalAreaCount = areas.Length;

        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].OnCleared += OnAreaCleared;
        }
    }

    private void LoadStage()
    {
        StageData stageData = GameManager.Instance.stageData;
        GameObject map = Instantiate(stageData.mapPrefab);

        ClearPlace clearPlace = map.GetComponentInChildren<ClearPlace>();
        Instantiate(GameManager.Instance.mainCharData.displayPrefab, clearPlace.mainCharPos.position, clearPlace.mainCharPos.rotation);
        Instantiate(GameManager.Instance.supportChar1_Data.displayPrefab, clearPlace.subChar1Pos.position, clearPlace.subChar1Pos.rotation);

        stageClear.SetCameraPoint(clearPlace.cameraPoint, clearPlace.CameraEndPoint);

        Transform playerSpawn = map.transform.Find("PlayerSpawn");
        GameObject playerObj = Instantiate(GameManager.Instance.mainCharData.battlePrefab, playerSpawn.position, Quaternion.identity);
        InGameUIManager.Instance.SetPlayer(playerObj);
        playerInput = playerObj.GetComponent<PlayerInput>();

        GameObject supportObj = Instantiate(GameManager.Instance.supportChar1_Data.battlePrefab);
        supportObj.SetActive(false);
        playerObj.GetComponent<SupportSkillManager>().SetSupport(supportObj);

        Area[] areas = map.GetComponentsInChildren<Area>();

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
        Init(areas);
    }
}
