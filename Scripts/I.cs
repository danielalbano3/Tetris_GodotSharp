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
        TurnPoints = 7;
        TurnPositions = new Vector2[TurnPoints];

        TurnTable = new Vector2[4,7]
        {
            {
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(0,2),
                new Vector2(2,2),
                new Vector2(2,3),
                new Vector2(3,2),
                new Vector2(3,3),
            },
            {
                new Vector2(0,3),
                new Vector2(1,3),
                new Vector2(2,0),
                new Vector2(2,1),
                new Vector2(2,3),
                new Vector2(3,0),
                new Vector2(3,1),
            },
            {
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(3,1),
                new Vector2(3,2),
                new Vector2(3,3),
            },
            {
                new Vector2(0,2),
                new Vector2(0,3),
                new Vector2(1,0),
                new Vector2(1,2),
                new Vector2(1,3),
                new Vector2(2,0),
                new Vector2(3,0),
            },
        };
    }





    protected override void CheckAround()
    {
        base.CheckAround();
        
    }



}
