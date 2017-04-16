using System;

internal class Shoot : IPlay
{
    public string GetName()
    {
        return "shoot";
    }

    public ITileSelector GetTargetSelector(Ballman player)
    {
        return null;
    }

    public void ShootTheJ(Ballman player, Ballman target)
    {

    }
}