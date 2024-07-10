﻿using System.Numerics;

namespace JumperGame.components;

public class PositionComponent
{
    public Vector3 Position { get; set; }

    public PositionComponent(Vector3 initialPosition)
    {
        Position = initialPosition;
    }

}