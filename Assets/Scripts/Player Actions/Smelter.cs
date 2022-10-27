using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

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
public class Smelter : PlayerAction
{
    [SerializeField] IngotData defaultIngot;
    MetalMixture moltenPool;

    public int maxTemperature;
    public float forgeLevel = 10f; 
    public float volume;

    public override bool Can(Player player, Zone zone) => zone.Inputs.Any(input => input is ISmeltable) && zone.Actions.Contains(this); 

    public override void Prepare(Player player, Zone zone)
    {
        moltenPool = new();

        player.SetCurrentAction(this);
        prepareEvent.Invoke(player, zone);
    }

    public override IEnumerator Progress(Player player, Zone zone)
    {
        while (zone.Inputs.Count(item => item.Weight > 0f && item is ISmeltable) > 0)
        {
            Debug.Log("Smelting Progress");
            MeltOre(zone);
            yield return new WaitForSeconds(1f);
        }
    }

    public override void Complete(Player player, Zone zone)
    {
        float moltenPoolVolume = moltenPool.Values.Sum();

        Debug.Log($"Smelting Complete {moltenPoolVolume.ToString("0.0")}kg");

        while (moltenPool.Values.Sum() > defaultIngot.Weight)
        {
            Ingot ingot = (Ingot)defaultIngot.Clone();
            List<MetalData> metals = moltenPool.Where(kvp => kvp.Value > 0).Select(k => k.Key).ToList();
            MetalMixture ingotMix = new();

            foreach (MetalData metal in metals)
            {
                ingotMix.Add(metal, moltenPool[metal] / moltenPoolVolume);
                moltenPool[metal] -= ingotMix[metal];
            }

            ingot.Setup(moltenPool);

            Debug.Log($"Created new {ingot.metalType} ({ingot.Weight.ToString("0.0")}kg) Ingot!");
            Debug.Log($"Molten Pool Volume: {moltenPool.Values.Sum().ToString("0.0")}kg");

            player.GiveItem(ingot);
        }
    }

    void MeltOre(Zone zone)
    {
        List<Item> inputs = zone.Inputs.Where(item => item.Weight > 0f && item is ISmeltable).ToList();

        foreach (ISmeltable smeltableItem in inputs)
        {
            foreach (MetalData metal in smeltableItem.MetalConcentrations.Where(kvp => kvp.Value > 0).Select(kvp => kvp.Key))
            {
                float smeltedAmount = smeltableItem.Weight * smeltableItem.MetalConcentrations[metal];

                Debug.Log($"Smelting {smeltedAmount.ToString("0.0")}kg {metal} from {smeltableItem} ({smeltableItem.Weight.ToString("0.0")}kg)");

                if (moltenPool.ContainsKey(metal))
                    moltenPool[metal] += smeltedAmount;
                else
                    moltenPool.Add(metal, smeltedAmount);
            }

            zone.Inputs.Remove(smeltableItem as Item);
        }
    }
}
