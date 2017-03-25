using System;
using System.Collections;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public int xSize, ySize;
    public Tile basicTile;
    private Tile[] tiles;

    void Start()
    {
    }

    void Update()
    {

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
            }
        }
    }
}
