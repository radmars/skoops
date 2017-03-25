using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public delegate void MouseOverTile(Tile t);
    public int X;
    public int Y;
    public MouseOverTile mouseOverEvent;
    private Material material;

    private bool hovered = false;

    private void OnMouseOver()
    {
        hovered = true;
        material.color = Color.red;
        mouseOverEvent(this);
    }

    private void OnMouseExit()
    {
        hovered = false;
        material.color = Color.white;
    }

    public void SetSelected(bool selected)
    {

    }

    public void Highlight(bool on, Color c)
    {
        if (!hovered)
        {
            material.color = on ? c : Color.white;
        }
    }

    // Update is called once per frame
    internal void SetupTile(int x, int y, int w, int h)
    {
        float width = w + 1;
        float height = h + 1;
        X = x;
        Y = y;
        name = "Tile " + x + ", " + y;
        transform.Translate(x - width / 2f, y - height / 2f, 0);
        gameObject.SetActive(true);
        material = GetComponent<Renderer>().material;
        material.mainTextureOffset = new Vector2(x / width, y / height);
        material.mainTextureScale = new Vector2(1 / width, 1 / height);
    }
}
