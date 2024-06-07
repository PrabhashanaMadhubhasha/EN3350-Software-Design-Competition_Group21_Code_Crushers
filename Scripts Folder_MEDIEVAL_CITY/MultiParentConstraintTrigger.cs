using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MultiParentConstraintTrigger : MonoBehaviour
{
    public MultiParentConstraint multiParentConstraint;

    public bool isSourceObject_1;
    public bool isSourceObject_2;
    void Update()
    {
        var data = multiParentConstraint.data.sourceObjects;

        if (isSourceObject_1)
        {
            data.SetWeight(0, 1f);
        }
        else if (!isSourceObject_1)
        {
            data.SetWeight(0, 0f);
        }
        if (isSourceObject_2)
        {
            data.SetWeight(1, 1f);
        }
        else if (!isSourceObject_2)
        {
            data.SetWeight(1, 0f);
        }
        multiParentConstraint.data.sourceObjects = data;

    }
}
