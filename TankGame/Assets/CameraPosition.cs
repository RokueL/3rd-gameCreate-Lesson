using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject cp = GameObject.Find("CameraPosition");
        if(cp!= null)
        {
            this.transform.position = cp.transform.position;
            this.transform.rotation = cp.transform.rotation;
        }
        
    }
}
