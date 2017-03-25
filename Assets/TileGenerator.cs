using System;
using System.Collections;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public int xSize, ySize;
    public Tile basicTile;
    private Tile[] tiles;
    private Tile over;
    private Tile selected;

    void Start()
    {
    }

    void Update()
    {
        var click = Input.GetMouseButtonUp(0);
        if (click)
        {
            if (!selected && over)
            {
                selected = over;
                Debug.Log("Selected " + selected.name);
                selected.SetSelected(true);
            }
            else if (selected && !over)
            {
                Debug.Log("Unselecting " + selected.name);
                ChangeHighlights(false);
                selected.SetSelected(false);
                selected = null;
            }
            else if (selected && over)
            {
                Debug.Log("Started from " + selected.name + " going to " + over.name);
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
