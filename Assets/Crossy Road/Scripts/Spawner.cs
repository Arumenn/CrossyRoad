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
    public bool useSpawnPlacement = false;
    public int spawnCountMin = 4;
    public int spawnCountMax = 20;

    private float lastTime = 0f;
    private float delayTime = 0f;
    private float speed = 0f;

    [HideInInspector] public GameObject item = null;
    [HideInInspector] public bool goLeft = false;
    [HideInInspector] public float spawnLeftPos = 0f;
    [HideInInspector] public float spawnRightPos = 0f;

    private void Start()
    {
        if (useSpawnPlacement) //is not a Mover
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
        if (useSpawnPlacement) { return; }

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
        GameObject obj = Instantiate(item) as GameObject;
        obj.transform.position = GetSpawnPosition();

        float direction = goLeft ? 180 : 0;
        if (!useSpawnPlacement) //so if it's a Mover
        {
            obj.GetComponent<Mover>().speed = speed;
            obj.transform.rotation = obj.transform.rotation * Quaternion.Euler(0, direction, 0);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        if (useSpawnPlacement)
        {
            return new Vector3(Random.Range(spawnLeftPos, spawnRightPos), startPos.position.y, startPos.position.z);
        } else
        {
            return startPos.position;
        }
    }
}
