using Godot;
using System;

public class Z : Shape
{
    public override void _Ready()
    {
        base._Ready();
        SHAPE = _SHAPE.Z;
        SetTurnMap();
        ColorChildren("red"); 
        SetCellPositions(0);
    }

}
