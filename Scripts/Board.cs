using Godot;
using System;
using System.Collections.Generic;

public class Board : Node2D
{
    private PackedScene CellScene;
    private PackedScene Iscene;
    private PackedScene Zscene;
    private PackedScene Jscene;
    private PackedScene Oscene;
    private PackedScene Lscene;
    private PackedScene Tscene;
    private PackedScene Sscene;
    private PackedScene[] Packs; 

    private Cell[,] GridSpaces;

    private Shape activeShape;

    private Queue<int> shapeQ;

    public override void _Ready()
    {
        GridSpaces = new Cell[20,10];

        shapeQ = new Queue<int>();

        Random randi = new Random();
        for (int i = 0; i < 7; i++)
        {
            shapeQ.Enqueue(randi.Next(7));
        }
        
        CellScene = ResourceLoader.Load<PackedScene>("res://Scenes/Cell.tscn");
        Iscene = ResourceLoader.Load<PackedScene>("res://Scenes/I.tscn");
        Zscene = ResourceLoader.Load<PackedScene>("res://Scenes/Z.tscn");
        Jscene = ResourceLoader.Load<PackedScene>("res://Scenes/J.tscn");
        Oscene = ResourceLoader.Load<PackedScene>("res://Scenes/O.tscn");
        Lscene = ResourceLoader.Load<PackedScene>("res://Scenes/L.tscn");
        Tscene = ResourceLoader.Load<PackedScene>("res://Scenes/T.tscn");
        Sscene = ResourceLoader.Load<PackedScene>("res://Scenes/S.tscn");

        Packs = new PackedScene[7]{Iscene,Zscene,Jscene,Oscene,Lscene,Tscene,Sscene};
        
        RunGame();
    }

    private void OccupyGrid(Cell c1, Cell c2, Cell c3, Cell c4)
    {
        Cell[] _cells = new Cell[4]{c1,c2,c3,c4};

        foreach (Cell cell in _cells)
        {
            Vector2 CellPos = cell.GlobalPosition - GlobalPosition;
            int xcol = (int)(CellPos.x / 25f);
            int yrow = (int)(CellPos.y / 25f);

            if (yrow < 0)
            {
                DeclareGameOver();
                return;
            }

            GridSpaces[yrow,xcol] = CloneCell(cell);
        }

        activeShape.QueueFree();
        RunGame();
    }

    private Cell CloneCell(Cell cell)
    {
        Vector2 CellPos = cell.GlobalPosition - GlobalPosition;
        int xcol = (int)(CellPos.x / 25f);
        int yrow = (int)(CellPos.y / 25f);

        Cell newcell = (Cell)CellScene.Instance();
        AddChild(newcell);
        newcell.GlobalPosition = cell.GlobalPosition;
        newcell.Modulate = cell.Modulate;
        
        return newcell;
    }

    private void RunGame()
    {
        Random rand = new Random();
        
        int shapeindex = shapeQ.Dequeue();
        shapeQ.Enqueue(rand.Next(7));
        SpawnShape(shapeindex);
    }

    private void SpawnShape(int randompick)
    {
        activeShape = (Shape)Packs[randompick].Instance();
        AddChild(activeShape);
        activeShape.GlobalPosition = new Vector2(100f,-50f);
        activeShape.Connect("RequestUpdateSignal", this, "UpdateCell");
        activeShape.Connect("NextShapeSignal", this, "OccupyGrid");
        activeShape.Connect("SeeAroundSignal", this, "CheckGrid");
    }   

    public void CheckGrid(Vector2 vec)
    {
        Vector2 ShapePos = activeShape.Position;
        int x = (int)(ShapePos.x / 25f) + (int)vec.x;
        int y = (int)(ShapePos.y / 25f) + (int)vec.y;

        if (x < 0 || x > 9 || y < 0 || y > 19) 
        {
            activeShape.AroundList.Add(false);
            return;
        }

        if (GridSpaces[y,x] == null)
        {
            activeShape.AroundList.Add(true);
        }
        else
        {
            activeShape.AroundList.Add(false);
        }
    }

    private int[] QtoArray()
    {
        int[] qArr = shapeQ.ToArray();
        return qArr;
    }

    public void UpdateCell(Cell c1, Cell c2, Cell c3, Cell c4)
    {
        Cell[] _cells = new Cell[4]{c1,c2,c3,c4};

        foreach (Cell cell in _cells)
        {
            Vector2 CellPos = cell.GlobalPosition - GlobalPosition;
            int xcol = (int)(CellPos.x / 25f);
            int yrow = (int)(CellPos.y / 25f);
            cell.CanGoDown = GetCell(yrow + 1,xcol) == null && CellPos.y < 475f;
            cell.CanGoLeft = GetCell(yrow,xcol - 1) == null && CellPos.x > 0f;
            cell.CanGoRight = GetCell(yrow,xcol + 1) == null && CellPos.x < 225f;
        }
    }

    public Cell GetCell(int _yrow, int _xcol)
    {
        if (_yrow >= 20 || _yrow <= 0 || _xcol >= 10 || _xcol <= 0)
        {
            return null;
        }
        else
        {
            return GridSpaces[_yrow,_xcol];
        }
    }

    private void DeclareGameOver()
    {
        GD.Print("Game Over!");
    }

    private void ScanLine()
    {}

    private void AddPoints()
    {}
   
}
