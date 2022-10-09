using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    float towerHealth = 20;
    public Slider towerHealthBar;
    bool isGameOver = false;


    private void Start()
    {
        towerHealthBar.maxValue = towerHealth;
        towerHealthBar.value = towerHealth;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("stickman"))
        {
            DecreaseHealth();
            Destroy(other.gameObject);
        }
    }



    private void Update()
    {
        //check if game over
        if(towerHealth <= 0 && !isGameOver)
        {
            towerHealth = 0;
            isGameOver = true;
            GameManager.Instance.GameOver();
        }

    }

    void DecreaseHealth()
    {
        towerHealth--;
        towerHealthBar.value = towerHealth;
    }

    
}
