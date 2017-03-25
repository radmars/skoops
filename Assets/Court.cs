using System;
using System.Collections.Generic;
using UnityEngine;

public class Court : MonoBehaviour
{
    public int xSize, ySize;
    public Tile basicTile;
    public Tile[] tiles;
    private Tile over;
    private Tile selected;
    private Dictionary<Tile, Ballman> ballmen;

    private void Start()
    {
        ballmen = new Dictionary<Tile, Ballman>();
    }

    public void SetBallmanPosition(Ballman b, Tile oldTile, Tile newTile)
    {
        if (ballmen.ContainsKey(newTile))
        {
            throw new Exception("COLLISION DETECTION OMG AAA GAMEDEV31337!111");
        }
        if (oldTile)
        {
            ballmen.Remove(oldTile);
        }
        ballmen.Add(newTile, b);
    }

    void Update()
    {
        var click = Input.GetMouseButtonUp(0);
        if (click)
        {
            if (!selected && over)
            {
                Ballman ballman;
                if (ballmen.TryGetValue(over, out ballman))
                {
                    selected = over;
                    selected.SetSelected(true);
                }
            }
            else if (selected && !over)
            {
                ChangeHighlights(false);
                selected.SetSelected(false);
                selected = null;
            }
            else if (selected && over)
            {
                Ballman ballman;
                if(ballmen.TryGetValue(selected, out ballman))
                {
                    ChangeHighlights(false);
                    ballman.MoveToTile(over);
                    selected = null;
                }
            }
        }

        if(selected)
        {
            ChangeHighlights(true);
        }

        over = null;
    }

    private void ChangeHighlights(bool on)
    {
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                var xd = selected.X - x;
                xd = xd * xd;
                var yd = selected.Y - y;
                yd = yd * yd;
                Color c = Color.green;
                c.g *= 1 - Mathf.Clamp01(Mathf.Sqrt(xd + yd) / 10);
                tiles[i].Highlight(on, c);
            }
        }
    }

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        tiles = new Tile[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                tiles[i] = Instantiate(basicTile, transform);
                tiles[i].SetupTile(x, y, xSize, ySize);
                tiles[i].mouseOverEvent += MouseOver;
            }
        }
    }

    private void MouseOver(Tile t)
    {
        this.over = t;
    }
}
