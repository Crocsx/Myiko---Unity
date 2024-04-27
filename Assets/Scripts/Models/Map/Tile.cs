[System.Serializable]
public class MapTile
{
    public string _id;
    public string mapId;
    public Point location;
    public MapTileType type;
    public MapTileOwnership ownership;
}

[System.Serializable]
public class Point
{
    public string type;
    public float[] coordinates;
}

public enum MapTileType
{
    land
}

[System.Serializable]
public class MapTileOwnership
{
    public MapTileOwnerType ownerType;
    public string ownerId;
}

public enum MapTileOwnerType
{
    player,
    npc,
    mine,
    gameElement,
    none
}