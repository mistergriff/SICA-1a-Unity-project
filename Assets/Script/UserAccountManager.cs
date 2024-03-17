﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;

    public static string LoggedInUsername;

    public string lobbySceneName = "Lobby";

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public void LogIn(Text username)
    {
        LoggedInUsername = username.text;
        Debug.Log(LoggedInUsername);
        SceneManager.LoadScene(lobbySceneName);
    }
}