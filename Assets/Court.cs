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
   
    public ITileSelector TileSelector { get; set; }

    public GameObject tileMarker;

    public Team TeamOne { get; private set; }
    public Team TeamTwo { get; private set; }

    private Tile over;
    private Dictionary<Tile, Ballman> ballmen;

    public IEnumerable<Ballman> GetBallmen()
    {
        return ballmen.Values;
    }

    public void Start()
    {
        var b = ballmen[ballmen.Keys.First()];
        b.HasBall = true;
    }

    public void SetBallmanPosition(Ballman b, Tile newTile)
    {
        if (ballmen.ContainsKey(newTile))
        {
            throw new Exception("COLLISION DETECTION OMG AAA GAMEDEV31337!111");
        }
        var oldTile = ballmen.First(pair => pair.Value == b).Key;
        if (oldTile)
        {
            ballmen.Remove(oldTile);
        }
        ballmen.Add(newTile, b);
    }

    public Ballman GetBallmanAt(Tile tile)
    {
        Ballman outman;
        ballmen.TryGetValue(tile, out outman);
        return outman;
    }

    // TODO: Move to play ui
    void Update()
    {
        if (TileSelector != null)
        {
            if (Input.GetMouseButtonUp(0)) {
                TileSelector.Select(this, over);
            }
        }

        if (over)
        {
            tileMarker.SetActive(true);
            tileMarker.transform.position = over.transform.position;
            tileMarker.transform.position += new Vector3(0, .05f, 0);
        }
        else
        {
            tileMarker.SetActive(true);
        }

        over = null;
    }

    public void Awake()
    {
        TeamOne = new Team("red");
        TeamTwo = new Team("blue");
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
        TeamTwo.RightToLeft = true;

        foreach (var team in new Team[] { TeamOne, TeamTwo })
        {
            var randomTiles = new Queue<Tile>(
                tiles.Where(
                    (tile) => (int)((tile.X - 1) / (xSize / 2.0)) == (team.RightToLeft ? 1 : 0)
                ).ToList().Shuffle()
            );
            for (var i = 0; i < 5; i++)
            {
                var tile = randomTiles.Dequeue();
                CreateBallman(team, tile, "Ballman " + i);
            }
        }
    }

    private void CreateBallman(Team team, Tile tile, string name)
    {
        var newBallman = Instantiate(ballmanTemplate);
        newBallman.gameObject.SetActive(true);
        newBallman.name = name;
        ballmen[tile] = newBallman;
        newBallman.MoveToTile(tile, false);
        newBallman.Team = team;
        team.Ballmen.Add(newBallman);
    }

    private void MouseOver(Tile t)
    {
        this.over = t;
    }
}
