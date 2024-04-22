using System;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationController: MonoBehaviour
{
    public GameObject registrationPanel;
    public GameObject loginPanel;
    public GameObject loggedPanel;
    private UnityMainThreadDispatcher mainThreadDispatcher;

    FirebaseAuthManager firebaseAuthManager;

    void Start()
    {
        firebaseAuthManager = FirebaseAuthManager.Instance;
        mainThreadDispatcher = UnityMainThreadDispatcher.Instance;

        FirebaseAuthManager.OnAuthStateChanged += HandleAuthStateChanged;
        if(firebaseAuthManager.User != null)
        {
            ShowLoggedPanel();
        }

    }


    void OnDestroy()
    {
        FirebaseAuthManager.OnAuthStateChanged -= HandleAuthStateChanged;
    }

    void HandleAuthStateChanged(bool signedIn, string userId)
    {
        if (signedIn)
        {
            mainThreadDispatcher.Enqueue(() =>
            {
                ShowLoggedPanel();
            });
        }
    }


    public void ShowRegistrationPanel()
    {
        registrationPanel.SetActive(true);
        loginPanel.SetActive(false);
        loggedPanel.SetActive(false);
    }

    public void ShowLoginPanel()
    {
        registrationPanel.SetActive(false);
        loginPanel.SetActive(true);
        loggedPanel.SetActive(false);
    }

    public void ShowLoggedPanel()
    {
        registrationPanel.SetActive(false);
        loginPanel.SetActive(false);
        loggedPanel.SetActive(true);
    }

    public void Login(string email, string password)
    {
        firebaseAuthManager.Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user signed successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            mainThreadDispatcher.Enqueue(() =>
            {
                ShowLoggedPanel();
            });
        });
    }

    public void Register(string email, string password)
    {
        firebaseAuthManager.Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            mainThreadDispatcher.Enqueue(() =>
            {
                ShowLoginPanel();
            });
        });
    }

    public void Logout()
    {
        firebaseAuthManager.Auth.SignOut();
        ShowLoginPanel();
    }
}
