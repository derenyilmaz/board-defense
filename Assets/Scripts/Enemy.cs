﻿using DG.Tweening;
using UnityEngine;
using static Constants;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float health;
    [SerializeField] private float speed; // block per second

    
    [HideInInspector] public int xIndex;
    [HideInInspector] public int yIndex;

    
    private SpriteRenderer _spriteRenderer;
    
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
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

        if (_spriteRenderer)
        {
            // a red flash to denote the enemy took damage 
            _spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(
                () => _spriteRenderer.DOColor(Color.white, 0.1f));
        }

        if (health <= 0 && !_dead)
        {
            _dead = true;
            EventManager.EnemyDied(enemyType);
            Destroy(gameObject);
        }
    }
}
