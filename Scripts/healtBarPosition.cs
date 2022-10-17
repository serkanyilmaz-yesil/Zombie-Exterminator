using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healtBarPosition : MonoBehaviour
{
    private Transform cam;


    private void Awake()
    {
        cam = Camera.main.transform;

    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
