using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] private Transform _path;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isPatrol;

    private Transform[] _points;
    private int _currentPoint;

    private void Start()
    {
        _points = new Transform[_path.transform.childCount];

        for (int i = 0; i < _points.Length; i++)
            _points[i] = _path.GetChild(i);
    }

    private void Update()
    {
        Transform targetPoint = _points[_currentPoint];

        if (_isPatrol)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, _speed * Time.deltaTime);

            if (transform.position == targetPoint.position)
            {
                _currentPoint++;

                if (_currentPoint >= _points.Length)
                    _currentPoint = 0;
            }
        }
    }
}
