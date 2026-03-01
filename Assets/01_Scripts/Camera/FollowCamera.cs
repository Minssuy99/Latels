using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Vector3 cameraOffset;
    private Transform player;

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }

    private void LateUpdate()
    {
        if (!player) return;

        transform.position = player.position + cameraOffset;
    }
}