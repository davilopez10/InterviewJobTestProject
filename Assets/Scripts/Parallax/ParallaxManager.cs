using UnityEngine;

/// <summary>
/// Manage all background layers
/// </summary>
public class ParallaxManager : MonoBehaviour
{
    [SerializeField]
    private ParallaxElement[] _parallaxElements;

    public bool ParallaxActive { get; set; }

    private void Update()
    {
        if (ParallaxActive == true)
        {
            for (int i = 0; i < _parallaxElements.Length; i++)
            {
                _parallaxElements[i].UpdateParallax();
            }
        }
    }
}
