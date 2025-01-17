﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float health;
    [SerializeField] private float speed; // block per second

    
    [HideInInspector] public int xIndex;
    [HideInInspector] public int yIndex;

    
    private Image _image;
    
    private float _timeElapsedSinceLastMoveInSeconds;
    private float _moveTimeInSeconds;
    private bool _canMove;
    private bool _dead;

    
    private void Start()
    {
        if (speed == 0)
        {
            // is this really needed?
            _moveTimeInSeconds = Mathf.Infinity;
        }
        
        _moveTimeInSeconds = 1 / speed;
        _image = GetComponentInChildren<Image>();
    }

    private void FixedUpdate()
    {
        if (_canMove)
        {
            return;
        }

        _timeElapsedSinceLastMoveInSeconds += Time.fixedDeltaTime;

        if (_timeElapsedSinceLastMoveInSeconds >= _moveTimeInSeconds)
        {
            _canMove = true;
            EventManager.EnemyReadyToMove(this);
        }
    }

    public void MoveToTile(GameTile newTile)
    {
        transform.SetParent(newTile.transform);
        transform.localPosition = Vector3.zero;

        _timeElapsedSinceLastMoveInSeconds = 0;
        _canMove = false;
    }
    
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (_image)
        {
            // a red flash to denote the enemy took damage 
            _image.DOColor(Color.red, 0.1f).OnComplete(
                () => _image.DOColor(Color.white, 0.1f));
        }

        if (health <= 0 && !_dead)
        {
            _dead = true;
            EventManager.EnemyDied(enemyType);
            
            if (_image)
            {
                _image.transform.DOScale(0f, 0.2f).OnComplete(() => Destroy(gameObject));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
