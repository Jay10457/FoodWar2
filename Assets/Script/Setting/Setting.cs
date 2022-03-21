using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : Windows<Setting>
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Dropdown qualityLevel;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private InputField playerNameInput;
    [SerializeField] private Toggle isFullScreen;
    [SerializeField] private CanvasGroup saveInfo;

    

    public override void OnOpen()
    {
        base.OnOpen();
        playerNameInput.text = SaveManager.instance.nowData.playerName;
        GetResolutions();
        

        if (SaveManager.instance.nowData.isFullScreen)
        {
            isFullScreen.isOn = true;
        }
        else
        {
            isFullScreen.isOn = false;
        }
        qualityLevel.value = SaveManager.instance.nowData.currentQualityIndex;

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });

        qualityLevel.onValueChanged.RemoveAllListeners();
        qualityLevel.onValueChanged.AddListener(delegate { SetQuality(qualityLevel.value); });


        isFullScreen.onValueChanged.RemoveAllListeners();
        isFullScreen.onValueChanged.AddListener(delegate { SetFullScreen(isFullScreen); });
    }

    public void Save()
    {
        //SaveManager.instance.nowData.currentResolutionIndex = currentResolutionIndex;
        SaveManager.instance.nowData.playerName = playerNameInput.text;
        SaveManager.instance.SaveGame();
        StartCoroutine(FadeCanvasGroup(saveInfo, saveInfo.alpha, 0));
    }

    private void GetResolutions()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < Resolutions.instance.GetResolutions().Count; i++)
        {
            string option = Resolutions.instance.GetResolutions()[i].width + "x" + Resolutions.instance.GetResolutions()[i].height + " "
            + Resolutions.instance.GetResolutions()[i].refreshRate.ToString() + "Hz";


            if (Resolutions.instance.GetResolutions()[i].width == Screen.currentResolution.width &&
                Resolutions.instance.GetResolutions()[i].height == Screen.currentResolution.height && 
                Resolutions.instance.GetResolutions()[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                if (SaveManager.instance.nowData.currentResolutionIndex == -1)
                {
                    currentResolutionIndex = i;
                    
                }
                else
                {
                    currentResolutionIndex = SaveManager.instance.nowData.currentResolutionIndex;
                    //Debug.LogError(currentResolutionIndex = SaveManager.instance.nowData.currentResolutionIndex);
                }
                
            }
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        Resolution resolution = Resolutions.instance.GetResolutions()[currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        resolutionDropdown.RefreshShownValue();
        
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Resolutions.instance.GetResolutions()[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        SaveManager.instance.nowData.currentResolutionIndex = resolutionIndex;
        SaveManager.instance.SaveGame();
       
        
    }
    private void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        SaveManager.instance.nowData.currentQualityIndex = qualityIndex;
        SaveManager.instance.SaveGame();
    }

    private void SetFullScreen(Toggle isFullScreen)
    {
        if (isFullScreen.isOn)
        {
            Screen.fullScreen = true;
            SaveManager.instance.nowData.isFullScreen = true;
            SaveManager.instance.SaveGame();
        }
        else
        {
            Screen.fullScreen = false;
            SaveManager.instance.nowData.isFullScreen = false;
            SaveManager.instance.SaveGame();
        }
    }
    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.75f)
    {
        saveInfo.gameObject.SetActive(true);
        float _timeStartedlerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedlerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedlerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            cg.alpha = currentValue;
            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();

        }
        saveInfo.gameObject.SetActive(false);
        saveInfo.alpha = 1;
    }
}
