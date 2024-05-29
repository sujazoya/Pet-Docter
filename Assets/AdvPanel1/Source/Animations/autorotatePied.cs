using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autorotatePied : MonoBehaviour
{
  
 public float yspeed = 0.0f;
 
   // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    yspeed+=Time.deltaTime*10;
transform.rotation= Quaternion.Euler(0,yspeed,0);
    }
}
