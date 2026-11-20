using UnityEngine;

[RequireComponent(typeof(Brain))]
public class GeneralGenetic : MonoBehaviour
{
    private Brain myBrain;


    [SerializeField]
    private float mutationGen = 0.01f;
    [SerializeField]
    private float energyMinuseGen = 0.0001f;
    [SerializeField]
    private float weightGen = 1f;
    [SerializeField]
    private float speedGen = 5f;
    [SerializeField]
    private float devideImpulsGen = 0f;
    [SerializeField]
    private float maxWeightChangeCountGen = 0;
    [SerializeField]
    private float weightChangeCoeffGen = 0;
    [SerializeField]
    private float weightChangeTriggerInPersentGen = 75;
   [SerializeField]
    private float mutationRate = 0.01f;

    public float EnergyMinuseGen => energyMinuseGen;
    public float WeightGen => weightGen;
    public float SpeedGen => speedGen;
    public float DevideImpulsGen => devideImpulsGen;
    public float MaxWeightChangeCountGen => maxWeightChangeCountGen;
    public float WeightChangeCoeffGen => weightChangeCoeffGen;
    public float WeightChangeTriggerInPersentGen => weightChangeTriggerInPersentGen;
    public float MutationRate => mutationRate;


    private bool isCarnivorous = false;
    private bool isPlant = false;
    private bool isHerbivore = false;

    public bool IsCarnivorous => isCarnivorous;
    public bool IsPlant => isPlant;
    public bool IsHerbivore => isHerbivore;


    private void Awake()
    {
        myBrain = GetComponent<Brain>();
        myBrain.enabled = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color color = sr.color;
        color.r += (mutationRate * Random.Range(-1, 2));
        color.g += (mutationRate * Random.Range(-1, 2));
        color.b += (mutationRate * Random.Range(-1, 2));
        sr.color = color;

        if (color.r > 0.4)
        {
            isCarnivorous = true;
        }
        else
        {
            isCarnivorous = false;
        }
        if (color.g > 0.4)
        {
            isPlant = true;
        }
        else
        {
            isPlant = false;
        }
        if (color.b > 0.4)
        {
            isHerbivore = true;
        }
        else
        {
            isHerbivore = false;
        }

        weightGen = weightGen + (mutationRate * Random.Range(-1, 2));
        if (weightGen < 0.1f) weightGen = 0.1f;

        devideImpulsGen += 0.01f * Random.Range(-1, 2);

        Vector3 newScale = Vector3.one;
        newScale *= weightGen;
        float r = Random.Range(-1, 2);
        if (r == 1 && newScale.y > 0.15)
        {
            newScale.x += 0.1f;
            newScale.y -= 0.1f;
        }
        else if (r == -1 && newScale.x > 0.15)
        {
            newScale.y += 0.1f;
            newScale.x -= 0.1f;
        }
        transform.localScale = newScale;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = weightGen;

        maxWeightChangeCountGen += Random.Range(-1, 2);
        weightChangeCoeffGen += Random.Range(-1, 2) * mutationRate;
        weightChangeTriggerInPersentGen += Random.Range(-1, 2) * mutationRate;

        myBrain.enabled = true;
    }

    public void DevideMoment()
    {
        mutationRate += (Random.Range(-1, 2) * mutationGen);
    }
}
