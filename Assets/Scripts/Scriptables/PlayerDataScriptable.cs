using System;
using UnityEngine;

/// <summary>
/// Save common data of player. 
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerDataScriptable : ScriptableObject
{
    private int _points = 0;

    /// <summary>
    /// Called when get a point
    /// </summary>
    public event Action<int> onGetPoint;

    /// <summary>
    /// Player points
    /// </summary>
    public int Points
    {
        get
        {
            return _points;
        }
        set
        {
            _points = value;
            onGetPoint?.Invoke(_points);
        }
    }

    /// <summary>
    /// Game time
    /// </summary>
    public float Time { get; set; }

    public void Reset()
    {
        Time = 0;
        _points = 0;
    }
}
