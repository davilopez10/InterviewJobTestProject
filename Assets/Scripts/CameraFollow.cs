using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  @INFO: Finally camera do not follow player (the player is always in same position). 
///  The script is disabled for not call to LateUpdate
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private bool _ignoreY;

    [SerializeField]
    private float _followSpeed = 3f;

    [SerializeField]
    private Vector2 _shakeEffectMagnitude = Vector2.one;

    public Transform Target { get => _target; }

    private Vector3 _cachePos = Vector3.zero;

    private Coroutine _shakeEffectCoroutine;

    /// <summary>
    /// @INFO: Added at the last moment. Generate camera shaked effects
    /// </summary>
    /// <param name="duration"></param>
    public void Shake(float duration)
    {
        if (_shakeEffectCoroutine != null)
        {
            StopCoroutine(_shakeEffectCoroutine);
        }

        _shakeEffectCoroutine = StartCoroutine(ShakeEffect(duration));
    }

    private IEnumerator ShakeEffect(float duration)
    {
        Vector3 defaultPos = transform.localPosition;
        Vector3 shakeEffect = default;

        float t = 0;
        while (t < duration)
        {
            shakeEffect.Set(Random.Range(-_shakeEffectMagnitude.x, _shakeEffectMagnitude.x),
                Random.Range(-_shakeEffectMagnitude.y, _shakeEffectMagnitude.y), 0);

            transform.localPosition += shakeEffect * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }

        //Return to default position doing lerp
        t = 0;
        while (t < 1)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPos, t);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = defaultPos;
    }

    private void Start()
    {
        _cachePos = _target.position + _offset;
        _cachePos.z = transform.position.z;
        transform.position = _cachePos;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            _cachePos = transform.position;
            _cachePos.x = Mathf.MoveTowards(_cachePos.x, _target.position.x + _offset.x, _followSpeed * Time.deltaTime);

            if (_ignoreY == false)
            {
                _cachePos.y = Mathf.MoveTowards(_cachePos.y, _target.position.y + _offset.y, _followSpeed * Time.deltaTime);
            }

            transform.position = _cachePos;
        }
    }
}
