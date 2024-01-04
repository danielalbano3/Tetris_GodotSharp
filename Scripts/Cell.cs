using Godot;
using System;

public class Cell : Sprite
{
    public int row;
    public int col;

    public bool CanGoLeft;
    public bool CanGoRight;
    public bool CanGoDown;

    [Signal] public delegate void DetectSignal(Cell cell);
    [Signal] public delegate void CellMoveSignal(Cell cell);

    public override void _Ready()
    {
        LookAround();
        CanGoDown = true;
        CanGoLeft = true;
        CanGoRight = true;
    }

    public void UpdateGridPosition(Vector2 gridOrigin)
    {
        Vector2 RelativePosition = GlobalPosition - gridOrigin;
        row = (int)RelativePosition.y;
        col = (int)RelativePosition.x;
    }

    public void MoveTo(float row, float col)
    {
        float drow = row * 25f;
        float dcol = col * 25f;
        
        Position = new Vector2(Position.x + drow, Position.y + dcol);
        EmitSignal("CellMoveSignal", this);
    }

    public void LookAround()
    {
        EmitSignal("DetectSignal", this);
    }
}
