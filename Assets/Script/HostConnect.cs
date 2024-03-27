using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class HostConnect : MonoBehaviour
{
    NetworkManager networkManager;
    public InputField ipInput;
    public GameObject HostConnectGo;

    void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    public void HostFunction()
    {
        networkManager.StartHost();

        HostConnectGo.SetActive(false);
    }

    public void ConnectFunction()
    {
        networkManager.networkAddress = ipInput.text;
        networkManager.StartClient();

        HostConnectGo.SetActive(false);
    }

    public void ServerFunction()
    {
        networkManager.StartServer();

        HostConnectGo.SetActive(false);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
