using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Board : Node2D
{
    public int[] LineCount;
    public Cell[,] Cells;

    private PackedScene ShapeScene;
    private PackedScene CellScene;

    public Queue<int> NextTetros;

    [Signal] public delegate void ComboSignal(int combo, int row);
    [Signal] public delegate void GameOver();


    public override void _Ready()
    {
        base._Ready();
        LineCount = new int[20];
        ShapeScene = ResourceLoader.Load<PackedScene>("res://Scenes/Shape.tscn");
        CellScene = ResourceLoader.Load<PackedScene>("res://Scenes/Cell.tscn");
        
        Cells = new Cell[20,10];
        NextTetros = new Queue<int>();
        for (int i = 0; i < 7; i++)
        {
            Random rand = new Random();
            NextTetros.Enqueue(rand.Next(7));
        }

        SpawnShape();


    }

    public void FillRow(int row)
    {
        for (int k = 0; k < 10; k++)
        {
            SpawnCell(row,k);
        }
    }

    public void DropChunk(int bottomrow, int dropcount = 1)
    {
        for (int row = bottomrow; row >= 0; row--)
        {
            DropLine(row, dropcount);
        }
    }

    public void DropCell(int row, int col, int dropcount)
    {
        Cell oldcell = Cells[row,col] as Cell;

        if (oldcell != null)
        {
            Cells[row + dropcount,col] = oldcell;

            float x = col * 25f;
            float y = (row + dropcount) * 25f;
            oldcell.Position = new Vector2(x, y);

            Cells[row,col] = null;
        }
    }

    public void DropLine(int row, int dropcount = 1)
    {
        for (int i = 0; i < 10; i++)
        {
            DropCell(row,i,dropcount);
        }
    }

    public void ScanLines()
    {
        int combo = 0;
        int current = 20;
        int previous;

        List<int> Deleted = new List<int>();
        
        for (int j = 19; j >= 0; j--)
        {
            int count = CountLine(j);
            if (count == 10)
            {
                if (combo == 0)
                {
                    previous = j + 1;
                }
                else
                {
                    previous = current;
                }
                current = j;

                DeleteLine(j);
                Deleted.Add(j);
                GD.Print(j);
                if (previous - current == 1)
                {
                    if (combo == 4)
                    {
                        ShowCombo(4, j);
                        combo = 0;
                    }
                    else
                    {
                        combo++;
                    }
                }
                else
                {
                    ShowCombo(combo, j);
                    combo = 0;
                }
            }
            else
            {
                if (combo > 0)
                {
                    ShowCombo(combo, j);
                }
                
                combo = 0;
            }
        }

        int[] del = Deleted.ToArray();
        
        for (int m = del.Length - 1; m >= 0; m--)
        {
            DropChunk(del[m]);
        }
        SpawnShape();
    }

    public void ShowCombo(int combo, int row)
    {
        GD.Print("Combo: " + combo);
        EmitSignal("ComboSignal", combo, row);
    }

    public void DeleteLine(int row)
    {
        for (int i = 0; i < 10; i++)
        {
            DeleteCell(row,i);
        }
    }

    public void DeleteCell(int row, int col)
    {
        if (row >= 0 && row <= 19 && col >=0 && col <= 9)
        {
            if (Cells[row,col] != null)
            {
                Cells[row,col].QueueFree();
                Cells[row,col] = null;
            }
        }
        else
        {
            return;
        }
    }

    public int CountLine(int row)
    {
        if (row < 0 || row > 19)
        {
            return -1;
        }

        int num = 0;
        for (int i = 0; i < 10; i++)
        {
            if (Cells[row,i] != null)
            {
                num++;
            }
        }

        return num;
    }

    public void SpawnCell(int row, int col)
    {
        Cell c = CellScene.Instance() as Cell;
        AddChild(c);
        c.Position = new Vector2(col * 25f, row * 25f);
        Cells[row,col] = c;
    }

    public void UpdateCell(Cell cell)
    {
        Vector2 pos = cell.GlobalPosition - GlobalPosition;
        int row = (int)(pos.y / 25f);
        int col = (int)(pos.x / 25f);

        if (row >= 0 && row <= 19 && col >=0 && col <=9)
        {
            cell.CanGoDown = row < 19;
            cell.CanGoLeft = col > 0;
            cell.CanGoRight = col < 9;

            if (row + 1 <= 19)
            {
                if (Cells[row + 1,col] != null) cell.CanGoDown = false;
            }
            
            if (col - 1 >= 0)
            {
                if (Cells[row,col - 1] != null) cell.CanGoLeft = false;
            }

            if (col + 1 <= 9)
            {
                if (Cells[row,col + 1] != null) cell.CanGoRight = false;
            }
            
        }
    }

    public void SpawnShape()
    {
        Shape shape = (Shape)ShapeScene.Instance();
        AddChild(shape);
        shape.Position = new Vector2(75f,-50f);
        
        shape.Connect("UpdateSignal", this, "UpdateCell");
        shape.Connect("NextShape", this, "CheckGameOver");
        shape.Connect("CheckRotations", this, "CheckRotate");

        int num = NextTetros.Dequeue();

        Random rand = new Random();
        NextTetros.Enqueue(rand.Next(7));

        shape.GetShape(num);
    }

    public void CheckRotate(Shape shape)
    {
        Vector2[] vec = shape.NextRotation();
        bool[] checkArray = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            Vector2 pos = shape.GlobalPosition - GlobalPosition;
            Vector2 v = vec[i] * 25f;
            Vector2 target = pos + v;
            bool result;

            int row = (int)(target.y / 25f);
            int col = (int)(target.x / 25f);

            if (row <= 19 && col >= 0 && col <= 9)
            {
                result = false;
            }
            else
            {
                result = true;
            }

            if (row >= 0 && row <= 19 && col >= 0 && col <= 9)
            {
                if (Cells[row,col] != null)
                {
                    result = true;
                }
            }
            checkArray[i] = result;
        }

        if (checkArray.Contains<bool>(true)) 
        {
            shape.CanRotate = false;
        }
        else
        {
            shape.CanRotate = true;
        }
    }

    public void RemoveShape(Shape shape)
    {
        foreach (Cell oldcell in shape.Cells)
        {
            Cell newcell = CellScene.Instance() as Cell;
            AddChild(newcell);
            newcell.GlobalPosition = oldcell.GlobalPosition;
            newcell.Modulate = oldcell.Modulate;
            Vector2 Pos = newcell.GlobalPosition - GlobalPosition;

            int row = (int)(Pos.y / 25f);
            int col = (int)(Pos.x / 25f);
            Cells[row,col] = newcell;
        }
        shape.QueueFree();
        ScanLines();
    }

    public void CheckGameOver(Shape shape)
    {
        foreach (Cell cell in shape.Cells)
        {
            Vector2 pos = cell.GlobalPosition - GlobalPosition;
            if (pos.y < 0)
            {
                GD.Print("Cell posY:" + cell.Position.y);
                DeclareGameOver();
                return;
            }
        }

        RemoveShape(shape);
    }

    public void DeclareGameOver()
    {
        EmitSignal("GameOver");
        GD.Print("OVER!");
    }

    public void RowCounter(int row)
    {
        LineCount[row] = LineCount[row] + 1;

        for (int i = 0; i < 20; i++)
        {
            GD.Print("row " + i + ":" + LineCount[i]);
        }
    }

}
