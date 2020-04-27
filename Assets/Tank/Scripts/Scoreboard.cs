using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{

    public Spawner Spawner;
    public Text CurrentScore;
    public Text PreviousScore;

    void Update()
    {
        CurrentScore.text = Spawner.GetCurrentTankScore().ToString() + " / " + Spawner.GetCurrentTankDeath().ToString();
        PreviousScore.text = Spawner.GetPreviousTankScore().ToString() + " / " + Spawner.GetPreviousTankDeath().ToString();
    }
}
