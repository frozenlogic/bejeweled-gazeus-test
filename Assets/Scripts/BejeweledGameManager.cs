using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BejeweledGameManager : MonoBehaviour
{
    public int PlayerScore;
    public GridSystem gridSystem;
    public GameView gameView;

    private void Start()
    {
        gridSystem.OnGemRemoved.AddListener(AddScore);
    }

    private void AddScore(int score)
    {
        PlayerScore += score;
        gameView.Refresh(PlayerScore);
    }
}
