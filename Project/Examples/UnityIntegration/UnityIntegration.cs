// You can remove this runtime conditional to run the script
#define DEBUG
#if !DEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CookLang;
using System;

/*
This script makes the rigidbody to jump every 4 seconds
THIS WORKS ONLY IN UNITY
 */
public class UnityIntegration : MonoBehaviour
{
    private CookCompiler cook;
    public Rigidbody rb;

    public float jumpTime;

    private void Start()
    {
        string Code = @"
-- Jump Every 4 seconds;
zone zero;
rbjump;
wait {jumpTime};
jumpto zero;
end;
";

        // Set custom events
        var customEvents = new Dictionary<string, Action<CookCompiler, string>>();
        customEvents.Add("print", (m, args)=>
        {
            Debug.Log(args);
            cook.pointer ++;
        });
        customEvents.Add("error", (m, args)=>
        {
            Debug.LogError(args);
            cook.pointer ++;
        });
        customEvents.Add("wait", (m, args)=>
        {
            var arg = args.Replace(" ", "");
            arg = m.ApplyVariablesInString(arg);
            time = float.Parse(arg);
            cook.pointer ++;
        });
        customEvents.Add("rbjump", (m, args)=>
        {
            rb.velocity = 10 * Vector3.up;
            cook.pointer ++;
        });

        // Set custom variables
        var customVariables = new Dictionary<string, string>();
        customVariables.Add("jumpTime", jumpTime.ToString());
        
        //Init everything
        cook = new CookCompiler(Code, customVariables, customEvents);        

        //Start the code
        StartCoroutine(RunTheCode());
    }

    private float time;

    private IEnumerator RunTheCode()
    {
        bool sw = true;
        while(sw)
        {
            cook.NextStep();
            sw = cook.running;
            if(time != 0)
            {
                yield return new WaitForSeconds(time);
                time = 0;
            }else{
                yield return null;
            }
        }
    }
}
#endif