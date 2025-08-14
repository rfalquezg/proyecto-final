using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebar : MonoBehaviour
{
    public float rotateSpeed = 75;

    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
