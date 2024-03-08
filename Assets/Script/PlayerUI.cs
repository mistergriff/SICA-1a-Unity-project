using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuellFill;

    private PlayerController controller;

    [SerializeField]
    private GameObject pauseMenu;


    public void SetFuelController(PlayerController _controller)
    {
        controller = _controller;
    }

    void Start()
    {
        PauseMenu.isOn = false;
    }

    void Update()
    {
        //SetFuelController(controller.GetThrusterFuelAmount());

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }

    void SetFuelAmount(float _amount)
    {
        thrusterFuellFill.localScale = new Vector3(1f, _amount, 1f);
    }
}
