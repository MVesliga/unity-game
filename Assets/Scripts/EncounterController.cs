using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterController : MonoBehaviour {

    #region Singleton
    //Instancira sam sebe i održava se na životu kroz sve scene
    public static EncounterController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("More then one instance of EncounterController found!");
            Destroy(gameObject);
        }
    }
    #endregion

    public AudioSource audioSource;

    public GameObject battleScreen;
    public Enemy[] enemySprites;

    private bool fightInProgress;

    private Image displayEnemy;
    private Text textLog;
    private Text enemyHP;
    private Text playerHP;
    private Enemy enemy;

    private Button attackButton;
    private Button defendButton;
    private Button fleeButton;
    private Button continueButton;

    private void Start()
    {
        fightInProgress = false;
        displayEnemy = battleScreen.transform.Find("DisplayEnemy").GetComponent<Image>();
        displayEnemy.preserveAspect = true;
        textLog = battleScreen.transform.Find("TextLog").GetComponent<Text>();
        enemyHP = battleScreen.transform.Find("EnemyHP").GetComponent<Text>();
        playerHP = battleScreen.transform.Find("PlayerHP").GetComponent<Text>();

        attackButton = battleScreen.transform.Find("AttackButton").GetComponent<Button>();
        defendButton = battleScreen.transform.Find("DefendButton").GetComponent<Button>();
        fleeButton = battleScreen.transform.Find("FleeButton").GetComponent<Button>();
        continueButton = battleScreen.transform.Find("ContinueButton").GetComponent<Button>();

        enemy = ScriptableObject.CreateInstance<Enemy>();
    }

    public bool RollForBattle() {

        if (Random.Range(1, 100) < 1)
        {
            Debug.Log("Battle!");
            fightInProgress = true;
            InitiateBattle();
        }
        return fightInProgress;
    }

    public void InitiateBattle() {
        int roll = Random.Range(0, enemySprites.Length);

        BackgroundMusic.instance.playBatlle();

        enemy.enemyName = enemySprites[roll].enemyName;
        enemy.enemySprite = enemySprites[roll].enemySprite;
        enemy.hp = enemySprites[roll].hp;
        enemy.strength = enemySprites[roll].strength;
        enemy.stamina = enemySprites[roll].stamina;
        enemy.staminaMax = enemySprites[roll].staminaMax;
        enemy.defence = enemySprites[roll].defence;
        enemy.endurance = enemySprites[roll].endurance;

        displayEnemy.sprite = enemy.enemySprite;
        playerHP.text = PlayerStats.instance.GetPlayerName() + "s hp: " + PlayerStats.instance.GetPlayerHp().ToString();
        enemyHP.text = enemy.enemyName + " hp: " + enemy.hp;
        textLog.text = enemy.enemyName + " appears before you!\n";

        attackButton.gameObject.SetActive(true);
        defendButton.gameObject.SetActive(true);
        fleeButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);

        battleScreen.SetActive(true);
    }

    public void FinishBattle() {
        attackButton.gameObject.SetActive(false);
        defendButton.gameObject.SetActive(false);
        fleeButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
    }


    public void StatusCheck() {
        if (enemy.hp < 1) {
            textLog.text = "You won!";
            FinishBattle();
        }

        if (PlayerStats.instance.GetPlayerHp()<1) {
            textLog.text = "You died!";
            FinishBattle();
        }
    }

    public void EnemyAction() {
        if (enemy.hp > 1 && PlayerStats.instance.GetPlayerHp()>1 && enemy.stamina > 10)
        {
            if (Random.Range(1, 100) < (float)((1.2 * enemy.stamina / enemy.staminaMax) * 100))
            {
                audioSource.Play();
                Debug.Log("exhaustion: " + ((double)enemy.stamina / enemy.staminaMax));
                Debug.Log("Str:" + enemy.strength);
                Debug.Log("PlDef: " + PlayerStats.instance.defStat);
                int damage = (int)(enemy.strength * ((double)enemy.stamina / enemy.staminaMax)) - PlayerStats.instance.defStat;
                if(damage<0)
                {
                    damage = 0;
                }
                PlayerStats.instance.SetPlayerHp( -damage);
                textLog.text += "\n" + enemy.enemyName + " hits you for " + damage + " DMG";
                if(PlayerStats.instance.GetPlayerHp() < 0)
                {
                    playerHP.text = PlayerStats.instance.GetPlayerName() + "s hp: 0";
                }
                else
                {
                    playerHP.text = PlayerStats.instance.GetPlayerName() + "s hp: " + PlayerStats.instance.GetPlayerHp().ToString();
                }
                
                enemy.stamina -= 10;
            }
            else
            {
                textLog.text += "\n" + enemy.enemyName + " missed";
            }
        }
        else
        {
            textLog.text += "\n" + enemy.enemyName + " decides to rest for a round";
            enemy.stamina += 10; 
        }
        StatusCheck();
    }


    IEnumerator battleFlow() {
        //vrijeme izmedu naseg i neprijateljevog poteza
        attackButton.interactable = false;
        defendButton.interactable = false;
        fleeButton.interactable = false;
        yield return new WaitForSeconds(1.0f);
        EnemyAction();
        attackButton.interactable = true;
        defendButton.interactable = true;
        fleeButton.interactable = true;
    }

    //Player actions
    public void ActionAttack()
    {
        double hitChance = (double)((120 * PlayerStats.instance.currentStamina / PlayerStats.instance.staStat));
        Debug.Log(hitChance);
        if (Random.Range(1, 100) < hitChance)
        {
            int damage = (int)((PlayerStats.instance.strStat * PlayerStats.instance.currentStamina / PlayerStats.instance.staStat )- enemy.defence);
            enemy.hp -= damage;

            audioSource.Play();
            textLog.text = "You hit " + enemy.enemyName + " for " + damage + " DMG.";
            if(enemy.hp < 0)
            {
                enemyHP.text = enemy.enemyName + " hp: 0";
            }
            else
            {
                enemyHP.text = enemy.enemyName + " hp: " + enemy.hp;
            }
            
            if (PlayerStats.instance.currentStamina < 10)
            {
                textLog.text += "\n" + "You are exhausted. Defend!!!";
                attackButton.interactable = false;
            }
        }
        else
        {
            textLog.text += "You missed";
        }

        StatusCheck();
        StartCoroutine(battleFlow());
        StatusCheck();
    }
    public void rest()
    {
        int maxHp = PlayerStats.instance.endStat;
        int currentHp = PlayerStats.instance.GetPlayerHp();
        int heal = (int)(PlayerStats.instance.endStat * 0.1);
        bool nearMax = false;
        if (PlayerStats.instance.GetPlayerHp() + (int)(PlayerStats.instance.endStat * 0.1) >= PlayerStats.instance.endStat)
        {
            nearMax = true;
        }
        if (nearMax)
        {
            PlayerStats.instance.SetPlayerHp(maxHp - currentHp);
        }
        else
        {
            PlayerStats.instance.SetPlayerHp(heal);
        }
        PlayerStats.instance.currentStamina = PlayerStats.instance.staStat;
        attackButton.interactable = true;
    }
    public void ActionDefend()
    {
        textLog.text = "You rest and defend.";
        rest();
        StartCoroutine(battleFlow());
        StatusCheck();
    }

    public void ActionFlee()
    {
        textLog.text = "You fled from battle.";
        FinishBattle();
    }

    public void ActionContinue() {
        if (PlayerStats.instance.GetPlayerHp() < 1) {
            PlayerStats.instance.PlayerIsDead();
        }
        fightInProgress = false;
        battleScreen.SetActive(false);
        BackgroundMusic.instance.playTheme();
    }


    //unlock movement after battle is over
    public bool AreWeFighting() {
        return fightInProgress;
    }

}
