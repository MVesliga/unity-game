using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

    #region Singleton
    //Instancira sam sebe i održava se na životu kroz sve scene
    public static PlayerStats instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("More then one instance of PlayerStats found!");
            Destroy(gameObject);
        }
    }
    #endregion

    public GameObject enterName;

    private string playerName;
    private int playerHp;
    public Text strength;
    public Text endurance;
    public Text stamina;
    public Text defence;
    public Text points;
    public Button confirmButton;
    public Button strButton;
    public Button endButton;
    public Button staButton;
    public Button defButton;
    public int strStat = 10;
    public int endStat = 40;
    public int staStat = 100;
    public int currentStamina;
    public int defStat = 5;
    public int pointStat = 3;
    public GameObject levelUpMenu;
    public GameObject doorLockedMessage;
    public GameObject keyFoundMessage;

    private void Start()
    {
        playerHp = endStat;
        currentStamina = staStat;
        playerName = "";
        levelUpMenu.SetActive(false);
        doorLockedMessage.SetActive(false);
        keyFoundMessage.SetActive(false);
        strength.text += " " + strStat.ToString();
        endurance.text += " " + endStat.ToString();
        stamina.text += " " + staStat.ToString();
        defence.text += " " + defStat.ToString();
        points.text += " " + pointStat.ToString();
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public int GetPlayerHp()
    {
        return playerHp;
    }

    public void SetPlayerName(InputField name)
    {
        playerName = name.text;
        Debug.Log(playerName);
    }

    public void Proceed()
    {
        if (playerName.Length > 0)
        {
            Debug.Log(playerName);
            enterName.SetActive(false);
        }
    }

    public void SetPlayerHp(int hp)
    {
        playerHp += hp;
    }

    public void PlayerIsDead()
    {
        Destroy(Inventory.instance.gameObject);
        Destroy(EncounterController.instance.gameObject);
        Destroy(PersistCanvas.instance.gameObject);
        Destroy(instance.gameObject);
        SceneManager.LoadScene("Menu");
    }

    public void toggleLevelUp(bool enter)
    {
        if (enter == true)
        {
            this.pointStat = 3;
            this.points.text = "Points: " + pointStat;
            levelUpMenu.SetActive(true);
        }
    }

    public void toggleLockedDoorMessage(bool enter)
    {
        if(enter == true)
        {
            doorLockedMessage.SetActive(true);
        }
        else
        {
            doorLockedMessage.SetActive(false);
        }
    }

    public void toggleKeyFoundMessage(bool enter)
    {
        if(enter == true)
        {
            keyFoundMessage.SetActive(true);
        }
        else
        {
            keyFoundMessage.SetActive(false);
        }
    }

    public void confirmLevel()
    {
        this.playerHp = endStat;
        this.currentStamina = staStat;
        this.levelUpMenu.SetActive(false);
    }

    public void upStr()
    {
        if (this.pointStat > 0)
        {
            this.pointStat--;
            this.points.text = "Points: " + pointStat;
            this.strStat += 2;
            this.strength.text = "Strength: " + strStat;
        }
        else
        {
            this.strButton.interactable = false;
            this.endButton.interactable = false;
            this.staButton.interactable = false;
            this.defButton.interactable = false;
            this.confirmButton.interactable = true;
        }
    }
    public void upEnd()
    {
        if (this.pointStat > 0)
        {
            this.pointStat--;
            this.points.text = "Points: " + pointStat;
            this.endStat += 5;
            this.endurance.text = "Endurance: " + endStat;
        }
        else
        {
            this.strButton.interactable = false;
            this.endButton.interactable = false;
            this.staButton.interactable = false;
            this.defButton.interactable = false;
            this.confirmButton.interactable = true;
        }
    }
    public void upSta()
    {
        if (this.pointStat > 0)
        {
            this.pointStat--;
            this.points.text = "Points: " + pointStat;
            this.staStat += 10;
            this.stamina.text = "Stamina: " + staStat;
        }
        else
        {
            this.strButton.interactable = false;
            this.endButton.interactable = false;
            this.staButton.interactable = false;
            this.defButton.interactable = false;
            this.confirmButton.interactable = true;
        }
    }
    public void upDef()
    {
        if (this.pointStat > 0)
        {
            this.pointStat--;
            this.points.text = "Points: " + pointStat;
            this.defStat += 1;
            this.defence.text = "Defence: " + staStat;
        }
        else
        {
            this.strButton.interactable = false;
            this.endButton.interactable = false;
            this.staButton.interactable = false;
            this.defButton.interactable = false;
            this.confirmButton.interactable = true;
        }
    }
}