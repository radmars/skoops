using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Court : MonoBehaviour
{
    public int xSize, ySize;
    public Tile tileTemplate;
    public Tile[] tiles;
    public Ballman ballmanTemplate;
    private Tile over;
    private Tile selected;
    private Dictionary<Tile, Ballman> ballmen;
    public GameObject selector;
    public GameObject tileMarker;

    public static IList<T> Shuffle<T>(IList<T> list)
    {
        var rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    private void Start()
    {
        selector.SetActive(false);
        for (var team = 0; team <= 1; team++)
        {
            var randomTiles = new Queue<Tile>(
                Shuffle(
                    tiles.Where(
                        (tile) => (int)((tile.X-1) / (xSize / 2.0)) == team
                    ).ToList()
                )
            );
            for (var i = 0; i < 5; i++)
            {
                var b = Instantiate(ballmanTemplate);
                b.gameObject.SetActive(true);
                b.name = "Ballman " + i;
                var t = randomTiles.Dequeue();
                ballmen[t] = b;
                b.MoveToTile(t);
                b.SetTeam(team);
            }
        }
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
                }
            }
            else if (selected && !over)
            {
                ChangeHighlights(false);
                selected = null;
            }
            else if (selected && over)
            {
                Ballman ballman;
                if(ballmen.TryGetValue(selected, out ballman))
                {
                    ballman.MoveToTile(over);
                    SetBallmanPosition(ballman, selected, over);
                    selected = null;
                }
            }
        }

        if(over)
        {
            tileMarker.SetActive(true);
            tileMarker.transform.position = over.transform.position;
            tileMarker.transform.position += new Vector3(0, .05f, 0);
        }
        else
        {
            tileMarker.SetActive(true);
        }
        ChangeHighlights(false);

        over = null;
    }

    private void ChangeHighlights(bool on)
    {
        if(selected)
        {
            selector.transform.position =
                selected.transform.position;
            selector.SetActive(true);
        }
        else
        {
            selector.SetActive(false);
        }
    }

    private void Awake()
    {
        ballmen = new Dictionary<Tile, Ballman>();
        tiles = new Tile[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                tiles[i] = Instantiate(tileTemplate, transform);
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
