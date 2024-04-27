using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField]
    GameObject prefab;

    [Header("Globe")]
    [SerializeField]
    Material texture;
    [SerializeField]
    float radius;

    public void GenerateMap(MapTile[] tiles)
    {

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(0, 0, 0);
        sphere.transform.localScale = new Vector3(radius , radius, radius);
        sphere.transform.parent = transform;
        sphere.GetComponent<MeshRenderer>().material = texture;


        GameObject wrapper = new GameObject("Tiles");
        foreach (var tile in tiles) 
        {

            float longitude = tile.location.coordinates[0] * Mathf.Deg2Rad;
            float latitude = (90 - tile.location.coordinates[1]) * Mathf.Deg2Rad;

            Vector3 position = new Vector3
            (
                radius * 0.5f * Mathf.Sin(latitude) * Mathf.Cos(longitude),
                radius * 0.5f * Mathf.Cos(latitude),
                radius * 0.5f * Mathf.Sin(latitude) * Mathf.Sin(longitude)
            );

            Instantiate(prefab, position, Quaternion.identity, wrapper.transform);
        }
    }
}