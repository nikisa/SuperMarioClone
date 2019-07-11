using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;

    public GameObject LifeText;
    public GameObject CoinText;

    int _life = 3;
    public int Life { get { return _life; } set { _life = value; } }

    int _coins = 0;
    public int Coins { get { return _coins; } set { _coins = value; } }

    public Vector2 velocity;

    public float jumpForce;
    public float jumpTime;
    private float jumpTimeCounter;

    private bool walk, walk_left, walk_right, jump , isGrounded , isJumping;

    public Transform feetPos;
    public float checkRadius;

    public LayerMask wallMask;
    public LayerMask floorMask;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();
        UpdatePlayerPosition();
        JumpSystem();
        UpdateAnimationStates();
        UpdateUI();

    }

    void UpdateUI() {
        LifeText.GetComponent<Text>().text = Life + "";
        CoinText.GetComponent<Text>().text = Coins.ToString();
    }
    
    void UpdateAnimationStates() {
        if (isGrounded && !walk) {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", false);
        }

        if (isGrounded && walk) {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", true);
        }

        if (isJumping) {
            GetComponent<Animator>().SetBool("isJumping", true);
            GetComponent<Animator>().SetBool("isRunning", false);
        }
    }

    void UpdatePlayerPosition() {
        Vector3 pos = transform.localPosition;
        Vector3 scale = transform.localScale;

        if (walk) {
            if (walk_left) {
                pos.x -= velocity.x * Time.deltaTime;
                scale.x = -1; //Setto la scala a -1 così si inverte la sprite e non devo creare uno stato per la camminata a Sx
            }
            if (walk_right) {
                pos.x += velocity.x * Time.deltaTime;
                scale.x = 1;
            }

            pos = checkWallRays(pos, scale.x);

        }

        transform.localPosition = pos;
        transform.localScale = scale;
    }

    void CheckPlayerInput() {
        bool input_left = Input.GetKey(KeyCode.LeftArrow);
        bool input_right = Input.GetKey(KeyCode.RightArrow);
        bool input_space = Input.GetKey(KeyCode.Space);

        walk = input_left || input_right;
        walk_left = input_left && !input_right;
        walk_right = !input_left && input_right;
        jump = input_space;
    } 

    void JumpSystem() {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space)) {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping) {
            if (jumpTimeCounter > 0) {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            isJumping = false;
        }
    }

    Vector3 checkWallRays(Vector3 pos , float direction) {
        Vector2 originTop = new Vector2(pos.x + direction * .4f, pos.y + 1 - .2f);
        Vector2 originMiddle = new Vector2(pos.x + direction * .4f, pos.y);
        Vector2 originBottom = new Vector2(pos.x + direction * .4f, pos.y - 1 + .2f);

        RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallMiddle = Physics2D.Raycast(originMiddle, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallBottom = Physics2D.Raycast(originBottom, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);

        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null){
            pos.x -= velocity.x * Time.deltaTime * direction;
        }

        return pos;
    }
    

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "EnemyHit") {
            collision.GetComponentInParent<EnemyAI>().Crush();
        }
    }
}
