using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{

    public GameObject gameArea;
    public GameObject boat;

    public int collectiblesCollected = 0;
    public TMP_Text collectiblesUiElement;

    public SoundManager soundMan;
    
    
    // For controlling the current theme
    public enum Theme
    {
        Chocolate,
        Candy
    }

    
    // Boat moves forward
    [SerializeField] public bool shouldMoveForwards = true;

    public float forwardSpeed = 1;
    public float sidewaysSpeed = 0.5f;
    public Player.direction direction;

    // Boat goes up and down variables
    [SerializeField] public bool shouldViewBobbing = true;
    public float newYPosition;
    public float bobbingRange = 10;
    public float bobbingAmount = 0.3f;
    public float originalYPosition;
    
    // Water moves correctly.
    public GameObject[] waters;
    public int currentWater = 0;
    public int waterDistanceDeadzone = 100;
    public const int WATER_LENGTH = 2000;

    
    // Spawning objects
    public bool shouldSpawnObjects = true;
    public const int WATER_WIDTH = 1000;
    
    public float collectibleSpawnRatePercentage = 0.2f;
    public float collectibleMinDistanceFromPlayer = 400;
    public float collectibleSpawnDistanceRange = 600;

    public GameObject[] Collectibles;
    
    
    public float obstacleSpawnRatePercentage = 0.05f;
    public float obstacleMinDistanceFromPlayer = 400;
    public float obstacleSpawnDistanceRange = 600;

    public GameObject[] Obstacles;


    // Start is called before the first frame update
    void Start()
    {
        collectiblesUiElement.text = $"Amount of collectibles: {collectiblesCollected}";
        
        originalYPosition = boat.transform.localPosition.y;
        newYPosition = originalYPosition;

        /*originalRotation = boat.transform.rotation.x;
        newRotation = originalRotation;*/


    }

    // Update is called once per frame
    void FixedUpdate()
    {


        // Movement

        if (shouldMoveForwards)
        {
            #region Bobbing

            if (shouldViewBobbing)
            {
                if (Math.Abs(boat.transform.localPosition.y - newYPosition) < bobbingAmount * 2)
                {
                    newYPosition = UnityEngine.Random.Range(originalYPosition, originalYPosition + bobbingRange);
                }
                else
                {
                    if (boat.transform.localPosition.y > newYPosition)
                    {
                        boat.transform.Translate(0, -bobbingAmount, 0);
                    }
                    else
                    {
                        boat.transform.Translate(0, bobbingAmount, 0);
                    }

                }

            }

            #endregion

            #region Tilting

            #endregion


            var newTranslatePosition = new Vector3(forwardSpeed, 0, 0);
            
            // Move player based on current direction.
            if (this.direction == Player.direction.Left)
            {
                newTranslatePosition.z = -sidewaysSpeed;
                // print("Turning : Left");
            }
            else if (this.direction == Player.direction.Right)
            {
                newTranslatePosition.z = -sidewaysSpeed;
              // print("Turning : Right");
            }
            
            gameArea.transform.Translate(newTranslatePosition);
        }

     
        

        // Obstacle & Collectible Spawning
        if (shouldSpawnObjects)
        {
            
            var randomValue = UnityEngine.Random.value;

            #region collectible


            if (randomValue <= collectibleSpawnRatePercentage)
            {
                var distanceFromPlayerToSpawnX = gameArea.transform.position.x + collectibleMinDistanceFromPlayer +
                                                (UnityEngine.Random.value * collectibleSpawnDistanceRange);

                var distanceFromPlayerToSpawnZ = WATER_WIDTH * UnityEngine.Random.value;

                Instantiate(Collectibles[0], new Vector3(distanceFromPlayerToSpawnX + gameArea.transform.position.x, 20, distanceFromPlayerToSpawnZ - WATER_WIDTH*0.5f), Quaternion.Euler( new Vector3(270, 0, 0)));
            }
            
            #endregion

            #region obstacle

            if (randomValue <= obstacleSpawnRatePercentage)
            {
                var distanceFromPlayerToSpawnX = gameArea.transform.position.x + obstacleMinDistanceFromPlayer +
                                                 (UnityEngine.Random.value * obstacleSpawnDistanceRange);

                var distanceFromPlayerToSpawnZ = WATER_WIDTH * UnityEngine.Random.value;

                Instantiate(Obstacles[0], new Vector3(distanceFromPlayerToSpawnX + gameArea.transform.position.x, 0, distanceFromPlayerToSpawnZ - WATER_WIDTH*0.5f), Quaternion.Euler( new Vector3(90, 0, 90)));
            }

            #endregion

        }



        // Water Movement

        #region Water Movement
        // To test that the following value is positive
        if (gameArea.transform.position.x > waters[currentWater].transform.position.x + WATER_LENGTH)
        {
            
            // To test the distance between two floats.
            var currentDistanceBetweenBoatAndCurrentWater =
                Mathf.Abs(gameArea.transform.position.x - waters[currentWater].transform.position.x);
            if (currentDistanceBetweenBoatAndCurrentWater > waterDistanceDeadzone)
            {
                // Moving the water just enough so it is seamless.
                waters[currentWater].transform.Translate(WATER_LENGTH * waters.Length-1, 0, 0, Space.World);

                // Reset the counter to not get out of bounds.
                if (++currentWater > waters.Length - 1)
                {
                    currentWater = 0;
                }

            }
            
        }
        #endregion



    }

    public void turn(Player.direction turnDirection)
    {
        this.direction = turnDirection;
    }
    public void SpawnWater()
    {

    }

    public void IncrementCollectiblesCollected()
    {
        collectiblesCollected++;
        collectiblesUiElement.text = $"Amount of collectibles: {collectiblesCollected}";
    }

    public void PlayerHitObstacle()
    {
        shouldMoveForwards = false;
        soundMan.playObstacleSound();
        soundMan.playDeadSound();
    }

    public void playCollectibleSound()
    {
        soundMan.playCollectibleSound();
    }
    
    
}
