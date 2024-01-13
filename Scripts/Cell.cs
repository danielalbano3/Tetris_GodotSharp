using Godot;
using System;

public class Cell : Sprite
{
    public bool CanGoLeft;
    public bool CanGoRight;
    public bool CanGoDown;

    public int row;
    public int col;

    public override void _Ready()
    {
        base._Ready();
        CanGoDown = true;
        CanGoLeft = true;
        CanGoRight = true;
        UpdateCell();
    }

    public void CellTo(float y, float x)
    {
        float addx = x * 25f;
        float addy = y * 25f;    

        Position = new Vector2(addx,addy);
    }

    public void UpdateCell()
    {
        row = (int)(Position.y / 25f);
        col = (int)(Position.x / 25f);
    }

}
