using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BackgroundMove : MonoBehaviour
{
    private enum Direction{ Left, Right };

    [Tooltip("Direction of player run")]
    [SerializeField]
    private Direction _runDirection = Direction.Left;

    [Tooltip("Speed of background movement")]
    [SerializeField]
    private float _gameSpeed = 0.0f;

    [Tooltip("Background holder object")]
    [SerializeField]
    private GameObject _backgroundsHolder = null;

    [Tooltip("List of background screens")]
    [SerializeField]
    private List<RectTransform> _backgrounds = new List<RectTransform>();

    private Transform _bgTransform = null;
    private float _bgSize = 0.0f;
    private float _bgPosX = 0.0f;
    private float endPosX = 0.0f;

    void Start()
    {
        _bgTransform = _backgroundsHolder.transform;
        _bgSize = _backgrounds[0].rect.width;

        _gameSpeed = _runDirection == Direction.Right ? -_gameSpeed : _gameSpeed;

        for (int i = 0; i < _backgrounds.Count; i++)
        {
            var bg = Instantiate(_backgrounds[i].gameObject, _bgTransform);

            _bgSize = bg.GetComponent<RectTransform>().rect.width;

            bg.transform.localPosition = new Vector3(_bgSize * i, bg.transform.localPosition.y, 0);
        }

        _bgPosX = _bgTransform.position.x;

        endPosX = -_bgSize * (_backgrounds.Count - 2);
    }

    void Update()
    {
        if (RunnerGameState.IsGameRunning)
        {
            MoveBackground();
        }
    }

    private void MoveBackground()
    {
        _bgPosX += _gameSpeed * Time.deltaTime;

        if (_bgPosX < endPosX)
        {
            _bgPosX = _bgSize;
        }

        _bgTransform.position = new Vector3(_bgPosX, _bgTransform.position.y, 0);
    }
}
