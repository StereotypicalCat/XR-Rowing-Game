using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Highscore : MonoBehaviour
{

    public TextAsset HighscoresTextDocument;
    public int scoresToKeep = 10;

    private Score[] highscores; 
    
    private void Start()
    {
        // It is assumed that all previously stored scores were saved and sorted.   
        highscores = new Score[scoresToKeep];
        
        
        var HighscoresTextDocumentWithoutCarriageReturn =
            HighscoresTextDocument.text.Replace(System.Environment.NewLine, "\n");


        var highscoresTextSplit = HighscoresTextDocumentWithoutCarriageReturn.Split('\n');


        for (int i = 0; i < scoresToKeep; i++)
        {
            // Test if there is a score
            if (highscoresTextSplit.Length - 1 >= i)
            {
                // String should look like this at this point: "10:Team Green Panda
                var scoreNamePair = highscoresTextSplit[i].Split(':');
                var score = Int32.Parse(scoreNamePair[0]);
                var teamName = scoreNamePair[1];
                
                highscores[i] = new Score(score, teamName);
                
                
            }
            
            
        }
    }

    public Score[] getScores()
    {
        return highscores;
    }

    public void addNewScores(Score newScore)
    {
        for (int i = highscores.Length; newScore.Value < highscores[i].Value; i--)
        {
            // Case its the first score
            if (i == 0)
            {
                if (highscores.Length == scoresToKeep)
                {
                    highscores[i] = newScore;
                }
                else
                {
                    continue;
                }
                
                
            }
            // Case its now the top score
            else
            {
                var temp = highscores[i];
                highscores[i] = newScore;
                highscores[i - 1] = temp;


            }
            
            // Case its in the "middle"
            
        }
        
        
    }
    
    
    
}
