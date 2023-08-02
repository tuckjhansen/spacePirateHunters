using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool paused = false;
    public GameObject pauseMenu;
    public GameObject mainPauseMenu;
    public GameObject helpPauseMenu;
    public GameObject bindingsMenu;
    public GameObject feedbackMenu;
    public GameObject mainHelpMenu;
    public InputActionReference pause;


    void Update()
    {
        if (!paused && !Inventory.inventoryOpen)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;

        }
        if (paused && !Inventory.inventoryOpen)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        if (pause.action.ReadValue<float>() != 0)
        {
            paused = true;
        }
    }
    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Resume()
    {
        paused = false;
    }
    public void Help()
    {
        mainPauseMenu.SetActive(false);
        helpPauseMenu.SetActive(true);
    }
    public void Back()
    {
        mainPauseMenu.SetActive(true);
        helpPauseMenu.SetActive(false);
    }
    public void Feedback()
    {
        feedbackMenu.SetActive(true);
        mainHelpMenu.SetActive(false);
    }
    public void Bindings()
    {
        bindingsMenu.SetActive(true);
        mainHelpMenu.SetActive(false);
    }
    public void BackFromBindings()
    {
        bindingsMenu.SetActive(false);
        mainHelpMenu.SetActive(true);
    }
    public void BackFromFeedback()
    {
        feedbackMenu.SetActive(false);
        mainHelpMenu.SetActive(true);
    }
}
