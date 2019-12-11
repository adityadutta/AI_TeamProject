using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

	public Vector3 bulletOffset = new Vector3(0, 0.5f, 0);

	public GameObject bulletPrefab;
	public GameObject missilePrefab;
    public AudioClip ShootSFX;
    public AudioClip MissileSFX;
    private AudioSource source;
    int bulletLayer;

	public float fireDelay = 0.25f;
    public float missileFireDelay = 1f;
	float cooldownTimer = 0;
	float missileCooldownTimer = 0;

	void Start() {
        source = GetComponent<AudioSource>();
        bulletLayer = gameObject.layer;
	}

	// Update is called once per frame
	void Update () {
		cooldownTimer -= Time.deltaTime;
        missileCooldownTimer -= Time.deltaTime;

		if( Input.GetButton("Fire1") && cooldownTimer <= 0 ) {
			// SHOOT!
			cooldownTimer = fireDelay;

			Vector3 offset = transform.rotation * bulletOffset;

			GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, transform.position + offset, transform.rotation);
			bulletGO.layer = bulletLayer;
            source.clip = ShootSFX;
            source.Play(0);
        }

        if (Input.GetButton("Fire2") && missileCooldownTimer <= 0)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy")[0])
            {
                // SHOOT HOMING MISSILE!
                missileCooldownTimer = missileFireDelay;

                Vector3 offset = transform.rotation * bulletOffset;

                GameObject missileGO = (GameObject)Instantiate(missilePrefab, transform.position + offset, transform.rotation);
                missileGO.layer = bulletLayer;
                source.clip = MissileSFX;
                source.Play(0);
            }
        }
    }
}
