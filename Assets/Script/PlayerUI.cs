using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuelFill;

    /*
    [SerializeField]
    private RectTransform healthBarFill;
    */

    //private Player player;
    private PlayerController controller;
    //private WeaponManager weaponmaganer;

    /*
    [SerializeField]
    private Text ammoText;
    */

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject scoreboard;


    /*
    public void SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerController>();
        //weaponManager = player.GetComponent<WeaponManager>();
    }
    */

    public void SetController(PlayerController _controller)
    {
        controller = _controller;
    }

    void Start()
    {
        PauseMenu.isOn = false;
    }

    void Update()
    {
        SetFuelAmount(controller.GetThrusterFuelAmount());
        //SetHealthAmount(player.GetHealthPct());
        //Mettre les currentAmmo SetAmmoAmount(weaponManager.currentMagazineSize); 

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
            
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }

    void SetFuelAmount(float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }

    /*
    void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(1f, _amount, 1f);
    }
    

    void SetAmmoAmount(int _amount)
    {
        ammoText.text = _amount.ToString();
    }
    */
}
