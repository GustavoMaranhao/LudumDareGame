using System;
using UnityEngine;

public static class GlobalEvents
{
    public static event EventHandler OnPlayerHitWall = delegate { };
    public static void PlayerHitWall(object sender, EventArgs args)
    {
        OnPlayerHitWall(sender, args);
    }

    public static event EventHandler OnPlayerLeaveWall = delegate { };
    public static void PlayerLeaveWall(object sender, EventArgs args)
    {
        OnPlayerLeaveWall(sender, args);
    }

    public static event EventHandler OnWaveStart = delegate { };
    public static void WaveStart(object sender, EventArgs args)
    {
        OnWaveStart(sender, args);
    }

    public static event EventHandler OnEnemyDeath = delegate { };
    public static void EnemyDeath(object sender, EventArgs args)
    {
        OnEnemyDeath(sender, args);
    }
}