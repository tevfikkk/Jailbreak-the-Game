using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForCoindPickup = 100;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;

            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);

            FindObjectOfType<GameSession>().AddToScore(pointsForCoindPickup);

            StartCoroutine(DestroyCoin());
            gameObject.SetActive(false);
        }
    }

    IEnumerator DestroyCoin()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
