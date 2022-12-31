using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerRadar : MonoBehaviour
{
    private const string HitTriggerName = "Hit";

    [SerializeField] private float _hitRecoilDistance;
    [SerializeField] private float _hitRecoilDuration;

    private PlayerMovement _playerMovement;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private AudioSource _audioSource;
    private Coroutine _applyRecoilToPlayerInJob;

    public bool IsAbleToMove { get; private set; }

    private void Start()
    {
        IsAbleToMove = true;
        _playerMovement = GetComponent<PlayerMovement>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void GetDamage()
    {
        _animator.SetTrigger(HitTriggerName);
        _audioSource.Play();

        if (_applyRecoilToPlayerInJob == null)
            _applyRecoilToPlayerInJob = StartCoroutine(ApplyRecoilToPlayer());
    }

    private IEnumerator ApplyRecoilToPlayer()
    {
        Vector3 recoilDirection;
        float timeLeft = 0;
        Vector3 targetPosition;
        float maxStep;
        float gravityScaleBeforeHit = _rigidbody2D.gravityScale;

        if (_playerMovement.IsFacingRight)
            recoilDirection = Vector3.left;
        else
            recoilDirection = Vector3.right;

        targetPosition = (transform.position + (recoilDirection * _hitRecoilDistance));
        maxStep = _hitRecoilDistance / _hitRecoilDuration;
        IsAbleToMove = false;
        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.velocity = Vector2.zero;

        while (timeLeft < _hitRecoilDuration)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxStep * Time.deltaTime);
            timeLeft += Time.deltaTime;

            yield return null;
        }

        _rigidbody2D.gravityScale = gravityScaleBeforeHit;
        IsAbleToMove = true;
        _applyRecoilToPlayerInJob = null;
    }
}
