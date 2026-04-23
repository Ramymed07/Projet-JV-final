using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ambiancesound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Collider Area;
    public GameObject Player;
    void Update()
    {
        Vector3 closestPoint=Area.ClosestPoint(Player.transform.position);
        transform.position=closestPoint;
    }

    // Update is called once per frame
}
