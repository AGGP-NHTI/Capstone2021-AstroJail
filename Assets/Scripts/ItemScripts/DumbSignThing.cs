using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DumbSignThing : MonoBehaviour
{

    public List<string> accusations, crimes, duration;
    public TextMeshPro text;
    float timer = 3;
    int randomAccu, randomCrime, randomDuration;

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

            }
        }
        text.text = "This Prisoner " + accusations + " " + crimes + ". Duration of Sentence:" + duration;
        Destroy(this);
    }

}
