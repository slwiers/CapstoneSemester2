using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    float[] rotations = { 0, 90, 180, 270 };

    public float[] correctRotation;
    [SerializeField]
    bool isPlaced = false;

    int PossibleRots = 1;

    ManagePipes managePipes;

    private void Awake()
    {
        managePipes = GameObject.Find("ManagePipes").GetComponent<ManagePipes>();
    }

    //Start is called before the first frame update
    void Start()
    {
        PossibleRots = correctRotation.Length;
        int rand = Random.Range(0, rotations.Length);
        transform.eulerAngles = new Vector3(0, 0, rotations[rand]);

        if(PossibleRots > 1)
        {
            if (transform.eulerAngles.z == correctRotation[0] || transform.eulerAngles.z == correctRotation[1])
            {
                isPlaced = true;
                managePipes.correctMove();
            }
        }
        else
        {
            if (transform.eulerAngles.z == correctRotation[0])
            {
                isPlaced = true;
                managePipes.correctMove();
            }
        }
        
    }

    private void OnMouseDown()
    {
        transform.Rotate(new Vector3(0, 0, 90));
        if (PossibleRots > 1)
        {
            if (transform.eulerAngles.z == correctRotation[0] || transform.eulerAngles.z == correctRotation[1] && isPlaced == false)
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
            if (transform.eulerAngles.z == correctRotation[0] && isPlaced == false)
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
