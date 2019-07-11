using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    public float bounceHeight = 0.5f;
    public float bounceSpeed = 4;

    public float coinMoveSpeed = 8;
    public float coinMoveHeight = 3;
    public float coinFallDistance = 2;

    Vector2 originalPostion;

    public Sprite emptyBlockSprite;

    bool canBounce = true;

    private void Start() {
        originalPostion = transform.localPosition;
    }

    public void QuestionBlockBounce() {
        if (canBounce) {
            canBounce = false;
            StartCoroutine(Bounce());
        }
    }

    private void Update() {
        
    }

    void ChangeSprite() {
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = emptyBlockSprite;
    }

    void PresentCoin() {
        GameObject spinningCoin = (GameObject) Instantiate(Resources.Load("Prefabs/Spinning_Coin", typeof(GameObject)));
        spinningCoin.transform.SetParent(transform.parent);
        spinningCoin.transform.localPosition = new Vector2(originalPostion.x, transform.localPosition.y + 1);
        StartCoroutine(MoveCoin(spinningCoin));
    }

    IEnumerator Bounce() {

        ChangeSprite();

        PresentCoin();

        while (true) {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + bounceSpeed * Time.deltaTime);
            

            if (transform.localPosition.y >= originalPostion.y + bounceHeight) {
                break;
            }
            yield return null;
        }

        while (true) {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - bounceSpeed * Time.deltaTime);

            if (transform.localPosition.y <= originalPostion.y) {
                transform.localPosition = originalPostion;
                break;
            }
            yield return null;
        }

    }


    IEnumerator MoveCoin(GameObject coin) {
        while (true) {
            coin.transform.localPosition = new Vector2(coin.transform.localPosition.x, coin.transform.localPosition.y + coinMoveSpeed * Time.deltaTime);
            if (coin.transform.localPosition.y >= originalPostion.y + coinMoveHeight + 1) {
                break;
            }

            yield return null;

        }

        while (true) {
            coin.transform.localPosition = new Vector2(coin.transform.localPosition.x, coin.transform.localPosition.y - coinMoveSpeed * Time.deltaTime);

            if (coin.transform.localPosition.y <= originalPostion.y + coinFallDistance + 1) {
                Destroy(coin.gameObject);
                break;
            }
        }
    }


}


