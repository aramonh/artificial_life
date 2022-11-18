using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils 
{
    public bool isObjectHere(Vector3 position)
    {
        Collider[] intersecting = Physics.OverlapSphere(position, 0.01f);
        if (intersecting.Length == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public Collider[] whatsObjectsHere(Vector3 position)
    {
        Collider[] intersecting = Physics.OverlapSphere(position, 0.01f);
        return intersecting;
        
    }

}
