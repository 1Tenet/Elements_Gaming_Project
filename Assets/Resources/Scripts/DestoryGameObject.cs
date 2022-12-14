using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryGameObject : MonoBehaviour

{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }
    */
}
