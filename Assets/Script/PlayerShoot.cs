using UnityEngine;
using Mirror;
using System.Collections;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private WeaponData currentWeapon;
    private WeaponManager weaponManager;

    [SerializeField] private GameObject tracerEffect;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("Pas de caméra renseignée sur le système de tir.");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (PauseMenu.isOn)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.R) && weaponManager.currentMagazineSize < currentWeapon.magazineSize)
        {
            StartCoroutine(weaponManager.Reload());
            return;
        }

        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }

    }

    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal, string tag)
    {
        RpcDoHitEffect(pos, normal, tag);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal, string tag)
    {
        var weaponGraphics = weaponManager.GetCurrentGraphics();
        if (weaponGraphics == null)
        {
            Debug.LogError("Les graphismes de l'arme sont null.");
            return;
        }

        if (weaponGraphics.hitEffectPrefab == null)
        {
            Debug.LogError("hitEffectPrefab est null sur les graphismes de l'arme.");
            return;
        }

        // Ne pas faire apparaitre les trou de mur sur les joueurs | Ajouter un switch si plus de particules a faire.
        if(tag == "Player" || tag == "Bonus")
        {
            GameObject hitEffect = Instantiate(weaponGraphics.hitEffectPrefab, pos, Quaternion.LookRotation(normal));
            Destroy(hitEffect, 10f);
        }
        else
        {
            GameObject hitEffect = Instantiate(weaponGraphics.hitWallEffectPrefab, pos, Quaternion.LookRotation(normal));
            Destroy(hitEffect, 10f);
        }
    }

    
    // Fonction appelée sur le serveur lorsque notre joueur tir (On prévient le serveur de notre tir)
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }
    
    // Fait apparaitre les effets de tir chez tous les clients / joueurs
    [ClientRpc]
    void RpcDoShootEffect()
    {
        var weaponGraphics = weaponManager.GetCurrentGraphics();
        if (weaponGraphics == null)
        {
            Debug.LogError("Les graphismes de l'arme sont null.");
            return;
        }

        if (weaponGraphics.muzzleFlash == null)
        {
            Debug.LogError("muzzleFlash est null sur les graphismes de l'arme.");
            return;
        }

        weaponGraphics.muzzleFlash.Play();
    }

    // Cette fonction est appelée sur le client, mais exécute la commande sur le serveur
    [Command]
    void CmdSendTracer(Vector3 start, Vector3 end)
    {
        RpcShowTracer(start, end);
    }

    // Cette fonction est appelée sur le serveur, mais exécute l'effet sur tous les clients
    [ClientRpc]
    void RpcShowTracer(Vector3 start, Vector3 end)
    {
        StartCoroutine(ShowTracer(start, end));
    }

    private IEnumerator ShowTracer(Vector3 start, Vector3 end)
    {
        if (tracerEffect != null)
        {
            GameObject tracerInstance = Instantiate(tracerEffect, start, Quaternion.identity); // Créez une instance de l'effet de traînée
            LineRenderer lr = tracerInstance.GetComponent<LineRenderer>(); // Obtenez le TrailRenderer de l'instance

            if (lr != null)
            {
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);

                yield return new WaitForSeconds(0.02f); // La durée pendant laquelle le tracer est visible

                Destroy(tracerInstance); // Détruisez l'instance après l'affichage
            }
        }
    }


    [Client]
    private void Shoot()
    {
        if (!isLocalPlayer || weaponManager.isReloading)
        {
            return;
        }

        if(weaponManager.currentMagazineSize <= 0)
        {
            StartCoroutine(weaponManager.Reload());
            return;
        }

        weaponManager.currentMagazineSize--;

        Debug.Log("Il reste " +  weaponManager.currentMagazineSize);

        CmdOnShoot();

        Vector3 start = cam.transform.position;
        Vector3 end = start + cam.transform.forward * currentWeapon.range;
        RaycastHit hit;

        if (Physics.Raycast(start, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            end = hit.point; // Si le raycast touche un objet, ajustez le point de fin au point d'impact
            if (hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage, transform.name);
            }

            CmdOnHit(hit.point, hit.normal, hit.collider.tag);
        }

        CmdSendTracer(start, end); // Envoyez les positions de départ et de fin pour afficher le tracer
    }

    [Command]
    private void CmdPlayerShot(string playerId, float damage, string sourceID)
    {
        Debug.Log(playerId + " a été touché.");

        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage, sourceID);
    }

}