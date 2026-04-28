using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    
    public GameObject Camera_1;
    public GameObject Camera_2;
    public int Manager = 1;

    public void ManageCamera()
    {
        if(Manager == 2)
        {
            Cam_2();
        }
        else
        {
            Cam_1();
        }
    }

    void Cam_1()
    {
        Camera_1.SetActive(true);
        Camera_2.SetActive(false);
    }

    void Cam_2()
    {
        Camera_1.SetActive(false);
        Camera_2.SetActive(true);
    }
    
    public void switchCamera(int player)
    {
        Manager = player;
        ManageCamera();
    }

}
