using Godot;
using System;
using System.Collections.Generic;

public class Level : Node2D
{
    private Label LevelLabel;
    private Board Board;

    private int Points;
    private int PlayerLevel;

    private int[] LevelNeeds;
    private Label ExpPoints;
    private ProgressBar PBar;

    private Resource PB0;
    private Resource PB25;
    private Resource PB50;
    private Resource PB75;

    public override void _Ready()
    {
        base._Ready();
        LevelNeeds = new int[10]
        {
            100,200,400,800,1600,3200,6400,12800,25600,51200
        };

        PB0 = ResourceLoader.Load<Theme>("res://Themes/PB_0.tres");
        PB25 = ResourceLoader.Load<Theme>("res://Themes/PB_25.tres");
        PB50 = ResourceLoader.Load<Theme>("res://Themes/PB_50.tres");
        PB75 = ResourceLoader.Load<Theme>("res://Themes/PB_75.tres");
        
        Points = 0;
        PlayerLevel = 0;
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
        bool notMax = true;
        while (Points >= LevelNeeds[PlayerLevel] && notMax)
        {
            Points -= LevelNeeds[PlayerLevel];

            PlayerLevel++;
            UpdateLevelLabel(PlayerLevel);
            if (PlayerLevel == LevelNeeds.Length) 
            {
                DeclareWin();
                notMax = false;
            }
        }

        UpdateExp(Points,LevelNeeds[PlayerLevel]);
        
        PBar.Value = Points;
        PBar.MaxValue = LevelNeeds[PlayerLevel];

        float prog = (float)(PBar.Value / PBar.MaxValue);
        
        if (prog < 0.25f) PBar.Theme = (Theme)PB0;
        if (prog >= 0.25f && prog < 0.5f) PBar.Theme = (Theme)PB25;
        if (prog >= 0.5f && prog < 0.75f) PBar.Theme = (Theme)PB50;
        if (prog >= 0.75f && prog <= 1f) PBar.Theme = (Theme)PB75;

    }

    public void DeclareWin()
    {

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
