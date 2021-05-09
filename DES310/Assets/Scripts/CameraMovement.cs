using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Func<Vector3> GetCameraMovePositionFunc;

    // Start is called before the first frame update
    public void Setup(Func<Vector3> GetCameraMovePositionFunc)
    {
        this.GetCameraMovePositionFunc = GetCameraMovePositionFunc;
    }

    public void SetGetCameraMovePositionFunc(Func<Vector3> GetCameraMovePositionFunc)
    {
        this.GetCameraMovePositionFunc = GetCameraMovePositionFunc;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraMovePosition = GetCameraMovePositionFunc();
        cameraMovePosition.z = transform.position.z;

        Vector3 cameraMoveDir = (cameraMovePosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraMovePosition, transform.position);
        float camSpeed = 2f;

        transform.position = transform.position + cameraMoveDir * distance * camSpeed * Time.deltaTime;      
    }
}
