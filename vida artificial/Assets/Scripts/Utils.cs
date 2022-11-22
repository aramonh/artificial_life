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

    public bool isObjectInRange(Vector3 position, float range)
    {
        Collider[] intersecting = Physics.OverlapSphere(position, range);
        if (intersecting.Length == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public Collider[] whatsObjectsInRange(Vector3 position, float range)
    {
        Collider[] intersecting = Physics.OverlapSphere(position, range);
        return intersecting;
        
    }


    public float AngleInRad(Vector3 vec1, Vector3 vec2) {
        return Mathf.Atan2(vec2.x - vec1.x, vec2.z - vec1.z );
    }
    
    public float AngleInDeg(Vector3 vec1, Vector3 vec2) {
        return FormatAngle(AngleInRad(vec1, vec2) * (180 / Mathf.PI));
    }

    public float FormatAngle(float angle)
    {
        if(angle >= 360){
            return angle % 360;
        }else if( angle < 0 ){
            return FormatAngle(360+angle);
        }else{
            return angle;
        }
    }

}
