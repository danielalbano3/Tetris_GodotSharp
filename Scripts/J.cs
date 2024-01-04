using Godot;
using System;

public class J : Shape
{
    public override void _Ready()
    {
        base._Ready();
        SHAPE = _SHAPE.J;
        SetTurnMap();
        ColorChildren("blue"); 

        SetCellPositions(0);
    }
}
