using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ObjCollider : MonoBehaviour
{

   private void OnCollisionEnter(Collision collision)
   {
      if (collision.gameObject.CompareTag("Floor"))
      {
         RuntimeManager.PlayOneShot("event:/Obj Collide");
      }
   }
}
