using UnityEngine;

/// <summary>
/// Player dead zone 
/// </summary>
public class Obstacle : MonoBehaviour
{
    [SerializeField, Range(0, 2.5f), Tooltip("Max Y random value")]
    private float _maxY = 2.5f;

    [SerializeField, Range(0, -2.5f), Tooltip("Min Y random value")]
    private float _minY = -2.5f;

    [SerializeField]
    private bool _randomOnEnable = true;

    private Vector3 _defaultPos;

    public void RandomPosition()
    {
        transform.localPosition = _defaultPos + Vector3.up * Random.Range(_minY, _maxY);
    }

    private void OnEnable()
    {
        _defaultPos = transform.localPosition;
        if (_randomOnEnable == true)
        {
            RandomPosition();
        }
    }

}
