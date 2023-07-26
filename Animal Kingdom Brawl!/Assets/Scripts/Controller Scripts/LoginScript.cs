using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Unity.VisualScripting;
using Firebase.Database;
using Google.MiniJSON;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System;
using UnityEngine.UI;
using UnityEditor.VersionControl;

public class LoginScript : MonoBehaviour
{
    [Header("Prompt Messages")]
    public TextMeshProUGUI promptText;
    private readonly string loginString = "Welcome back! Please login!";
    private readonly string registerString = "Welcome! Please sign up!";
    private readonly string userRegistered = "You are now registered, yay! Please login!";
    private readonly string requiredInput = "All fields are required!";
    private readonly string userNotExist = "User does not exist!";
    private readonly string invalidEmailFormat = "Invalid email address format!";
    private readonly string wrongPassword = "Wrong password!";
    private readonly string usernameTooShort = "Username too short! Must be at least 4 characters!";
    private readonly string usernameTaken = "Username is already taken!";
    private readonly string ageInvalidFormat = "Age is invalid! The age range is between 1 to 120!";
    private readonly string emailTaken = "Email already taken!";
    private readonly string passwordTooShort = "Weak password! Password must be at least 6 characters!";
    private readonly string passwordMismatch = "The repeated password is incorrect!";

    [Header("Firebase")]
    public FirebaseAuth auth;
    public FirebaseUser firebaseUser;
    public DatabaseReference firebaseDBReference;
    public DependencyStatus dependencyStatus;

    [Header("Login")]
    public GameObject loginPanel;
    public TMP_InputField loginEmailInput, loginPasswordInput;
    public List<TMP_InputField> loginInputFields;
    private string loginEmail;
    private string loginPassword;

