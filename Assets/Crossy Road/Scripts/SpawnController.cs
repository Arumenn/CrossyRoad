using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public bool goLeft = false;
    public bool itemsAreDecor = false;
    public List<GameObject> items = new List<GameObject>();
    public List<Spawner> spawnersLeft = new List<Spawner>();
    public List<Spawner> spawnersRight = new List<Spawner>();

    private void Start()
    {
        int direction = Random.Range(0, 2);
        goLeft = !(direction > 0);

        for (int i = 0; i < spawnersLeft.Count; i++)
        {
            spawnersLeft[i].items = items;
            spawnersLeft[i].goLeft = goLeft;
            spawnersLeft[i].gameObject.SetActive(!goLeft);
            spawnersLeft[i].spawnLeftPos = spawnersLeft[i].transform.position.x;
            spawnersLeft[i].randomizeItems = itemsAreDecor;
        }
        for (int i = 0; i < spawnersRight.Count; i++)
        {
            spawnersRight[i].items = items;
            spawnersRight[i].goLeft = goLeft;
            spawnersRight[i].gameObject.SetActive(goLeft);
            spawnersRight[i].spawnRightPos = spawnersRight[i].transform.position.x;
            spawnersRight[i].randomizeItems = itemsAreDecor;
        }
    }
}
