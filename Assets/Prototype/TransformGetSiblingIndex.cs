using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformGetSiblingIndex : MonoBehaviour
{
    //Use this to change the hierarchy of the GameObject siblings
    public int indexNumber;

    void Start()
    {
        //Initialise the Sibling Index to 0
        // indexNumber = 0;
        //Set the Sibling Index
        transform.SetSiblingIndex(indexNumber);
        //Output the Sibling Index to the console
        Debug.Log("Sibling Index : " + transform.GetSiblingIndex());
    }

    void OnGUI()
    {
        //Press this Button to increase the sibling index number of the GameObject
        if (GUI.Button(new Rect(0, 0 + indexNumber * 100, 200, 40), "Add Index Number"))
        {
            //Make sure the index number doesn't exceed the Sibling Index by more than 1
            if (indexNumber <= transform.GetSiblingIndex())
            {
                //Increase the Index Number
                indexNumber++;
            }
        }

        //Press this Button to decrease the sibling index number of the GameObject
        if (GUI.Button(new Rect(0, 40 + indexNumber * 100, 200, 40), "Minus Index Number"))
        {
            //Make sure the index number doesn't go below 0
            if (indexNumber >= 1)
            {
                //Decrease the index number
                indexNumber--;
            }
        }
        //Detect if any of the Buttons are being pressed
        if (GUI.changed)
        {
            //Update the Sibling Index of the GameObject
            transform.SetSiblingIndex(indexNumber);
            Debug.Log("Sibling Index : " + transform.GetSiblingIndex());
        }
    }
}
