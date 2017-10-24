﻿using UnityEngine;

/*
 In this class we use some basic probality maths to randomly damage a wind turbine.
 The main class is CalculateDamagePropability(), that produces a float " damagePropability " with 
 different values in each iteration.
 */
public class TurbineDamage : MonoBehaviour
{

    private Simulation simulator;
    public bool isDamaged = false;
    private TurbineController turbine;
    private float propabilityMultiplier;
    private float turbineUsage;
    private float rate; // te rate that the method to damage the turbine will be called
    private int damageStartTime;

    void Start()
    {
        damageStartTime = 0;
        float startCall = 30.0f;
        float rate = Random.Range(20.0f,40.0f);
        simulator = GameObject.FindGameObjectWithTag("Simulator").GetComponent<Simulation>();
        turbine = GetComponent<TurbineController>();
        //Calls the method for the first time in "startCall" with a repeat rate of the "rate" value.
        InvokeRepeating("CalculateDamagePropability", startCall, rate);
    }


    /*=====================================
		Calculate the propability that a
		turbine can get damaged
 	=====================================*/

    void CalculateDamagePropability()
    {

        //!Damage future is deprecated

        if (simulator.minutesCount >= damageStartTime && turbine.IsRotating() == true && turbine.IsDamaged() == false
            && TurbineController.damagedTurbines <= 2)
        {

            propabilityMultiplier = Random.Range(0.0f, 1.0f);
            turbineUsage = 0.0f;
            if (string.Compare(simulator.powerUsage, "-Over power") == 0)
            {
                turbineUsage = 1.0f;
            }
            else if (string.Compare(simulator.powerUsage, "-Correct power") == 0)
            {
                turbineUsage = 1.0f;
            }
            else
            {
                turbineUsage = 1.0f;
            }
            float damagePropability = turbineUsage * propabilityMultiplier;
            print(damagePropability);
            if (damagePropability > 0.85)
            {
                damageTurbine();
            }
        }
    }


    void damageTurbine()
    {
        turbine.DisableTurbine();
        isDamaged = true;
        turbine.setRepair(false);
        TurbineController.damagedTurbines++;
    }

}
