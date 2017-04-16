using System;

internal class Pick : IPlay
{
    public string GetName()
    {
        return "pick";
    }

    public ITileSelector GetTargetSelector(Ballman player)
    {
        return null;
    }

    public void ShootTheJ(Ballman player, Ballman target)
    {
    }
}