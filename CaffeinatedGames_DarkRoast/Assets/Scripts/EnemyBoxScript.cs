using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoxScript : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit " + other.gameObject.name);
        if (other.gameObject.tag == "hitbox")
        {
            Debug.Log("you were hit by " + gameObject.name);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("collided " + other.gameObject.name);
        if (other.gameObject.tag == "hitbox")
        {
            Debug.Log("you were collided by " + gameObject.name);
        }
    }
}
