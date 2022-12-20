using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float _hitRecoilDistance;
    [SerializeField] private float _hitRecoilDuration;

    public bool IsPlayerAbleToMove { get; private set; }

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Coroutine _applyRecoilToPlayerInJob;

    private void Start()
    {
        IsPlayerAbleToMove = true;
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
    }

    public void GetDamage()
    {
        _animator.SetTrigger("Hit");
        Debug.Log("Player Hitted");

        if (_applyRecoilToPlayerInJob == null)
            _applyRecoilToPlayerInJob = StartCoroutine(ApplyRecoilToPlayer());
    }

    private IEnumerator ApplyRecoilToPlayer()
    {
        Debug.Log("Coroutine Started");

        Vector3 recoilDirection;
        float timeLeft = 0;
        Vector3 targetPosition;
        float maxStep;
        float gravityScaleBeforeHit = _rigidbody2D.gravityScale;

        if (gameObject.GetComponent<PlayerMovement>().IsFacingRight)
            recoilDirection = Vector3.left;
        else
            recoilDirection = Vector3.right;

        targetPosition = (transform.position + (recoilDirection * _hitRecoilDistance));
        maxStep = _hitRecoilDistance / _hitRecoilDuration;
        IsPlayerAbleToMove = false;
        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.velocity = Vector2.zero;

        while (timeLeft < _hitRecoilDuration)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxStep * Time.deltaTime);
            timeLeft += Time.deltaTime;

            Debug.Log("1 cicle left");

            yield return null;
        }

        _rigidbody2D.gravityScale = gravityScaleBeforeHit;
        IsPlayerAbleToMove = true;
        _applyRecoilToPlayerInJob = null;
        Debug.Log("Coroutine Ended");
    }
}
