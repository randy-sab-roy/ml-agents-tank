using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float DestroyTimeout;

    private Tank owner;
    public Tank Owner { set { owner = value; } }

    void Start()
    {
        Destroy(gameObject, DestroyTimeout);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tank"))
        {
            var tank = collision.gameObject.GetComponent<Tank>();
            if (owner != tank)
                owner.GiveKill();

            tank.Kill();
            Destroy(gameObject);
        }
    }
}
