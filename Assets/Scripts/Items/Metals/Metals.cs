using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Metals : MonoBehaviour
{
    [field:SerializeField] public List<MetalData> metals {  get; private set; }

    public MetalData GetAlloy(MetalMixture moltenMetal) => metals.OrderByDescending(metal => AlloySimilarity(metal, moltenMetal)).First();

    public virtual float AlloySimilarity(MetalData metal, MetalMixture moltenMetal)
    {
        float disimilarity = 1;

        MetalMixture normalizedMix = new(); 
        foreach(MetalData meltedMetal in moltenMetal.Keys)
            normalizedMix.Add(meltedMetal, moltenMetal[meltedMetal] / moltenMetal.Values.Sum()); 

        foreach(MetalData m in metal.mixture.Keys)
        {
            if (normalizedMix.ContainsKey(metal) && metal.mixture.ContainsKey(metal))
                disimilarity += Mathf.Abs(normalizedMix[metal] - metal.mixture[metal]);
            else if (normalizedMix.ContainsKey(metal))
                disimilarity += normalizedMix[metal];
            else if (metal.mixture.ContainsKey(metal))
                disimilarity += metal.mixture[metal];
        }

        return 1 - disimilarity;
    }
}
