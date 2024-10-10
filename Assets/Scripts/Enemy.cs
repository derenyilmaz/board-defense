﻿using UnityEngine;
using static Constants;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float health;
    [SerializeField] private float speed; // block per second

    
    [HideInInspector] public int xIndex;
    [HideInInspector] public int yIndex;
    
    
    private float _timeElapsedSinceLastMoveInSeconds;
    private float _moveTimeInSeconds;
    private bool _canMove;

    
    private void Start()
    {
        _moveTimeInSeconds = 1 / speed;
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

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            EventManager.EnemyDied(enemyType);
            Destroy(gameObject);
        }
    }
}