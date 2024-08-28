using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class SeizmicRingScript : MonoBehaviour
{
    //component and script references
    public ParticleSystem collideP;
    private UIScript u;


    //automatically gets the UIScript reference
    private void Start()
    {
        u = (GameObject.FindWithTag("UIScripter")).GetComponent<UIScript>();
    }


    //defines collision properties
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.transform.tag == "Asteroid")//if it hits an asteroid:
        {
            //internally save the rotation and position
            Vector3 place = new Vector3(collision.transform.position.x, collision.transform.position.y,0);
            Quaternion rot = new Quaternion(collision.transform.rotation.x, collision.transform.rotation.y, collision.transform.rotation.z,0);

            //create a particle system on this position
            ParticleSystem newP = Instantiate(this.collideP, place,rot);

            //destroy the object, and adds to the score
            Destroy(collision.gameObject);
            StartCoroutine(destroy());
            u.AddScore(100); //references the UIScript
        }
    }


    //coroutine to destroy the ring potentially sooner if it does collide with something.
    IEnumerator destroy() {
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
    }
}
