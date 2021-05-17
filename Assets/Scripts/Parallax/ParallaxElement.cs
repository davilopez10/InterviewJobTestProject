using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Background parallax effect. Add to background layer
/// </summary>
public class ParallaxElement : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _elements = new List<Transform>();

    [SerializeField]
    private float _speed = 1f;

    [SerializeField]
    private float _size = 18f;

    public void UpdateParallax()
    {
        for (int i = 0; i < _elements.Count; i++)
        {
            _elements[i].localPosition -= Vector3.right * _speed * Time.deltaTime;

            if (_elements[i].localPosition.x < -_size)
            {
                Transform outOfRangeElement = _elements[i];
                _elements.RemoveAt(i);
                outOfRangeElement.localPosition = Vector3.right * _size * 2;
                _elements.Add(outOfRangeElement);
                i--;
            }
        }
    }

}
