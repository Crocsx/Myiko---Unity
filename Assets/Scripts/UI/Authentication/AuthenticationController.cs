using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationController: MonoBehaviour
{
    public GameObject registrationPanel;
    public GameObject loginPanel;
    public GameObject loggedPanel;

    UnityMainThreadDispatcher mainThread = UnityMainThreadDispatcher.Instance;

    void Start()
    {
        mainThread = UnityMainThreadDispatcher.Instance;

        FirebaseAuthManager.OnAuthStateChanged += HandleAuthStateChanged;
        if(FirebaseAuthManager.Instance.User != null)
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
            mainThread.Enqueue(() =>
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
        FirebaseAuthManager.Instance.Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
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

            mainThread.Enqueue(() =>
            {
                ShowLoggedPanel();
            });
        });
    }

    public void Register(string email, string password)
    {
        StartCoroutine(AuthApi.RegisterUser(new AuthRegistrationDto(email, password, email), (success, response) => {
            if (success)
            {
                ShowLoginPanel();
            }
            else
            {
                Debug.LogError(response);
            }
        }));
    }

    public void Logout()
    {
        FirebaseAuthManager.Instance.Auth.SignOut();
        ShowLoginPanel();
    }
}
