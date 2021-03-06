using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;
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
    public GameObject MainMenu;
    public bool attemptedConnection = false;
    public GameObject referenceToServerManagerSpawn;

    public string ConnectAddressStandalone;
    public string RelayAddressStandalone;
    public void StartAsHost()
    {
        /*
        Task listenTask = Task.Factory.StartNew(() =>
        {
            using (PuncherClient listener = new PuncherClient(PUNCHER_SERVER_HOST, PUNCHER_SERVER_PORT))
            {

                // 1234 is the port where the other peer will connect and punch through.
                // That would be the port where your program is going to be listening after the punch is done.
                listener.ListenForPunches(new IPEndPoint(IPAddress.Any, 1234));
            }
        });
        */
        NetworkManager.Singleton.StartHost();

    }

    public void JoinAsClient()
    {
        /*
        string address = ConnectAddress.GetComponent<Text>().text;
        Debug.Log(address);
        using (PuncherClient connector = new PuncherClient(PUNCHER_SERVER_HOST, PUNCHER_SERVER_PORT))
        {
            Debug.Log("attempting connect");
            // Punches and returns the result
            if (connector.TryPunch(IPAddress.Parse(address), out IPEndPoint remoteEndPoint))
            {
                NetworkManager.Singleton.gameObject.GetComponent<UNetTransport>().ConnectAddress = remoteEndPoint.Address.ToString();
                NetworkManager.Singleton.StartClient();
                MainMenu.gameObject.SetActive(false);
                Debug.Log("Punch Succeed"+ " " + remoteEndPoint.Address + ":" + remoteEndPoint.Port );

            }
              else
            {
                Debug.Log("Punch failed");
                // NAT Punchthrough failed.
            }
        }
        Debug.Log("After Punch");
        */

        if (attemptedConnection)
        {
            NetworkManager.Singleton.StopClient();
            NetworkManager.Singleton.GetComponent<UNetTransport>().Shutdown();
            attemptedConnection = false;
        }
        else
        {
            NetworkManager.Singleton.gameObject.GetComponent<UNetTransport>().ConnectAddress = ConnectAddressStandalone;
            NetworkManager.Singleton.gameObject.GetComponent<UNetTransport>().MLAPIRelayAddress = RelayAddressStandalone;
            NetworkManager.Singleton.StartClient();
            attemptedConnection = true;
        }
        
    }




}
