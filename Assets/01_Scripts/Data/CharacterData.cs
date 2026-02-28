using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "SO/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string charName;

    public GameObject battlePrefab;
    public GameObject displayPrefab;
    public CharacterStats stats;
    public CharacterSprites sprites;
}

[System.Serializable]
public class CharacterStats
{
    public float health;
    public float attackRange;
    public float damage;
    public float skillDamage;
    public float skillCoolTime;
}

[System.Serializable]
public class CharacterSprites
{
    public Sprite fullImage;
    public Sprite profileImage;
    public Sprite skillIcon;
    public Sprite UltimateIcon;
}