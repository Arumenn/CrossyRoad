using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float jumpIteration = 4.5f;
    
    public void Shake()
    {
        float height = Mathf.PerlinNoise(jumpIteration, 0) * 5f;
        height = height * height * 0.2f;

        float shakeAmount = height; //degrees to shake the camera
        float shakePeriodTime = 0.25f; //period of each shake
        float dropOffTime = 1.25f; //time before settling down

        LTDescr shakeTween = LeanTween.rotateAroundLocal(gameObject, Vector3.right, shakeAmount, shakePeriodTime).setEase(LeanTweenType.easeShake).setLoopClamp().setRepeat(-1);

        LeanTween.value(gameObject, shakeAmount, 0, dropOffTime).setOnUpdate((float val) => { shakeTween.setTo(Vector3.right * val); }).setEase(LeanTweenType.easeInOutQuad);

        //I have no idea what i'm doing.
    }
}
