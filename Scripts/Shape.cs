using Godot;
using System;
using System.Collections.Generic;

public class Shape : Node2D
{
    protected int rotateCount;
    protected enum _SHAPE
    {
        I,
        Z,
        J,
        O,
        L,
        T,
        S
    }
    protected _SHAPE SHAPE;

    protected Cell C1;
    protected Cell C2;
    protected Cell C3;
    protected Cell C4;

    public Board Board;

    [Signal] public delegate void NextShapeSignal();

    protected int[,,] ShapeTurns = new int[4,4,2];

    protected int[,,,] TurnMap =
    {
        //I [0]
        {
            //MOVES [0,0]
            {
                //CELL POSITIONS[0,0,0] x,y
                {1,0},{1,1},{1,2},{1,3}
            },
            {
                //CELL POSITIONS
                {0,2},{1,2},{2,2},{3,2}
            },
            {
                //CELL POSITIONS
                {2,0},{2,1},{2,2},{2,3}
            },
            {
                //CELL POSITIONS
                {0,1},{1,1},{2,1},{3,1}
            },
            
        },
        
        //Z
        {
            //MOVES
            {
                //CELL POSITIONS
                {0,0},{0,1},{1,1},{1,2}
            },
            {
                //CELL POSITIONS
                {0,2},{1,1},{1,2},{2,1}
            },
            {
                //CELL POSITIONS
                {1,0},{1,1},{2,1},{2,2}
            },
            {
                //CELL POSITIONS
                {0,1},{1,0},{1,1},{2,0}
            },
            
        },
        
        //J
        {
            //MOVES
            {
                //CELL POSITIONS
                {0,0},{1,0},{1,1},{1,2}
            },
            {
                //CELL POSITIONS
                {0,1},{0,2},{1,1},{2,1}
            },
            {
                //CELL POSITIONS
                {1,0},{1,1},{1,2},{2,2}
            },
            {
                //CELL POSITIONS
                {0,1},{1,1},{2,0},{2,1}
            },
            
        },
        
        //O
        {
            //MOVES
            {
                //CELL POSITIONS
                {0,0},{0,1},{1,0},{1,1}
            },
            {
                //CELL POSITIONS
                {0,0},{0,1},{1,0},{1,1}
            },
            {
                //CELL POSITIONS
                {0,0},{0,1},{1,0},{1,1}
            },
            {
                //CELL POSITIONS
                {0,0},{0,1},{1,0},{1,1}
            },
            
            
        },
        
        //L
        {
            //MOVES
            {
                //CELL POSITIONS
                {0,2},{1,0},{1,1},{1,2}
            },
            {
                //CELL POSITIONS
                {0,1},{1,1},{2,1},{2,2}
            },
            {
                //CELL POSITIONS
                {1,0},{1,1},{1,2},{2,0}
            },
            {
                //CELL POSITIONS
                {0,0},{0,1},{1,1},{2,1}
            },
            
        },
        
        //T
        {
            //MOVES
            {
                //CELL POSITIONS
                {0,1},{1,0},{1,1},{1,2}
            },
            {
                //CELL POSITIONS
                {0,1},{1,1},{2,1},{1,2}
            },
            {
                //CELL POSITIONS
                {1,0},{1,1},{1,2},{2,1}
            },
            {
                //CELL POSITIONS
                {0,1},{1,0},{1,1},{2,1}
            },
            
        },
        
        //S
        {
            //MOVES
            {
                //CELL POSITIONS
                {0,1},{0,2},{1,0},{1,1}
            },
            {
                //CELL POSITIONS
                {0,1},{1,1},{1,2},{2,2}
            },
            {
                //CELL POSITIONS
                {1,1},{1,2},{2,0},{2,1}
            },
            {
                //CELL POSITIONS
                {0,0},{1,0},{1,1},{2,1}
            },
            
        },
        
    };

    public override void _Ready()
    {   
        rotateCount = 0;
        C1 = GetNode<Cell>("C1");
        C2 = GetNode<Cell>("C2");
        C3 = GetNode<Cell>("C3");
        C4 = GetNode<Cell>("C4");

        // Board = (Board)GetTree().GetNodesInGroup("board")[0];
        // Connect("NextShapeSignal", Board, "NextShape");
    } 

    protected void SetTurnMap()
    {
        for (int a = 0; a < 4; a++)
        {
            for (int b = 0; b < 4; b++)
            {
                for (int c = 0; c < 2; c++)
                {
                    ShapeTurns[a,b,c] = TurnMap[(int)SHAPE,a,b,c];
                }
            }
        }
    }

    protected void ColorChildren(string shapecolor)
    {
        foreach (Cell cell in GetChildren())
        {
            cell.Modulate = Color.ColorN(shapecolor);
        }
    }

    public void MoveTo(float row, float col)
    {
        float drow = row * 25f;
        float dcol = col * 25f;
        
        Position = new Vector2(Position.x + drow, Position.y + dcol);
    }
  
    public void GoDown()
    {
        foreach (Cell cell in GetChildren())
        {
            cell.LookAround();

            if (!cell.CanGoDown)
            {
                EmitSignal("NextShapeSignal");
                return;
            } 
        }

        GD.Print("Can go down: " + GetChild<Cell>(2).CanGoDown);
        MoveTo(0,1);
    }

    public void GoLeft()
    {
        foreach (Cell cell in GetChildren())
        {
            cell.LookAround();
            if (!cell.CanGoLeft) return;
        }
        MoveTo(0,-1);
    }

    public void GoRight()
    {
        foreach (Cell cell in GetChildren())
        {
            cell.LookAround();
            if (!cell.CanGoRight) return;
        }
        MoveTo(0,1);
    }

    public virtual void GoTurn()
    {

    }

    protected void SetCellPositions(int moveNumber)
    {
        int[,] cellCoords = new int[4,2];

        for (int p = 0; p < 4; p++)
        {
            for (int q = 0; q < 2; q++)
            {
                cellCoords[p,q] = ShapeTurns[moveNumber,p,q];
            }
        }

        C1.Position = new Vector2(pixCon(cellCoords[0,1]),pixCon(cellCoords[0,0]));
        C2.Position = new Vector2(pixCon(cellCoords[1,1]),pixCon(cellCoords[1,0]));
        C3.Position = new Vector2(pixCon(cellCoords[2,1]),pixCon(cellCoords[2,0]));
        C4.Position = new Vector2(pixCon(cellCoords[3,1]),pixCon(cellCoords[3,0]));
    }

    protected float pixCon(int num)
    {
        float result = (float)(num * 25f);
        return result;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (Input.IsActionJustPressed("rotate"))
        {
            rotateCount++;
            rotateCount %= 4;
            SetCellPositions(rotateCount);
        }
    }
}
