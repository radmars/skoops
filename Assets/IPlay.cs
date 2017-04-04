using System;

public interface IPlay
{
    String GetName();
    ITileSelector GetTargetSelector(Ballman player);
    void ShootTheJ(Ballman player, Ballman target);
}
