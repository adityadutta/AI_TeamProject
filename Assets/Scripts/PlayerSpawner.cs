using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public GameObject playerPrefab;
	GameObject playerInstance;
    public Text livesText;
    public GameObject gameoverText;

	public int numLives = 4;

	float respawnTimer;

	// Use this for initialization
	void Start () {
		SpawnPlayer();
	}

	void SpawnPlayer() {
		numLives--;
		respawnTimer = 1;
		playerInstance = (GameObject)Instantiate(playerPrefab, transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		if(playerInstance == null && numLives > 0) {
			respawnTimer -= Time.deltaTime;

			if(respawnTimer <= 0) {
				SpawnPlayer();
			}
		}



        if (numLives > 0 || playerInstance != null)
        {
            livesText.text = "Lives left: " + numLives;
        }
        else
        {
            gameoverText.SetActive(true);

        }


    }
}
