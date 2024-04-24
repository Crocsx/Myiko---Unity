using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Unity.VisualScripting;

public class FirebaseAuthManager: Singleton<FirebaseAuthManager>
{

   public FirebaseAuth Auth
    {
        get { return auth; }
    }

    public FirebaseUser User
    {
        get { return user; }
    }

    public string Token
    {
        get { return token; }
    }

    FirebaseAuth auth;
    FirebaseUser user;
    string token;

    public delegate void AuthStateChangeHandler(bool signedIn, string userId);
    public static event AuthStateChangeHandler OnAuthStateChanged;

    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            DependencyStatus dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                SetToken();
            }

            OnAuthStateChanged?.Invoke(signedIn, user != null ? user.UserId : null);
        }
    }

    void SetToken()
    {
        user.TokenAsync(true).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("TokenAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("TokenAsync encountered an error: " + task.Exception);
                return;
            }
            token = task.Result;
        });
    }
}
