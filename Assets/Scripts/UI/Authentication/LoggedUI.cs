using TMPro;
using UnityEngine;

public class LoggedUI: MonoBehaviour
{

    public TMP_Text nameField;

    FirebaseAuthManager firebaseAuthManager;

    void Start()
    {
        firebaseAuthManager = FirebaseAuthManager.Instance;

        nameField.text = firebaseAuthManager.User.UserId;
    }
}
