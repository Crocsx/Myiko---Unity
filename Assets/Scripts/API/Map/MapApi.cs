using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapAPI
{
    public static IEnumerator GenerateMap(System.Action<bool, string> callback)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "authorization", "Bearer " + FirebaseAuthManager.Instance.Token }
        };

        return ApiManager.Instance.PostJsonRequest(ApiUrls.MapGenerate, "", callback, headers);
    }

    public static IEnumerator ClearMap(System.Action<bool, string> callback)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "authorization", "Bearer " + FirebaseAuthManager.Instance.Token }
        };

        return ApiManager.Instance.DeleteRequest(ApiUrls.MapClear, callback, headers);
    }

    public static IEnumerator GetMap(System.Action<bool, string> callback)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "authorization", "Bearer " + FirebaseAuthManager.Instance.Token }
        };

        return ApiManager.Instance.GetRequest(ApiUrls.MapGet, callback, headers);
    }
}