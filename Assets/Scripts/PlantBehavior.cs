using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class PlantBehavior : Behavior
{

    private SunBehavior sun;

    private Collider2D[] plantsCompetitorBuffer;
    private float scanCompetitorBatery = 0f;

    [SerializeField]
    private LayerMask myMask;


    private float plantBehaviorEnergyPenaltyPersent = 0;

    public float concurents = 0;

    public override bool CanBeActive()
    {
        return (myGens.IsPlant && !myGens.IsCarnivorous && !myGens.IsHerbivore);
    }

    protected override void OnAwake()
    {
        plantsCompetitorBuffer = new Collider2D[32];
        if (sun == null)
        {
            sun = GameObject.Find("Sun").GetComponent<SunBehavior>();
        }
    }

    protected override void OnStart()
    {
        int layer = LayerMask.NameToLayer("PlantBehavior");
        gameObject.layer = layer;
        
        energyMinuse = myGens.EnergyMinuseGen * weight;
    }

    protected override void OnUpdate(float deltaTime)
    {
        scanCompetitorBatery += deltaTime;
        if (scanCompetitorBatery > 1)
        {
            scanCompetitorBatery = 0f;
            ScanPlantsCompetitor();
        }
    }

    protected override void OnFixedUpdate(float fixedDeltaTime)
    {
        float deltaEnergy = sun.solarPower;
        if (weight > 1)
        {
            deltaEnergy *= weight;
        }

        AddEnergy(deltaEnergy);
    }

    public void AddEnergy(float addedEnergy)
    {
        float penaltyDelta = energyPenaltyPersent + plantBehaviorEnergyPenaltyPersent;
        energy += addedEnergy - (addedEnergy / 100 * penaltyDelta);

        allEnergyPenaltyPersent = penaltyDelta;
    }

    public void ParasitFactor(float parasitWeight, float parasitTake)
    {
        parasitTake *= (parasitWeight / weight);
        energy -= parasitTake;
    }


    protected override GameObject Divide()
    {
        if (energy < 1)
        {
            return null;
        }

        float direction = Random.Range(1, 5);
        Vector2 cloneImpuls = new Vector2();
        switch (direction)
        {
            case 1: cloneImpuls = cloneImpuls = Vector2.right; break;
            case 2: cloneImpuls = cloneImpuls = Vector2.left; break;
            case 3: cloneImpuls = cloneImpuls = Vector2.up; break;
            case 4: cloneImpuls = cloneImpuls = Vector2.down; break;
            default: cloneImpuls = transform.position; break;
        }

        energy = energy / 2 - devideImpuls;
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
        clone.GetComponent<Rigidbody2D>().AddForce(cloneImpuls * devideImpuls, ForceMode2D.Impulse);

        energyPenaltyPersent += 10;

        return clone;
    }

    private void ScanPlantsCompetitor()
    {
        plantBehaviorEnergyPenaltyPersent = 0;
        concurents = 0;

        float radius = weight;
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, radius, plantsCompetitorBuffer, myMask);

        for (int i = 0; i < count; i++)
        {
            Collider2D collider = plantsCompetitorBuffer[i];

            if (collider == myCollider)
            {
                continue;
            }

            concurents++;

            Behavior concurent = collider.gameObject.GetComponent<PlantBehavior>();
            plantBehaviorEnergyPenaltyPersent += 30 * (concurent.GetWeight() / weight);

            if (plantBehaviorEnergyPenaltyPersent > 100)
            {
                plantBehaviorEnergyPenaltyPersent -= (plantBehaviorEnergyPenaltyPersent - 100) / 2f;
            }
        }

    }

}
