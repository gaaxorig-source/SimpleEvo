using UnityEngine;
using UnityEngine.UI;

public class ParasiticPlantBehavior : Behavior
{
    private SpesParasiticPlantGens mySpesGen;

    [SerializeField]
    private float plantMatter = 0;

    private float scanVictimsBatery = 0;
    private Collider2D[] scanVictimsBuffer;
    [SerializeField]
    private LayerMask victimMask;

    private PlantBehavior currentVictim;
    [SerializeField]
    private float parasiticUptake = 0;

    public override bool CanBeActive()
    {
        return myGens.IsPlant && (!myGens.IsCarnivorous) && myGens.IsHerbivore;
    }

    protected override void OnAwake()
    {
        scanVictimsBuffer = new Collider2D[2];
    }

    protected override void OnStart()
    {
        if (mySpesGen == null)
        {
            mySpesGen = new SpesParasiticPlantGens();
            Debug.Log("1");
        }
        else
        {
            mySpesGen = mySpesGen.Clone();
            mySpesGen.Mutate(myGens);
            Debug.Log("2");
        }
        parasiticUptake = mySpesGen.ParasiticUptakeGen / weight;

        int layer = LayerMask.NameToLayer("PlantParasit");
        gameObject.layer = layer;
        
        energyMinuse = myGens.EnergyMinuseGen * weight;
    }

    protected override void OnFixedUpdate(float fixedDeltaTime)
    {
        if (energy > 1)
        {
            energy = 1f;
        }

        if (plantMatter > 1)
        {
            plantMatter = 1f;
        }

        if (currentVictim != null)
        {
            currentVictim.ParasitFactor(weight, parasiticUptake);
            energy += parasiticUptake - (parasiticUptake/100 * energyPenaltyPersent);
            plantMatter += parasiticUptake;
        } else
        {
            energy -= parasiticUptake;
        }
    }
    
    protected override void OnUpdate(float deltaTime)
    {
        scanVictimsBatery += deltaTime;

        if (scanVictimsBatery > 1)
        {
            scanVictimsBatery = 0;
            ScanVictims();
        }
    }

    protected override GameObject Divide()
    {
        if (energy < 1 || plantMatter < 1)
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
        plantMatter = 0f;
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
        ParasiticPlantBehavior cloneBehavior = clone.GetComponent<ParasiticPlantBehavior>();
        cloneBehavior.SetSpesGen(mySpesGen);

        energyPenaltyPersent += 10;

        return clone;
    }

    private void ScanVictims()
    {
        float radius = weight;
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, radius, scanVictimsBuffer, victimMask);

        if (count == 0) return;

        currentVictim = scanVictimsBuffer[0].gameObject.GetComponent<PlantBehavior>();
    }

    public void SetSpesGen(SpesParasiticPlantGens spesGen)
    {
        if (mySpesGen == null)
        {
            mySpesGen = spesGen;
        }
    }
}
