using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class ApiManager : Singleton<ApiManager>
{
    private const float defaultTimeout = 10f;
    private Dictionary<string, string> defaultHeaders = new Dictionary<string, string>();
    [SerializeField]
    APIConfig config;


    public IEnumerator GetRequest(string url, Action<bool, string> callback, Dictionary<string, string> headers = null, float timeout = defaultTimeout)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{config.apiUrl}{url}"))
        {
            yield return SendRequest(request, callback, headers, timeout);
        }
    }

    public IEnumerator PostJsonRequest(string url, string jsonBody, Action<bool, string> callback, Dictionary<string, string> headers = null, float timeout = defaultTimeout)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        using (UnityWebRequest request = new UnityWebRequest($"{config.apiUrl}{url}", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            if (headers == null || !headers.ContainsKey("Content-Type"))
            {
                request.SetRequestHeader("Content-Type", "application/json");
            }
            yield return SendRequest(request, callback, headers, timeout);
        }
    }

    public IEnumerator PostFormRequest(string url, WWWForm formData, Action<bool, string> callback, Dictionary<string, string> headers = null, float timeout = defaultTimeout)
    {
        using (UnityWebRequest request = UnityWebRequest.Post($"{config.apiUrl}{url}", formData))
        {
            yield return SendRequest(request, callback, headers, timeout);
        }
    }

    public IEnumerator PutRequest(string url, string jsonBody, Action<bool, string> callback, Dictionary<string, string> headers = null, float timeout = defaultTimeout)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        using (UnityWebRequest request = new UnityWebRequest($"{config.apiUrl}{url}", "PUT"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            if (headers == null || !headers.ContainsKey("Content-Type"))
            {
                request.SetRequestHeader("Content-Type", "application/json");
            }
            yield return SendRequest(request, callback, headers, timeout);
        }
    }

    public IEnumerator DeleteRequest(string url, Action<bool, string> callback, Dictionary<string, string> headers = null, float timeout = defaultTimeout)
    {
        using (UnityWebRequest request = UnityWebRequest.Delete($"{config.apiUrl}{url}"))
        {
            yield return SendRequest(request, callback, headers, timeout);
        }
    }

    private IEnumerator SendRequest(UnityWebRequest request, Action<bool, string> callback, Dictionary<string, string> headers, float timeout)
    {
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }
        }

        foreach (var defaultHeader in defaultHeaders)
        {
            if (request.GetRequestHeader(defaultHeader.Key) != "")
            {
                request.SetRequestHeader(defaultHeader.Key, defaultHeader.Value);
            }
        }

        yield return request.SendWebRequest();


        if (request.result == UnityWebRequest.Result.Success)
        {
            callback(true, request.downloadHandler.text);
        }
        else
        {
            callback(false, request.error);
        }
    }
}