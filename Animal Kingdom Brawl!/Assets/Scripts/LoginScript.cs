using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Unity.VisualScripting;

public class LoginScript : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    private readonly string loginString = "Welcome back! Please login!";
    private readonly string registerString = "Welcome! Please sign up!";
    private readonly string requiredInput = "All fields are required!";
    private readonly string userNotExist = "User does not exist!";
    private readonly string invalidEmailFormat = "Invalid email address format!";
    private readonly string wrongPassword = "Wrong password!";

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

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

    void Awake()
    {
        // Validate all necessary dependencies for Firebase are presented 
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitialiseFirebase();
            }
            else
            {
                // Dependencies missing
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void InitialiseFirebase() 
    {
        auth = FirebaseAuth.DefaultInstance;
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

    public void LoginButton() {

        if (LoginFieldIsEmpty())
        {
            SetPromptMessage(requiredInput);
            return;
        }
        else 
        { 
            loginUsername = loginUsernameInput.text;
            loginEmail = loginEmailInput.text;
            loginPassword = loginPasswordInput.text;
        }

        StartCoroutine(Login(loginEmail, loginPassword));
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

    private IEnumerator Login(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            // All login error handling
            Debug.LogWarning(message: $"Failed to register task with {loginTask.Exception}");
            FirebaseException firebaseExp = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError) firebaseExp.ErrorCode;

            switch (errorCode)
            {
                case AuthError.InvalidEmail:
                    SetPromptMessage(invalidEmailFormat);
                    break;

                case AuthError.UserNotFound:
                    SetPromptMessage(userNotExist);
                    break;

                case AuthError.WrongPassword:
                    SetPromptMessage(wrongPassword);
                    break;
            }
        }
        else
        {
            user = loginTask.Result;
            Debug.Log($"User signed in: {user.DisplayName} and {user.Email}");
        }

        ResetLoginInputField();
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

    public void RegisterButton()
    {
        if (RegisterFieldIsEmpty())
        {
            SetPromptMessage(requiredInput);
            return;
        }

        // https://www.youtube.com/watch?v=NsAUEyA2TRo&t=553s 9mins
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

    //private IEnumerator Register() {
    // TODO: Add in register user to Firebase, refer yooutube
    //}
}
