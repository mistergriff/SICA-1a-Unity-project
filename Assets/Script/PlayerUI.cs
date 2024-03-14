﻿using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuellFill;

    private PlayerController controller;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject scoreboard;


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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
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
        thrusterFuellFill.localScale = new Vector3(1f, _amount, 1f);
    }
}
