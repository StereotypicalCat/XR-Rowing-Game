using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    public float bobbingSpeed = 2;
    public float bobbingDistance = 10;
    public float initialPosition;
    public bool goDownwards = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        initialPosition = this.gameObject.transform.position.y;
    }

    private void Start()
    {
        // Initially randomize the objects cycle in the bobbing..
        this.gameObject.transform.Translate(0, (UnityEngine.Random.value - 0.5f) * bobbingDistance, 0, Space.World );
    }

    // Update is called once per frame
    void Update()
    {
        // Decide if the object is to go up or down.
        if (this.gameObject.transform.position.y > (initialPosition + bobbingDistance))
        {
            goDownwards = true;
        }
        else if (this.gameObject.transform.position.y < (initialPosition - bobbingDistance))
        {
            goDownwards = false;
        }

        // Move the object up or down.
        if (goDownwards)
        {
            this.transform.Translate(0, -bobbingSpeed, 0, Space.World);    
        }
        else
        {
            this.transform.Translate(0, bobbingSpeed, 0, Space.World);  
        }
        
        
        
        
        
    }
}
