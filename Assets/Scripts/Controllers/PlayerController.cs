using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Main Character 
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public const string POINTS_DETECTION_TAG = "PointsDetector";

    public event Action _onJumpAction;

    public event Action _onDieAction;

    [SerializeField]
    private LayerMask _detectionLayer;

    [SerializeField]
    private float _detectionRadius = 0.5f;

    [SerializeField]
    private float _jumpHeight = 0.5f;

    [SerializeField]
    private PlayerDataScriptable _playerData;

    [SerializeField]
    private UnityEvent _onJump;

    [SerializeField]
    private UnityEvent _onGetPoint;

    [SerializeField]
    private UnityEvent _onDie;

    private Rigidbody2D _rigid2D;

    private RaycastHit2D[] _rigid2Dinfo = new RaycastHit2D[2];

    private Vector2 _velocity;

    private bool _isJump = false;

    private float _jumpSpeed;

    private bool _isPlayerActive;

    private Collider2D _lastPointCollider;

    /// <summary>
    /// Set Player controller active/disable
    /// </summary>
    public bool IsPlayerActive
    {
        get
        {
            return _isPlayerActive;
        }

        set
        {
            _isPlayerActive = value;
            enabled = _isPlayerActive;
            _rigid2D.simulated = _isPlayerActive;
        }
    }

    /// <summary>
    /// Jump Event for Input System
    /// </summary>
    /// <param name="callbackContext"></param>
    public void ReceiveJumpEvent(InputAction.CallbackContext callbackContext)
    {
        if (enabled == true && callbackContext.performed == true)
        {
#if UNITY_EDITOR
            Debug.Log("Jump");
#endif
            _isJump = true;
        }
    }

    private void Jump()
    {
        _velocity = _rigid2D.velocity;
        _velocity.y = _jumpSpeed;
        _rigid2D.velocity = _velocity;

        _onJump?.Invoke();
        _onJumpAction?.Invoke();

        _isJump = false;
    }

    private void DoRotation()
    {
        transform.right = _rigid2D.velocity + Vector2.right;
    }

    private void CheckCollisions()
    {
        if (Physics2D.CircleCastNonAlloc(transform.localPosition, _detectionRadius, transform.forward,
            _rigid2Dinfo, Mathf.Infinity, _detectionLayer) > 0)
        {
            for (int i = 0; i < _rigid2Dinfo.Length; i++)
            {
                if (_rigid2Dinfo[i].collider != null)
                {
                    if (_rigid2Dinfo[i].collider.CompareTag(POINTS_DETECTION_TAG))
                    {
                        if (_lastPointCollider != _rigid2Dinfo[i].collider)
                        {
                            _lastPointCollider = _rigid2Dinfo[i].collider;

                            if (_playerData != null)
                            {
                                _playerData.Points++;
                                _onGetPoint?.Invoke();
                            }
                        }
                    }
                    else
                    {
                        PlayerDie();
                    }
                }
            }
        }
    }

    private void PlayerDie()
    {
#if UNITY_EDITOR
        Debug.Log("You die");
#endif
        IsPlayerActive = false;

        _onDieAction?.Invoke();
        _onDie?.Invoke();
    }


    private void Awake()
    {
        if (TryGetComponent(out _rigid2D) == false)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"You should add to Rigidbody2D component");
#endif
            enabled = false;
        }

        IsPlayerActive = false;
    }

    private void Start()
    {
        //we obtain the necessary speed to overcome gravity until the jump
        _jumpSpeed = Mathf.Sqrt(-2f * _rigid2D.gravityScale * Physics2D.gravity.y * _jumpHeight);
    }

    private void FixedUpdate()
    {
        CheckCollisions();

        if (_isJump == true)
        {
            Jump();
        }

        DoRotation();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.localPosition, _detectionRadius);
    }
}
