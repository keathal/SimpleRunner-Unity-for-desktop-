using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_trigger : MonoBehaviour {
    Transform door;
	// Use this for initialization
	void Start () {
        door = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");

        if((int)Random.Range(0,2) == 1) 
            door.Rotate(0, Mathf.Lerp(0, 90, Time.time), 0);
        
    }
}
