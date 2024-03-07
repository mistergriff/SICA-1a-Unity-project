using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] Behaviour[] componentsToDisable;

    Camera sceneCamera;

    [SerializeField]
    private GameObject playerUIPrefab;
    private GameObject playerUIInstance;

    private void Start()
    {
        if(!isLocalPlayer)
        {
            //Disable other Player Components
            for(int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

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
        
    }
}
