﻿using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    [SerializeField]
    private string dontDrawLayerName = "DontDraw";

    Camera sceneCamera;

    [SerializeField]
    private GameObject playerGraphics;

    [SerializeField]
    private GameObject playerUIPrefab;
    private GameObject playerUIInstance;

    private void Start()
    {
        if(!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            // Désactiver la partie graphique du joueur local
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            // Création du UI du joueur local
            playerUIInstance = Instantiate(playerUIPrefab);
        }

        GetComponent<Player>().Setup();
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId, player);
    }
    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void DisableComponents()
    {
        //Disable other Player Components
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
            //Creation du UI du joueur local
            playerUIInstance = Instantiate(playerUIPrefab);
        }
    }

    private void OnDisable()
    {
        Destroy(playerUIInstance);

        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
        
    }
}
