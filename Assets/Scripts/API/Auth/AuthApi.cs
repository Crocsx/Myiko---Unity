using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class AuthApi
{
    public static IEnumerator RegisterUser(AuthRegistrationDto payload, System.Action<bool, string> callback)
    {
        string jsonBody = JsonUtility.ToJson(payload);
        return ApiManager.Instance.PostJsonRequest(ApiUrls.UserRegister, jsonBody, callback);
    }
}
