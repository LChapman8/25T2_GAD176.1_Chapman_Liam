using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAttacking : MonoBehaviour
{
    Animator animator;
    bool isAttacking = false;
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
        }
    }
}
