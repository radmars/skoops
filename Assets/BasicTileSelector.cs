using UnityEngine;
class BasicTileSelector : ITileSelector
{
    TurnOrder order;
    public event TileSelected OnSelected;

    public BasicTileSelector(TurnOrder turnOrder)
    {
        this.order = turnOrder;
    }

    public void Select(Court c, Tile tile)
    {
        if(tile)
        {
            var ballman = order.CurrentTurn();
            if (!c.GetBallmanAt(tile))
            {
                ballman.MoveToTile(tile, true);
                c.SetBallmanPosition(ballman, tile);
                c.TileSelector = null;
            }
            else
            {
                // already someone there.
            }
        }
    }
}
