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

    private Team teamOne = new Team();
    private Team teamTwo = new Team();
    private Dictionary<Tile, Ballman> ballmen;
    public ITileSelector TileSelector { get; set; }

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

    public IEnumerable<Ballman> GetBallmen()
    {
        return ballmen.Values;
    }

    public void Start()
    {
        var b = ballmen[ballmen.Keys.First()];
        b.HasBall = true;
        b.Team.HasBall = true;
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

    void OnPlayFinished(Ballman man, string move)
    {
        selector.SetActive(false);
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
                TileSelector.Select(this, over, selector);
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
        selector.SetActive(false);
        teamTwo.RightToLeft = true;

        foreach (var team in new Team[] { teamOne, teamTwo })
        {
            var randomTiles = new Queue<Tile>(
                Shuffle(
                    tiles.Where(
                        (tile) => (int)((tile.X - 1) / (xSize / 2.0)) == (team.RightToLeft ? 1 : 0)
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
                b.MoveToTile(t, false);
                b.Team = team;
                team.Ballmen.Add(b);
                b.OnPlayFinished += OnPlayFinished;
            }
        }
    }

    private void MouseOver(Tile t)
    {
        this.over = t;
    }
}
