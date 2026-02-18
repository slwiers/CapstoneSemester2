using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearButtons : MonoBehaviour
{
    public GameObject HourHand;
    public GameObject MinuteHand;
    float[] rotations = { 0, -30, -60, -90, -120, -150, 180, 150, 120, 90, 60, 30 };

    public void RotateHand6()
    {
        Quaternion currentRotation = MinuteHand.transform.rotation;

        currentRotation.eulerAngles += new Vector3(0, 0, 180);

        HourHand.transform.Rotate(currentRotation.eulerAngles);
    }
    public void RotateHand3()
    {
        Quaternion currentRotation = MinuteHand.transform.rotation;

        currentRotation.eulerAngles += new Vector3(0, 0, 90);

        HourHand.transform.Rotate(currentRotation.eulerAngles);
    }
    public void RotateHand1 ()
    {
        Quaternion currentRotation = MinuteHand.transform.rotation;

        currentRotation.eulerAngles += new Vector3(0, 0, 30);

        HourHand.transform.Rotate(currentRotation.eulerAngles);
    }

    public void RotateHand30()
    {
        Quaternion currentRotation = HourHand.transform.rotation;

        currentRotation.eulerAngles += new Vector3(0, 0, 180);

        MinuteHand.transform.Rotate(currentRotation.eulerAngles);
    }

    public void RotateHand15()
    {
        Quaternion currentRotation = HourHand.transform.rotation;

        currentRotation.eulerAngles += new Vector3(0, 0, 90);

        MinuteHand.transform.Rotate(currentRotation.eulerAngles);
    }

    public void RotateHand5()
    {
        Quaternion currentRotation = HourHand.transform.rotation;

        currentRotation.eulerAngles += new Vector3(0, 0, 30);

        MinuteHand.transform.Rotate(currentRotation.eulerAngles);
    }
}
