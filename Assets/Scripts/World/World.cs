using System.Collections;
using UnityEngine;

public class World : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GetWorld());
    }

    IEnumerator GetWorld()
    {
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(MapAPI.GetMap((success, response) => {
            if (success)
            {
                Debug.Log(response);
                ServerResponse<GetMapResponse> mapResponse = JsonUtility.FromJson<ServerResponse<GetMapResponse>>(response);
     
                foreach (MapTile tile in mapResponse.data.world)
                {
                    Debug.Log(tile._id);
                }
            }
            else
            {
                Debug.LogError(response);
            }
        }));
    }
}
