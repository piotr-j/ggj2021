using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Dummy : MonoBehaviour
{
    Seeker seeker;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Move(Vector3 position)
    {
        seeker.StartPath(transform.position, position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        // Debug.Log("Reached path, problems? " + p.error);
    }

    internal void Stop()
    {
        seeker.CancelCurrentPathRequest();
    }
}
