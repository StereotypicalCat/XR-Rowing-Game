using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public GameObject gameArea;
    public GameObject boat;

    public int collectiblesCollected = 0;
    public TMP_Text collectiblesUiElement;

    public SoundManager soundMan;

    // Boat
    public AnimationController animCntrl;
    public bool rightPlayerIsPaddling = false;
    public bool leftPlayerIsPaddling = false;

    // Make sure the boat stays inside of bounds
    public int gameAreaRightRocksZValue = -363;
    public int gameAreaLeftRocksZValue = 455;


    // Boat moves forward
    [SerializeField] public bool shouldMoveForwards = true;

    public float forwardSpeed = 1;
    public float sidewaysSpeed = 0.5f;
    public Enums.direction direction;
    public float rowingSpeedMultiplierForward = 4f;
    public float rowingSpeedMultiplierSideways = 2f;

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

    // At this point only gummy bears
    public GameObject[] Collectibles;


    public float obstacleSpawnRatePercentage = 0.05f;
    public float obstacleMinDistanceFromPlayer = 400;
    public float obstacleSpawnDistanceRange = 600;

    public GameObject[] Obstacles;
    public float relativeSpawnChanceDonut = 5;
    public float relativeSpawnChanceOctopus = 15;
    
    // Used to make spawn chance depend on last obstacle spawn.
    public float distanceAtLastObstacleSpawn = 0;
    public float minimumDistanceToHaveTravelledBeforeNewObjectSpawn = 100;
    public float scalerMultiplication = 0.001f;


    // Speeding Up
    public int collectiblesToCollectToSpeedUp = 10;
    public float amountToSpeedUp = 0.1f;

    // Day-night cycle
    public bool shouldDoDayNightCycle = true;
    public Material skyboxDay;
    public Material skyboxNight;
    public int DayLength = 4;
    public bool nextShiftIsDay = false;
    public float timeAtLastSkyboxShift;


    // Menu and Game Over
    public Canvas gameCanvas;
    public Canvas gameOverCanvas;
    public Canvas firstNewGameCanvas;

    public bool isGameOver = false;
    public float gameOverWait = 2;
    private float gameOverTime = 0;
    public bool shouldStartGame = false;
    public TMP_Text HighscoresText;
    
    
    public TMP_Text teamNameTextUI;
    public string teamName;

    public NameGenerator nameGen;

    public HighScoreManager HSman;


    // Start is called before the first frame update
    void Start()
    {
        timeAtLastSkyboxShift = UnityEngine.Time.time;

        collectiblesUiElement.text = "0";

        originalYPosition = boat.transform.localPosition.y;
        newYPosition = originalYPosition;

        nameGen = this.gameObject.GetComponent<NameGenerator>();
        
        nameGen.InitiateNameGenerator();
        teamName = nameGen.generateTeamName();
        print("Team name is:" + teamName);
        teamNameTextUI.text = teamName;
    }

    // Update is called once per frame
    void Update()
    {
        if (!shouldStartGame)
        {
            gameCanvas.gameObject.SetActive(false);
            firstNewGameCanvas.gameObject.SetActive(true);

            if (leftPlayerIsPaddling && rightPlayerIsPaddling)
            {
                shouldStartGame = true;
                firstNewGameCanvas.gameObject.SetActive(false);
                gameOverCanvas.gameObject.SetActive(false);
                gameCanvas.gameObject.SetActive(true);
            }
        }
        else
        {
            if (!isGameOver)
            {
                // Movement

                
                if (shouldMoveForwards)
                {
                    #region Bobbing


                    #endregion

                    #region Tilting

                    #endregion

                    #region Find Paddle Direction

                    // EXNOR, If both are paddling or if noone is paddling.
                    if (leftPlayerIsPaddling == rightPlayerIsPaddling)
                    {
                        this.direction = Enums.direction.Forward;
                    }
                    else if (leftPlayerIsPaddling)
                    {
                        this.direction = Enums.direction.Left;
                    }
                    else
                    {
                        this.direction = Enums.direction.Right;
                    }

                    #endregion

                    #region Move Player based on paddling direction

                    var newTranslatePosition = new Vector3(forwardSpeed, 0, 0);

                    // Move player based on current direction.
                    if ((this.direction == Enums.direction.Forward) && (leftPlayerIsPaddling || rightPlayerIsPaddling))
                    {
                        newTranslatePosition.x += forwardSpeed * rowingSpeedMultiplierForward;
                    }

                    if (this.direction == Enums.direction.Left &&
                        gameArea.transform.position.z > gameAreaRightRocksZValue
                        )
                    {
                        newTranslatePosition.z = sidewaysSpeed;
                        // print("Turning : Left");
                        newTranslatePosition.x += forwardSpeed * rowingSpeedMultiplierSideways;
                    }
                    else if (this.direction == Enums.direction.Right && gameArea.transform.position.z < gameAreaLeftRocksZValue
                             )
                    {
                        newTranslatePosition.z = -sidewaysSpeed + 0.2f        ;
                        // print("Turning : Right");
                        newTranslatePosition.x += forwardSpeed * rowingSpeedMultiplierSideways;
                    }

                    gameArea.transform.Translate(newTranslatePosition);

                    #endregion
                }


                // Obstacle & Collectible Spawning
                if (shouldSpawnObjects)
                {
                    var randomValue = UnityEngine.Random.value;
                    var gameAreaXValue = gameArea.transform.position.x;

                    #region collectible

                    if (randomValue <= collectibleSpawnRatePercentage * 0.01)
                    {
                        var distanceFromPlayerToSpawnX =
                            gameArea.transform.position.x + collectibleMinDistanceFromPlayer +
                            (UnityEngine.Random.value * collectibleSpawnDistanceRange);

                        var distanceFromPlayerToSpawnZ = WATER_WIDTH * UnityEngine.Random.value;
                        // -0.001 to make sure we never go out of bounds.
                        int index = (int) Mathf.Floor((Collectibles.Length - 0.001f) * UnityEngine.Random.value);
                        Instantiate(Collectibles[index],
                            new Vector3(distanceFromPlayerToSpawnX + gameArea.transform.position.x, 20,
                                distanceFromPlayerToSpawnZ - WATER_WIDTH * 0.5f),
                            Quaternion.Euler(new Vector3(270, 0, 0)));
                    }

                    #endregion

                    #region obstacle

                    if ((gameAreaXValue - distanceAtLastObstacleSpawn > minimumDistanceToHaveTravelledBeforeNewObjectSpawn) && (randomValue <= (obstacleSpawnRatePercentage * 0.01) * ((gameAreaXValue - distanceAtLastObstacleSpawn) * scalerMultiplication)))
                    {
                        distanceAtLastObstacleSpawn = gameAreaXValue;
                        var distanceFromPlayerToSpawnX = gameAreaXValue + obstacleMinDistanceFromPlayer +
                                                         (UnityEngine.Random.value * obstacleSpawnDistanceRange);

                        var distanceFromPlayerToSpawnZ = WATER_WIDTH * UnityEngine.Random.value;


                        // If the chance is right, it spawns a donut.
                        if (UnityEngine.Random.value < (1 / relativeSpawnChanceDonut))
                        {
                            Instantiate(Obstacles[0],
                                new Vector3(distanceFromPlayerToSpawnX + gameAreaXValue, 0,
                                    distanceFromPlayerToSpawnZ - WATER_WIDTH * 0.5f),
                                Quaternion.Euler(new Vector3(90, 0, 90)));
                        }
                        // If the chance is right and it didnt spawn a donut, spawn an octopus.
                        else if (UnityEngine.Random.value < (1 / relativeSpawnChanceOctopus))
                        {
                            // The octopus needs to be spawned "in the middle", so we throw away its generated Z.
                            Instantiate(Obstacles[2],
                                new Vector3(distanceFromPlayerToSpawnX + gameAreaXValue, 530,
                                    0),
                                Quaternion.Euler(new Vector3(0, -90, 0)));
                        }
                        
                        // Else if the chance is right spawn an octopus
/*                else if (UnityEngine.Random.value < 1 / relativeSpawnChance)
                {
                    Instantiate(Obstacles[2],
                        new Vector3(distanceFromPlayerToSpawnX + gameArea.transform.position.x, 0,
                            distanceFromPlayerToSpawnZ - WATER_WIDTH * 0.5f), Quaternion.Euler(new Vector3(90, 0, 90)));
                }*/
                        else
                        {
                            Instantiate(Obstacles[1],
                                new Vector3(distanceFromPlayerToSpawnX + gameAreaXValue, 0,
                                    distanceFromPlayerToSpawnZ - WATER_WIDTH * 0.5f),
                                Quaternion.Euler(new Vector3(0, 0, 0)));
                        }
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
                        waters[currentWater].transform.Translate(WATER_LENGTH * waters.Length - 1, 0, 0, Space.World);

                        // Reset the counter to not get out of bounds.
                        if (++currentWater > waters.Length - 1)
                        {
                            currentWater = 0;
                        }
                    }
                }

                #endregion

                // Day-night cycle.

                #region day-night cycle

                if (shouldDoDayNightCycle)
                {
                    if (Time.time > timeAtLastSkyboxShift + DayLength)
                    {
                        if (nextShiftIsDay)
                        {
                            RenderSettings.skybox = skyboxDay;
                            nextShiftIsDay = false;
                        }
                        else
                        {
                            RenderSettings.skybox = skyboxNight;
                            nextShiftIsDay = true;
                        }

                        DynamicGI.UpdateEnvironment();
                        timeAtLastSkyboxShift = Time.time;
                    }
                }

                #endregion
            }
            else
            {
                gameCanvas.gameObject.SetActive(false);
                gameOverCanvas.gameObject.SetActive(true);
                
                if (String.IsNullOrEmpty(HighscoresText.text))
                {
                    HSman.SaveHighScore(teamName, collectiblesCollected);

                    var highscoress = HSman.GetHighScore();

                    var sb = new StringBuilder();

                    for (int i = 0; i < highscoress.Count; i++)
                    {
                        sb.Append($"{highscoress[i].name} : {highscoress[i].score} \n");
                    }

                    HighscoresText.text = sb.ToString();

                    HighscoresText.alignment = TextAlignmentOptions.Midline;

                }
                
                if (gameOverTime == 0)
                {
                    gameOverTime = UnityEngine.Time.time;
                }
                
                
                if (leftPlayerIsPaddling && rightPlayerIsPaddling && (UnityEngine.Time.time > gameOverTime + gameOverWait))
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
        if (shouldViewBobbing && !isGameOver)
        {
            if (Math.Abs(boat.transform.localPosition.y - newYPosition) < bobbingAmount * 2)
            {
                newYPosition =
                    UnityEngine.Random.Range(originalYPosition, originalYPosition + bobbingRange);
            }
            else
            {
                if (boat.transform.localPosition.y > newYPosition)
                {
                    boat.transform.Translate(0, -bobbingAmount, 0, Space.World);
                }
                else
                {
                    boat.transform.Translate(0, bobbingAmount, 0, Space.World);
                }
            }
        }
    }

    public void IncrementCollectiblesCollected()
    {
        collectiblesCollected++;
        collectiblesUiElement.text = collectiblesCollected.ToString();

        // Speed up if collected a milestone.

        if (collectiblesCollected % collectiblesToCollectToSpeedUp == 0)
        {
            forwardSpeed += amountToSpeedUp;
        }
    }

    public void PlayerHitObstacle()
    {
        shouldMoveForwards = false;
        soundMan.playObstacleSound();
        soundMan.playDeadSound();
        isGameOver = true;
    }

    public void playCollectibleSound()
    {
        soundMan.playCollectibleSound();
    }
}