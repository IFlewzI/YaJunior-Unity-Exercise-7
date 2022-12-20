using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpForce;

    public bool IsFacingRight { get; private set; }

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private bool _isRunning;
    private bool _isFlying;
    private bool _isJumping;
    private bool _isAbleToDoubleJump;
    private bool _isFalling;

    private void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
        IsFacingRight = true;
    }

    private void Update()
    {
        if (gameObject.GetComponent<PlayerCollision>().IsPlayerAbleToMove)
        {
            if (IsFacingRight)
                _spriteRenderer.flipX = false;
            else
                _spriteRenderer.flipX = true;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += (Vector3.right * _runSpeed * Time.deltaTime);
                IsFacingRight = true;
                _isRunning = true;
                _animator.SetTrigger("Run");
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += (Vector3.left * _runSpeed * Time.deltaTime);
                IsFacingRight = false;
                _isRunning = true;
                _animator.SetTrigger("Run");
            }
            else
            {
                _isRunning = false;
                _animator.ResetTrigger("Run");
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                Jump();

            if (_rigidbody2D.velocity.y != 0)
            {
                _isFlying = true;

                if (_rigidbody2D.velocity.y > 0)
                {
                    _isJumping = true;
                    _animator.SetTrigger("Jump");
                    _isFalling = false;
                    _animator.ResetTrigger("Fall");
                }
                else if (_rigidbody2D.velocity.y < 0)
                {
                    _isJumping = false;
                    _animator.ResetTrigger("Jump");
                    _isFalling = true;
                    _animator.SetTrigger("Fall");
                }
            }
            else
            {
                _isFlying = false;
                _isFalling = false;
                _isJumping = false;
            }

            if (!_isRunning && !_isFlying)
                _animator.SetTrigger("Idle");
            else
                _animator.ResetTrigger("Idle");
        }
        else
        {
            _animator.ResetTrigger("Idle");
            _animator.ResetTrigger("Fall");
            _animator.ResetTrigger("Jump");
            _animator.ResetTrigger("Run");
        }
    }

    private void Jump()
    {
        if (_isFlying == false || _isAbleToDoubleJump)
        {
            if (_isFlying)
                _isAbleToDoubleJump = false;
            else
                _isAbleToDoubleJump = true;

            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            _rigidbody2D.AddForce(Vector3.up * _jumpForce);
        }
    }
}
