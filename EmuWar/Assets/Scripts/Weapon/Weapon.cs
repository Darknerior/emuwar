using System;
using System.Collections;
using Tools;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private int magSize;
    [SerializeField] private float range;
    private int currentMag;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float shotsPerSecond;
    [SerializeField] private bool reloaded = true;
    [SerializeField] private Transform aimDirection;
    [SerializeField] private float accuracyPercentage;
    [SerializeField] private float maxErrorInAimRadius;
    private bool readyToFire = true;
    private Coroutine coroutine;
    private bool aimOverride;
    private Camera cam;
    private float SPS{
        get => 1f / shotsPerSecond;
        }


    private void Start()
    {
        currentMag = magSize;
        if(TryGetComponent(out PlayerController _)) aimOverride = true;
        cam = aimDirection.GetComponent<Camera>();
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Shoot();
        }
    }



    // ReSharper disable Unity.PerformanceAnalysis
    public void Shoot()
    {
        if (!readyToFire) return;

        if (currentMag == 0)
        {
            if (reloaded)
            {
                Reload();
            }
            return;
        }

        Vector3 direction = Vector3.zero;
        if (aimOverride)
        {
            var aimPos = cam.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2, range + 2));
            direction = (aimPos - transform.position).normalized;
        }
        else { direction = aimDirection.forward; }
        Accuracy(ref direction);
        GameManager.Instance.Pool.Get(ObjectList.BULLET, true).GetComponent<Bullet>().SetFactors(this, damage, range,transform.position, direction, projectileSpeed);
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
        reloaded = false;
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
/// <summary>
/// Modifies the given vector to take accuracy paramaters into account. Should only be used on directional vectors
/// </summary>
/// <param name="aim"></param>
    private void Accuracy(ref Vector3 aim)
    {
        var accuracy = 1f-(accuracyPercentage/100f);
        var tempAim = Random.insideUnitCircle;
        tempAim *= (accuracy*maxErrorInAimRadius);
        aim.AddVector2(tempAim);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        coroutine = null;
        readyToFire = true;
        currentMag = magSize;
        reloaded = true;
    }
}
