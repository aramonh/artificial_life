using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterLifeCycle : MonoBehaviour
{
    private Utils utils = new Utils();


    public int countEats = 0;


    public float timerReDirect = 1;
    private float timerReDirectRemaining;
    private float timerReDirectConstant;

    public float timerCanEat = 15;
    private float timerCanEatRemaining;
    private float timerCanEatConstant;

    public bool CanEat = false;
    private bool CanReDirect = true;

    private Vector3 lastPosition = Vector3.zero;
    private Vector3 position = Vector3.zero;

    private bool IsMoved = false;
    private float movementSpeed = 3;

    private Animator anim;
    private Rigidbody rb;


    public float direction = 90;
    public float vision = 5.0f;

    public Collider presa = null;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        position = transform.position;
        lastPosition = transform.position;
        timerCanEatRemaining=timerCanEat;
        timerCanEatConstant=timerCanEat;
        timerReDirectRemaining=timerReDirect;
        timerReDirectConstant=timerReDirect;
    }

    // Update is called once per frame

    void Update()
    {
        TimerReDirect();
        TimerEat();
        MoveCheck();
        CheckWalls();
        BoidsDirection();
        Walk();

        if(IsMoved==false){
            Stop();
        }
    }

    void BoidsDirection()
    {
        float angle = -999;


        if(CanEat == true){
            angle = SearchFood();
        }

        if(angle == -999)
        {
            angle = Random.Range(0.0f, 360.0f);
        }
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
                if (item.tag == "Agend") 
                {   
                    if(presa == null){
                        presa = item;
                        if(1 >= Vector3.Distance(transform.position, item.transform.position))
                        {
                            Destroy(item.gameObject);
                            Eat();
                            ChangeAngle(direction+Random.Range(100.0f, 260.0f));
                            Rotate();
                        }else{
                            return utils.AngleInDeg(transform.position, item.transform.position);
                        }
                    }else{
                        if(presa == item){
                            if(1 >= Vector3.Distance(transform.position, item.transform.position))
                            {
                                Destroy(item.gameObject);
                                Eat();
                                ChangeAngle(direction+Random.Range(100.0f, 260.0f));
                                Rotate();
                            }else{
                                return utils.AngleInDeg(transform.position, item.transform.position);
                            }
                        }
                    }

                }
            }
        }
        presa = null;
        return -999;
    }

    void Eat()
    {
    countEats++;
    CanEat = false;
    timerCanEatRemaining = timerCanEatConstant;
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
                    if(CanEat){
                        if(item.tag != "Ground" && item.tag != "Agend") 
                        {
                            if ( CanReDirect )
                            {
                                ChangeAngle(direction+Random.Range(100.0f, 260.0f));
                                CanReDirect = false;
                                break;
                            }
                        }
                    }else{
                        if(item.tag != "Ground" ) 
                        {
                            if ( CanReDirect )
                            {
                                ChangeAngle(direction+Random.Range(100.0f, 260.0f));
                                CanReDirect = false;
                                break;
                            }
                        }
                    }

                }
            }
        }
        Rotate();
    }



    void TimerReDirect()
    {
        if (timerReDirectRemaining > 0)
        {
            timerReDirectRemaining -= Time.deltaTime;
        }
        else
        {
            CanReDirect = true;
            timerReDirectRemaining = timerReDirectConstant;
        }
    }

        void TimerEat()
    {
        if (timerCanEatRemaining > 0)
        {
            timerCanEatRemaining -= Time.deltaTime;
        }
        else
        {
            CanEat = true;
            timerCanEatRemaining = timerCanEatConstant;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }


    void Walk()
    {
        anim.SetInteger("Walk", 1);
        transform.Translate(transform.forward * movementSpeed * Time.deltaTime, Space.World);
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
