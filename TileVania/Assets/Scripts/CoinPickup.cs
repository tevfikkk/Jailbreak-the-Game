using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int coinValue = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(DestroyCoin());
        }
    }

    IEnumerator DestroyCoin()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
