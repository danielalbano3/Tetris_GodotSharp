using Godot;
using System;

public class S : Shape
{
    public override void _Ready()
    {
        base._Ready();
        SHAPE = _SHAPE.S;
        SetTurnMap();
        ColorChildren("green"); 
        SetCellPositions(0);
    }

}
