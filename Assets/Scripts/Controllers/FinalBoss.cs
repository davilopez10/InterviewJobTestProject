using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Specific behaviour for Final Boss
/// </summary>
public class FinalBoss : MonoBehaviour
{
    //To optimize animator calls
    private const string SHOOT_ANIMATOR = "Shot";
    private const string DIE_ANIMATOR = "Die";
    private readonly int SHOOT_ANIMATOR_HASH = Animator.StringToHash(SHOOT_ANIMATOR);
    private readonly int DIE_ANIMATOR_HASH = Animator.StringToHash(DIE_ANIMATOR);

    [SerializeField]
    private float _initWaitTime = 3f;

    [SerializeField]
    private float _finishWaitTime = 2f;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Animator _laserAnimator;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Transform _laserTransform;

    [SerializeField]
    private float _reloadTime = 3f;

    [SerializeField]
    private float _reloadDecrementOnShot = 0.1f;

    [SerializeField]
    private int _shootsToDie = 9;

    [SerializeField, Header("Special Attack")]
    private int _shootsToSpecialAttack = 3;

    [SerializeField]
    private float _timeFollowSpecialAttack = 0.7f;

    [SerializeField]
    private float _speedFollowSpecialAttack = 0.2f;

    [SerializeField]
    private UnityEvent onShoot;

    [SerializeField]
    private UnityEvent onDie;

    private Coroutine _currentCoroutine;

    private int _shoots;

    /// <summary>
    /// Call when player die
    /// </summary>
    public void StopBehaviour()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            enabled = false;
        }
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(_initWaitTime);

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        _currentCoroutine = StartCoroutine(Shoot());
    }

    private IEnumerator Die()
    {
        _animator.SetTrigger(DIE_ANIMATOR_HASH);

        yield return new WaitForSeconds(_finishWaitTime);

        onDie?.Invoke();
    }

    private IEnumerator Shoot()
    {
        _laserTransform.right = -(_target.position - _laserTransform.position);

        _animator.SetTrigger(SHOOT_ANIMATOR_HASH);

        _laserAnimator.SetTrigger(SHOOT_ANIMATOR_HASH);

        onShoot?.Invoke();

        if (_shoots > 0 && _shoots % _shootsToSpecialAttack == 0)
        {
            float t = 0;

            while (t < _timeFollowSpecialAttack)
            {
                _laserTransform.right = Vector3.Lerp(_laserTransform.right, -(_target.position - _laserTransform.position),
                    Time.deltaTime * _speedFollowSpecialAttack);
                t += Time.deltaTime;
                yield return null;
            }
        }

        _shoots++;

        if (_shoots > _shootsToDie)
        {
            yield return new WaitForSeconds(_finishWaitTime);
            _currentCoroutine = StartCoroutine(Die());
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(_reloadTime);
            _currentCoroutine = StartCoroutine(Shoot());
        }

        _reloadTime -= _reloadDecrementOnShot;
    }

    private void Start()
    {
        _currentCoroutine = StartCoroutine(Initialize());
    }

}
