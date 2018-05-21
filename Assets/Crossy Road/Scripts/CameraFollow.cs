using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speed = 0.25f;
    public bool autoMove = true;
    public GameObject objectToFollow = null;
    public Vector3 offset = new Vector3(3, 6, -3);
    private Vector3 depth = Vector3.zero;
    private Vector3 pos = Vector3.zero;

    private void Update()
    {
        if (Manager.GetInstance.gameOver) { return; } 
        if (autoMove)
        {
            depth = this.gameObject.transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            CalcPosBetweenCameraAndObject();
            gameObject.transform.position = new Vector3(pos.x, offset.y, depth.z);
        } else
        {
            CalcPosBetweenCameraAndObject();
            gameObject.transform.position = new Vector3(pos.x, offset.y, pos.z);
        }
    }

    private void CalcPosBetweenCameraAndObject()
    {
        pos = Vector3.Lerp(gameObject.transform.position, objectToFollow.transform.position + offset, speed * Time.deltaTime);
    }
}
