[System.Serializable]
public class ServerResponse<T>
{
    public string status;
    public T data;
}

[System.Serializable]
public class GetMapResponse
{
    public MapTile[] world;
}
