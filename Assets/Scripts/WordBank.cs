using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordBank : MonoBehaviour
{
    private static List<string> fireWords = new List<string>()
    {
        "water", "AC", "aircon", "oxygen", "trees"
    };

    private static List<string> waterWords = new List<string>()
    {
        "arm-floaties", "lifeguard", "swimming skill"
    };

    private static List<string> gateWords = new List<string>()
    {
        "open", "build", "pass"
    };

    private static List<string> workingWords = new List<string>();

    /*private void Awake()
    {
        workingWords.AddRange(Words);
        Shuffle(workingWords);
    }*/

    private static void Shuffle(List<string> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            int random = Random.Range(i, list.Count);
            string temp = list[i];
            list[i] = list[random];
            list[random] = temp;
        }
    }

    public string GetWord()
    {
        string newWord = string.Empty;
        if(workingWords.Count != 0)
        {
            newWord = workingWords.Last();
            // Don't remove words from list
            workingWords.Remove(newWord);
        }
        return newWord;
    }

    public static string PickWord(RealmManager realm, int difficultyLevel) // difficultyLevel - {1, 2, 3}
    {
        switch(realm.currentRealm)
        {
            case(RealmManager.Realm.FIRE):
            {
                workingWords = fireWords;
                break;
            }
            case(RealmManager.Realm.WATER):
            {
                workingWords = waterWords;
                break;
            }
            case(RealmManager.Realm.GATE):
            {
                workingWords = gateWords;
                break;
            }
        }
            
        Shuffle(workingWords);
        string newWord = string.Empty;
        if(workingWords.Count != 0)
        {
            newWord = workingWords.Last();
            // Don't remove words from list
            //workingWords.Remove(newWord);
        }
        return newWord;
    }

    public static string PickGateWord()
    {
        workingWords = gateWords;
        Shuffle(workingWords);
        string newWord = string.Empty;
        if(workingWords.Count != 0)
        {
            newWord = workingWords.Last();
            // Don't remove words from list
            //workingWords.Remove(newWord);
        }
        return newWord;
    }
}
