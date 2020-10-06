using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlManager : MonoBehaviour {
    public Transform mainMenu, settings, missions, shop, colorPanel, trialPanel;
    public Scrollbar soundVolume, musicVolume;
    public Text colorBuySet, trialBuySet;
    string[] thisColor = new string[12];
    //цены товаров
    int[] colorCost = new int[] { 0, 5, 5, 7, 8, 9, 7, 5, 12, 45, 78, 12 };
    int[] trialCost = new int[] { 10, 5,7,8,45,17,25,46,10,47,13,15};
    int selectedColorIndex;
    int selectedTrialIndex;

    int ColorOwned=0;
    int TrialOwned=0;
    public Text money;
    //Game scene loading
    public void LoadScene(string name)
    {
        Application.LoadLevel(name);
    }

    public void StartMission(string name)
    {
        Application.LoadLevel(name);
    }

    //Quit
    public void QuitGame()
    {
        Application.Quit();
    }

    //To settings
    public void Settings(bool clicked)
    {
        if (clicked)
        {
            mainMenu.gameObject.SetActive(false);
            settings.gameObject.SetActive(true);
        }
    }

    //To missions 
    public void Missions(bool clicked)
    {
        if (clicked)
        {
            mainMenu.gameObject.SetActive(false);
            missions.gameObject.SetActive(true);
        }
    }

    //To shop
    public void Shop(bool clicked)
    {
        if (clicked)
        {
            money.text = PlayerPrefs.GetInt("money").ToString();
            mainMenu.gameObject.SetActive(false);
            shop.gameObject.SetActive(true);


            ColorOwned = PlayerPrefs.GetInt("colors");
            TrialOwned = PlayerPrefs.GetInt("trials");
            InitShop();
        }
    }

    //Back from settings
    public void BackToMenu(bool clicked)
    {
        if (clicked)
        {
            settings.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
    }

    //Back from shop
    public void BackFromShop(bool clicked)
    {
        if (clicked)
        {
            shop.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
    }

    public void BackFromMissions(bool clicked)
    {
        if (clicked)
        {
            missions.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
    }

    //Sound Volume scrollbar  enabling
    public void EnableSoundVolume(Toggle toggle)
    {
        soundVolume.interactable = toggle.isOn;
    }

    //Music Volume scrollbar enabling
    public void EnableMusicVolume(Toggle toggle)
    {
        soundVolume.interactable = toggle.isOn;
    }


    public void InitShop()
    {
        Debug.Log("Current color HEX is: " + PlayerPrefs.GetString("currentColor"));
        int i = 0;
        foreach(Transform t in colorPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();


            thisColor[i] = ColorUtility.ToHtmlStringRGB(b.image.color);
            b.onClick.AddListener(() => OnColorSelect(currentIndex));
            i++;
        }
        i = 0;
        foreach (Transform t in trialPanel)
        {
            int currentIndex = i;
            Button b = t.GetComponent<Button>();
            b.onClick.AddListener(() => OnTrialSelect(currentIndex));
            i++;
        }
    }

    private void OnTrialSelect(int currentIndex)
    {
        Debug.Log("Selecting trial button: " + currentIndex);

        selectedTrialIndex = currentIndex;
        if (IsTrialOwned(currentIndex))
        {
            trialBuySet.text = "select";
        }
        else
        {
            trialBuySet.text = "buy: " + trialCost[currentIndex].ToString();
        }
    }

    private void OnColorSelect(int currentIndex)
    {
        Debug.Log("Selecting color button: " + currentIndex);

        selectedColorIndex = currentIndex;
        Debug.Log(thisColor[currentIndex]);
        if (IsColorOwned(currentIndex))
        {
            colorBuySet.text = "select";
        }
        else
        {
            colorBuySet.text = "buy: " + colorCost[currentIndex].ToString();
        }

    }

    public void OnColorBuySet()
    {
        Debug.Log("Buy/set color");

        if(IsColorOwned(selectedColorIndex))
        {
            SetColor(selectedColorIndex);
        }
        else
        {
            if(BuyColor(selectedColorIndex, colorCost[selectedColorIndex]))
            {
                SetColor(selectedColorIndex);
            }
            else
            {
                Debug.Log("not enough gold");
            }
        }
    }
    public void OnTrialBuySet()
    {
        Debug.Log("Buy/set trial");

        if (IsTrialOwned(selectedTrialIndex))
        {
            SetTrial(selectedTrialIndex);
        }
        else
        {
            if (BuyTrial(selectedTrialIndex, colorCost[selectedTrialIndex]))
            {
                SetTrial(selectedTrialIndex);
            }
            else
            {
                Debug.Log("not enough gold");
            }
        }
    }

    void SetColor(int index)
    {
        //change the color

        //change button text
        colorBuySet.text = "current";
        PlayerPrefs.SetString("currentColor", "#"+thisColor[index]);
    }
    void SetTrial(int index)
    {
        trialBuySet.text = "current";
    }

    public bool IsColorOwned(int index)
    {
        //check if the bit is set(color owned)
        return (ColorOwned & (1 << index)) != 0;
    }
    public bool IsTrialOwned(int index)
    {
        return (TrialOwned & (1 << index)) != 0;
    }

    public bool BuyColor(int index, int cost)
    {
        if(PlayerPrefs.GetInt("money") >= cost)
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - cost);
            UnlockColor(index);

            return true;
        }
        else
        {
            return false;
        }
    }
    public bool BuyTrial(int index, int cost)
    {
        if (PlayerPrefs.GetInt("money") >= cost)
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - cost);
            UnlockTrial(index);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void UnlockColor(int index)
    {
        ColorOwned |= 1 << index;
        PlayerPrefs.SetInt("colors", ColorOwned);
    }
    public void UnlockTrial(int index)
    {
       TrialOwned |= 1 << index;
        PlayerPrefs.SetInt("trials", TrialOwned);
    }
    //reset
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }
}
