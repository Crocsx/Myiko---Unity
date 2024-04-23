using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationController: MonoBehaviour
{
    public GameObject registrationPanel;
    public GameObject loginPanel;
    public GameObject loggedPanel;

    void Start()
    {
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
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
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

            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                ShowLoggedPanel();
            });
        });
    }

    public void Register(string email, string password)
    {
        StartCoroutine(StartRegister(email, password));
    }

    IEnumerator StartRegister(string email, string password)
    {
        string url = "http://localhost:3333/auth/signup";
        string jsonBody = JsonUtility.ToJson(new AuthRegistrationData(email, password, email));
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "Authorization", "Bearer " + FirebaseAuthManager.Instance.Token }
        };

        yield return StartCoroutine(ApiManager.Instance.PostJsonRequest(url, jsonBody, HandlePostLogin, headers));
    }

    void HandlePostLogin(bool success, string response)
    {
        if (success)
        {
            Debug.Log("Register request successful. Response: " + response);
            ShowLoginPanel();
        }
        else
        {
            Debug.LogError("Register request failed. Error: " + response);

        }
    }

    public void Logout()
    {
        FirebaseAuthManager.Instance.Auth.SignOut();
        ShowLoginPanel();
    }
}
