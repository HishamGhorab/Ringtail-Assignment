using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_PlanetRotator : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
