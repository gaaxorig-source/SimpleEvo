using UnityEngine;

public class SpesParasiticPlantGens
{
    private float parasiticUptakeGen = 0.001f;

    public float ParasiticUptakeGen => parasiticUptakeGen;

    public SpesParasiticPlantGens() { }
    public SpesParasiticPlantGens(float parasiticUptakeGen) {
        this.parasiticUptakeGen = parasiticUptakeGen;
    } 
    public void Mutate(GeneralGenetic gens)
    {
        parasiticUptakeGen += Random.Range(-1, 2) * gens.MutationRate;

        if (parasiticUptakeGen < 0)
        {
            parasiticUptakeGen = 0;
        }
    }
    public SpesParasiticPlantGens Clone()
    {
        return new SpesParasiticPlantGens(parasiticUptakeGen);
    }
}
