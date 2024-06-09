using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensionMapMovement : MonoBehaviour
{
    private float _stepDown = -1.5f;
    private float _stepLeft = 1.5f;

    public void ChangePosition(int index, Transform objectTransform)
    {
        if (index % 2 == 0)
        {
            objectTransform.localPosition += new Vector3(0, 0, _stepLeft);
        }
        else
        {
            objectTransform.localPosition += new Vector3(_stepDown, 0, 0);
        }
    }

    public void ResetPosition(Transform objectTransform)
    {
        objectTransform.localPosition = new Vector3(0, 0, 0);
    }

    public void SetPosition(int amount,Transform objectTransform)
    {
        int index = 1;
        
        for (int i = 0; i < amount; i++)
        {
            Debug.Log(index);
            ChangePosition(index, objectTransform);
            index++;
        }
    }
}