using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class   LoadManager : Singleton<LoadManager>
{
    public GameObject GetPrefab(string rcode)
    {
        string path="";
        if(rcode.Contains("npc"))
            path = "Hero/" + rcode;
        if (rcode.Contains("Mob"))
            path = "Mob/" + rcode;
        return (GameObject)Resources.Load(path);
    }
    
}
