using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform Cam;
    public float MoveRate;
    private float StartPointX;
    private float StartPointY;

    public bool lockY;

    void Start()
    {
        StartPointX = transform.position.x;
    }

    
    void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(StartPointX + Cam.position.x * MoveRate, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(StartPointX + Cam.position.x * MoveRate, StartPointY + Cam.position.y * MoveRate);
        }
        
    }
}
