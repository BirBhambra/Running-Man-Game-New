using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private void onCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy Body"))
        {
            die();
        }
    }

    void die()
    {
        GetComponent<SkinnedMeshRenderer>().enabled = false;
        GetComponent<AnimationAndMovementController>().enabled = false;
    }
}
