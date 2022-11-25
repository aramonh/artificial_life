using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLifeCycle : MonoBehaviour
{
    private Utils utils = new Utils();

    public GameObject resource; 

    public int state = 0; 
    public int maxState = 4; 
    public int groundLimit = 10; 

    private Renderer rend ;
    private Color[] colors;

    private GameObject fowardChild;
    private GameObject backChild;
    private GameObject leftChild;
    private GameObject rightChild;



    // Fractal System

    private string axiom ="F";

    private Dictionary<string, string> ruleset = new Dictionary<string, string>
    {
        {"F","FF+[+F-F-F]-[-F+F+F]"}
    };
    

    private Dictionary<string, System.Action<Turtle>> commands = new Dictionary<string, System.Action<Turtle>>
    {
        //<sumary>
        // F move foward
        // + move left
        // - move right
        // [ Push current stat oto stack
        // ] Pop current drawing
    
        {"F", turtle => turtle.Translate(delta:new Vector3(x:0,y:0.05f,z:0))},
        {"+", turtle => turtle.Rotate(delta:new Vector3(x:Random.Range(23f,27f),y:0,z:0))},
        {"-", turtle => turtle.Rotate(delta:new Vector3(x:Random.Range(-23f,-27f),y:0,z:0))},
        {"[", turtle => turtle.Push()},
        {"]", turtle => turtle.Pop()},
    
    };






    // Start is called before the first frame update
    void Awake() {
        rend = GetComponent<Renderer>();

        colors = new Color[5];
        colors[0] = Color.red;
        colors[1] = Color.yellow;
        colors[2] = Color.white;
        colors[3] = Color.green;
        colors[4] = Color.blue;

        state = 1;
    }


    void Start()
    {
        ChangeSprite(state);
        ChangeSkin(state);
    }

    // Update is called once per frame
    void Update()
    {
        if(state > maxState){
            state = 0;
            GeneratorNeighbours();
        }
        ChangeSprite(state);
        ChangeSkin(state);
    }


    private void ChangeSprite(int state)
    {
        rend.material.color = colors[state];
    }

    private void ChangeSkin(int iterations)
    {
        
        // var lSystem = new LSystem(axiom , ruleset, commands , transform.position);
        
        // for(int i = 0 ; i<= iterations ; i++ ){
        //     lSystem.GenerateSentence() ;
        // }
        // lSystem.DrawSystem();
    }


    private void GeneratorNeighbours()
    {
        // foward
        Vector3 cord = transform.position + ( Vector3.forward );
        if(cord.z < groundLimit){// check ground limit
            if(utils.isObjectHere(cord)==false){// check if exist object in new position
                GameObject child = Instantiate(resource, cord, Quaternion.identity); // create new resouce in new position
                child.name = gameObject.name + "F";
                child.tag = "Resource";
            }else{
                var colliders = utils.whatsObjectsHere(cord);
                foreach (var item in colliders)
                {
                    if (item.transform.position == cord) {
                        ResourceLifeCycle resourceLifeCycle = item.GetComponent<ResourceLifeCycle>();
                        resourceLifeCycle.state += 1;
                        break;
                    }
                }
            }
        }
        // right
        cord = transform.position + ( Vector3.right );
        if(cord.x < groundLimit ){// check ground limit
            if(utils.isObjectHere(cord)==false){// check if exist object in new position
                GameObject child = Instantiate(resource, cord, Quaternion.identity); // create new resouce in new position
                child.name = gameObject.name + "R";
                child.tag = "Resource";
            }else{
                var colliders = utils.whatsObjectsHere(cord);
                foreach (var item in colliders)
                {
                    if (item.transform.position == cord) {
                        ResourceLifeCycle resourceLifeCycle = item.GetComponent<ResourceLifeCycle>();
                        resourceLifeCycle.state += 1;
                        break;
                    }
                }
            }
        }
        // left
        cord = transform.position + ( Vector3.left );
        if(cord.x > -groundLimit ){// check ground limit
            if(utils.isObjectHere(cord)==false){// check if exist object in new position
                GameObject child = Instantiate(resource, cord, Quaternion.identity); // create new resouce in new position
                child.name = gameObject.name + "L";
                child.tag = "Resource";
            }else{
                var colliders = utils.whatsObjectsHere(cord);
                foreach (var item in colliders)
                {
                    if (item.transform.position == cord) {
                        ResourceLifeCycle resourceLifeCycle = item.GetComponent<ResourceLifeCycle>();
                        resourceLifeCycle.state += 1;
                        break;
                    }
                }
            }
        }
        // back
        cord = transform.position + ( Vector3.back );
        if(cord.z > -groundLimit ){// check ground limit
            if(utils.isObjectHere(cord)==false){// check if exist object in new position
                GameObject child = Instantiate(resource, cord, Quaternion.identity); // create new resouce in new position
                child.name = gameObject.name + "B";
                child.tag = "Resource";
            }else{
                var colliders = utils.whatsObjectsHere(cord);
                foreach (var item in colliders)
                {
                    if (item.transform.position == cord) {
                        ResourceLifeCycle resourceLifeCycle = item.GetComponent<ResourceLifeCycle>();
                        resourceLifeCycle.state += 1;
                        break;
                    }
                }
            }
        }

    }

}
