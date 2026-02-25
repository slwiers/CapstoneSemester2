using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GearButtons : MonoBehaviour
{
    public GameObject HourHand;
    int hourAngle = 0;
    public GameObject MinuteHand;
    float[] rotations = { 0, -30, -60, -90, -120, -150, 180, 150, 120, 90, 60, 30 };
    public float delayTime = 5f; //amount of time the timer is going to wait (change this in engine if you need to change it)
    public GameObject YouWin; //the text to appear upon winning
    public void RotateHand6()
    {
        //DebugClock();
        //Quaternion currentRotation = MinuteHand.transform.rotation;

        //currentRotation.eulerAngles += new Vector3(0, 0, 180f);

        //HourHand.transform.Rotate(currentRotation.eulerAngles);
        HourHand.transform.Rotate(0,0,180,Space.Self);
        //hourAngle = (hourAngle + 180) % 360;
        //Debug.Log("hourAngle = " + hourAngle);
        DebugClock();
    }
    public void RotateHand3()
    {
        
        //Quaternion currentRotation = MinuteHand.transform.rotation;

        //currentRotation.eulerAngles += new Vector3(0, 0, 90f);

        //HourHand.transform.Rotate(currentRotation.eulerAngles);
        HourHand.transform.Rotate(0,0,90,Space.Self);
        DebugClock();
    }
    public void RotateHand1 ()
    {
       
        //Quaternion currentRotation = MinuteHand.transform.rotation;

        //currentRotation.eulerAngles += new Vector3(0, 0, 30f);

        //HourHand.transform.Rotate(currentRotation.eulerAngles);
        HourHand.transform.Rotate(0,0,30,Space.Self);
        DebugClock();
    }

    public void RotateHand30()
    {
        
        //Quaternion currentRotation = HourHand.transform.rotation;

        //currentRotation.eulerAngles += new Vector3(0, 0, 180f);

        //MinuteHand.transform.Rotate(currentRotation.eulerAngles);
        MinuteHand.transform.Rotate(0,0,180, Space.Self);
        DebugClock();
    }

    public void RotateHand15()
    {
        
        //Quaternion currentRotation = HourHand.transform.rotation;

        //currentRotation.eulerAngles += new Vector3(0, 0, 90f);

        //MinuteHand.transform.Rotate(currentRotation.eulerAngles);
        MinuteHand.transform.Rotate(0, 0, 90, Space.Self);
        DebugClock();
    }

    public void RotateHand5()
    {
       
        //Quaternion currentRotation = HourHand.transform.rotation;

        //currentRotation.eulerAngles += new Vector3(0, 0, 30f);

        //MinuteHand.transform.Rotate(currentRotation.eulerAngles);
        MinuteHand.transform.Rotate(0, 0, 30, Space.Self);
        DebugClock();
    }

    private IEnumerator waitForSceneChange() //function for the timer to be called as
    {
        yield return new WaitForSeconds(delayTime); //starts the timer
        {
            SceneManager.LoadScene("SecurityRoom5"); //loads the next scene
        }
    }

    private void DebugClock()
    {
        Debug.Log("Hour Hand z");
        Debug.Log(Mathf.Round( HourHand.transform.localEulerAngles.z));
        Debug.Log("Minute Hand z");
        Debug.Log(Mathf.Round( MinuteHand.transform.localEulerAngles.z));

        if (HourHand.transform.localEulerAngles.z == 270f && MinuteHand.transform.localEulerAngles.z == 180f)
        {
            Debug.Log("you win");
            HourHand.SetActive(false);
            MinuteHand.SetActive(false);
            YouWin.SetActive(true); //sets the text active
            StartCoroutine(waitForSceneChange()); //calls the timer function and waits to change the scene for a few seconds
        }
    }
}
