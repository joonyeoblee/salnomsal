using UnityEngine;

public interface ITurnActor
{
    public int BasicSpeed { get; set; }

    public void StartTurn();
    public void EndTurn();
}
