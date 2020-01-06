using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject spawnMonster = null;
    public List<GameObject> monsterList = new List<GameObject>();
    public int spawnMaxCount = 50;

    void Start()
    {
        InvokeRepeating("Spawn", 3f, 5f);
    }

    void Spawn()
    {
        if(monsterList.Count > spawnMaxCount)
        {
            return;
        }

        Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), 1000.0f, Random.Range(-10.0f, 10.0f));

        Ray ray = new Ray(position, Vector3.down);
        RaycastHit raycastHit = new RaycastHit();
        if(Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
        {
            position.y = raycastHit.point.y;
        }
        GameObject newMonster = Instantiate(spawnMonster, position, Quaternion.identity);
        monsterList.Add(newMonster);
    }
}
