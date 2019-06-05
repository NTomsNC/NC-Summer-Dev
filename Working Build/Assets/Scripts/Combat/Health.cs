using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health : MonoBehaviour
{
    [Tooltip("The start HP and Maximum HP")]
    public float hitPoints = 15;
    float maxHitPoints;    

    private void Start()
    {
        maxHitPoints = hitPoints;
    }

    private void Update()
    {

    }

    //Causes damage to hitpoints and returns if it reduces hitpoints to 0
    public bool Damage(float damage)
    {
        hitPoints -= Mathf.Abs(damage);

        if (hitPoints <= 0)
            return true;

        return false;
    }

    //Heals the entity up to its max hitpoints
    public void Heal(float healAmount)
    {
        hitPoints += Mathf.Abs(healAmount);

        if (hitPoints > maxHitPoints)
            hitPoints = maxHitPoints;
    }

    //Starts the process of regenerating health over time
    IEnumerator Regenerate(float healAmount, int timesHealed, float timeBetweenHeal)
    {
        for (int i = 0; i < timesHealed; i++)
        {
            hitPoints += Mathf.Abs(healAmount);

            if (hitPoints > maxHitPoints)
            {
                hitPoints = maxHitPoints;
                break;
            }

            yield return new WaitForSeconds(timeBetweenHeal);
        }
    }

    //Starts the process of damaging health over time
    IEnumerator DamageOverTime(float damageAmount, int timesHurt, float timeBetweenDam)
    {
        for (int i = 0; i < timesHurt; i++)
        {
            hitPoints -= Mathf.Abs(damageAmount);

            if (hitPoints > maxHitPoints)
            {
                hitPoints = maxHitPoints;
                break;
            }

            yield return new WaitForSeconds(timeBetweenDam);
        }
    }
}
