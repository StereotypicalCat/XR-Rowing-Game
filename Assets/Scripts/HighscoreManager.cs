using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{

    public TextAsset subjectivesInTxT;
    public TextAsset adjectivesInTxT;
    public TextAsset forFunSubjectivesTxT;

    public List<string> subjectives;
    public List<string> adjectives;
    public List<string> ForFunSubjectives;

    public bool forFunNamesEnabled = false;
    public float forFunChance = 0.5f;
    
    
    void Start()
    {
        
        print("SUBJECTIVES: " + subjectivesInTxT.text);
        
        
        subjectives = handleTXT(subjectivesInTxT);
        adjectives = handleTXT(adjectivesInTxT);
        ForFunSubjectives = handleTXT(forFunSubjectivesTxT);
  
    }

    // Update is called once per frame
    public string generateTeamName()
    {
        var adjective = "";
        var subjective = "";

        var adjectiveIndex = (int) Mathf.Floor(UnityEngine.Random.value * adjectives.Count);
        adjective = adjectives[adjectiveIndex];

        if (forFunNamesEnabled && (forFunChance > UnityEngine.Random.value))
        {
            var subjectiveIndex = (int) Mathf.Floor(UnityEngine.Random.value * ForFunSubjectives.Count);
            subjective = ForFunSubjectives[subjectiveIndex];
        }
        else
        {
            var subjectiveIndex = (int) Mathf.Floor(UnityEngine.Random.value * subjectives.Count);
            subjective = subjectives[subjectiveIndex];    
        }

        return $"Team {adjective} {subjective}";
    }


    private List<string> handleTXT(TextAsset textDocument)
    {
        // Get rid of those bleeping carriage returns.
        var copyOfTextWithoutCarriageReturns = textDocument.text.Replace(System.Environment.NewLine, "\n");
        
        
        var TXTSpliAtNewline = copyOfTextWithoutCarriageReturns.Split('\n');
        var returnList = new List<string>();

        foreach (var Word in TXTSpliAtNewline)
        {
            if (String.IsNullOrEmpty(Word) || Word.StartsWith("#"))
            {
                continue;
            }
            else
            {
                returnList.Add(Word);
            }

        }

        return returnList;
    }
    

}
