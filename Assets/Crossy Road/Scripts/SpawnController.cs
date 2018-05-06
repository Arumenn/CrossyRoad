using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public bool goLeft = false;
    public bool itemsAreDecor = false;
    public SpawnableItems[] spawnableItems;
    public List<Spawner> spawnersLeft = new List<Spawner>();
    public List<Spawner> spawnersRight = new List<Spawner>();

    [HideInInspector] public List<GameObject> itemsBasedOnProbability = new List<GameObject>();

    private void Start()
    {
        int direction = UnityEngine.Random.Range(0, 2);
        goLeft = !(direction > 0);
        
        GenerateProbabilityTable();

        for (int i = 0; i < spawnersLeft.Count; i++)
        {
            spawnersLeft[i].items = itemsBasedOnProbability;
            spawnersLeft[i].goLeft = goLeft;
            spawnersLeft[i].gameObject.SetActive(!goLeft);
            spawnersLeft[i].spawnLeftPos = spawnersLeft[i].transform.position.x;
            spawnersLeft[i].randomizeItems = itemsAreDecor;
        }
        for (int i = 0; i < spawnersRight.Count; i++)
        {
            spawnersRight[i].items = itemsBasedOnProbability;
            spawnersRight[i].goLeft = goLeft;
            spawnersRight[i].gameObject.SetActive(goLeft);
            spawnersRight[i].spawnRightPos = spawnersRight[i].transform.position.x;
            spawnersRight[i].randomizeItems = itemsAreDecor;
        }
    }

    private void GenerateProbabilityTable()
    {
        foreach (SpawnableItems sI in spawnableItems)
        {
            int count = (int)(sI.spawnChance * 100);
            for (int c = 0; c < count; c++)
            {
                itemsBasedOnProbability.Add(sI.itemPrefab);
            }
        }
    }
}
