using System;

internal class FakeShot : IPlay
{
    public string GetName()
    {
        return "fake-shot";
    }

    public ITileSelector GetTargetSelector(Ballman player)
    {
        return null;
    }

    public void ShootTheJ(Ballman player, Ballman target)
    {
    }
}