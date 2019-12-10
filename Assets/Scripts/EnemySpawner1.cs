using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner1 : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float SpawnerDelay;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > SpawnerDelay)
        {
            SpawnEnemyShip();
            timer = 0.0f;
        }
    }

    //Spawn enemy ship
    void SpawnEnemyShip()
    {
        Vector3 SpawnPoint = new Vector3(transform.position.x + Random.Range(-7.5f, 7.5f), transform.position.y, transform.position.z);
        GameObject tempEnemy = (GameObject)Instantiate(enemyPrefab, SpawnPoint, Quaternion.identity);
        tempEnemy.transform.Rotate(0.0f, 0.0f, 180.0f);
    }
        
}
