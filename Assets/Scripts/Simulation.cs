﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Simulation : MonoBehaviour {

    [Header ("Text fields")]
	public Text windText;
	public Text timeText;
	public Text powerReqText;
	public Text powerOutputText;
	public Text powerUsageText;

	[Space]
	[Header ("Action added Text")] // the text that is displayed for 2 seconds(upon the minimap) when interacting with the turbine.

	//the side text next to the panel of the output power.
	public Text  powerOutputSideText;
	public Image powerOutputsideImage;

	[Space]
	[Header ("Simulation variables")]

	public int currentWindSpeed;
    public int currentPowerReqs;
    private int randomWindValue;

    /* =====================================
			time simulation fields
	======================================*/
    private float startTime;
	public float minutesCount;
	private float seconds;
	public int simulationSpeed = 2;
	private float time;
	private string minutes;
	private string secondstr;
    /* =====================================
		power Output simulation fields
	======================================*/
    private float turbineDefaultOutput;
    public  float totalPowerOutput;
    public TurbineSpawnManager spawnManager;

    public string powerUsage = "-Under power"; //TODO : maybe this can be changed to a enum, but it will less readable to the next developer that gets the source code.
    enum DisplayedTextValue { wind ,powerReqs,powerOutput,powerUsage}

    //arrays wind
    private int[] wind = new int[15];
    private int[] windClass1 = { 1, 2, 3, 4, 5, 6, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

    //arrays turbine output
    private float[] singleTurbineOutput = new float[15];
    private float[] turbineAOutput = {0.0f,  0.01f,0.02f,0.05f,0.09f,0.19f,0.38f,0.19f,0.38f,0.75f,1.50f,3.0f,6.0f,6.0f,6.0f,6.0f};
    private float[] turbineBOutput = {0.0f,  0.01f,0.01f,0.02f,0.05f,0.09f,0.19f,0.09f,0.19f,0.38f,0.75f,1.5f,3.0f,3.0f,3.0f,3.0f};
    private float[] turbineCOutput = {0.0f,  0.0f,0.0f,0.01f,0.01f,0.03f,0.06f,0.03f,0.06f,0.11f,0.23f,0.45f,0.9f,0.9f,0.9f,0.9f};



    void Start(){
		powerOutputSideText.enabled = false;
		powerOutputsideImage.enabled = false;

        CalculatePowerRequirements();
        currentWindSpeed = 10;
		startTime = Time.time;
        InitializeWindArray();
        InitializeOutputArray();
    }

	void Awake() {
		float firstExecution = 0.0f;
        InvokeRepeating("CalculateWindSpeed", firstExecution, 10.0f);
	}
	
	// Update is called once per frame
	void Update () {
		CalculateTime();
        EndSimulation();
	}

	//it is not called every frame, but every fixed frame (helps performance).
	void FixedUpdate(){
		CalculatePowerOutput();
		CalculatePowerUsage();
	}

    void InitializeWindArray() {
        if(GameManager.instance.Areachoice == GameManager.MainArea.mountains)
        {
            wind = windClass1;
        }
    }

    void InitializeOutputArray()
    {
        if (GameManager.instance.Type == TurbineSelector.TurbineType.A)
        {
            singleTurbineOutput = turbineAOutput;
        }
        else if (GameManager.instance.Type == TurbineSelector.TurbineType.B)
        {
            singleTurbineOutput = turbineBOutput;
        }
        else if (GameManager.instance.Type == TurbineSelector.TurbineType.C)
        {
            singleTurbineOutput = turbineCOutput;
        }
    }

    #region Calculated Simulation values

    void CalculateTime(){
		Time.timeScale = simulationSpeed;
		time = Time.time - startTime ;
		minutes = ((int) (time/60)).ToString();
		minutesCount = ((int) (time/60));
		seconds = (time % 60);
		secondstr = ((int)seconds).ToString();
		timeText.text = minutes + ":" + secondstr ;
	}

    void CalculateWindSpeed()
    {
         randomWindValue = Random.Range(0, 14);
        //currentWindSpeed = RandomGaussianGenerator.GenerateNormalRandom(10.0f, 1.0f, 1, 13);
        currentWindSpeed = wind[randomWindValue];
        DisplayText(DisplayedTextValue.wind);
    }

    void CalculatePowerRequirements()
    {
        currentPowerReqs = 6;
        DisplayText(DisplayedTextValue.powerReqs);
    }

    void CalculatePowerOutput()
    {
        totalPowerOutput = spawnManager.numberOfTurbinesOperating * singleTurbineOutput[randomWindValue+1];
        DisplayText(DisplayedTextValue.powerOutput);
    }


    void CalculatePowerUsage(){
		int localpowerDiff = (int)totalPowerOutput - currentPowerReqs;
		if (localpowerDiff < 0){
			powerUsage = "-Under power";
            powerUsageText.color = Color.red;
		}
		else if((totalPowerOutput - currentPowerReqs) > 0){
			powerUsage = "-Over power";
			powerUsageText.color = Color.blue;
		}
		else {
			powerUsage = "-Correct power";
			powerUsageText.color = Color.green;
		}
		DisplayText(DisplayedTextValue.powerUsage);
	}

    #endregion


    #region Added / substracted action power
    /* displays the added power output to the total amount 
	that each turbine is producing (text above the power output)*/
    public IEnumerator calculateAddedPower()
    {
        float addedAmount = singleTurbineOutput[randomWindValue + 1];
        powerOutputSideText.text = " + " + addedAmount.ToString();
        powerOutputSideText.enabled = true;
        powerOutputsideImage.enabled = true;
        yield return new WaitForSeconds(2f);
        powerOutputSideText.enabled = false;
        powerOutputsideImage.enabled = false;
    }

    public IEnumerator calculateSubstractedPower()
    {
        float substractedAmount = singleTurbineOutput[randomWindValue + 1];
        powerOutputSideText.text = " - " + substractedAmount.ToString();
        powerOutputSideText.enabled = true;
        powerOutputsideImage.enabled = true;
        yield return new WaitForSeconds(2f);
        powerOutputSideText.enabled = false;
        powerOutputsideImage.enabled = false;
    }

    #endregion


    #region DisplayText

    void DisplayText(DisplayedTextValue textValue)
    {

        if (textValue == DisplayedTextValue.wind)
        {
            windText.text = currentWindSpeed.ToString();
        }
        else if (textValue == DisplayedTextValue.powerOutput)
        {
            powerOutputText.text = totalPowerOutput.ToString();
        }
        else if (textValue == DisplayedTextValue.powerReqs)
        {
            powerReqText.text = currentPowerReqs.ToString();
        }
        else if (textValue == DisplayedTextValue.powerUsage)
        {
            powerUsageText.text = powerUsage;
        }
        else
        {
            Debug.Log("wrong input at DisplayText() , check given parameters");
        }
    }

    #endregion


    #region Control Simulation

    public void EndSimulation()
    {
        if (minutesCount >= GameManager.instance.simulationDurationTime || GameManager.instance.endGame == true)
        {
            minutesCount = 0;
            GameManager.instance.endGame = false;
            GameManager.instance.LoadNextLevel();
            Resources.UnloadUnusedAssets(); //removes unused assets to free memory
        }
    }

    #endregion

}