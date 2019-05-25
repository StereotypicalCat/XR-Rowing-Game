using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private int rightRotationSensorValuesFromLastRead = 0;
    private int leftRotationSensorValuesFromLastRead = 0;

    public int movementDeadzone = 20;
    public Player player;
    public GameManager gm;

    public float lastTimeRightPlayerPaddled;
    public float lastTimeLeftPlayerPaddled;
    public float timeToWaitForNextRow = 1;

    private float ignoreFirstMessagesOnStartTime;
    public float ignoreFirstMessagesOnStartTimeToWait;

    public AnimationController animCtrl;
    private bool leftIsPaddling;
    private bool rightIsPaddling;

    private bool isFirstRow = true;

    // Invoked when a line of data is received from the serial device.
    private void Awake()
    {
        ignoreFirstMessagesOnStartTime = UnityEngine.Time.time;
    }

    void OnMessageArrived(string msg)
    {
        if (ignoreFirstMessagesOnStartTime + ignoreFirstMessagesOnStartTimeToWait < UnityEngine.Time.time)
        {
            print("Recieved: " + msg);

            // The string that arrives looks something like this: R:325|L:286

            var messageSplitToRightAndLeft = msg.Split('|');

            var leftRotationSensorValueAsString = messageSplitToRightAndLeft[1].Split(':')[1];

            var rightRotationSensorValueAsString = messageSplitToRightAndLeft[0].Split(':')[1];


            Int32.TryParse(leftRotationSensorValueAsString, out var leftRotationSensorValue);
            Int32.TryParse(rightRotationSensorValueAsString, out var rightRotationSensorValue);
            if (!isFirstRow)
            {
                if (Math.Abs(leftRotationSensorValue - leftRotationSensorValuesFromLastRead) > movementDeadzone)
                {
                    gm.leftPlayerIsPaddling = true;
                    leftIsPaddling = true;
                    lastTimeLeftPlayerPaddled = Time.time;
                }
                else if (lastTimeLeftPlayerPaddled < Time.time + timeToWaitForNextRow)
                {
                    gm.leftPlayerIsPaddling = false;
                    leftIsPaddling = false;
                }

                if (Math.Abs(rightRotationSensorValue - rightRotationSensorValuesFromLastRead) > movementDeadzone)
                {
                    gm.rightPlayerIsPaddling = true;
                    rightIsPaddling = true;
                    lastTimeRightPlayerPaddled = Time.time;
                }
                else if (lastTimeRightPlayerPaddled < Time.time + timeToWaitForNextRow + 1f)
                {
                    rightIsPaddling = false;
                    gm.rightPlayerIsPaddling = false;
                }

                leftRotationSensorValuesFromLastRead = leftRotationSensorValue;
                rightRotationSensorValuesFromLastRead = rightRotationSensorValue;
                animCtrl.UpdateAnimations(leftIsPaddling, rightIsPaddling);
            }
            else
            {
                isFirstRow = false;
                leftRotationSensorValuesFromLastRead = leftRotationSensorValue;
                rightRotationSensorValuesFromLastRead = rightRotationSensorValue;
            }
        }
    }


    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
    }
}