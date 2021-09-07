using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ObstacleMove : MonoBehaviour
{
    [Tooltip("Speed of background movement")]
    [SerializeField]
    private float _speed = 0.0f;

    private float _newPosX = 0.0f;

    private void Awake()
    {
        RunnerDistance.GameWin += OnGameWin;
    }
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private void Start()
    {
        _newPosX = transform.position.x;
    }

    private void Update()
    {
        Move();
    }

    private void OnGameWin()
    {
        Destroy(gameObject);
    }

    private void Move()
    {
        _newPosX += _speed * Time.deltaTime;

        transform.position = new Vector3(_newPosX, transform.position.y, 0);
    }

    private void OnDestroy()
    {
        RunnerDistance.GameWin -= OnGameWin;
    }
}
