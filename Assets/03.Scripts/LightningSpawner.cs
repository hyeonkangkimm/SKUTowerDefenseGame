using UnityEngine;
using System.Collections.Generic;

public class LightningSpawner : MonoBehaviour
{
    public GameObject lightningPrefab;

    public void SpawnLightningOnFiveRandomMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        if (monsters.Length == 0)
            return;

        // 리스트로 변환해서 셔플
        List<GameObject> monsterList = new List<GameObject>(monsters);
        Shuffle(monsterList);

        // 최대 5마리까지만 선택
        int count = Mathf.Min(5, monsterList.Count);
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = monsterList[i].transform.position + Vector3.up * 2f;  // 약간 위에서
            Instantiate(lightningPrefab, pos, Quaternion.identity);
        }
    }

    // Fisher–Yates 셔플
    void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            GameObject temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }
}
