using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    [Header("Mixer")]
    [SerializeField] private AudioMixer myAudioMixer;
    [Range(200,8000)][SerializeField] private float muffled=3000;
    [Range(17000,22000)][SerializeField] private float not_muffled=22000;
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
        bool isMenuActive = !pauseMenu.activeSelf;
        pauseMenu.SetActive(isMenuActive);

        // Pause or resume the game time based on the menu's visibility
        Time.timeScale = isMenuActive ? 0 : 1;
        Cursor.lockState = isMenuActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isMenuActive ? true : false;

        // Pause or resume the game time based on the menu's visibility
        Time.timeScale = (pauseMenu.activeSelf) ? 0 : 1;
        myAudioMixer.SetFloat("MUFFLED_SOUND", isMenuActive ? muffled : not_muffled);
    }
}
