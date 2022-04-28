using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraMotion : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera FaceCamera;
    [SerializeField] private CinemachineBrain cameraBrain;

    public float GetBlendSeconds()
    {
        return cameraBrain.m_DefaultBlend.BlendTime;
    }
    public void EnableFaceCamera(bool active)
    {
        //StartCoroutine(CameraEnamble(active));
        FaceCamera.gameObject.SetActive(active);
    }

    private IEnumerator CameraEnamble(bool active)
    {
        yield return new WaitForSeconds(0.1f);

    }
}
