using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gm;
    public GameObject pickupParticleSystem;
    public GameObject explosionParticleSystem;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            gm.IncrementCollectiblesCollected();
            var pickupParticles = Instantiate(pickupParticleSystem, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            StartCoroutine(deleteGameObjectAfterSeconds(pickupParticles, 3));
            gm.playCollectibleSound();
        }
        else if (other.CompareTag("Obstacle"))
        {
            gm.PlayerHitObstacle();
            Instantiate(explosionParticleSystem, this.transform.position, Quaternion.identity);

        }
    }


    IEnumerator deleteGameObjectAfterSeconds(GameObject objectToDestroy, int timeToWaitInSeconds)
    {
        yield return new WaitForSeconds(timeToWaitInSeconds);
        Destroy(objectToDestroy);
    }
}