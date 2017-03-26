using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballman : MonoBehaviour {
    public Tile currentTile;
    public float speed = 3.0f;
    public int team;
    private Animator animator;
    private GameObject ball;

    public delegate void PlayListener(Ballman m, string play);
    public delegate void MoveListener(Ballman m);

    public event MoveListener onMoveFinished;
    public event PlayListener onPlayFinished;

    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.Play("idle_no_ball");
        ball = transform.FindChild("basket_ball_boy/ball").gameObject;
        ball.SetActive(false);
    }

    public void SetTeam(int t)
    {
        if(t == 0)
        {
            transform.Rotate(new Vector3(0, 90, 0), Space.Self);
        }
        if (t == 1)
        {
            transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        }
    }
    public void RunPlay(string play)
    {
        if (onPlayFinished != null)
        {
            onPlayFinished(this, play);
        }
    }

    public void MoveToTile(Tile t, bool asMove)
    {
        this.currentTile = t;
        StartCoroutine(AnimateMoveToTile(t, asMove));
    }

    private IEnumerator AnimateMoveToTile(Tile tile, bool asMove)
    {
        var wffu = new WaitForFixedUpdate();
        Vector3 beginPosition = transform.position;
        Vector3 endPosition = tile.transform.position;
        Vector3 direction = (endPosition - beginPosition).normalized;

        float beginTime = Time.time;
        float endTime = beginTime + Vector3.Distance(beginPosition, endPosition) / speed;
        var duration = endTime - beginTime;
        yield return wffu;

        animator.Play("run_dribble");

        for (var t = Time.time - beginTime; t < duration; t = Time.time - beginTime)
        {
            transform.position = Vector3.Lerp(beginPosition, endPosition, t / duration);
            yield return wffu;
        }

        transform.position = endPosition;

        if (onMoveFinished != null && asMove)
        {
            Debug.Log("Moved");
            onMoveFinished(this);
        }
    }
}
