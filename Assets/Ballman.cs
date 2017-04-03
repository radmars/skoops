using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballman : MonoBehaviour {
    public Tile currentTile;
    public float speed = 3.0f;
    public Team team;
    private Animator animator;
    private GameObject ball;

    public delegate void PlayListener(Ballman m, string play);
    public delegate void MoveListener(Ballman m);

    public event MoveListener OnMoveFinished;
    public event PlayListener OnPlayFinished;
    private bool hasBall;

    public void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        ball = transform.FindChild("basket_ball_boy/ball").gameObject;
        HasBall = false;
    }

    public string[] GetPlays()
    {
        if (HasBall)
        {
            return new string[] { "shoot", "bounce_pass", "chest_pass" };
        }
        else if (team.HasBall)
        {
            return new string[] { "pick", "spin", "step out", "wait" };
        }
        else
        {
            return new string[] { "guard_low", "guard_high", "reach" };
        }
    }

    public bool HasBall
    {
        set
        {
            Debug.Log("Hash ball" + value);
            hasBall = value;
            animator.SetBool("has_ball", value);
            ball.SetActive(hasBall);
        }
        get
        {
            return hasBall;
        }
    }

    public Team Team
    {
        set
        {
            team = value;
            transform.Rotate(new Vector3(0, team.Rotation, 0), Space.Self);
        }
        get
        {
            return team;
        }
    }

    public void RunPlay(string play)
    {
        if (OnPlayFinished != null)
        {
            animator.SetTrigger(play);
            OnPlayFinished(this, play);
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

        for (var t = Time.time - beginTime; t < duration; t = Time.time - beginTime)
        {
            transform.position = Vector3.Lerp(beginPosition, endPosition, t / duration);
            yield return wffu;
        }

        transform.position = endPosition;

        if (OnMoveFinished != null && asMove)
        {
            Debug.Log("Moved");
            OnMoveFinished(this);
        }
    }
}
