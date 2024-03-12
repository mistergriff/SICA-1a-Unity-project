using Mirror;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar]
    private float currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;

    [SerializeField]
    private GameObject[] disableGameObjectOnDeath;
    private bool[] wasEnabledOnStart;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    public void Setup()
    {
        wasEnabledOnStart = new bool[disableOnDeath.Length];
        for(int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.MatchSettings.respawnTimer);
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        SetDefaults();
    }

    public void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999);
        }
#endif
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        //Réactivation des script du joueur
        for(int i = 0;i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }

        //Réactivation des gameobjects du joueurs
        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(true);
        }

        //Réactivation du collider du joueur
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        //Changement de camera
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        //Apparition du système de particule d'apparition
        GameObject _gfxIns = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    }

    [ClientRpc]
    public void RpcTakeDamage(float amount)
    {
        if(isDead)
        {
            return;
        }

        currentHealth -= amount;
        Debug.Log(transform.name + " a maintenant: " + currentHealth + " points de vie.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        for(int i = 0;i < disableOnDeath.Length;i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(false);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        //Apparition du système de particule de mort
        GameObject _gfxIns = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        //Changement de camera
        if(isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + "a été éliminé");

        StartCoroutine(Respawn());
    }
}
