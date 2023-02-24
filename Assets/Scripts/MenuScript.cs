using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;

public class MenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject logInMenu;
    public GameObject registerMenu;
    public TMP_InputField usernameInputRegisterMenu;
    public TMP_InputField usernameInputLogInMenu;
    private bool signInMenuOpen = false;
    private bool registerMenuOpen = false;
    private string  username;
    public TMP_Text registerErrorText;
    public TMP_Text logInErrorText;
    public Button registerEnterButton;
    public Button logInButton;
    public Button createAccountButton;
    public Button toLogInMenuButton;
    public Button newGameButton;
    public Button enterLogInButton;
    public Button continueButton;
    private Canvas MainMenuCanvas;
    private AreaScript areaScript;
    public TMP_Text continueAreaText;
    private string area;

    private void Start()
    {
        MainMenuCanvas = GetComponent<Canvas>();
    }

    public void EnterRegister()
    {
        registerEnterButton.interactable = false;
        username = usernameInputRegisterMenu.text;
        StartCoroutine(GetUser());
    }
    public void LogIn()
    {
        enterLogInButton.interactable = false;
        username = usernameInputLogInMenu.text;
        if (username == "")
        {
            logInErrorText.text = "Username is required";
            enterLogInButton.interactable = true;
        }
        else
        {
            StartCoroutine(GetUser());
        }
    }
    public class usernameJson
    {
        public string sk;
        public string saveData;
    }

    void ReplyRegister(string returnData)
    {
        usernameJson json = JsonConvert.DeserializeObject<usernameJson>(returnData);
        if (json == null)
        {
            logInButton.interactable = true;
            registerErrorText.text = "User Created";
            StartCoroutine(CreateUser());
        }
        else
        {
            if (username == "")
            {
                registerErrorText.text = "Username is required";
                registerEnterButton.interactable = true;
            }
            else
            {
                registerErrorText.text = "Username taken. Try another";
                registerEnterButton.interactable = true;
            }
        }
    }
    void ReplyLogIn(string returnData)
    {
        usernameJson json = JsonConvert.DeserializeObject<usernameJson>(returnData);
        if (json == null)
        {
            logInErrorText.text = "Username '" + username + "' does not exist";
            enterLogInButton.interactable = true;
        }
        else
        {
            logInErrorText.text = "Logged in as '" + username + "'";
            createAccountButton.interactable = false;
            toLogInMenuButton.interactable = false;
            newGameButton.interactable = true;
            enterLogInButton.interactable = false;
            if (json.saveData != null)
            {
                continueButton.interactable = true;
                continueAreaText.text = json.saveData.ToString();
                area = json.saveData;
            }
        }
    }

    IEnumerator CreateUser()
    {
        string uri = "https://aur9yy6bag.execute-api.us-west-2.amazonaws.com/v1/users?action=createUser&username=" + username;
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
    IEnumerator GetUser()
    {
        string uri = "https://aur9yy6bag.execute-api.us-west-2.amazonaws.com/v1/users?action=getUserData&username=" + username;
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            if (registerMenuOpen)
            {
                ReplyRegister(uwr.downloadHandler.text);
            }
            else if (signInMenuOpen)
            {
                ReplyLogIn(uwr.downloadHandler.text);
            }
        }

    }
    public void Continue()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(ContinueWait());
        MainMenuCanvas.enabled = false;
        SceneManager.LoadScene("Game");
    }
    public void SignIn()
    {
        signInMenuOpen = true;
        registerMenuOpen = false;
        mainMenu.SetActive(false);
        logInMenu.SetActive(true);
        registerMenu.SetActive(false);
    }
    public void Register()
    {
        registerMenuOpen = true;
        mainMenu.SetActive(false);
        logInMenu.SetActive(false);
        registerMenu.SetActive(true);
    }
    public void Back()
    {
        registerMenuOpen = false;
        signInMenuOpen = false;
        mainMenu.SetActive(true);
        logInMenu.SetActive(false);
        registerMenu.SetActive(false);
    }
    public void NewGame()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(NewGameWait());
        MainMenuCanvas.enabled = false;
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator NewGameWait()
    {
        yield return new WaitForSeconds(.2f);
        areaScript = FindObjectOfType<AreaScript>();
        areaScript.username = username;
        Destroy(gameObject);
    }
    IEnumerator ContinueWait()
    {
        yield return new WaitForSeconds(.15f);
        areaScript = FindObjectOfType<AreaScript>();
        areaScript.username = username;
        areaScript.LoadUser(area);
        Destroy(gameObject);
    }
}
