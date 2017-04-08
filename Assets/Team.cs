using System.Collections.Generic;
using System.Linq;

public class Team
{
    public bool RightToLeft;

    public Team(string name)
    {
        Ballmen = new List<Ballman>();
        Name = name;
    }

    public string Name
    {
        get;
        private set;
    }

    public IList<Ballman> Ballmen
    {
        get;
        private set;
    }

    public int Rotation
    {
        get
        {
            return RightToLeft ? -90 : 90;
        }
    }

    public bool HasBall()
    {
        return Ballmen.Any((Ballman b) => { return b.HasBall; });
    }
}
