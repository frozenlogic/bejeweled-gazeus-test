using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Text textScore;
    
    public void Refresh(int score)
    {
        textScore.text = "Score: " + score.ToString();
    }
}
