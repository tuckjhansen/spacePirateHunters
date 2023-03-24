using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool paused = false;
    public GameObject pauseMenu;
    private Inventory inventoryScript;

    private void Start()
    {
        inventoryScript = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        if (!paused && !inventoryScript.inventoryOpen)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;

        }
        if (paused && !inventoryScript.inventoryOpen)
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
