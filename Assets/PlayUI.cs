using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    public Court court;
    private Ballman currentBallman;
    public Button buttonTemplate;
    public Canvas canvas;
    public List<Button> buttons;

    void Start()
    {
        buttons = new List<Button>();
        foreach(var b in court.GetBallmen())
        {
            b.onMoveFinished += ShowMoveButtons;
        }
    }

    private void RunPlay(Ballman bm, string play)
    {
        bm.RunPlay(play);
        Debug.Log(bm.name + " running play " + play);
        court.SelectorEnabled = true;
        buttons.ForEach((b) => Destroy(b.gameObject));
        buttons.Clear();
    }

    private void ShowMoveButtons(Ballman bm)
    {
        court.SelectorEnabled = false;
        currentBallman = bm;

        var buttonText = new string[]{ "shoot", "dunk", "layup", "pass" };
        var position = new Vector2(Screen.width / 2f - 40, Screen.height / 2f - buttonText.Length * 15);
        foreach ( var b in buttonText)
        {
            var newButton = Instantiate(buttonTemplate, canvas.transform);
            newButton.GetComponentInChildren<Text>().text = b;
            newButton.gameObject.SetActive(true);
            newButton.onClick.AddListener(() =>
            {
                RunPlay(currentBallman, b);
            });
            newButton.transform.position = position;
            position += new Vector2(0, 30);
            buttons.Add(newButton);
        }
    }
}
