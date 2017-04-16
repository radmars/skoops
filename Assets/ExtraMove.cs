using System;

internal class ExtraMove : IPlay
{
    public string GetName()
    {
        return "sprint";
    }

    public ITileSelector GetTargetSelector(Ballman player)
    {
        return null;
    }

    public void ShootTheJ(Ballman player, Ballman target)
    {
    }
}