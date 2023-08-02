using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommandScript : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private GameObject commandInput;
    [SerializeField] private InputActionReference openCommands, submitCommand;
    public static bool IsPaused { get; private set; } = false;
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    
    void Update()
    {
        if (openCommands.action.ReadValue<float>() != 0)
        {
            commandInput.SetActive(true);
            TMP_InputField inputText = commandInput.GetComponent<TMP_InputField>();
            inputText.text = "";
            IsPaused = true;
            Time.timeScale = 0f;
        }  
        if (commandInput.activeInHierarchy && submitCommand.action.ReadValue<float>() != 0)
        {
            Submit();
        }
    }
    void Submit()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        TMP_InputField inputText = commandInput.GetComponent<TMP_InputField>();
        string[] commands = new string[3];
        if (inputText.text.Contains(" %% "))
        {
            commands = inputText.text.Split(" %% ");
        } 
        else
        {
            commands[0] = inputText.text;
            List<string> list = new(commands);
            for (int i = 1; i <commands.Length; i++)
            {
                list.Remove(commands[i]);
            }
            commands = list.ToArray();
        }
        commandInput.SetActive(false);
        if (commands != null)
        {
            foreach (string command in commands)
            {
                if (command.StartsWith("repeat"))
                {
                    Debug.Log("command: " + command);
                    string parameters = command.Split("-")?[1]?.ToString();
                    Debug.Log("parameters: " + parameters);
                    string commandMethodtoInvoke = command.Split("-")?[0]?.ToString();
                    Debug.Log("commandMethodtoInvoke: " + commandMethodtoInvoke);
                    string commandName = commandMethodtoInvoke.Split(" ")?[1]?.ToString();
                    Debug.Log("commandName: " + commandName);
                    string commandValue = commandMethodtoInvoke.Split(" ")?[2]?.ToString();
                    Debug.Log("commandValue: " + commandValue);
                    StartCoroutine(Repeat(commandName, int.Parse(commandValue), (string[])parameters.Split(" ")));
                }
                else if (command != null || command != "")
                {
                    string methodName = command.Split(" ")?[0]?.ToString();
                    object[] parameters = new object[] 
                    { 
                        command.Split(" ")?[1]?.ToString(),
                        command.Split(" ")?[2]?.ToString(),
                        command.Split(" ")?[3]?.ToString()
                    };

                    // Find the method by name using reflection
                    MethodInfo method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);

                    if (method != null)
                    {
                        method.Invoke(this, parameters);
                    }
                    else
                    {
                        Debug.LogError("Method " + methodName + " not found.");
                    }
                }
            }

        }
        else
        {
            Debug.Log("command syntax incorrect");
        }
    }
    IEnumerator Repeat(string Function, int repeatAmount, string[] parameters = null)
    {
        for (int i = 0; i <= repeatAmount; i++)
        {
            MethodInfo method = GetType().GetMethod(Function, BindingFlags.Instance | BindingFlags.Public);

            if (method != null)
            {
                if (parameters != null)
                {
                    method.Invoke(this, parameters);
                }
                else
                {
                    method.Invoke(this, null);
                }
            }
            else
            {
                Debug.LogError("Method '" + Function + "' not found.");
            }
            yield return new WaitForSecondsRealtime(.3f);
        }
    }
    public void KillAll()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.SetActive(false);
        }
    }
    public void Augment(string variableToAugment, string value, string augmentingBool, string additive)
    {
        bool augmentingBool2 = bool.Parse(augmentingBool);
        if (!augmentingBool2)
        {
            bool additivebool = bool.Parse(additive);
            if (!additivebool)
            {
                int value2 = int.Parse(value);
                FieldInfo variableName = playerController.GetType().GetField(variableToAugment);
                variableName.SetValue(playerController, value2);
            }
            else
            {
                int value2 = int.Parse(value);
                FieldInfo variableName = playerController.GetType().GetField(variableToAugment);
                float newValue = value2 + (int)variableName.GetValue(playerController);
                variableName.SetValue(playerController, newValue);
            }
        }
        else
        {
            bool value2 = bool.Parse(value);
            FieldInfo variableName = playerController.GetType().GetField(variableToAugment);
            if (!value2)
            {
                variableName.SetValue(playerController, false);
            }
            else if (value2)
            {
                variableName.SetValue(playerController, true);
            }
        }
    }
}
