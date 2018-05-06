using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> platforms = new List<GameObject>();
    public List<float> heights = new List<float>();

    private int rndRange = 0;
    private float lastPos = 0f;
    private float lastScale = 0f;
    private int lastPlatformRange = -1;

    public void SetupNewLevel()
    {
        Manager.GetInstance.isNight = (Random.Range(0, 3) == 0);
        Manager.GetInstance.sun.gameObject.SetActive(!Manager.GetInstance.isNight);
        Manager.GetInstance.moon.gameObject.SetActive(Manager.GetInstance.isNight);
    }

    public void RandomGenerator()
    {
        randomPlatform();
        while (rndRange == lastPlatformRange)
        {
            randomPlatform();
        }
        lastPlatformRange = rndRange;

        Debug.Log("Generating platform " + rndRange);

        CreateLevelObject(platforms[rndRange], heights[rndRange], rndRange);
    }

    public void CreateLevelObject(GameObject obj, float height, int value)
    {
        
        GameObject go = Instantiate(obj) as GameObject;

        float offset = lastPos + (lastScale * 0.5f);
        offset += (go.transform.localScale.z) * 0.5f;
        Vector3 pos = new Vector3(0, height, offset);

        go.transform.position = pos;

        lastPos = go.transform.position.z;
        lastScale = go.transform.localScale.z;

        go.transform.parent = this.transform;
    }

    private void randomPlatform()
    {
        rndRange = Random.Range(0, platforms.Count);
    }
}
