using System;
using UnityEngine;

public static class GlobalEvents
{
    public static event EventHandler OnPlayerHitWall = delegate { };
    public static void PlayerHitWall(object sender, WallEventArgs args)
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

    public static event EventHandler OnDeathTouched = delegate { };
    public static void DeathTouched(object sender, EventArgs args)
    {
        OnDeathTouched(sender, args);
    }

    public static event EventHandler OnDeathUnTouched = delegate { };
    public static void DeathUnTouched(object sender, EventArgs args)
    {
        OnDeathUnTouched(sender, args);
    }

    public static event EventHandler OnItemCollected = delegate { };
    public static void ItemCollected(object sender, ItemCollectedEventArgs args)
    {
        OnItemCollected(sender, args);
    }
}

public class WallEventArgs : EventArgs
{
    public string tag;

    public WallEventArgs(string tag)
    {
        this.tag = tag;
    }
}

public class EnemyDeathArgs : EventArgs
{
    public string tag;

    public EnemyDeathArgs(string tag)
    {
        this.tag = tag;
    }
}

public class ItemCollectedEventArgs : EventArgs
{
    public ItemType item;

    public ItemCollectedEventArgs(ItemType item)
    {
        this.item = item;
    }
}