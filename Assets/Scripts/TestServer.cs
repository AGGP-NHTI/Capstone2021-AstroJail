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
        NetworkingManager.Singleton.StartHost();
        gameObject.SetActive(false);
    }

    public void JoinAsClient()
    {
        string address = ConnectAddress.GetComponent<Text>().text;

            using (PuncherClient connectPeer = new PuncherClient(PUNCHER_SERVER_HOST, PUNCHER_SERVER_PORT))
            {
                System.Console.WriteLine("[CONNECTOR] Punching...");

                if (connectPeer.TryPunch(IPAddress.Parse(address), out IPEndPoint connectResult))
                {
                    System.Console.WriteLine("[CONNECTOR] Punched through to peer: " + connectResult);
                }
                else
                {
                    System.Console.WriteLine("[CONNECTOR] Failed to punch");
                }
            }
        
        NetworkingManager.Singleton.StartClient();
        gameObject.SetActive(false);
    }




}
