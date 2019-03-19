using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gm;
    public GameObject pickupParticleSystem;

    public enum direction
    {
        Left,
        Right
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            gm.IncrementCollectiblesCollected();
            var pickupParticles = Instantiate(pickupParticleSystem, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            StartCoroutine(deleteGameObjectAfterSeconds(pickupParticles, 3));


        }
        else if (other.CompareTag("Obstacle"))
        {
            gm.PlayerHitObstacle();
        }   
    }

    public void turn(direction turnDirection)
    {
        if (turnDirection == direction.Left)
        {
            print("Turning: Left");
            gm.speed = new Vector3(1, 0, 5f);
            
        }

        else
        {
            print("Turning right");
            gm.speed = new Vector3(1, 0, -5f);

        }
        
    }


    IEnumerator deleteGameObjectAfterSeconds(GameObject objectToDestroy, int timeToWaitInSeconds)
    {
        yield return new WaitForSeconds(timeToWaitInSeconds);
        Destroy(objectToDestroy);
    }
    
    
}
