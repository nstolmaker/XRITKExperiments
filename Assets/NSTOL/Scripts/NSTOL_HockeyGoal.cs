using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine.XR.Interaction.Toolkit
{
    public class NSTOL_HockeyGoal : MonoBehaviour
    {
        public int playerNum = 1;
        public int score = 0;
        public NSTOL_HockeyGoal scoreboard;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "AirHockeyPuck")
            {
                Debug.Log("Goal!");
                NSTOL_DebugHelpers.Log("Goal! For player " + playerNum);
                score += 1;
                this.GetComponentInParent<NSTOL_HockeyController>().GoalScored(playerNum);
            }
        }

    }
}
