using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlant : MonoBehaviour
{

    /*
    axiom = F
    F -> FF+[+F-F-F]-[-F+F+F]
    angle = 22.5
    */

    public int iterations = 3;

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
    
        {"F", turtle => turtle.Translate(delta:new Vector3(x:0,y:0.1f,z:0))},
        {"+", turtle => turtle.Rotate(delta:new Vector3(x:Random.Range(23f,27f),y:0,z:0))},
        {"-", turtle => turtle.Rotate(delta:new Vector3(x:Random.Range(-23f,-27f),y:0,z:0))},
        {"[", turtle => turtle.Push()},
        {"]", turtle => turtle.Pop()},
    
    };

    // Start is called before the first frame update
    void Start()
    {

        var lSystem = new LSystem(axiom , ruleset, commands , transform.position);
        
        for(int i = 0 ; i<= iterations ; i++ ){
            Debug.Log(message: lSystem.GenerateSentence() );
        }
        lSystem.DrawSystem();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
