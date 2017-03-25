using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Update is called once per frame
    internal void SetupTile(int x, int y, int w, int h)
    {
        name = "Tile " + x + ", " + y;
        transform.Translate(x, y, 0);
        gameObject.SetActive(true);
        float width = w +1;
        float height = h +1;
        var material = GetComponent<Renderer>().material;
        material.mainTextureOffset = new Vector2(x / width, y / height);
        material.mainTextureScale = new Vector2(1 / width, 1 / height);
    }
}
