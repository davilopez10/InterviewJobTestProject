using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// @INFO Finally i do not use the static instance but i did not want remove it because would be usefull
    /// </summary>
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private PlayerDataScriptable _playerData;

    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private ModulesManager _modulesManager;

    [SerializeField]
    private CanvasManager _canvasManager;

    [SerializeField]
    private float _pointsToWin = 10;

    [SerializeField, Header("Events")]
    private UnityEvent _onStartGame;

    [SerializeField]
    private UnityEvent _onEndGame;

    [SerializeField, Tooltip("The final round start when only one point left")]
    private UnityEvent _onFinalRoundStart;

    private bool _onePointToWin;

    private bool _isPlaying;

    public void StartGame()
    {
        _isPlaying = true;
        _playerController.IsPlayerActive = true;
        _modulesManager.Initialize();
        _canvasManager.ShowCanvas(CanvasManager.POINTS_CANVAS);
        _onStartGame?.Invoke();
    }

    public void Win()
    {
        EndGame();
        _canvasManager.ShowCanvas(CanvasManager.WIN_CANVAS);
    }

    public void GameOver()
    {
        EndGame();
        _canvasManager.ShowCanvas(CanvasManager.GAMEOVER_CANVAS);
    }

    private void EndGame()
    {
        _modulesManager.enabled = false;
        _isPlaying = false;
        _playerController.IsPlayerActive = false;
        _onEndGame?.Invoke();
    }

    private void OnGetPoint(int points)
    {
        if (points >= _pointsToWin - 1)
        {
            if (_onePointToWin == true)
            {
                Win();
            }
            else
            {
                _onePointToWin = true;
                StartCoroutine(StartFinalRound());
            }
        }
    }

    private IEnumerator StartFinalRound()
    {
        _modulesManager.Clear();
        yield return new WaitForSeconds(2f);
        _onFinalRoundStart?.Invoke();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _playerData.Reset();
    }

    private void OnEnable()
    {
        _playerController._onDieAction += GameOver;
        _playerData.onGetPoint += OnGetPoint;
    }


    private void OnDisable()
    {
        _playerController._onDieAction -= GameOver;
        _playerData.onGetPoint -= OnGetPoint;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (_isPlaying == true)
        {
            _playerData.Time += Time.deltaTime;
        }
    }

}
