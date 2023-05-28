using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeHUB : MonoBehaviour
{
    public Slider healthSlider;
    public Image healthFill;
    public Gradient gradient;
    public TMP_Text healthText;

    public Image shieldImage;
    public TMP_Text shieldText;

    public void SetMaxHealth(int maxHealth) 
    { 
        healthSlider.maxValue = maxHealth;
        SetHealthBar(maxHealth);
        SetHealthText(maxHealth.ToString());

        SetHealthFillColor(1f);
    } 

    public void SetHealthBar(int health) 
    {
        healthSlider.value = health;
        SetHealthText(health.ToString());

        SetHealthFillColor(healthSlider.normalizedValue);
    }

    private void SetHealthText(string health) 
    {
        if (int.Parse(health) >= 1) 
        { 
            healthText.text = health;    
        }
        else 
        {
            healthText.text = "0";
        }
    }

    private void SetHealthFillColor(float value)
    {
        healthFill.color = gradient.Evaluate(value);
    }

    public void SetShield(int shield) 
    {
        if (shield >= 1)
        {
            shieldImage.enabled = true;
            shieldText.enabled = true;

            SetShieldText(shield);
        }
        else 
        {
            shieldImage.enabled = false;
            shieldText.enabled = false;
        } 

    }

    private void SetShieldText(int shield) 
    {
        shieldText.text = shield.ToString();
    }
}
