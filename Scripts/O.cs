using Godot;
using System;

public class O : Shape
{
    public override void _Ready()
    {
        base._Ready();
        SHAPE = _SHAPE.O;
        SetTurnMap();
        ColorChildren("yellow"); 
        SetCellPositions(0);
        TurnPoints = 0;
        TurnPositions = new Vector2[TurnPoints];


        TurnTable = new Vector2[4,0]
        {
            {},
            {},
            {},
            {},
        };
    }

}
