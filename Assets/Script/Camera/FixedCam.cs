using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCam : MonoBehaviour
{
    [SerializeField]
    private Transform TragetFollow;
    private float maxX=3;//Right Side
    private float minX=-10;//Left side 
    private float maxY=2;//Upside
    private float minY=0;//DownSide


    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector3(
            Mathf.Clamp(TragetFollow.position.x,minX,maxX),
            Mathf.Clamp(TragetFollow.position.y,minY,maxY),
            transform.position.z);
    }
}
