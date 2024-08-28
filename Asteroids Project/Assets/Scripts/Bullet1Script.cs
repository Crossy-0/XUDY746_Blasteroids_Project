using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class Bullet1Script : MonoBehaviour
{
    //creates references to the prefabs and components attatched to the script.
    private Rigidbody2D rb;
    public ParticleSystem bulletExplosion;

    //private vars for bullet properties
    [SerializeField]
    private float shootSpeed = 50f;
    private float life = 8f;
    

    private void Awake() //gets components
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //shoot method, takes a direction of where to shoot
    public void shoot(Vector2 dir){
        
        //add force, and trigger destruction after set time
        rb.AddForce(dir * shootSpeed);
        Destroy(this.gameObject, this.life);
    }

    //defines what happens on collision with other objects
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //gets transform information, spawns the particle system explosion, destroys the gameObject
        Vector3 p = this.transform.position;
        Quaternion r = this.transform.rotation;
        Instantiate(bulletExplosion,p,r);
        Destroy(this.gameObject);

    }
}
