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
        TurnPoints = 4;
        TurnPositions = new Vector2[TurnPoints];

        TurnTable = new Vector2[4,4]
        {
            {
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(2,1),
                new Vector2(2,2),
            },
            {
                new Vector2(0,2),
                new Vector2(1,0),
                new Vector2(1,2),
                new Vector2(2,0),
            },
            {
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(2,1),
                new Vector2(2,2),
            },
            {
                new Vector2(0,2),
                new Vector2(1,0),
                new Vector2(1,2),
                new Vector2(2,0),
            },
        };
    
    }

}
