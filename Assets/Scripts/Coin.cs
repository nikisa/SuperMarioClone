using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<Player>().Coins++;
            Debug.Log(collision.GetComponent<Player>().Coins);
        }
        Destroy(gameObject);
    }
}
