using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine.XR.Interaction.Toolkit
{
    public class HockeyGoal : MonoBehaviour
    {
        public int playerNum = 1;
        public int score = 0;
        public HockeyGoal scoreboard;
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
                DebugHelpers.Log("Goal! For player " + playerNum);
                score += 1;
                this.GetComponentInParent<HockeyController>().GoalScored(playerNum);
            }
        }

    }
}
