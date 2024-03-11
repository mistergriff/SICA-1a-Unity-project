using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PauseMenu : NetworkBehaviour
{
    public static bool isOn = false;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void LeaveRoomBtn()
    {
        if(isClientOnly)
        {
            networkManager.StopClient(); //Seul le client quitte
        }
        else
        {
            networkManager.StopHost(); //L'hôte quitte donc tous les joueurs sont déconnectés
        }
    }
}
