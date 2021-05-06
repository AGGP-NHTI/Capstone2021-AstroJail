using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DumbSignThing : MonoBehaviour
{

    public List<string> prisoners, accusations, crimes, duration;
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
        }
    }

    void ActivateText()
    {
        foreach (PlayerController player in GameObject.FindObjectsOfType<PlayerController>())
        {
            if (player.selectedPlayerType == PlayerType.Prisoner)
            {
                prisoners.Add(player.name);
            }
        }
        randomPrisoner = Random.Range(0, prisoners.Count);
        randomAccu = Random.Range(0, accusations.Count);
        randomCrime = Random.Range(0, crimes.Count);
        randomDuration = Random.Range(0, duration.Count);
        text.text = prisoners[randomPrisoner] + " " + accusations[randomAccu] + " " + crimes [randomCrime] + ". Duration of Sentence:" + duration[randomDuration];
        Destroy(this);
    }

}
