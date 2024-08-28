using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/
public class PowerUp : MonoBehaviour
{
    //types of power ups
    private enum powerUpType { 
        Health,
        Ammo,
        AmmoRecharge
    };

    //necessary component values
    [SerializeField] private powerUpType type;
    private PlayerMovement player;
    private int healthAmount = 20;
    private int ammoIncreaase = 2;
    private int ammoRechargeIncrease = 1;
    private float pingTimer = 0f;
    private float timeMax = 3f;
    private AudioController aController;

    private void Awake()
    {
        aController = FindObjectOfType<AudioController>();
    }

    private void Update()
    {
        pingTimer = Mathf.Max(0, pingTimer -= Time.deltaTime);
        if (pingTimer == 0) {
            aController.PlayPowerupBeepSFX(); 
            pingTimer = timeMax;
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.GetComponent<PlayerMovement>();
            aController.PlayPowerupPickupSFX();
            switch (type)
            {
                case powerUpType.Health:
                    player.AddHealth(10);
                    break;
                case powerUpType.Ammo:
                    player.IncreaseMaxAmmo(ammoIncreaase);
                    break;
                case powerUpType.AmmoRecharge:
                    player.IncreaseRechargeAmount(ammoRechargeIncrease);
                    break;
            }
            player.UpdateUI();
            Destroy(this.gameObject);
        }
    }


}
