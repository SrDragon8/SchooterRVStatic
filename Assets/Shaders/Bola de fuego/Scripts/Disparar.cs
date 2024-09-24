using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparar : MonoBehaviour
{

    public Transform throwpoint;
    public GameObject throwpointPrefab;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Disparo();
        }
    }

    void Disparo()
    {
        Instantiate(throwpointPrefab, throwpoint.transform.position, throwpoint.transform.rotation);
    }
}
