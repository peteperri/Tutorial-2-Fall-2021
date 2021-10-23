using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    Animator anim;
    public Text score;
    public Text lives;
    public Text winText;

    public float speed;
    public int coinsLeft = 4;
    public int livesLeft = 3;
    public static int level;
    private bool grounded;
    private bool facingRight = true;
    public static bool isAlive;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    public AudioClip coin;
    public AudioClip badCoin;
    public AudioSource playerSounds;
    public AudioClip levelUp;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        isAlive = true;
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Coins Left: " + coinsLeft.ToString();
        winText.text = "";
        anim.SetBool("isDead", false);
        playerSounds.loop = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isAlive){
            anim.SetBool("isMoving", true);
            float hozMovement = Input.GetAxis("Horizontal");
            float vertMovement = Input.GetAxis("Vertical");
            isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
            rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
            if(rd2d.velocity == new Vector2(0.0f, 0.0f)){
                anim.SetBool("isMoving", false);
            }
            if (facingRight == false && hozMovement > 0)
            {
                Flip();
            }
            else if (facingRight == true && hozMovement < 0)
            {
                Flip();
            }    

            if(livesLeft == 0){
                winText.text = "You lose! Game Restarting...";
                anim.SetBool("isDead", true);
                isAlive = false;
                StartCoroutine(RestartGame(3));
            }

            if(coinsLeft == 0 && level == 1){
                coinsLeft = 4;
                level++;
                livesLeft = 3;
                lives.text = "Lives: " + livesLeft.ToString();
                score.text = "Coins Left: " + coinsLeft.ToString();
                playerSounds.clip = levelUp;
                playerSounds.Play();
                gameObject.transform.position = new Vector2(37.42f, -2.19f);
            }
            if(coinsLeft == 0 && level == 2){
                winText.fontSize = 14;
                winText.text = "You win!\n Game by Peter Perri";
                level++;
            }
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }  
    }


    void Update(){
        if(isAlive){
            if((Input.GetKeyDown(KeyCode.W) || (Input.GetKeyDown(KeyCode.UpArrow))) && grounded == false)
            {
                anim.SetBool("isFlying", true);
                rd2d.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
            }
            if(Input.GetKeyUp(KeyCode.W) || (Input.GetKeyUp(KeyCode.UpArrow)))
                {
                    rd2d.AddForce(new Vector2(0, -2), ForceMode2D.Impulse);
                    anim.SetBool("isFlying", false);
                }
            }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            coinsLeft -= 1;
            playerSounds.clip = coin;
            playerSounds.Play();
            score.text = "Coins Left: " + coinsLeft.ToString();
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Enemy")
        {
            livesLeft -= 1;
            playerSounds.clip = badCoin;
            playerSounds.Play();
            lives.text = "Lives: " + livesLeft.ToString();
            Destroy(collision.collider.gameObject);
        }

         if (collision.collider.tag == "Ground")
        {
            anim.SetBool("isGrounded", true);
            grounded = true;
            
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            isOnGround = true;
            anim.SetBool("isGrounded", true);
            anim.SetBool("isFlying", false);

            if (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.UpArrow) ))
            {
                isOnGround = false;
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetBool("isFlying", true);
                anim.SetBool("isGrounded", false);
            }
            
        }
    }

    void Flip()
    {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
    }

    IEnumerator RestartGame(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("SampleScene");
    }
}