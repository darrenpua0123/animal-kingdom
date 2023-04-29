using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Unity.VisualScripting;
using Firebase.Database;

public class LoginScript : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    private readonly string loginString = "Welcome back! Please login!";
    private readonly string registerString = "Welcome! Please sign up!";
    private readonly string userRegistered = "You are now registered, yay! Please login!";
    private readonly string requiredInput = "All fields are required!";
    private readonly string userNotExist = "User does not exist!";
    private readonly string invalidEmailFormat = "Invalid email address format!";
    private readonly string wrongPassword = "Wrong password!";

    private readonly string usernameTooShort = "Username too short! Must be at least 4 characters!";
    private readonly string ageInvalidFormat = "Age is invalid! The age range is between 1 to 120!";
    private readonly string emailTaken = "Email already taken!";
    private readonly string passwordTooShort = "Weak password! Password must be at least 6 characters!";
    private readonly string passwordMismatch = "The repeated password is incorrect!";

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
    private string registerUsername;
    private string registerAge;
    private string registerGender = "male";
    private string registerEmail;
    private string registerPassword;
    private string registerRepassword;

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

    public void LoginButton()
    {
        loginUsername = loginUsernameInput.text;
        loginEmail = loginEmailInput.text;
        loginPassword = loginPasswordInput.text;

        // One or more Login fields are empty
        if (LoginFieldIsEmpty())
        {
            SetPromptMessage(requiredInput);
            return;
        }

        StartCoroutine(Login(loginEmail, loginPassword));
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
            Debug.LogWarning(message: $"Failed to login task with {loginTask.Exception}");
            FirebaseException firebaseExp = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseExp.ErrorCode;

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
            // TODO: User loggged in, perform logic here
            user = loginTask.Result;
            SceneManager.LoadScene("MainMenuScene");

            Debug.Log($"User signed in: {user.UserId}, {user.DisplayName}, and {user.Email}");
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
        registerUsername = registerUsernameInput.text;
        registerAge = registerAgeInput.text;
        // var registerGender set during OnGenderChanged;
        registerEmail = registerEmailInput.text;
        registerPassword = registerPasswordInput.text;
        registerRepassword = registerRepasswordInput.text;

        // One or more fields are empty
        if (RegisterFieldIsEmpty())
        {
            SetPromptMessage(requiredInput);
            return;
        }
        // Username too short or long
        else if (UsernameTooShortOrLong())
        {
            SetPromptMessage(usernameTooShort);
            return;
        }

        // TODO: MISSING USERNAME TAKEN
        //else if ()
        //{ 
        
        //}

        // Age >= 120, or  <= 0 
        else if (AgeInvalid())
        {
            SetPromptMessage(ageInvalidFormat);
            return;
        }
        // Password mismatch
        else if (PasswordIsMismatch()) 
        {
            SetPromptMessage(passwordMismatch);
            return;
        }

        StartCoroutine(Register(registerUsername, registerEmail, registerPassword));
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

    private bool UsernameTooShortOrLong() 
    {
        string username = registerUsernameInput.text;

        return (username.Length < 4 || username.Length >= 21);
    }

    private bool AgeInvalid() 
    {
        int age = int.Parse(registerAgeInput.text);

        return (age <= 0 || age >= 121);
    }

    private bool PasswordIsMismatch() 
    { 
        string password = registerPasswordInput.text;
        string repassword = registerRepasswordInput.text;

        return (!password.Equals(repassword));
    }

    private void ResetRegisterInputField()
    {
        registerUsernameInput.text = null;
        registerAgeInput.text = null;
        registerEmailInput.text = null;
        registerPasswordInput.text = null;
        registerRepasswordInput.text = null;
    }

    private IEnumerator Register(string username, string email, string password)
    {
        // Check username here
        // here

        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            // All register error handling
            Debug.LogWarning(message: $"Failed to register task with {registerTask.Exception}");
            FirebaseException firebaseExp = registerTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseExp.ErrorCode;

            switch (errorCode)
            {
                case AuthError.InvalidEmail:
                    SetPromptMessage(invalidEmailFormat);
                    break;

                case AuthError.EmailAlreadyInUse:
                    SetPromptMessage(emailTaken);
                    break;

                case AuthError.WeakPassword:
                    SetPromptMessage(passwordTooShort);
                    break;
            }
        }
        else 
        {
            // User registered as FirebaseUser into FirebaseAuth
            user = registerTask.Result;

            if (user != null) 
            {
                // Creates new userprofile to FirebaseAuth with the username
                UserProfile profile = new UserProfile{ DisplayName = username };

                var profileUpdatetask = user.UpdateUserProfileAsync(profile);
                yield return new WaitUntil(predicate: () => profileUpdatetask.IsCompleted);

                if (profileUpdatetask.Exception != null)
                {
                    // All profile updates error handling
                    Debug.LogWarning($"Failed to update username task with {profileUpdatetask.Exception}");
                    FirebaseException firebaseExp = profileUpdatetask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseExp.ErrorCode;

                    //switch (errorCode)
                    //{

                    //}
                }
                else 
                {   
                    // User registered with DisplayName and as FirebaseAuth. All register logic goes here.
                    ResetRegisterInputField();
                    SetPromptMessage(userRegistered);

                    loginPanel.SetActive(true);
                    registerPanel.SetActive(false);
                    Debug.Log($"User registered as: {user.DisplayName} and {user.Email}");
                } 
            }
        }
    }
}
