using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballman : MonoBehaviour {
    public Tile currentTile;
    public float speed = 3.0f;

    public void MoveToTile(Tile t)
    {
        this.currentTile = t;
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
    }
}
