using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorScrollViewController : MonoBehaviour
{
    private MotorButtonContainer previousMotorButtonContainer;
    public void SetMainMotor(MotorButtonContainer motorButtonContainer)
    {
        if (motorButtonContainer == null)
        {
            Debug.LogWarning($"haven't set a parameter");
            return;
        }


        if (previousMotorButtonContainer == motorButtonContainer)
        {
            return;
        }


        if (previousMotorButtonContainer == null)
        {
            previousMotorButtonContainer = motorButtonContainer;
            motorButtonContainer.SetMainMotor();
            return;
        }


        motorButtonContainer.SetMainMotor();
        previousMotorButtonContainer.SetOutline(false);


        previousMotorButtonContainer = motorButtonContainer;

    }    
}
