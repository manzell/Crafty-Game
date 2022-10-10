using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

/* How Smelting Works
 * 
 * A player provides both Ore (ISmeltable). 
 * The Smelter itself determines the maximum temperature of the smelt. 
 * Each tick, we melt an amount of each metal whose melting point we are above. The amount is a function of furnace volume
 * This Creates both Molten Ore and Slag, based on the metal purity of the ore that was actually melted. 
 * The purity of the molten ore is determined by the purity of the input ore and how much higher the furnace temp is than the melting point of the metal
 * The tick time is fixed, but decreases by 1% for the player level in smelting squared (max 10%). This sets the functional upper limit of level 100 in smelting. 
 * Before we create new molten ore, we cool any existing molten ore into Ingots of 
 * 
 */
[CreateAssetMenu]
public class Smelt : OngoingAction
{
    public int temperature;
    public float volume; 

    public override bool Can(Player player) => Inputs.Any(input => input is ISmeltable);

    public override void Complete(Player player)
    {
        Dictionary<Metal, float> metalWeight = new(); 

        foreach(Item item in Inputs.Where(_item => _item is ISmeltable))
        {
            foreach(KeyValuePair<Metal, float> metals in (item as ISmeltable).MetalConcentrations)
            {                
                if (metalWeight.ContainsKey(metals.Key))
                    metalWeight[metals.Key] += item.Weight * metals.Value;
                else
                    metalWeight.Add(metals.Key, item.Weight * metals.Value);
            }
        }

        foreach (Metal metal in metalWeight.Keys)
        {
            int ingots = Mathf.FloorToInt(Mathf.Max(metalWeight[metal] / 10));
            float ingotWeight = metalWeight[metal] / ingots; 

            while(ingots > 0)
            {
                Ingot ingot = new Ingot(); 

                ingot.SetWeight(ingotWeight);
                ingot.MetalConcentrations.Add(metal, Mathf.Pow(metalWeight[metal] / Inputs.Sum(input => input.Weight), player.GetLevel(this) + 1)); 
            }
        }
    }

    public override void Prepare(Player player)
    {
    }

    public override IEnumerator Progress(Player player)
    {
        yield return null; 
    }
}
