using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.Collections.Generic;


namespace UnityEngine.XR.Interaction.Toolkit { 
    public class NSTOLLog
    {
  
        public Text debugTextObject;

        public NSTOLLog(Text debugTextObject)
        {
            this.debugTextObject = debugTextObject;
        }

        public void Log(string debugText)
        {
            string newText = debugText;
            debugTextObject.text = newText;
        }

    }

}