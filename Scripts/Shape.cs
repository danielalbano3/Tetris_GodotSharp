using Godot;
using System;
using System.Collections.Generic;

public class Shape : Node2D
{
    [Signal] public delegate void UpdateSignal(Cell cell);
    [Signal] public delegate void NextShape(Shape oldShape);
    [Signal] public delegate void CheckRotations(Shape thisShape);

    public bool CanRotate;

    private Timer Clock;
    public Cell C1;
    public Cell C2;
    public Cell C3;
    public Cell C4;
    public Cell[] Cells;

    private enum Form
    {
        J,O,L,T,S,I,Z
    }
    private Form form;

    public int TurnCount;   
    
    public Vector2[,,] ShapeMap = new Vector2[7,4,4]
    {
        //J
        {
            //phase * 4
            {
                //cells (coords) * 4
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(1,2),
            },
            {
                //cells (coords) * 4
                new Vector2(0,1),
                new Vector2(0,2),
                new Vector2(1,1),
                new Vector2(2,1),
            },
            {
                //cells (coords) * 4
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(1,2),
                new Vector2(2,2),
            },
            {
                //cells (coords) * 4
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(2,0),
                new Vector2(2,1),
            },
        },
    
        //O
        {
            //phase * 4
            {
                //cells (coords) * 4
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(1,1),
            },
            {
                //cells (coords) * 4
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(1,1),
            },
            {
                //cells (coords) * 4
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(1,1),
            },
            {
                //cells (coords) * 4
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(1,1),
            },
            
        },
    
        //L
        {
            //phase * 4
            {
                //cells (coords) * 4
                new Vector2(0,2),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(1,2),
            },
            {
                //cells (coords) * 4
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(2,1),
                new Vector2(2,2)
            },
            {
                //cells (coords) * 4
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(1,2),
                new Vector2(2,0),
            },
            {
                //cells (coords) * 4
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(2,1),
            },
        },
    
        //T
        {
            //phase * 4
            {
                //cells (coords) * 4
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(1,2),
            },
            {
                //cells (coords) * 4
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(1,2),
                new Vector2(2,1),
            },
            {
                //cells (coords) * 4
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(1,2),
                new Vector2(2,1),
            },
            {
                //cells (coords) * 4
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(2,1),
            },
        },
    
        //S
        {
            //phase * 4
            {
                //cells (coords) * 4
                new Vector2(0,1),
                new Vector2(0,2),
                new Vector2(1,0),
                new Vector2(1,1),
            },
            {
                //cells (coords) * 4
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(1,2),
                new Vector2(2,2),
            },
            {
                //cells (coords) * 4
                new Vector2(1,1),
                new Vector2(1,2),
                new Vector2(2,0),
                new Vector2(2,1),
            },
            {
                //cells (coords) * 4
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(2,1),
            },
        },
    
        //I
        {
            //phase * 4
            {
                //cells (coords) * 4
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(1,2),
                new Vector2(1,3),
            },
            {
                //cells (coords) * 4
                new Vector2(0,2),
                new Vector2(1,2),
                new Vector2(2,2),
                new Vector2(3,2),
            },
            {
                //cells (coords) * 4
                new Vector2(2,0),
                new Vector2(2,1),
                new Vector2(2,2),
                new Vector2(2,3),
            },
            {
                //cells (coords) * 4
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(2,1),
                new Vector2(3,1),
            },
        },
    
        //Z
        {
            //phase * 4
            {
                //cells (coords) * 4
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(1,2),
            },
            {
                //cells (coords) * 4
                new Vector2(0,2),
                new Vector2(1,1),
                new Vector2(1,2),
                new Vector2(2,1),
            },
            {
                //cells (coords) * 4
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(2,1),
                new Vector2(2,2),
            },
            {
                //cells (coords) * 4
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(2,0),
            },
        }
    
    };
    public Vector2[,] PhaseMap = new Vector2[4,4];
    public Vector2[] CellMap = new Vector2[4];

