using UnityEngine;
using UnityEngine.InputSystem;

public enum CharacterRole
{
    Display,
    Main,
    Support,
}

public class CharacterSetup : MonoBehaviour
{
    public CharacterRole Role { get; private set; }

    public void SetRole(CharacterRole role)
    {
        Role = role;

        switch (Role)
        {
            case CharacterRole.Main:
            {
                foreach (var component in GetComponentsInChildren<MonoBehaviour>())
                {
                    if (component != this)
                    {
                        component.enabled = true;
                    }
                }
                break;
            }
            case CharacterRole.Support:
            {
                foreach (var component in GetComponents<MonoBehaviour>())
                {
                    if (component != this && component is ISkillComponent)
                    {
                        component.enabled = true;
                    }
                }
                break;
            }
            case CharacterRole.Display:
            {
                break;
            }
        }
    }
}