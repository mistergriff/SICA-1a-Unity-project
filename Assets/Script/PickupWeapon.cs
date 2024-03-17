using System.Collections;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] WeaponData theWeapon;

    [SerializeField] float delay = 30f;

    private GameObject pickupGraphics;
    private bool canPickup;
 
    void Start()
    {
        ResetWeapon();
    }

    private void ResetWeapon()
    {
        canPickup = true;
        pickupGraphics = Instantiate(theWeapon.graphics, transform);
        pickupGraphics.transform.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canPickup)
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            EquipNewWeapon(weaponManager);
        }
    }

    private void EquipNewWeapon(WeaponManager weaponManager)
    {
        // Retire l'arme actuel des main du joueur
        Destroy(weaponManager.GetCurrentGraphics().gameObject);

        // Equipe la nouvelle arme
        weaponManager.EquipWeapon(theWeapon);

        canPickup = false;
        Destroy(pickupGraphics);

        StartCoroutine(DelayResetBonus());
    }

    IEnumerator DelayResetBonus()
    {
        yield return new WaitForSeconds(delay);
        ResetWeapon();
    }
}
