using Godot;
using System;
using System.Collections.Generic;

public class Board : Node2D
{
    private Cell[,] CellGrid;

    private PackedScene ShapeScene;


    public override void _Ready()
    {
        base._Ready();
        ShapeScene = ResourceLoader.Load<PackedScene>("res://Scenes/Shape.tscn");
        CellGrid = new Cell[10,20];

        SpawnShape();

    }

    public void SpawnShape()
    {
        Shape shape = (Shape)ShapeScene.Instance();
        AddChild(shape);
        shape.Connect("UpdateSignal", this, "GridReport");
        shape.GetShape(6);
    }

    public void GridReport(Cell cell)
    {
        Vector2 pos = cell.GlobalPosition - GlobalPosition;
        int x = (int)pos.x;
        int y = (int)pos.y;


    }
}
