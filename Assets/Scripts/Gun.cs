using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    public bool reFire = true;

    public int maxAmmo = 10;
    private int currentAmmo = -1;
    public int reservedAmmo = 80;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera fpsCam;
    public GameObject impactEffect;
    public AudioSource shotSound;
    public ParticleSystem muzzleFlash;
    public Text ammunitions;
    public AudioSource reload;
    public Animator animator;
    public AudioSource emptyCartridge;

    private float nextTimeToFire = 0f;

    void Start(){
        if(currentAmmo == -1)
            currentAmmo = maxAmmo;

    }
    void Update()
    {
        if(isReloading)
            return;
        ammunitions.text = " Ammo: " + currentAmmo + "/" + reservedAmmo;
        if(currentAmmo <= 0 && reservedAmmo > 0){
            StartCoroutine(Reload());
            return;
        }
        else if(Input.GetButtonDown("Fire1") && reservedAmmo <= 0 && currentAmmo <= 0){
            emptyCartridge.Play();
            return;
        }
        if(reFire == true){
            if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0){
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

    IEnumerator Reload(){
        isReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("Reloading", true);
        reload.Play();
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("Reloading", false);
        if(maxAmmo <= reservedAmmo){
            currentAmmo = maxAmmo;
            reservedAmmo = reservedAmmo - maxAmmo;
        }
        else if(maxAmmo > reservedAmmo){
            currentAmmo = reservedAmmo;
            reservedAmmo = 0;
        }
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot (){
        shotSound.Play();
        muzzleFlash.Play();
        currentAmmo--;
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
