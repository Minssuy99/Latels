using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "SO/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string charName;
    public Sprite fullImage;
    public Sprite profileImage;
    public GameObject battlePrefab;
    public GameObject displayPrefab;
    public CharacterStats stats;
}

[System.Serializable]
public class CharacterStats
{
    public float health;
    public float damage;
    public float skillDamage;

    public float lockOnRange;
    public float attackRange;

    public float skillCoolTime;
}