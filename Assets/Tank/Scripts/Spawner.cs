using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MLAgents.Policies;

public class Spawner : MonoBehaviour
{
    public GameObject TankPrefab;
    public GameObject PreviousTankPrefab;
    public int TankCount;
    public int SpawnRadius;
    public int PreviousTankCount;
    public int PreviousTankOffset;

    private List<Tank> previousTanks = new List<Tank>();
    private List<Tank> currentTanks = new List<Tank>();

    void Start()
    {
        SpawnTanks();
    }

    void SpawnTanks()
    {
        // Tanks
        for (int i = 0; i < TankCount; i++)
        {
            var pos = transform.position + Quaternion.Euler(0, i * 360 / TankCount, 0) * Vector3.forward * SpawnRadius;
            var rot = Quaternion.Euler(0, i * 360 / TankCount, 0);
            var tank = Instantiate(TankPrefab, pos, rot, transform);
            var tankComponent = tank.GetComponent<Tank>();
            tankComponent.StartPosition = pos;
            tankComponent.StartRotation = rot;
            currentTanks.Add(tankComponent);
            tank.GetComponent<BehaviorParameters>().TeamId = i;
        }

        // Previous Tanks
        for (int i = 0; i < PreviousTankCount; i++)
        {
            var pos = transform.position + Quaternion.Euler(0, (i * 360 / PreviousTankCount) + PreviousTankOffset, 0) * Vector3.forward * SpawnRadius;
            var rot = Quaternion.Euler(0, (i * 360 / PreviousTankCount) + PreviousTankOffset, 0);
            var tank = Instantiate(PreviousTankPrefab, pos, rot, transform);
            var tankComponent = tank.GetComponent<Tank>();
            tankComponent.StartPosition = pos;
            tankComponent.StartRotation = rot;
            previousTanks.Add(tankComponent);
            tank.GetComponent<BehaviorParameters>().TeamId = i + TankCount;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        for (int i = 0; i < TankCount; i++)
        {
            var pos = transform.position + Quaternion.Euler(0, i * 360 / TankCount, 0) * Vector3.forward * SpawnRadius;
            Gizmos.DrawCube(pos, new Vector3(1, 1, 1));
        }

        Gizmos.color = new Color(0, 1, 0, 0.5f);
        for (int i = 0; i < PreviousTankCount; i++)
        {
            var pos = transform.position + Quaternion.Euler(0, (i * 360 / PreviousTankCount) + PreviousTankOffset, 0) * Vector3.forward * SpawnRadius;
            Gizmos.DrawCube(pos, new Vector3(1, 1, 1));
        }
    }

    public int GetCurrentTankScore()
    {
        return currentTanks.Sum(t => t.KillCount);
    }

    public int GetCurrentTankDeath()
    {
        return currentTanks.Sum(t => t.DeathCount);
    }

    public int GetPreviousTankScore()
    {
        return previousTanks.Sum(t => t.KillCount);
    }

    public int GetPreviousTankDeath()
    {
        return previousTanks.Sum(t => t.DeathCount);
    }
}
