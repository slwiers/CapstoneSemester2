using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    int [] rotations = { 0, 1, 2,3 };

    public float[] correctRotation;
    [SerializeField]
    bool isPlaced = false;

    int PossibleRots = 1;

    ManagePipes managePipes;
    int currentRotation = 0;

    private void Awake()
    {
        managePipes = GameObject.Find("ManagePipes").GetComponent<ManagePipes>();
    }

    //Start is called before the first frame update
    void Start()
    {
        PossibleRots = correctRotation.Length;
        int rand = Random.Range(0, rotations.Length);
        currentRotation = rand;
        transform.eulerAngles = new Vector3(0, 0, rotations[currentRotation] *90);
        //checkRotation();
        if (PossibleRots > 3)
        {
            isPlaced = true;
            managePipes.correctMove();
        }
        else if (PossibleRots > 1)
        {
            if (currentRotation == correctRotation[0] || currentRotation == correctRotation[1])
            {
                isPlaced = true;
                managePipes.correctMove();
            }
        }
        else
        {
            if (currentRotation == correctRotation[0])
            {
                isPlaced = true;
                managePipes.correctMove();
            }
        }
    }

    void checkRotation()
    {
        Debug.Log("Check Rotation");
    }

    private void OnMouseDown()
    {
        currentRotation = (currentRotation + 1) % 4;
        transform.eulerAngles = new Vector3(0, 0, rotations[currentRotation] * 90);
        Debug.Log("transform.eulerAngles " + transform.eulerAngles);
        if (PossibleRots > 3)
        {
            isPlaced = true;
            managePipes.correctMove();
        }
        else if (PossibleRots > 1)
        {
            if ((currentRotation == correctRotation[0] || currentRotation == correctRotation[1]) && isPlaced == false)
            {
                isPlaced = true;
                managePipes.correctMove();
            }
            else if (isPlaced == true)
            {
                isPlaced = false;
                managePipes.wrongMove();
            }
        }
        else
        {
            if (currentRotation == correctRotation[0] && isPlaced == false)
            {
                isPlaced = true;
                managePipes.correctMove();
            }
            else if (isPlaced == true)
            {
                isPlaced = false;
                managePipes.wrongMove();
            }
        }
    }
}
