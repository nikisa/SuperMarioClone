using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlockHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            transform.parent.GetComponent<QuestionBlock>().QuestionBlockBounce();
            collision.GetComponent<Player>().Coins++;
        }
    }
}
