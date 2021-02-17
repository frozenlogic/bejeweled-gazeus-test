using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour
{
    public TextMeshProUGUI textScore;
    
    public void Refresh(int score)
    {
        textScore.text = "Score: " + score.ToString();
    }
}
