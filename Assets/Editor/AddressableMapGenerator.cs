#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using System.IO;
using UnityEditor.AddressableAssets;
using UnityEngine;

public class AddressableMapGenerator
{
    [MenuItem("Tools/Generate AddressableMap.json")]
    public static void Generate()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        var mapData = new AddressableMapData();

        foreach (var group in settings.groups)
        {
            foreach (var entry in group.entries)
            {
                string path = entry.AssetPath;
                string key = Path.GetFileNameWithoutExtension(path).ToLower();
                string address = entry.address;
                EAddressableType type = GuessTypeFromPath(path);

                mapData.list.Add(new AddressableMap
                {
                    addressableType = type,
                    key = key,
                    path = address
                });
            }
        }

        string outputDir = "Assets/Addressables/MapData";
        Directory.CreateDirectory(outputDir);
        File.WriteAllText($"{outputDir}/AddressableMap.json", JsonUtility.ToJson(mapData, true));
        AssetDatabase.Refresh();
        Debug.Log("✅ AddressableMap.json generated.");
    }

    private static EAddressableType GuessTypeFromPath(string path)
    {
        if (path.EndsWith(".prefab")) return EAddressableType.PREFAB;
        if (path.EndsWith(".json")) return EAddressableType.DATA;
        if (path.EndsWith(".mp3") || path.EndsWith(".wav")) return EAddressableType.AUDIO;
        return EAddressableType.DATA;
    }
}
#endif
