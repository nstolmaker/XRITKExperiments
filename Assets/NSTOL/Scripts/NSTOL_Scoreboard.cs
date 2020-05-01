using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NSTOL_Scoreboard : MonoBehaviour
{

    public TextMeshPro side1;
    public TextMeshPro side2;

    public void UpdateScoreboard(int p1Score, int p2Score)
    {
        string scoreText = p1Score + " - " + p2Score; 
        this.side1.SetText(scoreText);
        this.side2.SetText(scoreText);
    }
}
