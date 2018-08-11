using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float Speed = 1;

    private void LateUpdate()
    {
        Vector3 target = new Vector3(GameManager.I.Frog.transform.position.x, GameManager.I.Frog.transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, target, Speed);
    }
}
