using Godot;
using System;
using System.Collections.Generic;

public class Shape : Node2D
{
    protected int TurnPoints;

    protected Timer Clock;
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
    protected bool isActive;

    protected Cell C1;
    protected Cell C2;
    protected Cell C3;
    protected Cell C4;
    protected Cell[] cells;

    protected bool CanTurn;
    public List<bool> AroundList;
    protected Vector2[,] TurnTable;
    protected Vector2[] TurnPositions;

    [Signal] public delegate void NextShapeSignal(Cell c1, Cell c2, Cell c3, Cell c4);
    [Signal] public delegate void RequestUpdateSignal(Cell c1, Cell c2, Cell c3, Cell c4);
    [Signal] public delegate void SeeAroundSignal(Vector2 pos);

    protected virtual void CheckAround()
    {}

    protected int[,,] ShapeTurns = new int[4,4,2];

    protected int[,,,] TurnMap =
    {
        //I
        {
            {
                {1,0},{1,1},{1,2},{1,3}
            },
            {
                {0,2},{1,2},{2,2},{3,2}
            },
            {
                {2,0},{2,1},{2,2},{2,3}
            },
            {
                {0,1},{1,1},{2,1},{3,1}
            },
        },
        
        //z
        {
            {
                {0,0},{0,1},{1,1},{1,2}
            },
            {
                {0,2},{1,1},{1,2},{2,1}
            },
            {
                {1,0},{1,1},{2,1},{2,2}
            },
            {
                {0,1},{1,0},{1,1},{2,0}
            },
            
        },
        
        //j
        {
            {
                {0,0},{1,0},{1,1},{1,2}
            },
            {
                {0,1},{0,2},{1,1},{2,1}
            },
            {
                {1,0},{1,1},{1,2},{2,2}
            },
            {
                {0,1},{1,1},{2,0},{2,1}
            },
        },
        
        //o
        {
            {
                {0,0},{0,1},{1,0},{1,1}
            },
            {
                {0,0},{0,1},{1,0},{1,1}
            },
            {
                {0,0},{0,1},{1,0},{1,1}
            },
            {
                {0,0},{0,1},{1,0},{1,1}
            },
        },
        
        //L
        {
            {
                {0,2},{1,0},{1,1},{1,2}
            },
            {
                {0,1},{1,1},{2,1},{2,2}
            },
            {
                {1,0},{1,1},{1,2},{2,0}
            },
            {
                {0,0},{0,1},{1,1},{2,1}
            },
        },
        
        //t
        {
            {
                {0,1},{1,0},{1,1},{1,2}
            },
            {
                {0,1},{1,1},{2,1},{1,2}
            },
            {
                {1,0},{1,1},{1,2},{2,1}
            },
            {
                {0,1},{1,0},{1,1},{2,1}
            },
        },
        
        //s
        {
            {
                {0,1},{0,2},{1,0},{1,1}
            },
            {
                {0,1},{1,1},{1,2},{2,2}
            },
            {
                {1,1},{1,2},{2,0},{2,1}
            },
            {
                {0,0},{1,0},{1,1},{2,1}
            },
        },
    };

    public override void _Ready()
    {   
        AroundList = new List<bool>();
        CanTurn = true;
        isActive = true;
        rotateCount = 0;
        Clock = GetNode<Timer>("Clock");
        C1 = GetNode<Cell>("C1");
        C2 = GetNode<Cell>("C2");
        C3 = GetNode<Cell>("C3");
        C4 = GetNode<Cell>("C4");
        cells = new Cell[4]{C1,C2,C3,C4};
        
        foreach (Cell cell in cells)
        {
            cell.AddToGroup("squares");
        }
        Clock.Start();
        Clock.Connect("timeout", this, "GoDown");

    } 
   
    protected void EndShape()
    {
        isActive = false;
        EmitSignal("NextShapeSignal", C1, C2, C3, C4);
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
        foreach (Cell cell in cells)
        {
            cell.ColorCell(shapecolor);
        }
    }

    public void MoveTo(float row, float col)
    {
        float drow = row * 25f;
        float dcol = col * 25f;
        
        Position = new Vector2(Position.x + dcol, Position.y + drow);
        CheckTurn();
    }

    public void CheckTurn()
    {
        if (SHAPE == _SHAPE.O) return;

        AroundList.Clear();
        foreach (Vector2 pos in TurnPositions)
        {
            EmitSignal("SeeAroundSignal", pos);
        }

        CanTurn = !AroundList.Contains(false);
    }
  
    public void GoDown()
    {
        EmitSignal("RequestUpdateSignal", C1, C2, C3, C4);
        foreach (Cell cell in cells)
        {
            if (!cell.CanGoDown) 
            {   
                EndShape();
                return;
            }
        }
        MoveTo(1,0);
    }

    public void GoLeft()
    {
        if (!isActive) return;

        EmitSignal("RequestUpdateSignal", C1, C2, C3, C4);
        foreach (Cell cell in cells)
        {
            if (!cell.CanGoLeft) return;
        }
        MoveTo(0,-1);
    }

    public void GoRight()
    {
        if (!isActive) return;

        EmitSignal("RequestUpdateSignal", C1, C2, C3, C4);
        foreach (Cell cell in cells)
        {
            if (!cell.CanGoRight) return;
        }
        MoveTo(0,1);
    }

    protected void GoTurn()
    {
        UpdateTurnPositions();
        if (!isActive) return;
        if (!CanTurn) return;

        EmitSignal("RequestUpdateSignal", C1, C2, C3, C4);

        rotateCount++;
        rotateCount %= 4;
        SetCellPositions(rotateCount);
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
            GoTurn();
        }

        if (Input.IsActionJustPressed("go_down"))
        {
            GoDown();
        }

        if (Input.IsActionJustPressed("go_left"))
        {
            GoLeft();
        }

        if (Input.IsActionJustPressed("go_right"))
        {
            GoRight();
        }
    }

    protected void UpdateTurnPositions()
    {
        int turn = rotateCount % 4;
        for (int g = 0; g < TurnPoints; g++)
        {
            TurnPositions[g] = TurnTable[turn,g];
        }
    }
}
