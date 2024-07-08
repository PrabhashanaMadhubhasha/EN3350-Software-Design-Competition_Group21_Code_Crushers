using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigTrigger : MonoBehaviour
{
    public Rig RigLayer_HandIk;
    public TwoBoneIKConstraint RightHandIk;
    public TwoBoneIKConstraint LeftHandIk;
    public TwoBoneIKConstraint RightForeArmIk;
    public TwoBoneIKConstraint LeftForeArmIk;

    public bool RigLayer_HandIk_Fill;
    public bool RightHandIk_Fill;
    public bool LeftHandIk_Fill;
    public bool RightForeArmIk_Fill;
    public bool LeftForeArmIk_Fill;

    public void RigLayer_HandIk_Calss()
    {
        if (RigLayer_HandIk_Fill)
        {
            RigLayer_HandIk.weight = 1.0f;
        }
        else if (!RigLayer_HandIk_Fill)
        {
            RigLayer_HandIk.weight = 0.0f;
        }
    }
    void Update()
    {
        if (RigLayer_HandIk_Fill)
        {
            RigLayer_HandIk.weight = 1.0f;
        }
        else if (!RigLayer_HandIk_Fill)
        {
            RigLayer_HandIk.weight = 0.0f;
        }

        if (RightHandIk_Fill)
        {
            RightHandIk.weight = 1.0f;
        }
        else if (!RightHandIk_Fill)
        {
            RightHandIk.weight = 0.0f;
        }

        if (LeftHandIk_Fill)
        {
            LeftHandIk.weight = 1.0f;
        }
        else if (!LeftHandIk_Fill)
        {
            LeftHandIk.weight = 0.0f;
        }

        if (RightForeArmIk_Fill)
        {
            RightForeArmIk.weight = 1.0f;
        }
        else if (!RightForeArmIk_Fill)
        {
            RightForeArmIk.weight = 0.0f;
        }

        if (LeftForeArmIk_Fill)
        {
            LeftForeArmIk.weight = 1.0f;
        }
        else if (!LeftForeArmIk_Fill)
        {
            LeftForeArmIk.weight = 0.0f;
        }
    }
}
