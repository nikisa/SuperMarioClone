using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHole : MonoBehaviour
{
    public GameObject YouLose;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            YouLose.SetActive(true);
        }
    }
}
