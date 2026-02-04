using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Vector3 cameraOffset;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");

        if (_player == null)
        {
            Debug.Log("플레이어의 태그를 가진 오브젝트가 없습니다.");
        }
    }

    private void Update()
    {
        if (!_player)
            return;

        transform.position = _player.transform.position + cameraOffset;
    }
}