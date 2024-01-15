using Godot;
using System;
using System.Collections.Generic;

public class Level : Node2D
{
    private Label XP;
    private Label LevelLabel;
    private Board Board;

    private int Points;
    private int PlayerLevel;

    private Vector2[] LevelNeeds;
    private Label ExpPoints;
    private ProgressBar PBar;

    public override void _Ready()
    {
        base._Ready();
        LevelNeeds = new Vector2[20]
        {
            new Vector2 (100,100), 
            new Vector2 (210,310), 
            new Vector2 (290,600), 
            new Vector2 (325,925),
            new Vector2 (350,1275),
            new Vector2 (375,1650),
            new Vector2 (400,2050),
            new Vector2 (425,2475),
            new Vector2 (450,2925),
            new Vector2 (475,3400),
            new Vector2 (500,3900),
            new Vector2 (525,4425),
            new Vector2 (550,4975),
            new Vector2 (575,5550),
            new Vector2 (679,6229),
            new Vector2 (704,6933),
            new Vector2 (729,7662),
            new Vector2 (754,8416),
            new Vector2 (779,9195),
            new Vector2 (804,9999)
        };
        
        Points = 0;
        PlayerLevel = 0;
        XP = GetNode<Label>("XP");
        PBar = GetNode<ProgressBar>("VBoxContainer/ProgressBar");
        LevelLabel = GetNode<Label>("VBoxContainer/ColorRect/LevelLabel");
        Board = GetNode<Board>("Board");
        ExpPoints = GetNode<Label>("VBoxContainer/ProgressBar/ExpPoints");

        Board.Connect("ComboSignal", this, "ProcessPoints");
        Board.Connect("GameOver", this, "GameOverEnd");

        UpgradeLevel();
    }

    public void GameOverEnd()
    {}

    public void ProcessPoints(int combo, int row)
    {
        int bonus = 100;
        switch (combo)
        {
            case 1:
                bonus = 100;
                break;
            case 2:
                bonus = 150;
                break;
            case 3:
                bonus = 200;
                break;
            case 4:
                bonus = 250;
                break;
            default:
                break;
        }
        Points += combo * bonus;
        UpgradeLevel();
    }


    public void UpgradeLevel()
    {
        while (Points >= LevelNeeds[PlayerLevel].y) PlayerLevel++;
        GD.Print("Level: " + PlayerLevel);

        UpdateLevelLabel(PlayerLevel);

        int denominator = (int)LevelNeeds[PlayerLevel].x;
        int numerator = Points % denominator;
        UpdateExp(numerator,denominator);

        PBar.MaxValue = denominator;
        PBar.Value = numerator;
    }

    public void UpdateExp(int numerator, int denominator)
    {
        ExpPoints.Text = numerator + " / " + denominator;
    }

    public void UpdateLevelLabel(int lvl)
    {
        LevelLabel.Text = "Level " + lvl;
    }

}
