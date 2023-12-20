using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f;

    protected override void CollectPellet()
    {
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
    }

}
