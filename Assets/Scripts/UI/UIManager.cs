using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainSceneLoad()
    {
        SceneManager.LoadScene("Sandbox_Niesso");
    }

    public void menuSceneLoad()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LeaveGame()
    {
        Application.Quit();
    }


}
