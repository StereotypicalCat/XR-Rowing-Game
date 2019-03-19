using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private int cycle = 0;
    private int[,] latestValues = new int[3,2];
    public int movementDeadzone = 20;
    public Player player;
    
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

                        
        latestValues[cycle, 0] = leftRotationSensorValue;
        latestValues[cycle, 1] = rightRotationSensorValue;


        
        if (cycle != 0)
        {
            var lastLeftValue = latestValues[cycle - 1, 0];
            var lastRightValue = latestValues[cycle - 1, 1];
            
            if (Math.Abs(leftRotationSensorValue - lastLeftValue) > movementDeadzone)
            {
             player.turn(Player.direction.Left);   
            }            
            else if (Math.Abs(rightRotationSensorValue - lastRightValue) > movementDeadzone)
            {
                player.turn(Player.direction.Right);   

            }
        }

        if (cycle == 0)
        {
            var lastLeftValue = latestValues[2, 0];
            var lastRightValue = latestValues[2, 1];
            
            if (Math.Abs(leftRotationSensorValue - lastLeftValue) > movementDeadzone)
            {
                player.turn(Player.direction.Left);   
            }            
            else if (Math.Abs(rightRotationSensorValue - lastRightValue) > movementDeadzone)
            {
                player.turn(Player.direction.Right);   

            }
        }

        
        cycle++;
        if (cycle >= 3)
        {
            cycle = 0;
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
