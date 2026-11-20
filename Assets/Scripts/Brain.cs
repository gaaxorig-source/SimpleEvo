using Unity.VisualScripting;
using UnityEngine;

public class Brain : MonoBehaviour
{
    [SerializeField]
    private UnitManager unitManager;

    private Rigidbody2D rb;
    private BoxCollider2D myCollider;
    private GeneralGenetic myGens;

    public Rigidbody2D RigidBody2D => rb;
    public BoxCollider2D BoxCollider2D => myCollider;
    public GeneralGenetic Gens => myGens;

    private Behavior[] allBehaviors;

    void Awake()
    {
        allBehaviors = GetComponents<Behavior>();
        foreach (var behavior in allBehaviors)
        {
            behavior.enabled = false;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        myGens = GetComponent<GeneralGenetic>();

        InitBehavior();
    }

    private void InitBehavior()
    {
        Behavior b = null;
        foreach (var behavior in allBehaviors)
        {
            if (behavior.CanBeActive())
            {
                behavior.enabled = true;
                b = behavior;
                break;
            }
        }

        if (b == null) Destroy(this.gameObject);
        else
        {
            UnitManager.Sub(b);
        }
    }
}
