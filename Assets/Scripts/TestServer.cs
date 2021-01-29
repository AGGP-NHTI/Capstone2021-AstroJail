using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Puncher.Client;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public class TestServer : MonoBehaviour
{
    public const string PUNCHER_SERVER_HOST = "18.216.176.155";
    public const int PUNCHER_SERVER_PORT = 6776;
    public GameObject ConnectAddress;
    public void StartAsHost()
    {

        Debug.Log("1");
        Task listenTask = Task.Factory.StartNew(() =>
        {
            Debug.Log("2");

            using (PuncherClient listener = new PuncherClient(PUNCHER_SERVER_HOST, PUNCHER_SERVER_PORT))
            {

                // 1234 is the port where the other peer will connect and punch through.
                // That would be the port where your program is going to be listening after the punch is done.
                listener.ListenForPunches(new IPEndPoint(IPAddress.Any, 1234));
                Debug.Log("3");

            }
            Debug.Log("4");
        });
        Debug.Log("5");
        gameObject.SetActive(false);
        NetworkingManager.Singleton.StartHost();
    }

    public void JoinAsClient()
    {
        string address = ConnectAddress.GetComponent<Text>().text;

        using (PuncherClient connector = new PuncherClient("puncher.midlevel.io", 6776))
        {
            // Punches and returns the result
            if (connector.TryPunch(IPAddress.Parse(address), out IPEndPoint remoteEndPoint))
            {
                // NAT Punchthrough was successful. It can now be connected to using your normal connection logic.
                NetworkingManager.Singleton.StartClient();
                gameObject.SetActive(false);
            }
              else
            {
                // NAT Punchthrough failed.
            }
        }


    }




}
