using Unity.VisualScripting;
using UnityEngine;

public abstract class Behavior : MonoBehaviour
{
    public int managerIndex;
    protected GeneralGenetic myGens;
    protected Collider2D myCollider;

    public bool isDead = false;
    private bool isUnsubbed = false;
    private static int cloneCount = 0;

    [SerializeField]
    protected float energy;
    protected float energyMinuse = 0.0001f;
    [SerializeField]
    protected float energyPenaltyPersent = 0;
    [SerializeField]
    protected float allEnergyPenaltyPersent = 0; 
    protected float speed;
    [SerializeField]
    protected float weight;

    protected float devideImpuls;

    private float maxWeightChangeCount = 0;
    private float weightChangeCoeff = 0;
    float adjustAccum = 0f;
    float adjustBatery = 0;

    private void Awake()
    {
        myGens = GetComponent<GeneralGenetic>();
        myCollider = GetComponent<Collider2D>();

        OnAwake();
    }

    protected abstract void OnAwake(); 
    private void Start()
    {
      
        energyPenaltyPersent = 0;

        speed = myGens.SpeedGen;
        weight = myGens.WeightGen;
        speed = speed / weight;

        devideImpuls = myGens.DevideImpulsGen;

        maxWeightChangeCount = myGens.MaxWeightChangeCountGen;
        weightChangeCoeff = myGens.WeightChangeCoeffGen;

        adjustBatery = Mathf.Abs(weightChangeCoeff);

        OnStart();
    }

    protected abstract void OnStart();

    public void MyUpdate(float deltaTime)
    {
        AdjustWeight();

        OnUpdate(deltaTime);
    }

    protected abstract void OnUpdate(float deltaTime);

    public void MyFixedUpdate(float fixedDeltaTime)
    {
        if (energy <= 0 || isDead)
        {
            isDead = true;
            if (!isUnsubbed)
            {
                isUnsubbed = true;
                UnitManager.UnSub(this);
            }

            return;
        }

        AbstractcDivide();

        energy -= energyMinuse;

        OnFixedUpdate(fixedDeltaTime);
    }
    protected abstract void OnFixedUpdate(float fixedDeltaTime);
    private void AbstractcDivide() {
        GameObject clone = Divide();
        if (clone != null)
        {
            myGens.DevideMoment();
            cloneCount++;
            clone.name = cloneCount + "";
        }
    }
    protected abstract GameObject Divide();

    private void AdjustWeight()
    {
        if (adjustAccum < adjustBatery)
        {
            adjustAccum += Time.deltaTime;
            return;
        }
        adjustAccum = 0;

        if (allEnergyPenaltyPersent > myGens.WeightChangeTriggerInPersentGen && maxWeightChangeCount > 0)
        {
            float weightValue = weight + weightChangeCoeff;
            if (!(weightValue < 0.1f))
            {
                if (weightChangeCoeff > 0)
                {
                    if (energy < 0.5) return;
                    energy -= weightChangeCoeff / 2;
                }
                weight = weightValue;
                Vector3 changedScale = transform.localScale;
                changedScale.x += weightChangeCoeff;
                changedScale.y += weightChangeCoeff;
                transform.localScale = changedScale;

                energyMinuse = myGens.EnergyMinuseGen * weight;

                maxWeightChangeCount--;
            }
        }
        else if (allEnergyPenaltyPersent < myGens.WeightChangeTriggerInPersentGen && maxWeightChangeCount < myGens.MaxWeightChangeCountGen)
        {
            float weightValue = weight - weightChangeCoeff;
            if (weightChangeCoeff < 0)
            {
                if (energy < 0.5) return;
                energy -= Mathf.Abs(weightChangeCoeff) / 2;
            }
            weight = weightValue;
            Vector3 changedScale = transform.localScale;
            changedScale.x -= weightChangeCoeff;
            changedScale.y -= weightChangeCoeff;
            transform.localScale = changedScale;

            energyMinuse = myGens.EnergyMinuseGen * weight;

            maxWeightChangeCount++;
        }
    }

    public float GetWeight()
    {
        return weight;
    }
    public abstract bool CanBeActive();

}