    [Header("Register")]
    public GameObject registerPanel;
    public TMP_InputField registerUsernameInput, registerAgeInput, registerEmailInput, registerPasswordInput, registerRepasswordInput;
    public List<TMP_InputField> registerInputFields;
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
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus} at {this.name}");
            }
        });
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (loginPanel.activeSelf)
            {
                TabToSwitchField("login");
            }
            else if (registerPanel.activeSelf)
            {
                TabToSwitchField("register");
            }
        }

        // DEV: Remove these when done
        if (Input.GetKeyDown(KeyCode.R)) {
            loginEmailInput.text = "dummy@gmail.com";
            loginPasswordInput.text = "123123";
        }
    }

    private void InitialiseFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        firebaseDBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void SetPromptMessage(string message)
    {
        promptText.text = message;
    }

    private void TabToSwitchField(string panel)
    {
        TMP_InputField currentField = null;

        switch (panel)
        {
            case "login":
                foreach (var field in loginInputFields)
                {
                    if (field.isFocused)
                    {
                        currentField = field;
                        break;
                    }
                }

                if (currentField != null)
                {
                    int currentIndex = loginInputFields.IndexOf(currentField);

                    int nextIndex = (currentIndex + 1) % loginInputFields.Count;
                    loginInputFields[nextIndex].ActivateInputField();
                }

                break;

            case "register":
                foreach (var field in registerInputFields)
                {
                    if (field.isFocused)
                    {
                        currentField = field;
                        break;
                    }
                }

                if (currentField != null)
                {
                    int currentIndex = registerInputFields.IndexOf(currentField);

                    int nextIndex = (currentIndex + 1) % registerInputFields.Count;
                    registerInputFields[nextIndex].ActivateInputField();
                }

                break;
        }
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
        string emailText = loginEmailInput.text;
        string passwordText = loginPasswordInput.text;

        return (string.IsNullOrEmpty(emailText) || string.IsNullOrEmpty(passwordText));
    }

    private void ResetLoginInputField()
    {
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
            Debug.LogWarning($"Failed to login task with {loginTask.Exception} at {this.name}");
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
            firebaseUser = loginTask.Result;

            SceneManager.LoadScene("MainMenuScene");        

            Debug.Log($"User logged in with ID:{firebaseUser.UserId}, username: {firebaseUser.DisplayName}, and email: {firebaseUser.Email}");
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
        else if (UsernameTooShortOrLong(registerUsername))
        {
            SetPromptMessage(usernameTooShort);
            return;
        }
        // Age >= 120, or  <= 0 
        else if (AgeInvalid(registerAge))
        {
            SetPromptMessage(ageInvalidFormat);
            return;
        }
        // Password mismatch
        else if (PasswordIsMismatch(registerPassword, registerRepassword)) 
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

    private bool UsernameTooShortOrLong(string username) 
    {
        return (username.Length < 4 || username.Length >= 21);
    }

    private bool AgeInvalid(string age) 
    {
        int parsedAge = int.Parse(age);

        return (parsedAge <= 0 || parsedAge >= 121);
    }

    private bool PasswordIsMismatch(string password, string repassword) 
    { 
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
        // Check username taken or not
        var checkUsernameTask = firebaseDBReference.Child("users").OrderByChild("username").EqualTo(username).GetValueAsync();
        yield return new WaitUntil(predicate: () => checkUsernameTask.IsCompleted);

        if (checkUsernameTask.Exception != null)
        {
            // Handle the database query error
            Debug.LogWarning($"Failed to check username with {checkUsernameTask.Exception} at {this.name}");
        }
        else 
        {
            var usernameResult = checkUsernameTask.Result;

            if (usernameResult.Exists)
            {
                // The username is already taken, display an error message to the user
                SetPromptMessage(usernameTaken);
            }
            else {
                // Username not taken, proceed with registering the user
                var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
                yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

                if (registerTask.Exception != null)
                {
                    // All register error handling
                    Debug.LogWarning($"Failed to register task with {registerTask.Exception} at {this.name}");
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
                    firebaseUser = registerTask.Result;

                    if (firebaseUser != null)
                    {
                        // Creates new userprofile to FirebaseAuth with the username
                        UserProfile profile = new UserProfile { DisplayName = username };

                        var profileUpdatetask = firebaseUser.UpdateUserProfileAsync(profile);
                        yield return new WaitUntil(predicate: () => profileUpdatetask.IsCompleted);

                        if (profileUpdatetask.Exception != null)
                        {
                            // All profile updates error handling
                            Debug.LogWarning($"Failed to update username task with {profileUpdatetask.Exception} at {this.name}");
                        }
                        else
                        {   
                            // User is now registered with DisplayName and as FirebaseAuth.
                            // User Class is then being created and stored into the Database with it's additional credential
                            User user = new User(firebaseUser.DisplayName, registerAge, registerGender);
                            string userJSON = JsonUtility.ToJson(user);

                            var insertUserToDbTask = firebaseDBReference.Child("users").Child(firebaseUser.UserId).SetRawJsonValueAsync(userJSON);
                            yield return new WaitUntil(() => insertUserToDbTask.IsCompleted);

                            if (insertUserToDbTask.Exception != null)
                            {
                                Debug.LogWarning($"Failed to perform set user's value task with {insertUserToDbTask.Exception} at {this.name}");
                            }
                            else
                            {
                                // Insert player's default value
                                #region Player's Default Value
                                firebaseDBReference.Child("users").Child(firebaseUser.UserId).Child("currency").SetValueAsync(GameData.DefaultStartingCurrency);
                                firebaseDBReference.Child("users").Child(firebaseUser.UserId).Child("unlockedAnimalHeroes").SetValueAsync(GameData.DefaultHeroes);
                                #endregion

                                ResetRegisterInputField();
                                SetPromptMessage(userRegistered);

                                loginPanel.SetActive(true);
                                registerPanel.SetActive(false);
                                Debug.Log($"User registered with ID: {firebaseUser.UserId}, username: {firebaseUser.DisplayName}, email: {firebaseUser.Email}, age: {registerAge}, and gender: {registerGender}");
                            }
                        }
                    }
                }
            }
        }
    }
}
