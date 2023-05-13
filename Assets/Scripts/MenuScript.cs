using System.Collections;
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
    public string areaFromDB;
    public float moneyFromDB;
    public bool haveBombFromDB;
    public float laserlevelFromDB;
    public float bomblevelFromDB;
    public float steelhulllevelFromDB;

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
    public class UsernameJson
    {
        public string sk;
        public SaveData saveData;
    }
    public class SaveData
    {
        public string level;
        public float money;
        public bool haveBomb;
        public float laserLevel;
        public float bombLevel;
        public float steelHullLevel;
    }

    void ReplyRegister(string returnData)
    {
        UsernameJson json = JsonConvert.DeserializeObject<UsernameJson>(returnData);
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
        UsernameJson json = JsonConvert.DeserializeObject<UsernameJson>(returnData);
        Debug.Log("json in replyLogin " + json);
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
                continueAreaText.text = json.saveData.level;
                areaFromDB = json.saveData.level;
                moneyFromDB = json.saveData.money;
                haveBombFromDB = json.saveData.haveBomb;
                laserlevelFromDB = json.saveData.laserLevel;
                bomblevelFromDB = json.saveData.bombLevel;
                steelhulllevelFromDB = json.saveData.bombLevel;
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
            Debug.Log("Received: " + uwr.downloadHandler.text + " type " + uwr.downloadHandler.text.GetType());
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
        yield return new WaitForSeconds(.5f);
        foreach (AreaScript areaScript in FindObjectsOfType<AreaScript>())
        {
            areaScript.username = username;
        }
        
        Destroy(gameObject);
    }
    IEnumerator ContinueWait()
    {
        yield return new WaitForSeconds(.2f);
        foreach (AreaScript areaScript in FindObjectsOfType<AreaScript>())
        {
            areaScript.username = username;
            this.areaScript = areaScript;
        }
        areaScript.LoadUser(areaFromDB, moneyFromDB, haveBombFromDB, laserlevelFromDB, bomblevelFromDB, steelhulllevelFromDB);
        Destroy(gameObject);
    }
}
