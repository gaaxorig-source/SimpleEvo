using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private SpriteRenderer border;

    private float minX = 0f;
    private float maxX = 10f;
    private float minY = 0f;
    private float maxY = 10f;

    public float spawnCooldown = 5f;
    private float batery = 0f;
    public int maxCount = 10;
    private int count = 0;

    public bool isInfinityLife = false;
   
    public int lifeTimeInSeconds = 10;
    public bool isRandomLifeTime = true;


    [SerializeField]
    private GameObject prephab;

    private List<EnergyBehavior> energies = new List<EnergyBehavior>();

    void Start()
    {
        Bounds bounds = border.bounds;
        minX = bounds.min.x + 1f;
        maxX = bounds.max.x - 1f;
        minY = bounds.min.y + 1f;
        maxY = bounds.max.y - 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (batery < spawnCooldown)
        {
            batery += Time.deltaTime;
            return;
        }

        if (count < maxCount)
        {
            batery = 0;


            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);

            GameObject clone = Instantiate(prephab) as GameObject;

            EnergyBehavior energyBehavior = clone.GetComponent<EnergyBehavior>();
            energyBehavior.SetSpawner(this);
            energies.Add(energyBehavior);
            clone.transform.position = new Vector3(x, y, 1);

            count++;

            if (!isInfinityLife)
            {
                int lifeTime = lifeTimeInSeconds;
                if (isRandomLifeTime)
                {
                    lifeTime = Random.Range(0, lifeTimeInSeconds);
                }

                StartCoroutine(destroyObjectAfter(clone, lifeTime));
            }
        }   
    }

    private IEnumerator destroyObjectAfter(GameObject obj, int lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        count--;
        Destroy(obj);
    }

    public void MinuseOne()
    {
        count--;
    }

    public List<EnergyBehavior> GetEnergies()
    {
        return energies;
    }
}
