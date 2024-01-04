using Godot;
using System;

public class L : Shape
{
    public override void _Ready()
    {
        base._Ready();
        SHAPE = _SHAPE.L;
        SetTurnMap();
        ColorChildren("orange"); 
        SetCellPositions(0);
    }

}
