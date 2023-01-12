using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public bool enemyLaser;
    void Start()
    {
        StartCoroutine("DeathWait");
    }
    IEnumerator DeathWait()
    {
        if (!enemyLaser)
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
        if (enemyLaser)
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!enemyLaser && collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
        if (enemyLaser && collision.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}                                                  
