using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void TileSelected(Tile t);

public interface ITileSelector
{
    event TileSelected OnSelected;
    void Select(Court c, Tile tile, GameObject selector);
}

class BasicTileSelector : ITileSelector
{
    Tile selected;
    public event TileSelected OnSelected;

    public void Select(Court c, Tile tile, GameObject selector)
    {
        if(!selected && tile)
        {
            var ballman = c.GetBallmanAt(tile);
            if(ballman)
            {
                selected = tile;
                selector.transform.parent = ballman.transform;
                selector.transform.position = ballman.transform.position;
                selector.SetActive(true);
            }
        }
        else if(selected && !tile)
        {
            selected = null;
            selector.SetActive(false);
        }
        else if(selected && tile)
        {
            var ballman = c.GetBallmanAt(selected);
            if (ballman)
            {
                if (!c.GetBallmanAt(tile))
                {
                    ballman.MoveToTile(tile, true);
                    c.SetBallmanPosition(ballman, selected, tile);
                    selected = null;
                }
                else
                {
                    // already someone there.
                }
            }
        }
    }
}

class TeamTileSelector : ITileSelector
{
    public event TileSelected OnSelected;
    private Team team;

    public TeamTileSelector(Team t)
    {
        team = t;
    }

    public void Select(Court c, Tile tile, GameObject selector)
    {
        if (tile)
        {
            var b = c.GetBallmanAt(tile);
            if (b && b.Team == team)
            {
                OnSelected(tile);
            }
        }
    }
}

public class PlayUI : MonoBehaviour
{
    public Court court;
    private Ballman currentBallman;
    public Button buttonTemplate;
    public Canvas canvas;
    public List<Button> buttons;
    private BasicTileSelector basicSelector = new BasicTileSelector();

    void Start()
    {
        buttons = new List<Button>();
        foreach(var b in court.GetBallmen())
        {
            b.OnMoveFinished += ShowMoveButtons;
        }
        court.TileSelector = basicSelector;
    }

    private void RunPlay(IPlay play, Ballman player, Ballman target)
    {
        play.ShootTheJ(player, target);
        court.TileSelector = basicSelector;
    }

    private void ShowMoveButtons(Ballman bm)
    {
        court.TileSelector = null;
        currentBallman = bm;

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
