using System;

internal class GuardShot : IPlay
{
    public string GetName()
    {
        return "guard-shot";
    }

    public ITileSelector GetTargetSelector(Ballman player)
    {
        return null;
    }

    public void ShootTheJ(Ballman player, Ballman target)
    {
    }
}