using System.Collections.Generic;
public class Team
{
    private List<Ballman> ballmen;
    private bool right;

    public Team()
    {
        ballmen = new List<Ballman>();
    }

    public IList<Ballman> Ballmen
    {
        get { return ballmen; }
        private set { }
    }

    public bool RightToLeft
    {
        get
        {
            return right;
        }
        set
        {
            right = value;
        }
    } 

    public int Rotation
    {
        get
        {
            return RightToLeft ? -90 : 90;
        }
    }

    public bool HasBall { get; internal set; }
}
