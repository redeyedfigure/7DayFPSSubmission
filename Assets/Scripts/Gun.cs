using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    public bool reFire = true;

    public Camera fpsCam;
    public GameObject impactEffect;
    public AudioSource shotSound;
    public ParticleSystem muzzleFlash;

    private float nextTimeToFire = 0f;

    // Update is called once per frame
    void Update()
    {
        if(reFire == true){
            if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire){
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
            }
        }
        if(reFire == false){
            if(Input.GetButtonDown("Fire1")){
            Shoot();
            }
        }
    }

    void Shoot (){
        shotSound.Play();
        muzzleFlash.Play();
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if(target != null){
                target.TakeDamage(damage);
            }

            if(hit.rigidbody != null){
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }


            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}
