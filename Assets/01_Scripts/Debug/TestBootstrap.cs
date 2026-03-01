using UnityEngine;

public class TestBootstrap : MonoBehaviour
{
    [Header("[Player Settings]")]
    [SerializeField] private CharacterData playerData;
    [SerializeField] CharacterStats playerStatsOverride;

    [Space(10)]
    [Header("[Enemy Settings]")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] EnemyStats enemyStatsOverride;
    [SerializeField] private Transform[] spawnPoints;

    [Space(10)]
    [Header("Runtime Test")]
    public CharacterData runtimePlayerData;
    public EnemyData runtimeEnemyData;

    private GameObject playerObj;

    private void Start()
    {
        playerObj = Instantiate(playerData.battlePrefab);
        CharacterData playerDataCopy = Instantiate(playerData);
        playerDataCopy.stats = playerStatsOverride;
        runtimePlayerData = playerDataCopy;

        playerObj.GetComponent<CharacterSetup>().SetRole(CharacterRole.Main, playerDataCopy);
        playerObj.tag = "Player";
        playerObj.name = "Player";

        TimeManager.Instance.SetAnimator(playerObj.GetComponent<Animator>());
        InGameUIManager.Instance.SetPlayer(playerObj.GetComponent<PlayerStateManager>());
        Camera.main.GetComponent<FollowCamera>().SetPlayer(playerObj.transform);

        EnemyData enemyDataCopy = Instantiate(enemyData);
        enemyDataCopy.stats = enemyStatsOverride;
        runtimeEnemyData = enemyDataCopy;

        foreach (Transform point in spawnPoints)
        {
            GameObject enemyObj = Instantiate(enemyData.prefab, point.position, point.rotation);
            EnemyStateManager enemy = enemyObj.GetComponent<EnemyStateManager>();
            enemy.name = "Enemy";
            enemy.SetData(enemyDataCopy);
            enemy.Activate();
        }
    }
}