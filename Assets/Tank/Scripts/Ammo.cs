using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public float Cooldown = 5f;
    private float time = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Tank"))
        {
            other.GetComponent<Tank>().ReloadBullets();
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
        time = Cooldown;
    }

    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                GetComponent<Collider>().enabled = true;
                GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

}
