﻿using Kubus;
using Microsoft.Xna.Framework;
using System;

static class Core
{
    internal static GameController Controller { get; private set; }

    internal static T GetComponent<T>() where T : IGameComponent
        => Controller.ComponentManager.GetComponent<T>();

    [STAThread]
    private static void Main(string[] args)
    {
        using (Controller = new GameController())
            Controller.Run();
    }
}

