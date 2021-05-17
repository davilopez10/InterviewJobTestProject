using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// UI for Points
/// </summary>
public class PointsCanvas : UICanvas
{
    [SerializeField]
    private PlayerDataScriptable _playerData;

    [SerializeField]
    private TextMeshProUGUI _pointsText;

    private void RefreshPoints(int points)
    {
        _pointsText.text = points.ToString();
    }

    public override void ActiveCanvas()
    {
        base.ActiveCanvas();
        RefreshPoints(_playerData.Points);
    }

    private void OnEnable()
    {
        Assert.IsNotNull(_playerData);

        _playerData.onGetPoint += RefreshPoints;
    }

    private void OnDisable()
    {
        Assert.IsNotNull(_playerData);

        _playerData.onGetPoint -= RefreshPoints;
    }
}
