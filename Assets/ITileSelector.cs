public delegate void TileSelected(Tile t);

public interface ITileSelector
{
    event TileSelected OnSelected;
    void Select(Court c, Tile tile);
}
