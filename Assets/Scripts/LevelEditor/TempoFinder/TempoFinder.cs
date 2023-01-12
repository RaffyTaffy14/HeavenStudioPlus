using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeavenStudio.Editor 
{
    public class TempoFinder : Dialog
    {
        private bool pressed;
        private float timePressed;
        [SerializeField] private BPMText bpmText;
        private void Awake() 
        {
            pressed = false;
            timePressed = 0f;
        }
        public void SwitchTempoDialog()
        {
            if(dialog.activeSelf) {
                dialog.SetActive(false);
                timePressed = 0;
                bpmText.ResetText();
            } else {
                ResetAllDialogs();
                dialog.SetActive(true);
            }
        }
        public void TapBPM()
        {
            pressed = true;
        }
        private void Update()
        {
            timePressed += Time.deltaTime;
            if(pressed)
            {
                pressed = false;
                bpmText.ChangeText(timePressed);
                timePressed = 0;
            }
        }
    }
}