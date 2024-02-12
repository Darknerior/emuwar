using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private int magSize;
    private int currentMag;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float shotsPerSecond;
    [SerializeField] private bool reloaded = true;
    private bool readyToFire = true;
    private Coroutine coroutine;
    private float SPS{
        get => 1f / shotsPerSecond;
        }


    private void Start()
    {
        currentMag = magSize;
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Shoot();
        }
    }



    public void Shoot()
    {
        if (!readyToFire) return;

        if (currentMag == 0)
        {
            if (reloaded)
            {
                Reload();
                reloaded = false;
            }
            return;
        }
        GameObject bullet = GameManager.Instance.Pool.Get(ObjectList.BULLET,true);
        bullet.transform.position = this.transform.position;
        bullet.transform.forward = this.transform.forward;
        bullet.gameObject.SetActive(true);
        bullet.GetComponent<Rigidbody>().AddRelativeForce(this.transform.forward * projectileSpeed,ForceMode.Impulse);
        currentMag--;
        
        if (currentMag != 0)
        {
            readyToFire = false;
            coroutine = StartCoroutine(ShotCooldown());
        }
    }

    public void Reload()
    {
         coroutine ??= StartCoroutine(ReloadCooldown());
    }

    private IEnumerator ReloadCooldown()
    {
        yield return new WaitForSeconds(reloadSpeed);

        currentMag = magSize;
       coroutine = null;
        reloaded = true;
    }

    private IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(SPS);

        coroutine = null;
        readyToFire = true;
    }
}
