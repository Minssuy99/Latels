using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private CharacterController playerCC;
    private PlayerMovement playerMovement;
    private Vector3 previousPosition;

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameTags.Player))
        {
            playerCC = other.GetComponent<CharacterController>();
            playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.SetOnPlatform(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameTags.Player))
        {
            playerMovement.SetOnPlatform(false);
            playerCC = null;
            playerMovement = null;
        }
    }

    private void LateUpdate()
    {
        if (playerCC)
        {
            Vector3 delta = transform.position - previousPosition;
            playerCC.Move(delta);
        }

        previousPosition = transform.position;
    }
}
