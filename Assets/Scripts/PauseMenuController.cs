using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool paused = false;
    public GameObject pauseMenu;

    void Update()
    {
        if (!paused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;

        }
        if (paused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        if (Input.GetKey(KeyCode.Q)) 
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
}
