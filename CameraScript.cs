using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(GlobalSettings.width/2, GlobalSettings.height/2, -10);
        gameObject.GetComponent<Camera>().orthographicSize = GlobalSettings.height / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
