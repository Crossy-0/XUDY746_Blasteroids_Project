using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class Asteroid : MonoBehaviour
{

    /*
     * This method is in charge of handling the interactions and properties of asteroids within the game.
     */



    //public references to scripts or prefabs
    public AudioController aController;
    public Asteroid asPrefab;
    public UIScript UI;
    public PlayerMovement player;
 
    //initial values for first spawn
    public float initialSize = 3.5f;
    public int initialBreak = 2;
    public float initialVel = 150f;

    //standard components
    [SerializeField]
    private Sprite[] sprites;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //internal variables for creating prefabs
    private float size;
    private int breaks;
    private float velocity;


    private void Awake()
    {
        //collects all components needed for the script from the scene
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        UI = FindObjectOfType<UIScript>();
        player = FindObjectOfType<PlayerMovement>();
        aController = FindObjectOfType<AudioController>();
    }


    //initial spawn -- called when the asteroid prefab made for the first time
    public void asteroid() {
        //sets essential variables to default or chosen values
        breaks = initialBreak;
        sr.sprite = sprites[Random.Range(0, sprites.Length)];
        size = initialSize;

        //sets the rotation,scale,mass and velocity of the spawn
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;
        rb.mass = this.size;
        velocity = initialVel;

        //adds a force to the rigidbody to move it
        Vector2 dir = this.transform.up;
        rb.AddForce(dir * initialVel);
    }



    /*
     * The method for creating asteroids after they break
     * Takes inputs for Size,Rotation,Breaks left, velocity, left check
     * NOTE: Left is an unused var, the feature has not been implemented yet.
     */
    public void asteroid(float S, float dir, int b, float vel,bool left) {

        //sets layer name, and reduces number of breaks left
        this.gameObject.layer = LayerMask.NameToLayer("Asteroid");
        breaks = b - 1;

        //setting basic variables such as size rotation etc...
        size = S;
        velocity = vel;
        sr.sprite = sprites[Random.Range(0, sprites.Length)];
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, dir);
        this.transform.localScale = Vector3.one * this.size;
        rb.mass = this.size;
    }

    //resets the movement of the asteroid after it has spawned
    public void resetMove() {
        rb.AddForce(this.transform.up * velocity);
    }


    //Method in charge of handling the break functionality of an asteroid
    public void breakAsteroid()
    {
        if (breaks <= 0) 
        {
            Destroy(this.gameObject);
            UI.AddScore(5); //references UIScript
        }
        else {
            //gets the rotation, position and other components needed for spawning the new prefabs
            float dir = Random.Range(0,360f);
            Vector3 destroyPos = this.transform.position;

            //removes original asteroid from collision layer, and spawns the two new asteroid
            this.gameObject.layer = LayerMask.NameToLayer("DeadAsteroid");
            Asteroid ast = Instantiate(this.asPrefab, new Vector3(destroyPos.x+0.1f, destroyPos.y-0.2f, destroyPos.z), this.transform.rotation);
            ast.asteroid((this.size / 1.2f), dir, breaks, velocity,true);

            
            Asteroid ast2 = Instantiate(this.asPrefab, new Vector3(destroyPos.x - 1, destroyPos.y + 0.5f, destroyPos.z), this.transform.rotation);
            ast2.asteroid((this.size / 1.35f), dir + Random.Range(60,160), breaks, velocity,false);

            //starts movement for new prefabs
            ast.resetMove();
            ast2.resetMove();

            //destroys original
            Destroy(this.gameObject);
            UI.AddScore(10); //references UIScript
        }
    }


    //details collision functionality
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit by bullet: trigger the break, if hit by player: reduce health
        if (collision.transform.tag == "Bullet")
        {
            aController.playBreakSound();//references the Audio Controller script
            breakAsteroid();
        }
        else {
            if (collision.transform.tag == "Player") {
                player.reduceHealth(5);//references the PlayerMovement script
            }
        }
        
    }
}
