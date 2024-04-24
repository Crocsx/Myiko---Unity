using UnityEngine;

[CreateAssetMenu(fileName = "APIConfig", menuName = "ScriptableObjects/APIConfig", order = 1)]
public class APIConfig : ScriptableObject
{
    public string apiUrl = "http://localhost:3333";
}