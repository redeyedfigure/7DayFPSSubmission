using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGStatManager : MonoBehaviour
{
    [Header("Health Variables")]
    public float Health = 0f;
    public float MaxHealth = 0f;
    [Header("Levelling Up Level")]
    public int currentLevel = 1;
    public int levelCap = 30;
    public int currentXP = 0;
    public int XPToNextLevel;
    int leftoverXP;
    int currentLevelMinusOne; 
    public List<int> levelRequirements = new List<int>();
    

    void Update()
    {
        currentLevelMinusOne = currentLevel - 1;
        XPToNextLevel = currentXP - levelRequirements[currentLevelMinusOne];
        if(currentXP >= levelRequirements[currentLevelMinusOne]){
            leftoverXP = currentLevel - levelRequirements[currentLevelMinusOne];
            currentLevel++;
            currentXP = leftoverXP;
            leftoverXP = 0;
        }
    }

    public void HitState(float hitAmount){
        Health = Health - hitAmount;
    }

    void DeathState(){
        Debug.Log("You're Dead");
    }

}
