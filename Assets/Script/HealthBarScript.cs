using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Mathematics;

public class HealthBarScript : MonoBehaviour
{
    public UnityEngine.UI.Image healthbarSprite;
    public float duration = 0.2f;


    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        if (healthbarSprite == null)
        {
            healthbarSprite = GetComponentInChildren<UnityEngine.UI.Image>();
        }

        float targetFill = currentHealth / maxHealth;

        // Kill any previous tweens on this image to prevent overlap
        DOTween.Kill(healthbarSprite);

        // Smooth fill amount animation with link
        healthbarSprite.DOFillAmount(targetFill, duration)
            .SetEase(Ease.InOutSine)
            .SetTarget(healthbarSprite.gameObject)
            .SetLink(healthbarSprite.gameObject);

        // Change color based on health percentage
        healthbarSprite.color = Color.Lerp(Color.red, Color.green, targetFill);
    }
}
