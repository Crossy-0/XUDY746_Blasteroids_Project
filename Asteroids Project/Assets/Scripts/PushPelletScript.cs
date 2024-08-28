using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class PushPelletScript : MonoBehaviour
{
    public int fuse = 3;
    [SerializeField]
    private TextMesh countdownTextMesh;
    [SerializeField]
    [Range(0f, 20f)]
    private float rangeOfImpact;

    [SerializeField]
    [Range(0f, 1f)]
    private float smallImpactDist,midImpactDist;

    [SerializeField]
    private float pushForce;




    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position,rangeOfImpact);
    }


    public void Start()
    {
        countdownTextMesh.text = "";
        StartCoroutine(StartCountDown());
    }

    public void Explode() {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x,transform.position.y),rangeOfImpact);
        foreach (var hit in hitColliders) {
            if (hit.gameObject.tag == "Asteroid") {
                GameObject asteroid = hit.gameObject;
                PushObject(asteroid);
            }
        }
        Kill();

    }

    public void PushObject(GameObject ast) {
        float dist = Vector2.Distance((Vector2)transform.position, (Vector2)ast.transform.position);
        if (dist < (rangeOfImpact * smallImpactDist))
        {
            PushMove((pushForce),ast);
        }
        else if (dist < (rangeOfImpact * midImpactDist))
        {
            PushMove((pushForce * midImpactDist), ast);
        }
        else {
            PushMove((pushForce * smallImpactDist),ast);
        }

    }

    public void PushMove(float force,GameObject target) {
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos = lookPos.normalized;

        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        rb.AddForce(force * lookPos);
        
    }

    public void Kill() {
        Destroy(this.gameObject);
    }

    


    IEnumerator StartCountDown() {
        while (fuse > 0)
        {
            countdownTextMesh.text = fuse.ToString();
            yield return new WaitForSeconds(1f);
            fuse -= 1;
            if (fuse <= 0) {
                countdownTextMesh.text = "";
                Explode();
            }
        }
    }

}
