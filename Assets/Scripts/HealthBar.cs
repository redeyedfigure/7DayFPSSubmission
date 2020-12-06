using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    public Text healthCounter;

    void Update()
    {
        healthCounter.text = " Health: " + health + "/" + maxHealth;
    }

    public void AddHealth(int healthAdded){
        if(health + healthAdded >= maxHealth){
            health = maxHealth;
        }
        else{
            health = health + healthAdded;
        } 
    }

    public void SubtractHealth(int damage){
        if(health - damage <= 0){
            Die();
        }
        else{
            health = health - damage;
        }
    }

    public void Die(){
        Debug.Log("You Died");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
