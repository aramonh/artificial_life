using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesGenerator : MonoBehaviour
{    
    private Utils utils = new Utils();

    public GameObject resource;

    public  float timerStations = 60;
    private float timerStationsRemaining;
    private float timerStationsConstant;

    public  float timerResources = 2;
    private float timerResourcesRemaining;
    private float timerResourcesConstant;

    private string stationA = "A";
    private string stationB = "B";
    private string currentStation = "";

    void Awake() {
        currentStation = stationA;

        timerStationsRemaining = timerStations;
        timerStationsConstant = timerStations;

        timerResourcesRemaining = timerResources;
        timerResourcesConstant = timerResources;
    }

    void Start()
    {
        Generator();
    }

    void Update()
    {
        Timer();
    }


    void Timer(){
        // Stations Timer
        if (timerStationsRemaining > 0)
        {
            timerStationsRemaining -= Time.deltaTime;
        }
        else
        {
            ChangeStations();
            timerStationsRemaining = timerStationsConstant;
        }

        // Resources Timer
        if (timerResourcesRemaining > 0)
        {
            timerResourcesRemaining -= Time.deltaTime;
        }
        else
        {
            Generator();
            timerResourcesRemaining = timerResourcesConstant;
        }

    }

    void ChangeStations()
    {
        // Cambiar de stations cada que finalice el timer
        if(currentStation == stationA){
            currentStation = stationB;
        }else{
            currentStation = stationA;
        }
    }

    void Generator()
    {
        if(currentStation == stationA){
            // Crear recurso central en ubicacion A
            Vector3 cord = new Vector3(-5, 0.5f, -5);
            if( utils.isObjectHere(cord) == false ){
                GameObject child = Instantiate(resource , cord, Quaternion.identity); // create new resouce in new position
                child.name = "ResourceA";

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
        }else{
            // Crear recurso central en ubicacion B
            Vector3 cord = new Vector3(5, 0.5f, 5);
            if(utils.isObjectHere(cord)==false){
                GameObject child = Instantiate(resource , cord, Quaternion.identity); // create new resouce in new position
                child.name = "ResourceB";
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
