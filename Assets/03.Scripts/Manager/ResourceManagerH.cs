using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public class AddressableMap
{
    public EAddressableType addressableType;
    public string key;
    public string path;
}

public class AddressableMapData
{
    public List<AddressableMap> list = new List<AddressableMap>();

    public void Add(AddressableMap data) => list.Add(data);
    public void AddRange(List<AddressableMap> items) => list.AddRange(items);
}

public class ResourceManagerH : Singleton<ResourceManagerH>
{
    private readonly Dictionary<EAddressableType, Dictionary<string, string>> addressableMap = new();
    public bool IsInit { get; private set; } = false;

    public async Task InitAsync()
    {
        await Addressables.InitializeAsync().Task;
        await LoadAddressableMap();
        IsInit = true;
    }

    private async Task LoadAddressableMap()
    {
        var handle = Addressables.LoadAssetsAsync<TextAsset>("AddressableMap", null);
        await handle.Task;

        foreach (var textAsset in handle.Result)
        {
            var data = JsonUtility.FromJson<AddressableMapData>(textAsset.text);
            foreach (var map in data.list)
            {
                var key = map.key.ToLower();
                if (!addressableMap.TryGetValue(map.addressableType, out var dict))
                {
                    dict = new();
                    addressableMap[map.addressableType] = dict;
                }
                dict[key] = map.path;
            }
        }
    }

    public string GetPath(string key, EAddressableType type)
    {
        key = key.ToLower();
        if (addressableMap.TryGetValue(type, out var dict) && dict.TryGetValue(key, out var path))
            return path;

        Debug.LogError($"[ResourceManager] Missing key: {key} for type: {type}");
        return string.Empty;
    }

    public async Task<GameObject> InstantiateAsync(string key, EAddressableType type, Transform parent = null)
    {
        var path = GetPath(key, type);
        if (string.IsNullOrEmpty(path)) return null;

        var handle = Addressables.InstantiateAsync(path, parent);
        await handle.Task;
        return handle.Result;
    }
}
