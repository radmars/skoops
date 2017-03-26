using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour
{
    public Court court;
    private bool showGUI;
    private Ballman currentBallman;

    void Start()
    {
        foreach(var b in court.GetBallmen())
        {
            b.onMoveFinished += ShowMoveButtons;
        }
    }

    private void RunMove(Ballman bm, string play)
    {
        Debug.Log(bm.name + " running play " + play);
        court.SelectorEnabled = true;
        showGUI = false;
    }

    private void ShowMoveButtons(Ballman bm)
    {
        showGUI = true;
        court.SelectorEnabled = false;
        currentBallman = bm;
    }

    private void OnGUI()
    {
        if (!showGUI)
            return;
        Debug.Log("no rly");
        var buttons = new string[]{ "shoot", "dunk", "layup", "pass" };
        var position = new Rect(Screen.width/ 2f - 40, Screen.height / 2f - buttons.Length *  15, 80, 30);
        foreach( var b in buttons)
        {
            if (GUI.Button(position, b))
            {
                RunMove(currentBallman, b);
            }
            position.position += new Vector2(0, 30);
        }

    }
}
