using UnityEngine;
class TeamTileSelector : ITileSelector
{
    public event TileSelected OnSelected;
    private Team team;

    public TeamTileSelector(Team t)
    {
        team = t;
    }

    public void Select(Court c, Tile tile)
    {
        if (tile)
        {
            var b = c.GetBallmanAt(tile);
            if (b && b.Team == team)
            {
                OnSelected(tile);
            }
        }
    }
}
