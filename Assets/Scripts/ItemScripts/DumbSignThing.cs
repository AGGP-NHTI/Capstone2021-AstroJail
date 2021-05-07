using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DumbSignThing : MonoBehaviour
{

    public string[] accusations, crimes, duration;
    public List<string> prisoners;
    public TextMeshPro text;
    float timer = 3;
    int randomPrisoner, randomAccu, randomCrime, randomDuration;

    // Start is called before the first frame update
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            ActivateText();
            Destroy(this);
        }
    }

    void ActivateText()
    {
        foreach (PlayerController player in GameObject.FindObjectsOfType<PlayerController>())
        {
            if (player.selectedPlayerType == PlayerType.Prisoner)
            {
                prisoners.Add(player.playerName.Value);
            }
        }
        randomPrisoner = Random.Range(0, prisoners.Count);
        randomAccu = Random.Range(0, accusations.Length);
        randomCrime = Random.Range(0, crimes.Length);
        randomDuration = Random.Range(0, duration.Length);
        string output = prisoners[randomPrisoner] + " " + accusations[randomAccu] + " " + crimes[randomCrime] + ". Duration of Sentence: " + duration[randomDuration];
        Debug.Log(text);
        text.SetText(output);

    }

}
