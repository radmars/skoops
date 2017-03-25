using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballman : MonoBehaviour {
    public Tile currentTile;
    public float speed = 3.0f;
    private Court court;
    private bool moving = false;

	// Use this for initialization
	void Start () {
        court = GameObject.FindObjectOfType<Court>();
        transform.localPosition = new Vector3(0, 1, 0);
        SetCurrentTile(court.tiles[(int)UnityEngine.Random.Range(0, court.tiles.Length)]);
        MoveToCurrentTile();
	}

    private void MoveToCurrentTile()
    {
        transform.position = currentTile.transform.position;
        transform.Translate(0, 1, 0);
    }

    public void MoveToTile(Tile t)
    {
        this.moving = true;
        StartCoroutine(AnimateMoveToTile(t));
    }

    private IEnumerator AnimateMoveToTile(Tile tile)
    {
        var wffu = new WaitForFixedUpdate();
        Vector3 beginPosition = transform.position;
        Vector3 endPosition = tile.transform.position;
        endPosition += new Vector3(0, 1, 0);
        Vector3 direction = (endPosition - beginPosition).normalized;

        float beginTime = Time.time;
        float endTime = beginTime + Vector3.Distance(beginPosition, endPosition) / speed;
        var duration = endTime - beginTime;

        for (var t = Time.time - beginTime; t < duration; t = Time.time - beginTime)
        {
            transform.position = Vector3.Lerp(beginPosition, endPosition, t / duration);
            yield return wffu;
        }
        transform.position = endPosition;
        this.SetCurrentTile(tile);
        this.moving = false;
    }

    private void SetCurrentTile(Tile tile)
    {
        court.SetBallmanPosition(this, this.currentTile, tile);
        this.currentTile = tile;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
