using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgendsCycleLife : MonoBehaviour
{
    private Utils utils = new Utils();

    
    public  float timerEnergy = 1;
    private float timerEnergyRemaining;
    private float timerEnergyConstant;

    private Vector3 lastPosition = Vector3.zero;
    private bool IsMoved = false;


    public float movementSpeed = 3;
    Animator anim;
    Rigidbody rb;

    public Vector3 position = Vector3.zero;
    public int direction = 90;

    public int vision = 3;

    public int maxEnergy = 10;
    public int energy = 1;
    public float foodPriority = 0.5f; // 0-1 Range


    
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        position = transform.position;
        lastPosition = transform.position;
        timerEnergyRemaining=timerEnergy;
        timerEnergyConstant=timerEnergy;
    }

    void Update()
    {
        moveCheck();


        if(energy>0){
            BoidsDirection();
            CheckWalls();
            Walk();
        }else{
            // TO DO 
            //Die
            Debug.Log("DIE");
        }

        //If not moving IDLE state
        if(IsMoved==false){
            Stop();
        }
    }

    void BoidsDirection(){
        int angle = 0;
        int angleA = 0;
        int angleB = 0;
        int angleC = 0;
        // Angles Prioritys Food and Sex
        if(energy > (maxEnergy*foodPriority)){
            //Search Sex
            Debug.Log("FindSex");
            // TO DO 
            // 1.Search somebody inside vision range
            // 1.YES - set angle to him direction
            // 1.NOT - set random angle
            //
        }else{
            //Search food
            Debug.Log("FindFood");
            // TO DO 
            // Search somebody inside vision range
            // YES - set angle to him direction
            // NOT - set random angle
        }
        // Positions trends from anothers chickens
        // Positions contras from depredators
        angle = ((angleA + angleB + angleC)/3);
        //Set new direction to rotate
        //ChangeAngle(angle);
        //Rotate the agend
        //Rotate();
    }

    void CheckWalls(){
        int angle = 180;
        // TO DO - check what direction wall faces to set opposite angle 
        Vector3 cord = transform.position + ( Vector3.forward );
        if(utils.isObjectHere(cord)){// check if exist object in new position
            var colliders = utils.whatsObjectsHere(cord);
            foreach (var item in colliders)
            {
                Debug.Log(item.tag);
                if (item.tag == "Wall") {
                    //Set new direction to rotate
                    ChangeAngle(direction+angle);
                    break;
                }
            }
            Rotate();
            
        }
        cord = transform.position + ( Vector3.back );
        if(utils.isObjectHere(cord)){// check if exist object in new position
            var colliders = utils.whatsObjectsHere(cord);
            foreach (var item in colliders)
            {
                Debug.Log(item.tag);
                if (item.tag == "Wall") {
                    //Set new direction to rotate
                    ChangeAngle(direction+angle);
                    break;
                }
            }
            Rotate();
            
        }
        cord = transform.position + ( Vector3.left );
        if(utils.isObjectHere(cord)){// check if exist object in new position
            var colliders = utils.whatsObjectsHere(cord);
            foreach (var item in colliders)
            {
                Debug.Log(item.tag);
                if (item.tag == "Wall") {
                    //Set new direction to rotate
                    ChangeAngle(direction+angle);
                    break;
                }
            }
            Rotate();
            
        }
        cord = transform.position + ( Vector3.right );
        if(utils.isObjectHere(cord)){// check if exist object in new position
            var colliders = utils.whatsObjectsHere(cord);
            foreach (var item in colliders)
            {
                Debug.Log(item.tag);
                if (item.tag == "Wall") {
                    //Set new direction to rotate
                    ChangeAngle(direction+angle);
                    break;
                }
            }
            Rotate();
            
        }


    }

    void ChangeAngle(int angle){
        if(angle >= 360){
            direction = angle % 360;
        }else{
            direction = angle;
        }
    }

    void Rotate(){
        transform.rotation = Quaternion.Euler(new Vector3(0,direction,0));
    }

    void Walk()
    {
        anim.SetInteger("Walk", 1);
        transform.Translate(transform.forward * movementSpeed * Time.deltaTime, Space.World);
        if (timerEnergyRemaining > 0)
        {
            timerEnergyRemaining -= Time.deltaTime;
        }
        else
        {
            energy -= 1;
            timerEnergyRemaining = timerEnergyConstant;
        }
        
    }

    void Stop(){
        position = transform.position;
        anim.SetInteger("Walk", 0);
    }

    void moveCheck()
    {
        Vector3 currentPosition = transform.position;
        IsMoved  = (currentPosition != lastPosition);
        lastPosition = currentPosition;
    }

}


