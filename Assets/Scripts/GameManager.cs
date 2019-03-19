using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject gameArea;
    public GameObject boat;

    public int collectiblesCollected = 0;
    public TMP_Text collectiblesUiElement;
    
    
    
    
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


            var newTranslatePosition = new Vector3();
            if (this.direction == Player.direction.Left)
            {
                newTranslatePosition = new Vector3(forwardSpeed, 0, -sidewaysSpeed);
                print("Turning : Left");
            }
            else if (this.direction == Player.direction.Right)
            {
                newTranslatePosition = new Vector3(forwardSpeed, 0, sidewaysSpeed);
                print("Turning : Right");

            }
            
            print(newTranslatePosition);


            gameArea.transform.Translate(newTranslatePosition);
        }

     
        

        // Obstacle Spawning




        // Water Movement



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
    }
    
}
