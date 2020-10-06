using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    //player position
    Transform lookAt;
    //start cam position offset
    Vector3 startOffset;
    Vector3 moveVector;
    float transition = 0.0f;
    float animationDuration = 3.0f;
    Vector3 animationOffset = new Vector3(5, 5, 4);
    // Use this for initialization
    void Start () {
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        startOffset = transform.position - lookAt.position;
	}

    // Update is called once per frame
    void Update()
    {
        moveVector = lookAt.position + startOffset;
        moveVector.x = 0;
        moveVector.y = Mathf.Clamp(moveVector.y, 3, 5);

        if(transition>1.0f)
        {
            transform.position = moveVector;
        }
        else
        {
            //Start animation
            transform.position = Vector3.Lerp(moveVector + animationOffset, moveVector, transition);
            transition += Time.deltaTime * 1 / animationDuration;
            transform.LookAt(lookAt.position + Vector3.up);
        }
    }
}
