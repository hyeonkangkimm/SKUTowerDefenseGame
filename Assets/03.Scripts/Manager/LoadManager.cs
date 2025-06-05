using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : Singleton<LoadManager>
{
    public GameObject GetHeroPrefab(string path)
    {
        path = "Hero/" + path;
        return (GameObject)Resources.Load(path);
    }
    public GameObject GetMobPrefab(string path)
    {
        path = "Mob/" + path;
        return (GameObject)Resources.Load(path);
    }
}
