using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    private string loginString = "Welcome back! Please login!";
    private string registerString = "Welcome! Please sign up!";
    private string requiredInput = "All fields are required!";

    [Header("Login")]
    public GameObject loginPanel;
    public TMP_InputField loginUsernameInput, loginEmailInput, loginPasswordInput;
    private string loginUsername;
    private string loginEmail;
    private string loginPassword;

    [Header("Register")]
    public GameObject registerPanel;
    public TMP_InputField registerUsernameInput, registerAgeInput, registerEmailInput, registerPasswordInput, registerRepasswordInput;

    private string registerGender = "male";

    void Start()
    {

    }

    void Update()
    {

    }

    private void SetPromptMessage(string message)
    {
        promptText.text = message;
    }

    // LOGIN
    public void HyperlinkToRegister()
    {
        SetPromptMessage(registerString);

        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void LoginUser() {

        if (LoginFieldIsEmpty()) {
            SetPromptMessage(requiredInput);
        }

        ResetLoginInputField();
        //SceneManager.LoadScene("MainMenuScene");
    }

    private bool LoginFieldIsEmpty()
    {
        string userText = loginUsernameInput.text;
        string emailText = loginEmailInput.text;
        string passwordText = loginPasswordInput.text;

        return (string.IsNullOrEmpty(userText) || string.IsNullOrEmpty(emailText) || string.IsNullOrEmpty(passwordText));
    }

    private void ResetLoginInputField()
    {
        loginUsernameInput.text = null;
        loginEmailInput.text = null;
        loginPasswordInput.text = null;
    }

    // REGISTER
    public void OnGenderDropdown(int index)
    {
        switch (index)
        {
            case 0:
                registerGender = "male";
                break;

            case 1:
                registerGender = "female";
                break;
        }
    }

    public void HyperlinkToLogin()
    {
        SetPromptMessage(loginString);

        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    public void RegisterUser()
    {
        if (RegisterFieldIsEmpty())
        {
            SetPromptMessage(requiredInput);
        }

        ResetRegisterInputField();
        //SceneManager.LoadScene("MainMenuScene");
    }

    private bool RegisterFieldIsEmpty()
    {
        string userText = registerUsernameInput.text;
        string ageText = registerAgeInput.text;
        string emailText = registerEmailInput.text;
        string passwordText = registerPasswordInput.text;
        string repasswordText = registerRepasswordInput.text;

        return (
            string.IsNullOrEmpty(userText) || string.IsNullOrEmpty(ageText) || string.IsNullOrEmpty(emailText) || 
            string.IsNullOrEmpty(passwordText) || string.IsNullOrEmpty(repasswordText)
        );
    }

    private void ResetRegisterInputField()
    {
        registerUsernameInput.text = null;
        registerAgeInput.text = null;  
        registerEmailInput.text = null; 
        registerPasswordInput.text = null;
        registerRepasswordInput.text = null;
    }
}
