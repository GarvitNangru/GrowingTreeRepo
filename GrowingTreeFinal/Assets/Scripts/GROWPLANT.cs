using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class GROWPLANT : MonoBehaviour
{
    #region initialization

    [Header("ABOUT TREE")]
   
    public int currentProgression = 0;
    public int maxGrowth=30;
    public int timesWatered = 0;
    [Range(0f, 1f)]
    public float wateredProgress =0f;
    
 

    [Header("GAMEOBJECT AND STUFF")]
    [Space(20)]
    public Image WaterBar;
    public Button RainButton;
    public Text rainTextTimer;
    public GameObject[] trees;
    public GameObject cloudAndRainPrefab;
    public GameObject[] days;

  //  DateTime x = new DateTime(0,0,0,0,0,0);
    
    DateTime currentDate;
    DateTime oldWateredDate;
 //   DateTime lastGrowthTime;
    TimeSpan remainingWaterTime;
   // TimeSpan remainingNextGrowthTime;
    TimeSpan CountDown=new TimeSpan(0,1,0);

    #endregion

    #region unity Functions
    void Start()
    {
        currentDate = DateTime.Now;
        timesWatered = PlayerPrefs.GetInt("TimesWatered");
        currentProgression = PlayerPrefs.GetInt("CurrentProgression");
      
        enableCurrentTree();

        if (PlayerPrefs.GetString("WaterTime", "") != "")
        {
            long temp = Convert.ToInt64(PlayerPrefs.GetString("WaterTime"));
            oldWateredDate = DateTime.FromBinary(temp);
        }
       
        //temp = Convert.ToInt64(PlayerPrefs.GetString("lastGrowthTime"));
        //lastGrowthTime = DateTime.FromBinary(temp);
        wateredProgress = PlayerPrefs.GetFloat("WaterProgress", 0);
        if (oldWateredDate.ToBinary() != 0)
        {
            remainingWaterTime = currentDate - oldWateredDate;
            print(remainingWaterTime.TotalMinutes);
            wateredProgress -= 0.01f * (float)remainingWaterTime.TotalMinutes / 2;
        }

        

        WaterBar.fillAmount = wateredProgress / 1;

        StartCoroutine(dayCycle());
        dayChange();
      
    }

    // Update is called once per frame
    void Update()
    {
        currentDate = DateTime.Now;
        remainingWaterTime= currentDate - oldWateredDate;
        //  print((int)remainingWaterTime.Minutes +" : " +(int)remainingWaterTime.Seconds);

      
        TimeSpan ts = CountDown.Subtract(remainingWaterTime);

    
        //  print("COuntDown " + ts.Minutes + " : " + ts.Seconds);

        if (ts.TotalSeconds <= 0)
        {
            rainTextTimer.text = " ";
            RainButton.interactable = true;
        }
        else
        {
            rainTextTimer.text = ts.Minutes.ToString() + " : " + ts.Seconds.ToString();
            RainButton.interactable = false;
        }
    }
    public void exitGame()
    {
        SceneManager.LoadScene("MenuScene");
    }
    #endregion

    #region TreeGrowth and Water;
    public void waterPlant()
    {
        if (remainingWaterTime.TotalMinutes> 1)
        {
            Instantiate(cloudAndRainPrefab,transform.position,Quaternion.identity);
            PlayerPrefs.SetString("WaterTime", System.DateTime.Now.ToBinary().ToString());
            long temp = Convert.ToInt64(PlayerPrefs.GetString("WaterTime"));
            oldWateredDate = DateTime.FromBinary(temp);
            timesWatered++;
            PlayerPrefs.SetInt("TimesWatered", timesWatered);

            wateredProgress += 0.33f;
            PlayerPrefs.SetFloat("WaterProgress", wateredProgress);
            WaterBar.fillAmount = wateredProgress / 1;
        }
    }
   
    public void Growth()
    {
        timesWatered = 0;
        PlayerPrefs.SetInt("TimesWatered", timesWatered);
        if (currentProgression < maxGrowth)
        {
            currentProgression++;
        }
        enableCurrentTree();
     
        PlayerPrefs.SetInt("CurrentProgression", currentProgression);
        PlayerPrefs.SetString("lastGrowthTime", System.DateTime.Now.ToBinary().ToString());
        //long temp = Convert.ToInt64(PlayerPrefs.GetString("lastGrowthTime"));
        //lastGrowthTime = DateTime.FromBinary(temp);
    }
    public void enableCurrentTree()
    {
        int i= 0;
        foreach(GameObject go in trees)
        {
            if(i==currentProgression)
            {
                go.SetActive(true);
            }
            else
                go.SetActive(false);
            i++;
        }
    }

    #endregion

    #region About day and WaterProgress Decrease
    IEnumerator dayCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            dayManager();
        }
    }
    public void dayManager()
    {
        dayChange();
        if(currentDate.Second %30==0)
        {
            if(WaterBar.fillAmount >= 0.75f)
            {
                Growth();
            }
        }
        wateredProgress -= 0.01f * (float)remainingWaterTime.TotalMinutes / 2;
        WaterBar.fillAmount = wateredProgress / 1;
        PlayerPrefs.SetFloat("WaterProgress", wateredProgress);

    }
    void dayChange()
    {
        if(currentDate.Minute <= 15 || (currentDate.Minute >30 && currentDate.Minute<=45) )
        {
            days[0].SetActive(true);
            days[1].SetActive(false);
            Debug.Log("SUNNNNNN");
        }
        else if (currentDate.Minute > 45 || (currentDate.Minute > 15 && currentDate.Minute <= 30))
        {
            days[1].SetActive(true);
            days[0].SetActive(false);
            Debug.Log("RAATAN KAALIYA");
        }
    }
    #endregion
}
