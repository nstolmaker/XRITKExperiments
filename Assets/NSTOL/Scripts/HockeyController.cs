using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HockeyController : MonoBehaviour
{
    public GameObject tableTop;
    public HockeyPaddle player1;
    public HockeyPaddle player2;
    public HockeyGoal goal1;
    public HockeyGoal goal2;
    public Puck puck;
    public Vector3 dropPuckPos = new Vector3(0, 0, 0);
    public Scoreboard score;
    public int[] pscore = { 0, 0 };
    private bool justScored = false;

    // Start is called before the first frame update
    void Start()
    {
        //puck.transform.position = dropPuckPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (justScored)
        {
            Debug.Log("Scored. Moving the puck back to center.");
            justScored = false;
            ResetPuck();
        }
    }

    public void ResetPuck()
    {
        this.puck.GetComponentInParent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        this.puck.GetComponentInParent<Transform>().localPosition = dropPuckPos;    //use localPosition, since thats where the numbers I saved came from. Otherwise you have to add the position of it's parent, which you wouldn't do. Just remember about localSpace!
    }

    public void ResetScore()
    {
        this.pscore[0] = 0; this.pscore[1] = 0;
        this.score.UpdateScoreboard(pscore[0], pscore[1]);
        ResetPuck();
    }

    public void GoalScored(int playerNumber)
    {
        pscore[playerNumber-1]++;
        this.score.UpdateScoreboard(pscore[0], pscore[1]);
        justScored = true;
    }
}
