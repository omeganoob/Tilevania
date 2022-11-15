using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }
    private int _sceneIndex;
    private int _coinCount = 0;

    [SerializeField] private int _playerLives = 2;

    [SerializeField] private TextMeshProUGUI lbLives;
    [SerializeField] private TextMeshProUGUI lbCoins;
    void Awake()
    {
        //if(Instance != null && Instance != this)
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    Instance = this;
        //}
        int instances = FindObjectsOfType<GameSession>().Length;
        if(instances > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        lbLives.text = _playerLives+"";
        lbCoins.text = _coinCount+"";
    }

    public void OnPlayerDead()
    {
        if(_playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            StartCoroutine(ResetSession());
        }
    }

    public void OnCoinPickup(int score)
    {
        _coinCount+=score;
        lbCoins.text = _coinCount + "";
    }

    private void TakeLife()
    {
        _playerLives--;
        _sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(_sceneIndex);
        lbLives.text = _playerLives + "";
    }

    private IEnumerator ResetSession()
    {
        yield return new WaitForSecondsRealtime(1);
        _sceneIndex = SceneManager.GetActiveScene().buildIndex;
        ScenePersist.Instance.ResetScenePersist();
        SceneManager.LoadScene(_sceneIndex);
        Destroy(gameObject);
    }
}
 