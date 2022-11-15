using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip _pickUpSfx;
    [SerializeField] int _score = 1;
    private delegate void OnPickup(int score);
    private OnPickup onPickup;
    private GameSession _gameSession;

    private void Awake()
    {
        _gameSession = FindObjectOfType<GameSession>();
        if(_gameSession)
        {
            onPickup += _gameSession.OnCoinPickup;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            onPickup?.Invoke(_score);
            AudioSource.PlayClipAtPoint(_pickUpSfx, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
