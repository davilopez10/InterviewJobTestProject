using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates and moves level modules
/// </summary>
public class ModulesManager : MonoBehaviour
{
    /// <summary>
    /// @TODO Do different difficulty levels
    /// </summary>
    public enum DifficultyModes { Easy, Medium, Hard };

    [SerializeField]
    private float _startMovementSpeed = -2f;

    [SerializeField]
    private float _gcPosition = -28f;

    [SerializeField]
    private float _spawnDistance = 18f;

    [SerializeField]
    private Transform _initModule;

    [SerializeField]
    private List<Transform> _modules;

    private DifficultyModes _currentLevel = DifficultyModes.Easy;

    private float _currentMovementSpeed;

    private List<Transform> _currentModules = new List<Transform>();

    public void Initialize()
    {
        _currentMovementSpeed = _startMovementSpeed;
        GenerateInitModules();
        enabled = true;
    }

    public void Clear()
    {
        for (int i = 0; i < _currentModules.Count; i++)
        {
            _currentModules[i].gameObject.SetActive(false);
            _modules.Add(_currentModules[i]);
            _currentModules.RemoveAt(i);
            i--;
        }

        enabled = false;
    }

    private void GenerateModule()
    {
        int randomValue;
        switch (_currentLevel)
        {
            case DifficultyModes.Easy:
                randomValue = Random.Range(0, _modules.Count);
                Transform randomModule = _modules[randomValue];
                _modules.RemoveAt(randomValue);

                randomModule.localPosition = _currentModules[0].localPosition + Vector3.right * _spawnDistance;
                randomModule.gameObject.SetActive(true);
                _currentModules.Add(randomModule);
                break;

            //@INFO: No time for this
            //case DifficultyModes.Medium:
            //    break;
            //case DifficultyModes.Hard:
            //    break;
        }
    }

    private void GenerateInitModules()
    {
        _initModule.transform.localPosition = Vector3.zero;
        _initModule.gameObject.SetActive(true);
        _currentModules.Add(_initModule);
        _modules.Remove(_initModule);

        GenerateModule();
    }

    public void Update()
    {
        for (int i = 0; i < _currentModules.Count; i++)
        {
            _currentModules[i].transform.localPosition += Vector3.right * _currentMovementSpeed * Time.deltaTime;

            if (_currentModules[i].transform.localPosition.x < _gcPosition)
            {
                _currentModules[i].gameObject.SetActive(false);
                _modules.Add(_currentModules[i]);
                _currentModules.RemoveAt(i);
                i--;
                GenerateModule();
            }
        }
    }
}
