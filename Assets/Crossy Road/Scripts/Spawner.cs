using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform startPos = null;

    // spawn time based
    [Header("Time Based")]
    public float delayMin = 1.5f;
    public float delayMax = 5f;
    public float speedMin = 1f;
    public float speedMax = 4f;

    // spawn at start
    [Header("At Start")]
    public bool spawnAtRandomPositions = false;
    public int spawnCountMin = 4;
    public int spawnCountMax = 20;

    private float lastTime = 0f;
    private float delayTime = 0f;
    private float speed = 0f;
    private List<int> usedPlacements = new List<int>();

    [HideInInspector] public List<GameObject> items = new List<GameObject>();
    [HideInInspector] public GameObject item = null;
    [HideInInspector] public bool randomizeItems = false;
    [HideInInspector] public bool goLeft = false;
    [HideInInspector] public float spawnLeftPos = 0f;
    [HideInInspector] public float spawnRightPos = 0f;

    private void Start()
    {
        if (spawnAtRandomPositions) //is not a Mover
        {
            int spawnCount = Random.Range(spawnCountMin, spawnCountMax);
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnItem();
            }
        } else
        {
            speed = Random.Range(speedMin, speedMax);
        }
    }

    private void Update()
    {
        if (spawnAtRandomPositions) { return; }

        if (Time.time > lastTime + delayTime)
        {
            lastTime = Time.time;
            delayTime = Random.Range(delayMin, delayMax);
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        Debug.Log("Spawning new Item");
        ChooseItem();
        GameObject obj = Instantiate(item) as GameObject;
        obj.transform.position = GetSpawnPosition();

        float direction = goLeft ? 180 : 0;
        if (!spawnAtRandomPositions) //so if it's a Mover
        {
            obj.GetComponent<Mover>().speed = speed;
            obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(0, direction, 0);
        }
    }

    private void ChooseItem()
    {
        if (item == null || randomizeItems)
        {
            int itemId = Random.Range(0, items.Count);
            item = items[itemId];
        }
    }

    private Vector3 GetSpawnPosition()
    {
        if (spawnAtRandomPositions)
        {
            int x = GetRandomX();
            while (!usedPlacements.Contains(x))
            {
                x = GetRandomX();
                if (!usedPlacements.Contains(x))
                {
                    usedPlacements.Add(x);
                }
            }
            Vector3 pos = new Vector3(x, startPos.position.y, startPos.position.z);

            return pos;
        } else
        {
            return startPos.position;
        }
    }

    private int GetRandomX()
    {
        return (int)Random.Range(spawnLeftPos, spawnRightPos);
    }
}
