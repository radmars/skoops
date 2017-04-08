class Pass : IPlay
{
    private string PassType
    {
        get
        {
            return "chest_pass"; 
        }
    }

    public void ShootTheJ(Ballman player, Ballman target)
    {
        player.HasBall = false;
        player.RunPlay(PassType);

        target.HasBall = true;
        target.RunPlay("catch_pass");
    }

    public string GetName()
    {
        return "pass";
    }

    public ITileSelector GetTargetSelector(Ballman player)
    {
        return new TeamTileSelector(player.Team);
    }
}
