using UnityEngine;
using System.Collections;

public class DamageHandler : MonoBehaviour {

	public int health = 1;

	public float invulnPeriod = 0;

    public AudioClip DeadSFX;
    public bool bIsBullet;
    public Sprite explosionSprite;
    private AudioSource source;
    private bool bIsDying;

    float invulnTimer = 0;
	int correctLayer;

	SpriteRenderer spriteRend;

	void Start() {
		correctLayer = gameObject.layer;

		// NOTE!  This only get the renderer on the parent object.
		// In other words, it doesn't work for children. I.E. "enemy01"
		spriteRend = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();

        if (spriteRend == null) {
			spriteRend = transform.GetComponentInChildren<SpriteRenderer>();

			if(spriteRend==null) {
				Debug.LogError("Object '"+gameObject.name+"' has no sprite renderer.");
			}
		}
	}

	void OnTriggerEnter2D() {
		health--;

		if(invulnPeriod > 0) {
			invulnTimer = invulnPeriod;
			gameObject.layer = 10;
		}
	}

	void Update() {

		if(invulnTimer > 0) {
			invulnTimer -= Time.deltaTime;

			if(invulnTimer <= 0) {
				gameObject.layer = correctLayer;
				if(spriteRend != null) {
					spriteRend.enabled = true;
				}
			}
			else {
				if(spriteRend != null) {
					spriteRend.enabled = !spriteRend.enabled;
				}
			}
		}

		if(health <= 0) {
			Die();
		}
	}

	void Die() {
        if(bIsBullet)
        {
            Destroy(gameObject);
        }
        else if(!bIsDying && !bIsBullet)
        {
            source.PlayOneShot(DeadSFX);
            Destroy(gameObject, 0.2f);
            bIsDying = true;
            spriteRend.sprite = explosionSprite;
        }
        transform.Rotate(0.0f, 0.0f, 5.0f);
	}

}
