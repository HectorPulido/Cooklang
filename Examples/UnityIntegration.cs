using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CookLang;
using System;

/*
This script makes the rigidbody to jump every 4 seconds
THIS WORKS ONLY IN UNITY
 */
public class TestBehaviour : MonoBehaviour
{
    private CookCompiler cook;
    public Rigidbody rigidbody;

    void InitiateCode()
    {
        string Code = @"
#Jump Every 4 seconds;

Zone zero;

RbJump;
Wait 4;

JumpTo zero;

END;
";

        //Init everything
        cook = new CookCompiler(Code);        
        var customEvents = new Dictionary<string, Action<string>>();

        //Set custom events
        customEvents.Add("Print", (args)=>
        {
            Debug.Log(args);
        });
        customEvents.Add("Error", (args)=>
        {
            Debug.LogError(args);
        });
        customEvents.Add("Wait", (args)=>
        {
            timetowait = float.Parse(args.Replace(" ", ""));
        });
        customEvents.Add("RbJump", (args)=>
        {
            rigidbody.velocity += 10 * Vector3.up;
        });
        
        cook.SetCustomEvents(customEvents);
        
        //Start the code
        StartCoroutine(RunTheCode());
    }

    float timetowait;
    IEnumerator RunTheCode()
    {
        bool sw = true;
        while(sw)
        {
            sw = cook.TicTac();
            if(timetowait != 0)
            {
                yield return new WaitForSeconds(timetowait);
                timetowait = 0;
            }
        }
    }
}
