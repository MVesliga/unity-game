using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Encounter/Enemy")]

public class Enemy : ScriptableObject
{
    public string enemyName;
    public int hp;
    public int strength;
    public int endurance;
    public int stamina;
    public int staminaMax;
    public int defence;
    public Sprite enemySprite;

}
