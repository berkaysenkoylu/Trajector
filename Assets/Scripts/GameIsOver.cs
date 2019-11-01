using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIsOver : MonoBehaviour
{
    public delegate void GameOver();
    public static event GameOver OnGameOver;

    public void PlayerIsDead()
    {
        OnGameOver();
    }
}
