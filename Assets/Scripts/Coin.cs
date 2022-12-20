using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class Coin : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    private AudioSource _audioSource;
    private bool _isCoinCollected;
    private Vector4 _colorAfterCollecting;

    private void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _collider2D = gameObject.GetComponent<Collider2D>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        _colorAfterCollecting = new Vector4(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isCoinCollected)
        {
            _audioSource.Play();
            _isCoinCollected = true;
            _spriteRenderer.color = _colorAfterCollecting;
            _collider2D.enabled = false;
        }
    }
}
