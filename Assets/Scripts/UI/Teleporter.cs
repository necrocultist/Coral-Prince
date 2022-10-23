using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] GameObject camera;
    private float wall;

    private void Start()
    {
        wall = 8.56f;
    }

    private void Update()
    {
        if (transform.position.x > wall)
        {
           
            camera.transform.position += new Vector3( 17.77271f,0,0); 
            wall += 17.77271f;
            
        }
    }
}
