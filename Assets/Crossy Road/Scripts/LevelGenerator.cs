using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Platform[] platforms;

    private int rndRange = 0;
    private PlatformType curPlatformType;
    private float lastPos = 0f;
    private float lastScale = 0f;
    private int lastPlatformRange = -1;
    private PlatformType lastPlatformType;

    public void SetupNewLevel()
    {
        Manager.GetInstance.isNight = (Random.Range(0, 3) == 0);
        Manager.GetInstance.sun.gameObject.SetActive(!Manager.GetInstance.isNight);
        Manager.GetInstance.moon.gameObject.SetActive(Manager.GetInstance.isNight);
    }

    public void RandomGenerator()
    {
        randomPlatform();
        while (rndRange == lastPlatformRange || curPlatformType == lastPlatformType)
        {
            randomPlatform();
        }
        lastPlatformRange = rndRange;
        lastPlatformType = platforms[rndRange].platformType;

        CreateLevelObject(platforms[rndRange].platformPrefab, platforms[rndRange].height, rndRange);
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
        rndRange = Random.Range(0, platforms.Length );
        curPlatformType = platforms[rndRange].platformType;
    }
}
