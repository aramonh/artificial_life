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

    public Mesh mesh1 = null; 
    public Mesh mesh2 = null; 
    public Mesh mesh3 = null; 
    public Mesh mesh4 = null; 



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

    private void ChangeSkin(int state)
    {
        
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Debug.Log(mesh);

        switch (state)
        {
        case 4:
            mesh = mesh4;
            break;
        case 3:
            mesh = mesh3;
            break;
        case 2:
            mesh = mesh2;
            break;
        case 1:
            mesh = mesh1;
            break;
        default:
            mesh = mesh1;
            break;
        }

        GetComponent<MeshFilter>().mesh = mesh;
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
