using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class Board : Node2D
{
    private Timer Clock;

    private PackedScene Iscene;
    private PackedScene Jscene;
    private PackedScene Lscene;
    private PackedScene Zscene;
    private PackedScene Oscene;
    private PackedScene Tscene;
    private PackedScene Sscene;

    private PackedScene[] Packs = new PackedScene[7];
    private Shape activeShape;

    private bool hasShape;

    private Queue<int> shapeQ;

    public override void _Ready()
    {
        hasShape = false;

        shapeQ = new Queue<int>();
        Random randi = new Random();
        for (int i = 0; i < 7; i++)
        {
            shapeQ.Enqueue(randi.Next(7));
        }
        Clock = GetNode<Timer>("Clock");

        Iscene = ResourceLoader.Load<PackedScene>("res://Scenes/I.tscn");
        Zscene = ResourceLoader.Load<PackedScene>("res://Scenes/Z.tscn");
        Jscene = ResourceLoader.Load<PackedScene>("res://Scenes/J.tscn");
        Oscene = ResourceLoader.Load<PackedScene>("res://Scenes/O.tscn");
        Lscene = ResourceLoader.Load<PackedScene>("res://Scenes/L.tscn");
        Tscene = ResourceLoader.Load<PackedScene>("res://Scenes/T.tscn");
        Sscene = ResourceLoader.Load<PackedScene>("res://Scenes/S.tscn");

        Packs[0] = Iscene;
        Packs[1] = Zscene;
        Packs[2] = Jscene;
        Packs[3] = Oscene;
        Packs[4] = Lscene;
        Packs[5] = Tscene;
        Packs[6] = Sscene;

        Clock.Connect("timeout", this, "RunGame");
        Clock.Start();
    }

    private void NextShape()
    {
        hasShape = false;
    }

    private void RunGame()
    {
        Random rand = new Random();
        if (!hasShape)
        {
            hasShape = true;
            int shapeindex = shapeQ.Dequeue();
            shapeQ.Enqueue(rand.Next(7));
            SpawnShape(shapeindex);
        }
        else
        {
            activeShape.GoDown();
        }
    }

    private void SpawnShape(int randompick)
    {
        if (randompick < 0 || randompick > 6) return;

        activeShape = (Shape)Packs[randompick].Instance();
        AddChild(activeShape);
        activeShape.GlobalPosition = new Vector2(50f,50f);
    } 

    public void CheckAround(Cell cell)
    {
        // cell.UpdatePosition();

        Cell left = GetCell(cell.row, cell.col - 1);
        Cell right = GetCell(cell.row, cell.col + 1);
        Cell down = GetCell(cell.row + 1, cell.col);

        cell.CanGoLeft = left == null && cell.col > 0;
        cell.CanGoRight = right == null && cell.col < 9;
        cell.CanGoDown = down == null && cell.row < 19;
    }

    public Cell GetCell(int _row, int _col)
    {
        Godot.Collections.Array Cells;
        Cells = GetTree().GetNodesInGroup("Cells");
        // foreach (Cell cell in Cells)
        // {
        //     cell.UpdatePosition();
        //     if (cell.row == _row && cell.col == _col) return cell;
        // }
        return null;
    }


    private void CheckGameOver()
    {}

    private void DeclareGameOver()
    {}

    private void ScanLine()
    {}

    private void AddPoints()
    {}
   
}
