using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] Vector3[] locations = default;
    [SerializeField] int locationIndex = 0;
    [SerializeField] Vector3 target = default;

    [SerializeField] float tolerance = default;
    [SerializeField] float speed = default;
    [SerializeField] float delayTime = default;

    [SerializeField] float delayStart = default;

    [SerializeField] bool automatic = default;


    void Start()
    {
        if(locations.Length > 0)
        {
            target = locations[0];
        }
    }

    
    void FixedUpdate()
    {
        if(transform.position != target)
        {
            MoveObject();
        }
        else
        {
            ChangeTarget();
        }
    }

    void MoveObject()
    {
        Vector3 heading = target - transform.position;
        transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
        if(heading.magnitude < tolerance)
        {
            transform.position = target;
            delayStart = Time.time;
        }
    }
    void ChangeTarget()
    {
        if(automatic)
        {
            if (Time.time - delayStart > delayTime)
            {
                NextTarget();
            }
        }
    }

    void NextTarget()
    {
        locationIndex++;
        if (locationIndex >= locations.Length)
        {
            locationIndex = 0;
        }

        target = locations[locationIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
        if (!automatic)
        {
            NextTarget();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
        if (!automatic)
        {
            NextTarget();
        }
    }

}
