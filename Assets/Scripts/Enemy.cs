using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private UnityEvent _hit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _hit.Invoke();
    }
}
