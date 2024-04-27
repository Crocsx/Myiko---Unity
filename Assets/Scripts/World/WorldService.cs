using System.Collections;
using UnityEngine;

public class WorldService : MonoBehaviour
{
    WorldGeneration worldGenerator;

    void Start()
    {
        worldGenerator = GetComponent<WorldGeneration>();
        StartCoroutine(GetWorld());
    }

    IEnumerator GetWorld()
    {
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(MapAPI.GetMap((success, response) => {
            if (success)
            {
                ServerResponse<GetMapResponse> mapResponse = JsonUtility.FromJson<ServerResponse<GetMapResponse>>(response);

                worldGenerator.GenerateMap(mapResponse.data.world);
            }
            else
            {
                Debug.LogError(response);
            }
        }));
    }
}
