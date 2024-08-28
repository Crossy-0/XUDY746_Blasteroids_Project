using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class PlayerMovement : MonoBehaviour
{
    /*
     * ------This is the MAIN script in charge of player movement---------
     * all player input from the keyboard and mouse,
     * and player interactions such as health, shooting, moving the character
     */


    //references for components or other scripts
    public UIScript uScript;
    public AudioController aControler;
    private Rigidbody2D rb;
    public bool paused = false;


    //variables in charge of whether you are moving 
    private bool forward = false;
    private bool backwards = false;
    private bool boosting = false;


    //setting the Input Controls for the user (If time, add playerPrefs to save these choices)
    private KeyCode forwards_key = KeyCode.W;
    private KeyCode backwards_key = KeyCode.S;
    private KeyCode boost_key = KeyCode.LeftShift;
    private KeyCode shoot_key = KeyCode.Space;

    private Keyboard keyboard;
    //[SerializeField] private GameInputSettings inputSys;
    [SerializeField] private InputGetter ig;
    private InputActionAsset inputAsset;
    private InputAction a_Move;
    private InputAction a_Forward;
    private InputAction a_Backward;
    private InputAction a_Boost;
    private InputAction a_PrimFire;
    private InputAction a_SecondFire;


    //general Property Vars
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private float speedCap = .5f;
    [SerializeField]
    private float boostSpeed = 2.0f;

    
    
    //adding the prefab for the bullets and property vars
    [SerializeField]
    private Bullet1Script normalPref;
    private float normalFireRate = 0.2f;
    private bool canShoot = true;
    private float shootDelayTimer;


    //variables in charge of the seizmic bomb
    [SerializeField]
    private SeizmicBombScript sBomb;
    [SerializeField]
    private int charges = 3;
    private float fireRateSeizmic = 2f;
    private bool canShootSeizmic = true;
    [SerializeField] private float seizmicRechargeTime;


    //internal counters
    private int health = 100;
    private float pauseMenuTimer = 0;//the timer used to stop pause menu from activating each frame.
    private int ammoCounter = 5;// the number of normal bullets the player has
    private float ammoCounterTime;
    [SerializeField] private int maxAmmo = 5;
    [SerializeField] private float bulletRegenTimer;//the time it takes to regen ammo
    [SerializeField] private int bulletRegenAmount; // how many bullets it will regen

    //particle system for getting hit
    public ParticleSystem shipHitParticles;

    private void Awake() //auto grabs rb component and sets internal move speed
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
    }

    private void Start()
    {
        ammoCounter = maxAmmo;
        keyboard = Keyboard.current;
        uScript.UpdateAmmoCounter(ammoCounter,maxAmmo);
    }

    private void OnEnable()
    {

        inputAsset = ig.GetInputSettings();
        a_Forward = inputAsset.FindAction("Forward");
        a_Forward.Enable();
        a_Backward = inputAsset.FindAction("Backwards");
        a_Backward.Enable();
        a_Boost = inputAsset.FindAction("Boost");
        a_Boost.Enable();
        a_PrimFire = inputAsset.FindAction("PrimaryFire");
        a_PrimFire.Enable();
        a_SecondFire = inputAsset.FindAction("SecondaryFire");
        a_SecondFire.Enable();
    }

    private void OnDisable()
    {
        a_Forward.Disable();
        a_Backward.Disable();
        a_Boost.Disable();
        a_PrimFire.Disable();
        a_SecondFire.Disable();
    }


    public void UpdateUI() {
        uScript.UpdateAmmoCounter(ammoCounter, maxAmmo);
        uScript.updateHealth(health);
    }

    public void setChargesForSeiz(int n) {
        charges = n;
    }

    void Update()
    {
        //check for no health
        if (health <= 0) {
            uScript.endGame();//references UIScript
        }

        //timer for the ammo regen (only works if the mouse is not clicked) 
        if (!a_PrimFire.IsPressed()) {
            if (ammoCounter < maxAmmo)
            {
                ammoCounterTime = Mathf.Max(0, ammoCounterTime -= Time.deltaTime);
                if (ammoCounterTime == 0)
                {
                    ammoCounter = Mathf.Min(maxAmmo, ammoCounter += bulletRegenAmount);
                    ammoCounterTime = bulletRegenTimer;
                    uScript.UpdateAmmoCounter(ammoCounter, maxAmmo);
                }
            }
            else
            {
                ammoCounterTime = bulletRegenTimer;
            }
        }
        


        //reduce the pause menu timer 
        pauseMenuTimer = Mathf.Max(0, pauseMenuTimer -= Time.deltaTime);

        
        if ((keyboard != null) && (keyboard.escapeKey.isPressed) && (pauseMenuTimer <=0)) {
            paused = !paused;
            pauseMenuTimer = 0.8f;//the fastest you can pause and resume

            //pauses or un-pauses the game based on internal state using the UIScript references
            if (paused)
            {
                uScript.pauseGame();
            }
            else {
                uScript.resumeGame();
            }
        }

        //check to prevent input when paused
        if (!paused)
        {

            forward = a_Forward.IsPressed();
            backwards = a_Backward.IsPressed();
            boosting = a_Boost.IsPressed();

            shootDelayTimer = Mathf.Max(0,shootDelayTimer -= Time.deltaTime);

            
            if (a_PrimFire.IsPressed() && shootDelayTimer == 0)
            {
                shootDelayTimer = normalFireRate;
                if (ammoCounter > 0)
                {
                    shoot();
                    ammoCounter -= 1;
                    uScript.UpdateAmmoCounter(ammoCounter, maxAmmo);
                }
                else {
                    //play sound for out of ammo
                    aControler.PlayOutOfAmmo();
                }
                
            }

            else if (a_SecondFire.WasReleasedThisFrame())
            {
                StartCoroutine(shootSBomb());
            }
        }
    }


    //prevents frame rate based move speed 
    private void FixedUpdate()
    {
        movement();
    }


    public void IncreaseMaxAmmo(int amount) {
        maxAmmo += amount;
    }

    public void IncreaseRechargeAmount(int amount) {
        bulletRegenAmount += amount;
    }

    public void AddHealth(int amount) {
        health += amount;
    }


    /*---------The methods in charge of shooting --------
     * these will instantiate a prefab of the script(the one attatched to the prefab),
     * instead of the bullet. This means it can reference the script attatched directly.
     */


    //method for basic bullets
    void shoot() {
        //spawning it, and playing the sfx.
        Bullet1Script bScript = Instantiate(this.normalPref, this.transform.position, this.transform.rotation);
        aControler.playShootSound();//references the audio controller script
        bScript.shoot(this.transform.up);//referencing the attatched bullet script
    }

    //method for shooting a seizmic projectile
    void shootSeizmic() {
        SeizmicBombScript sScript = Instantiate(this.sBomb,this.transform.position,this.transform.rotation);//spawning
        sScript.shoot();//referencing seizmicBomb script
    }



    //method reduces the internal health by the amount passed in.
    public void reduceHealth(int reduceBy) {
        health -= reduceBy;
        uScript.updateHealth(health);//references UIScript to update UI
        aControler.PlayShipHitSFX();
        ParticleSystem collisionParticles = Instantiate(shipHitParticles, this.transform);
        collisionParticles.Play();
    }



    //movement method (controls all basic movement)
    void movement() {
        if (forward)
        {
            rb.AddForce(transform.up * currentSpeed);
        }
        else if (backwards)
        {
            rb.AddForce(transform.up * -currentSpeed);
        }

        //changes the force added when moving if boosting
        if (boosting)
        {
            currentSpeed = boostSpeed;
        }
        else {
            currentSpeed = moveSpeed;
        }

        //caps the max speed to avoid infinite speed growth
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speedCap);


        //mouse rotation vvv
        Vector2 screenMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //get screen pos of mouse //changed for new input system ---------------------------------------------------------------
        Vector2 dir = (screenMousePos - (Vector2)this.transform.position).normalized;
        this.transform.up = dir;
    }

    //shooting bullets
    IEnumerator shootNormal() {
        //boolean lock to limit fire rate
        if (canShoot){
            canShoot = false;
            shoot();
            yield return new WaitForSeconds(normalFireRate);
            canShoot = true;
        }

    }


    //shooting a seizmic bomb
    /*NOTE:
     * recharging ammo hasn't been implemented yet, so the max charges has been set to 200.
     * It has no notification to let the user know how many are left yet, however it is unlikely
     * the user will run out of charges.
     */
    IEnumerator shootSBomb()
    {
        //boolean lock to cap fire rate
        if (canShootSeizmic)
        {
            canShootSeizmic = false;
            //only fires if you have a charge
            if (charges > 0)
            {
                charges -= 1;
                uScript.UpdateSeizmicAmmo(charges);
                aControler.PlaySeizmicShootSFX();
                aControler.playSeizmicBomb();//references Audio Controler script
                shootSeizmic();
            }
            yield return new WaitForSeconds(fireRateSeizmic);
            canShootSeizmic = true;

            yield return new WaitForSeconds(seizmicRechargeTime);
            charges += 1;
            uScript.UpdateSeizmicAmmo(charges);
            aControler.PlaySeizmicRechargeSFX();
        }
    }
}
