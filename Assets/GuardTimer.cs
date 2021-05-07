using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuardTimer : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = ServerManager.Instance.guardWinTimer.ToString("0");
    }

}
