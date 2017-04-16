using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballman : MonoBehaviour {
    public Tile currentTile;
    public float speed = 3.0f;
    private Team team;
    private Animator animator;
    private GameObject ball;

    public delegate void PlayListener(Ballman m, string play);
    public delegate void MoveListener(Ballman m);

    public event MoveListener OnMoveFinished;
    private bool hasBall;

    static Dictionary<string, IPlay> playBook = new Dictionary<string, IPlay>();
    static Ballman()
    {
        foreach (var p in new IPlay[]
        {
            new Pass(),
            new Shoot(),
            new FakeShot(),
            new GuardShot(),
            new Pick(),
            new ExtraMove(),
        })
        {
            playBook.Add(p.GetName(), p);
        }
    }

    public void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        ball = transform.FindChild("basket_ball_boy/ball").gameObject;
        HasBall = false;
    }

    public IPlay[] GetPlays()
    {
        if (HasBall)
        {
            return new IPlay[] { playBook["pass"], playBook["sprint"], playBook["fake-shot"], playBook["shoot"] };
        }
        else
        {
            return new IPlay[] { playBook["guard-shot"], playBook["sprint"], };
        }
    }

    public bool HasBall
    {
        set
        {
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
        animator.SetTrigger(play);
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
            OnMoveFinished(this);
        }
    }
}
