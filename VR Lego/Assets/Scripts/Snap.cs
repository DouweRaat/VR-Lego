using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Snap : MonoBehaviour
{
    public float oneXOneSize;
    private float yDegreesChange = 0;
    private float xLengthFromStud = 0;
    private float zLengthFromStud = 0;
    public Interactable interactable;

    public void Update()
    {
        if (interactable.isAttachedToHand == false)
        {
            SnapToGrid();
        }
    }

    public void SnapToGrid()
    {
        xLengthFromStud = transform.position.x % oneXOneSize;
        zLengthFromStud = transform.position.z % oneXOneSize;
        yDegreesChange = transform.rotation.y % 90;
        
        //correcting x pos
        if (xLengthFromStud > 0 && xLengthFromStud < oneXOneSize / 2)
        {
            transform.position = new Vector3((transform.position.x - xLengthFromStud), transform.position.y, transform.position.z);
        }
        else if (xLengthFromStud >= oneXOneSize / 2)
        {
            transform.position = new Vector3((transform.position.x - xLengthFromStud + oneXOneSize), transform.position.y, transform.position.z);
        }
        
        //correcting z pos
        if (zLengthFromStud > 0 && zLengthFromStud < oneXOneSize / 2)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.z - zLengthFromStud));
        }
        else if (xLengthFromStud >= oneXOneSize / 2)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.z - zLengthFromStud + oneXOneSize));
        }

        //correcting rotation
        if (yDegreesChange > 0 && xLengthFromStud < 45)
        {
            transform.rotation = Quaternion.Euler(0, (transform.rotation.y - yDegreesChange), 0);
        }
        else if (yDegreesChange >= 45) {
            transform.rotation = Quaternion.Euler(0, (transform.rotation.y - yDegreesChange + 90), 0);
        }
    }
}
