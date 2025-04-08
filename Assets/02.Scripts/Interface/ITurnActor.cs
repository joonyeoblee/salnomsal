using UnityEngine;

public interface ITurnActor
{
    public int BasicSpeed { get; set; }
    public int CurrentSpeed { get; set; }

    public bool IsAlive { get; }
    public void StartTurn();
    public void EndTurn();
}
