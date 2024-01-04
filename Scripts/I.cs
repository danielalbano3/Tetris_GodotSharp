using Godot;
using System;

public class I : Shape
{
    public override void _Ready()
    {
        base._Ready();
        SHAPE = _SHAPE.I;
        SetTurnMap();
        ColorChildren("aqua"); 
        
        SetCellPositions(0);
    }
}
