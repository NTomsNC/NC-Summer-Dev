using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Health : MonoBehaviour
{
    [Tooltip("The start HP and Maximum HP")]
    public float hitPoints = 15;
    public bool destroyOnDeath = false;
    public bool removeDataOnDeath = false;

    [Space(10)]
    [Tooltip("This is the mesh that will be used to flash red when damaged. Optional")]
    public GameObject mesh;
    public float flashDuration = 0.5f;
    public int timesFlashed = 8;

    float maxHitPoints;    

    private void Start()
    {
        maxHitPoints = hitPoints;
    }

    private void Update()
    {
        //Removes object on death
        if(hitPoints <= 0 && destroyOnDeath)
        {
            Destroy(transform.gameObject);
        }
        else if(hitPoints <= 0 && removeDataOnDeath)
        {
            LevelBuilder lvlBuilder = GameObject.FindGameObjectWithTag("LevelBuilder").GetComponent<LevelBuilder>();
            SaveClass.DeleteSave(lvlBuilder.currentSave);

            int numberOfSaves = PlayerPrefs.GetInt("SaveCount", 0);
            numberOfSaves--;
            PlayerPrefs.SetInt("SaveCount", numberOfSaves);
            PlayerPrefs.Save();


            SceneManager.LoadScene(1);
        }
    }

    //Causes damage to hitpoints and returns if it reduces hitpoints to 0
    public bool Damage(float damage)
    {
        hitPoints -= Mathf.Abs(damage);
        StartCoroutine(FlashRed());

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

    IEnumerator FlashRed()
    {
        if (mesh != null)
        {
            Material mat = mesh.GetComponent<MeshRenderer>().material;

            for (int i = 0; i < timesFlashed; i++)
            {
                mat.color = Color.red;
                yield return new WaitForSeconds(flashDuration / timesFlashed / 2);
                mat.color = Color.white;
                yield return new WaitForSeconds(flashDuration / timesFlashed / 2);

            }
        }
    }
}
