using Godot;
using System;

public class T : Shape
{
    public override void _Ready()
    {
        base._Ready();
        SHAPE = _SHAPE.T;
        SetTurnMap();
        ColorChildren("purple"); 
        SetCellPositions(0);
    }
}
