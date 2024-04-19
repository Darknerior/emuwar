using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairEnabler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Subscribe(EnableCrosshair);
    }

    private void EnableCrosshair(bool isEnabled) => gameObject.SetActive(!isEnabled);
}
