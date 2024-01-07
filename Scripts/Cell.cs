using Godot;
using System;

public class Cell : Sprite
{
    public bool CanGoLeft;
    public bool CanGoRight;
    public bool CanGoDown;

    public override void _Ready()
    {
        CanGoDown = true;
        CanGoLeft = true;
        CanGoRight = true;
    }

    public void CellTo(float y, float x)
    {
        float addx = x * 25f;
        float addy = y * 25f;    

        Position = new Vector2(addx,addy);
    }
}
