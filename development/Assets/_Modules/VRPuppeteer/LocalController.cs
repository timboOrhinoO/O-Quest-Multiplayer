using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalController : MonoBehaviour
{
    public Transform localHead, remoteHead;
    public Transform localHandR, remoteHandR;
    public Transform localHandL, remoteHandL;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (localHead && remoteHead) 
            CopyTransform(localHead, remoteHead);
        if (localHandR && remoteHandR) 
            CopyTransform(localHandR, remoteHandR);
        if (localHandL && remoteHandL) 
            CopyTransform(localHandL, remoteHandL);
    }

    void CopyTransform(Transform source, Transform target){
        target.position = source.position;
        target.rotation = source.rotation;
    }
}
