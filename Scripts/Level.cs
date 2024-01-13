using Godot;
using System;

public class Level : Node2D
{
    private Label XP;
    private Board Board;

    public override void _Ready()
    {
        base._Ready();
        XP = GetNode<Label>("XP");
        Board = GetNode<Board>("Board");

        Board.Connect()
    }
}
