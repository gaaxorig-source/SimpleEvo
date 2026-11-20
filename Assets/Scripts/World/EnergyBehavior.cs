using UnityEngine;

public class EnergyBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private EnergySpawner energySpawner;

    public float energy = 0.1f;
    void Start()
    {
        Vector3 scale = transform.localScale;
        scale.x += energy;
        scale.y += energy;

        transform.localScale = scale;
    }

    public float Eaten()
    {
        energySpawner.MinuseOne();
        Destroy(this.gameObject);

        return energy;
    }

    public void SetSpawner(EnergySpawner spawner)
    {
        energySpawner = spawner;
    }
}
