using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
    private int value = 0;

    public int Value
    {
        get => this.value;
        set => this.value = value;
    }

    private string teamName = "";
    public string TeamName
    {
        get => teamName;
        set => teamName = value;
    }

    

    public Score(int score, string teamName)
    {
        this.value = score;
        this.teamName = teamName;
    }

}
