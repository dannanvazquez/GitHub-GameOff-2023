using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        // Toggle the visibility of the pause menu
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        // Pause or resume the game time based on the menu's visibility
        Time.timeScale = (pauseMenu.activeSelf) ? 0 : 1;
    }
}
