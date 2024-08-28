using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Author:    Declan Cross
 * Created:   14.08.2024
 * 
 **/



public class AudioController : MonoBehaviour
{


    /*
     * NOTE:  
     * All sound effects were initially created on a single reference game object. 
     * this means that any sound that is played will start again when requested to play again.
     * This causes a small audio error whereby sounds restart.
     * this needs to be changed so that the audiosources are attatched to a new empty game object in the scene as a prefab.
     * NOT added to the prefab it relates to since the audio would cut off when gameObject is destroyed.
     * 
     * 
     * NOTE: Approaching this 2 years later, i learned the problem was due to using the AudioClip.Play() not audioClip.PlayOneShot()
     * Relatively simple once i learned of this, but now it means that the audio is structured to play out of individual audio sources instead of just one.
     * Very inefficient and unnecessary. 
     * Suggest making them into one audio source when possible.
     */

    

    

    //public references to the audio sources on the scene
    public AudioSource seizmicBomb;
    public AudioSource breakSFX;
    public AudioSource bulletShoot;
    public AudioSource outOfAmmoSource;
    public AudioSource shipHitSource;
    public AudioSource powerupSource;
    public AudioSource seizmicRechargeSource;
    public AudioSource seizmicShootSource;

    public AudioClip breakClip, seizmicClip, bulletClip, outOfAmmoClip, shipHitClip,powerupBeep, powerupPickup, seizmicRechargeClip, seizmicShootClip;

 
    [SerializeField]
    private float minPitch;
    [SerializeField]
    private float maxPitch;
    [SerializeField]
    private float minVol;
    [SerializeField]
    private float maxVol;
    [Range(-3.0f,3.0f)]
    [SerializeField] private float shipHitPitchMin, shipHitPitchMax; 

    /* Public methods for playing sounds.
     * All the following are intended to be called via other scripts.
     */
     
    public void playBreakSound() { //asteroid Break sfx
        breakSFX.PlayOneShot(breakClip);
    }

    public void playShootSound() { //bullet shoot sfx
        bulletShoot.pitch = Random.Range(minPitch,maxPitch);
        bulletShoot.volume = Random.Range(minVol,maxVol);
        bulletShoot.PlayOneShot(bulletClip);
    }

    public void playSeizmicBomb() { // seizmic bomb sfx
        seizmicBomb.PlayOneShot(seizmicClip);
    }

    public void PlayOutOfAmmo() { // out of ammo
        outOfAmmoSource.PlayOneShot(outOfAmmoClip);
    }

    public void PlayShipHitSFX() {
        shipHitSource.pitch = Random.Range(shipHitPitchMin,shipHitPitchMax);
        shipHitSource.PlayOneShot(shipHitClip);
    }


    public void PlayPowerupBeepSFX() {
        powerupSource.PlayOneShot(powerupBeep);
    }

    public void PlayPowerupPickupSFX() {
        powerupSource.PlayOneShot(powerupPickup);
    }

    public void PlaySeizmicRechargeSFX() {
        seizmicRechargeSource.PlayOneShot(seizmicRechargeClip);
    }
    public void PlaySeizmicShootSFX()
    {
        seizmicShootSource.PlayOneShot(seizmicShootClip);
    }
}
