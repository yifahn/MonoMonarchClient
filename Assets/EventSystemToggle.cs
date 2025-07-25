using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventSystemToggle : MonoBehaviour
{
    public string sceneToWatch = "MonoMonarch";

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneToWatch)
        {
            // Disable this EventSystem when MonoMonarch is loaded
            gameObject.SetActive(false);
        }
        else if (scene.name == "MainMenu")
        {
            // Enable this EventSystem when MainMenu is loaded
            gameObject.SetActive(true);
        }
    }
}
