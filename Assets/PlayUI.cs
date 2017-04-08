using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrder
{
    public int CurrentTurnIndex { get; private set; }
    public int CurrentRound { get; private set; }

    private List<Ballman> Turns = new List<Ballman>();

    public TurnOrder(Team one, Team two)
    {
        CurrentTurnIndex = 0;
        CurrentRound = 0;
        Turns.AddRange(one.Ballmen);
        Turns.AddRange(two.Ballmen);
        Turns.Shuffle();
    }

    public Ballman CurrentTurn()
    {
        return Turns[CurrentTurnIndex];
    }

    public void FinishTurn()
    {
        CurrentTurnIndex++;
        if(CurrentTurnIndex >= Turns.Count)
        {
            CurrentTurnIndex = 0;
            CurrentRound++;
        }
    }
}

public class PlayUI : MonoBehaviour
{
    public GameObject selectedBallmanMarker;
    public Court court;
    public Button buttonTemplate;
    public Canvas canvas;

    private BasicTileSelector basicSelector;
    private TurnOrder turnOrder;


    void Start()
    {
        turnOrder = new TurnOrder(court.TeamOne, court.TeamTwo);
        foreach(var b in court.GetBallmen())
        {
            b.OnMoveFinished += ShowMoveButtons;
        }
        basicSelector = new BasicTileSelector(turnOrder);
        court.TileSelector = basicSelector;
        selectedBallmanMarker.SetActive(false);
        UpdateSelectedPlayer();
    }

    void UpdateSelectedPlayer()
    {
        var currentBallman = turnOrder.CurrentTurn();
        selectedBallmanMarker.transform.parent = currentBallman.transform;
        selectedBallmanMarker.transform.position = currentBallman.transform.position;
        selectedBallmanMarker.SetActive(true);
    }

    private void RunPlay(IPlay play, Ballman player, Ballman target)
    {
        play.ShootTheJ(player, target);
        court.TileSelector = basicSelector;
    }

    private void ShowMoveButtons(Ballman bm)
    {
        court.TileSelector = null;
        var currentBallman = bm;
        var buttons = new List<Button>();

        var plays = bm.GetPlays();
        var position = new Vector2(Screen.width / 2f - 40, Screen.height / 2f - plays.Length * 15);
        foreach (var play in plays)
        {
            var newButton = Instantiate(buttonTemplate, canvas.transform);
            newButton.GetComponentInChildren<Text>().text = play.GetName();
            newButton.gameObject.SetActive(true);
            newButton.onClick.AddListener(() =>
            {
                buttons.ForEach((b) => Destroy(b.gameObject));
                buttons.Clear();
                var selector = play.GetTargetSelector(bm);
                if (selector != null)
                {
                    court.TileSelector = selector;

                    selector.OnSelected += (selected) =>
                    {
                        RunPlay(play, currentBallman, court.GetBallmanAt(selected));
                        turnOrder.FinishTurn();
                        UpdateSelectedPlayer();
                    };
                }
                else
                {
                    RunPlay(play, currentBallman, null);
                }
            });
            newButton.transform.position = position;
            position += new Vector2(0, 30);
            buttons.Add(newButton);
        }
    }
}
