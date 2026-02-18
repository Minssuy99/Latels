using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "SO/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public GameObject battlePrefab;
    public GameObject displayPrefab;
}