using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject[] arr;
    private void Update()
    {
        if (transform.position.x > 9f && transform.position.x < 26.6f)
        {
            GameObject.Find("Main Camera").transform.position = new Vector3(arr[0].transform.position.x,0,-10);
        }
        else if (transform.position.x > 26.6f)
        {
            GameObject.Find("Main Camera").transform.position = new Vector3(arr[1].transform.position.x,0,-10);
        }
    }
}
