using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerRadar))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpForce;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private bool _isAbleToMove;
    private States _state;
    private bool _isGrounded;
    private bool _isAbleToDoubleJump;

    public bool IsFacingRight { get; private set; }

    private enum States
    {
        Idle,
        Jump,
        Fall,
        Run,
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        IsFacingRight = true;
    }

    private void Update()
    {
        _isAbleToMove = GetComponent<PlayerRadar>().IsAbleToMove;
        _isGrounded = CheckGrounding();

        if (_isAbleToMove)
        {
            _spriteRenderer.flipX = !IsFacingRight;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                Run();
            else
                _state = States.Idle;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (CheckJumpingAbility())
                    DoJump();
            }

            if (!_isGrounded)
                SwitchFlyingState();

            CheckForIdle();
            UpdateAnimatorState();
        }
        else
        {
            ResetAllTriggers();
        }
    }

    private void ResetAllTriggers()
    {
        _animator.ResetTrigger(States.Idle.ToString());
        _animator.ResetTrigger(States.Fall.ToString());
        _animator.ResetTrigger(States.Jump.ToString());
        _animator.ResetTrigger(States.Run.ToString());
    }

    private void Run()
    {
        _state = States.Run;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * _runSpeed * Time.deltaTime, Space.Self);
            IsFacingRight = true;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * _runSpeed * Time.deltaTime, Space.Self);
            IsFacingRight = false;
        }
    }

    private void SwitchFlyingState()
    {
        if (_rigidbody2D.velocity.y > 0)
            _state = States.Jump;
        else if (_rigidbody2D.velocity.y < 0)
            _state = States.Fall;
    }

    private bool CheckGrounding()
    {
        bool isGrounded = _rigidbody2D.velocity.y == 0;

        return isGrounded;
    }

    private bool CheckForIdle()
    {
        bool isIdle = _isGrounded && _state != States.Run;

        if (isIdle)
            _state = States.Idle;
        
        return isIdle;
    }

    private void DoJump()
    {
        _isAbleToDoubleJump = _isGrounded;
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
        _rigidbody2D.AddForce(Vector2.up * _jumpForce);
    }

    private bool CheckJumpingAbility()
    {
        return _isGrounded || _isAbleToDoubleJump;
    }

    private void UpdateAnimatorState()
    {
        ResetAllTriggers();
        _animator.SetTrigger(_state.ToString());
    }
}
