using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour , IPoolable
{
    [SerializeField] private float range;
    [SerializeField] private float timeOut;
    private Vector3 startPos;
    private float clock;
    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        startPos = transform.position;
        clock = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        while (Vector3.Distance(transform.position, startPos) < range && clock < timeOut)
        {
            clock += Time.deltaTime;
            return;
        }
        
        ReturnToPool();
    }
}
