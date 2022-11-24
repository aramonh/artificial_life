using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgendsLifeCycle : MonoBehaviour
{
    private Utils utils = new Utils();

    public GameObject agend; 

    public float timerEnergy = 5;
    private float timerEnergyRemaining;
    private float timerEnergyConstant;

    public float timerSex = 5;
    private float timerSexRemaining;
    private float timerSexConstant;
    private bool CanSex = true;

    private Vector3 lastPosition = Vector3.zero;
    private Vector3 position = Vector3.zero;

    private bool IsMoved = false;
    private float movementSpeed = 3;

    private Animator anim;
    private Rigidbody rb;

    private const string STATE_EAT = "EAT";
    private const string STATE_SEX = "SEX";
    private const string STATE_WALK = "WALK";
    public string state = "";


    public float direction = 90;
    public float vision = 3.0f;

    public int maxEnergy = 10;
    public int energy = 1;
    public float foodPriority = 0.5f; // 0-1 Range


    
    void Start()
    {
        state = STATE_WALK;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        position = transform.position;
        lastPosition = transform.position;
        timerEnergyRemaining=timerEnergy;
        timerEnergyConstant=timerEnergy;
        timerSexRemaining=timerSex;
        timerSexConstant=timerSex;
    }

    void Update()
    {
        SexTimer();
        MoveCheck();
        if(energy>=1){
            CheckWalls();
            BoidsDirection();
            Walk();
        }else{
            Destroy(gameObject);
        }

        if(IsMoved==false){
            Stop();
        }
    }

    void BoidsDirection()
    {
        float angle = -999;
        float angleA = -999;
        float angleB = -999;
        float angleC = -999;
        // Angles Prioritys Food and Sex
        if(energy > (maxEnergy*foodPriority)){
            CheckFood();
            angleA = SearchSex();
        }else{
            angleA = SearchFood();
        }
        if(angleA == -999)
        {
            angleA = Random.Range(0.0f, 360.0f);
        }
        // Positions trends from anothers chickens
        // Positions contras from depredators
        //angle = ((angleA + angleB + angleC)/3);
        angle = angleA;
        float diff = utils.FormatAngle(angle - direction);
        if(diff != 0){ 
            if(diff <= 180){
                ChangeAngle(direction+1);
            }else{
                ChangeAngle(direction-1);
            }
            Rotate();
        }
    }

    float SearchFood()
    {
        if(utils.isObjectInRange(transform.position, vision))
        {
            var colliders = utils.whatsObjectsInRange(transform.position, vision);
            foreach (var item in colliders)
            {
                if (item.tag == "Resource") 
                {   
                    ResourceLifeCycle resourceLifeCycle = item.GetComponent<ResourceLifeCycle>();
                    if(resourceLifeCycle.state > 0){
                        if(1 >= Vector3.Distance(transform.position, item.transform.position))
                        {
                            state = STATE_EAT;
                            int diff = maxEnergy - energy;
                            if(resourceLifeCycle.state > diff){
                                energy += diff;
                                resourceLifeCycle.state -= diff;
                            }else{
                                energy += resourceLifeCycle.state;
                                Destroy(item.gameObject);
                            }
                            ChangeAngle(direction+Random.Range(100.0f, 260.0f));
                            Rotate();
                            state = STATE_WALK;
                        }else{
                            state = STATE_WALK;
                            return utils.AngleInDeg(transform.position, item.transform.position);
                        }
                    }
                    
                }
            }
        }
        state = STATE_WALK;
        return -999;
    }

    float SearchSex()
    {
        if(utils.isObjectInRange(transform.position, vision))
        {
            var colliders = utils.whatsObjectsInRange(transform.position, vision);
            foreach (var item in colliders)
            {
                if (item.tag == "Agend" && item.name != name) 
                {
                    AgendsLifeCycle agendsLifeCycle = item.GetComponent<AgendsLifeCycle>();
                    if(1 > Vector3.Distance(transform.position, item.transform.position))
                    {       
                        state = STATE_SEX;
                        agendsLifeCycle.CanSex = false;
                        agendsLifeCycle.timerSexRemaining = agendsLifeCycle.timerSexConstant;
                        if( CanSex == true ){
                            GameObject son = Instantiate(agend,( transform.position + Vector3.back + Vector3.back ), Quaternion.identity); // create new resouce in new position
                            son.name = name + agendsLifeCycle.name;
                            son.tag = "Agend";
                            AgendsLifeCycle child = son.GetComponent<AgendsLifeCycle>();
                            child.vision = ((vision + agendsLifeCycle.vision)/2);
                            child.maxEnergy = ((maxEnergy + agendsLifeCycle.maxEnergy)/2);
                            child.energy = ((energy + agendsLifeCycle.energy)/2);
                            child.foodPriority = ((foodPriority + agendsLifeCycle.foodPriority)/2);
                            energy = (energy/2);
                        }
                        ChangeAngle(direction+Random.Range(100.0f, 260.0f));
                        Rotate();
                        state = STATE_WALK;
                    }else{
                        if(agendsLifeCycle.energy > (agendsLifeCycle.maxEnergy*agendsLifeCycle.foodPriority)){
                            state = STATE_WALK;
                            return utils.AngleInDeg(transform.position, item.transform.position);
                        }
                    }
                }
            }
        }
        state = STATE_WALK;
        return -999;
    }

    float SearchDanger()
    {
        if(utils.isObjectInRange(transform.position, vision))
        {
            var colliders = utils.whatsObjectsInRange(transform.position, vision);
            foreach (var item in colliders)
            {
                if (item.tag == "Hunter") 
                {
                    return utils.AngleInDeg(transform.position, item.transform.position);
                }
            }
        }
        return -999;
    }



    void CheckWalls()
    {   
        Vector3[] cardinales = { Vector3.forward , Vector3.back ,  Vector3.left ,Vector3.right };
        foreach(var cardinal in cardinales)
        {
            Vector3 cord = transform.position + ( cardinal );
            if(utils.isObjectHere(cord)){
                var colliders = utils.whatsObjectsHere(cord);
                foreach (var item in colliders)
                {
                    if (item.tag == "Wall" ) 
                    {
                        ChangeAngle(direction+Random.Range(100.0f, 260.0f));
                        break;
                    }
                }
            }
        }
        Rotate();
    }

    void CheckFood()
    {   
        Vector3[] cardinales = { Vector3.forward , Vector3.back ,  Vector3.left ,Vector3.right };
        foreach(var cardinal in cardinales)
        {
            Vector3 cord = transform.position + ( cardinal );
            if(utils.isObjectHere(cord)){
                var colliders = utils.whatsObjectsHere(cord);
                foreach (var item in colliders)
                {
                    if (item.tag == "Resource" ) 
                    {
                        ChangeAngle(direction+Random.Range(100.0f, 260.0f));
                        break;
                    }
                }
            }
        }
        Rotate();
    }

    void Die()
    {
            Debug.Log("DIE");
    }


    void Walk()
    {
        if(state == STATE_WALK)
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
    }

    void SexTimer(){
            if (timerSexRemaining > 0)
            {
                timerSexRemaining -= Time.deltaTime;
            }
            else
            {
                CanSex = true;
                timerSexRemaining = timerSexConstant;
            }
    }


    void ChangeAngle(float angle)
    {
        direction = utils.FormatAngle(angle);
    }

    void Rotate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0,direction,0));
    }

    void Stop()
    {
        position = transform.position;
        anim.SetInteger("Walk", 0);
    }

    void MoveCheck()
    {
        Vector3 currentPosition = transform.position;
        IsMoved  = (currentPosition != lastPosition);
        lastPosition = currentPosition;
    }

}


