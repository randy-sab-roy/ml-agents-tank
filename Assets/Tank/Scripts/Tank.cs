using UnityEngine;
using UnityEngine.UI;
using MLAgents;
using MLAgents.Sensors;

public class Tank : Agent
{
    [Header("Game stuff")]
    public float Speed;
    public float RotateSpeed;
    public float BulletSpeed;
    public float BulletOffset;
    public float BulletHeight;
    public float ShootCooldown;
    public float DeadCooldown;
    public int Bullets;
    public GameObject BulletPrefab;
    public GameObject TankModel;
    public GameObject GhostModel;

    [Header("AI Stuff")]
    public float KillReward = 1;
    public float ShootReward = 0.05f;
    public float DeathPenalty = -1;

    [Header("UI")]
    public Text KillText;

    private Rigidbody agentRb;
    private BoxCollider boxCollider;
    private float lastShot = 0;
    private float lastDied = 0;
    private int bulletCount;
    private Vector3? startPosition = null;
    private Quaternion startRotation;
    private bool didKill = false;
    private int killCount = 0;
    private int deathCount = 0;

    public int KillCount => killCount;
    public int DeathCount => deathCount;
    public Vector3 StartPosition {set {startPosition = value;}}
    public Quaternion StartRotation {set {startRotation = value;}}

    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        bulletCount = Bullets;

        if (startPosition == null)
        {
            startPosition = transform.position;
            startRotation = transform.rotation;
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        AddReward(-1f / 3000f);
        if (lastDied <= 0 && !boxCollider.enabled)
        {
            ResetTank();
            EndEpisode();
        }

        if (lastDied <= 0)
        {
            int translation = (int)vectorAction[0] - 1;
            int rotation = (int)vectorAction[1] - 1;
            agentRb.MoveRotation(agentRb.rotation * Quaternion.Euler(transform.up * Time.deltaTime * RotateSpeed * rotation));
            agentRb.velocity = transform.forward * Speed * Time.deltaTime * translation;
        }
        else
        {
            agentRb.velocity = Vector3.zero;
            lastDied -= Time.deltaTime;
        }

        if (lastDied <= 0 && lastShot <= 0 && bulletCount > 0 && vectorAction[2] == 1)
        {
            bulletCount--;
            lastShot = ShootCooldown;
            var bullet = Instantiate(BulletPrefab, transform.position + transform.forward * -BulletOffset + transform.up * BulletHeight, transform.rotation, transform.parent);
            bullet.GetComponent<Bullet>().Owner = this;
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
            if (!didKill)
            {
                AddReward(ShootReward);
            }
        }
        else
        {
            lastShot -= Time.deltaTime;
        }
    }

    public override float[] Heuristic()
    {
        var actionsOut = new float[3];
        actionsOut[0] = -Input.GetAxisRaw("Vertical") + 1;
        actionsOut[1] = Input.GetAxisRaw("Horizontal") + 1;
        actionsOut[2] = Input.GetKey("space") ? 1 : 0;

        return actionsOut;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(bulletCount);
    }


    private void ResetTank()
    {
        gameObject.layer = LayerMask.NameToLayer("Tank");
        GhostModel.SetActive(false);
        TankModel.SetActive(true);
        boxCollider.enabled = true;
        transform.position = startPosition ?? Vector3.zero;
        transform.rotation = startRotation;
    }

    public void Kill()
    {
        AddReward(DeathPenalty);
        GhostModel.SetActive(true);
        TankModel.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Ghost");
        boxCollider.enabled = false;
        lastDied = DeadCooldown;
        deathCount++;
    }

    public void ReloadBullets()
    {
        bulletCount = Bullets;
    }

    public void GiveKill()
    {
        AddReward(KillReward);
        killCount++;
        didKill = true;

        if (KillText != null)
            KillText.text = killCount.ToString();
    }
}
