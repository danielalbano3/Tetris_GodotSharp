using Godot;
using System;

public class NextShapesDisplay : Node2D
{
    private AnimatedSprite SA1;
    private AnimatedSprite SA2;
    private AnimatedSprite SA3;
    private AnimatedSprite SA4;
    private AnimatedSprite SA5;

    private Node2D ShapesBox;

    public override void _Ready()
    {
        base._Ready();
        ShapesBox = GetNode<Node2D>("ShapesBox");
        SA1 = GetNode<AnimatedSprite>("ShapesBox/SA1");
        SA2 = GetNode<AnimatedSprite>("ShapesBox/SA2");
        SA3 = GetNode<AnimatedSprite>("ShapesBox/SA3");
        SA4 = GetNode<AnimatedSprite>("ShapesBox/SA4");
        SA5 = GetNode<AnimatedSprite>("ShapesBox/SA5");

    }

    public void SetShapes(int a, int b, int c, int d, int e)
    {
        SA1.Animation = ShapeName(a);
        SA2.Animation = ShapeName(b);
        SA3.Animation = ShapeName(c);
        SA4.Animation = ShapeName(d);
        SA5.Animation = ShapeName(e);
    }

    public string ShapeName(int num)
    {
        string result = "";
        switch (num)
        {
            case 0:
                result = "J";
                break;
            case 1:
                result = "O";
                break;
            case 2:
                result = "L";
                break;
            case 3:
                result = "T";
                break;
            case 4:
                result = "S";
                break;
            case 5:
                result = "I";
                break;
            case 6:
                result = "Z";
                break;
            default:
                break;
        }

        return result;
    }
}
