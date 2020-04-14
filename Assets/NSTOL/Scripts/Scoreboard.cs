using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{

    public TextMeshPro side1;
    public TextMeshPro side2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScoreboard(int p1Score, int p2Score)
    {
        string scoreText = p1Score + " - " + p2Score; 
        this.side1.SetText(scoreText);
        this.side2.SetText(scoreText);
    }
}
