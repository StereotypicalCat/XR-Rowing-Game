﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private int rightRotationSensorValuesFromLastRead = 0;
    private int leftRotationSensorValuesFromLastRead = 0;
    
    public int movementDeadzone = 20;
    public Player player;
    public GameManager gm;
    
    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        
        // print("Recieved: " + msg);

        // The string that arrives looks something like this: R:325|L:286

        var messageSplitToRightAndLeft = msg.Split('|');

        var leftRotationSensorValueAsString = messageSplitToRightAndLeft[1].Split(':')[1];

        var rightRotationSensorValueAsString = messageSplitToRightAndLeft[0].Split(':')[1];


        Int32.TryParse(leftRotationSensorValueAsString, out var leftRotationSensorValue);
        Int32.TryParse(rightRotationSensorValueAsString, out var rightRotationSensorValue);
        

            
            if (Math.Abs(leftRotationSensorValue - leftRotationSensorValuesFromLastRead) > movementDeadzone)
            {
             gm.turn(Player.direction.Left);   
            }            
            else if (Math.Abs(rightRotationSensorValue - rightRotationSensorValuesFromLastRead) > movementDeadzone)
            {
                gm.turn(Player.direction.Right);   

            }

        leftRotationSensorValuesFromLastRead = leftRotationSensorValue;
        rightRotationSensorValuesFromLastRead = rightRotationSensorValue;

    }


    
    
    
    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
    }

}