    public override void _Ready()
    {
        base._Ready();
        CanRotate = true;
        TurnCount = 0;

        Clock = GetNode<Timer>("Clock");
        C1 = GetNode<Cell>("C1");
        C2 = GetNode<Cell>("C2");
        C3 = GetNode<Cell>("C3");
        C4 = GetNode<Cell>("C4");
        Cells = new Cell[4]{C1,C2,C3,C4};

        Clock.Connect("timeout", this, "GoDown");
        Clock.Start();

    }

    public void TestNext()
    {
        Vector2[] test = new Vector2[4];
        test = NextRotation();
        foreach (Vector2 v in test)
        {
            GD.Print("V.x: " + v.x + ", v.y: " + v.y);
        }
    }

    public void CheckGrid()
    {
        foreach (Cell cell in Cells)
        {
            EmitSignal("UpdateSignal", cell);
        }
        EmitSignal("CheckRotations", this);
    }

    public void MoveTo(float y, float x)
    {
        CheckGrid();
        float addx = x * 25f;
        float addy = y * 25f;
        Position = new Vector2(Position.x + addx,Position.y + addy);
    }

    public void GoRotate()
    {
        if (!CanRotate) return;

        TurnCount++;
        TurnCount %= 4;

        PositionCells();
    }  

    public Vector2[] NextRotation()
    {
        Vector2[] vec = new Vector2[4];
        int nextCount = TurnCount + 2;
        nextCount %= 4;

        for (int i = 0; i < 4; i++)
        {
            vec[i] = PhaseMap[nextCount,i];
        }

        return vec;
    }

    public void PositionCells()
    {
        for (int z = 0; z < 4; z++)
        {
            CellMap[z] = PhaseMap[TurnCount,z];
        }
        for (int w = 0; w < 4; w++)
        {
            Vector2 vec = CellMap[w];
            float addx = vec.x;
            float addy = vec.y;
            Cells[w].CellTo(addx,addy);
        }
    }

    public void GoDown()
    {
        CheckGrid();
        foreach (Cell cell in Cells)
        {
            
            if (!cell.CanGoDown) 
            {
                EmitSignal("NextShape", this);
                return;
            }
        }
        MoveTo(1,0);
        CheckGrid();

    }

    public void GoLeft()
    {
        CheckGrid();
        foreach (Cell cell in Cells)
        {
            if (!cell.CanGoLeft) return;
        }
        MoveTo(0,-1);
        CheckGrid();
    }

    public void GoRight()
    {
        CheckGrid();
        foreach (Cell cell in Cells)
        {
            if (!cell.CanGoRight) return;
        }
        MoveTo(0,1);
        CheckGrid();

    }
 
    private void UpdateMap()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                PhaseMap[x,y] = ShapeMap[(int)form,x,y];
            }
        }
    }

    private void ColorCells()
    {
        string color;

        switch (form)
        {
            case Form.J:
                color = "blue";
                break;
            case Form.O:
                color = "yellow";
                break;
            case Form.L:
                color = "orange";
                break;
            case Form.T:
                color = "purple";
                break;
            case Form.S:
                color = "green";
                break;
            case Form.I:
                color = "aqua";
                break;
            case Form.Z:
                color = "red";
                break;
            default:
                color = "gold";
                break;
        }

        foreach (Cell cell in Cells)
        {
            cell.Modulate = Color.ColorN(color);
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (Input.IsActionJustPressed("go_left")) GoLeft();
        if (Input.IsActionJustPressed("go_right")) GoRight();
        if (Input.IsActionJustPressed("go_rotate")) GoRotate();
        if (Input.IsActionPressed("go_down")) GoDown();
    }

    public void GetShape(int num)
    {
        switch (num)
        {
            case 0:
                form = Form.J;
                break;
            case 1:
                form = Form.O;
                break;
            case 2:
                form = Form.L;
                break;
            case 3:
                form = Form.T;
                break;
            case 4:
                form = Form.S;
                break;
            case 5:
                form = Form.I;
                break;
            case 6:
                form = Form.Z;
                break;
            default:
                break;
        }

        UpdateMap();
        PositionCells();
        ColorCells();
    }     


}
