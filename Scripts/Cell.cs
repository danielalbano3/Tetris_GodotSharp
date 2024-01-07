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

}
