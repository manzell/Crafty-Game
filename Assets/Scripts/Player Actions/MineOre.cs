using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/* How mining works:
 * 
 * The amount of Ore mined is 1 KG + random(0.1 kg per level)
 * The amount of time it takes is 10 seconds, -0.1 seconds per level
 * the quality of Ore is random and determined by Vein
 * The chance of durability loss is 50% - 2% per level
 * Higher quality gathering items have more durability
 */

public class MineOre : PlayerAction, ITakeTime
{
    [field: SerializeField] public float time { get; private set; } = 10f;
    [field: SerializeField] public float interval { get; private set; } = 1.1f;

    public override bool Can(Player player, IEnumerable<Item> inputs) => inputs.Any(input => input is IMiningTool) && player.currentZone.availableItems.Any(item => item.Key is IMineable);

    public override void Prepare(Player player, IEnumerable<Item> inputs)
    {
        time = Mathf.Clamp(10f - player.GetLevel(this) * 0.1f, 5f, 10f);
    }

    public override void Complete(Player player, IEnumerable<Item> inputs)
    {
        IMiningTool tool = inputs.First(input => input is IMiningTool) as IMiningTool; 
        int seed = Random.Range(0, player.currentZone.availableItems.Where(item => item.Key is IMineable).Sum(item => player.currentZone.availableItems[item.Key]));
        float amountMined = 1f + Random.Range(0, 0.1f * player.GetLevel(this));
        float damageChance = Mathf.Clamp(.5f - (player.GetLevel(this) * 0.02f), 0.075f, .5f) - tool.DamageChanceReduction;

        foreach (OreData oreData in player.currentZone.availableItems.Where(item => item.Key is IMineable).Select(item => item.Key))
        {
            seed -= player.currentZone.availableItems[oreData];

            if (seed <= 0)
            {
                GameObject g = new GameObject(oreData.name);
                g.transform.parent = player.transform;

                Ore ore = g.AddComponent<Ore>();
                ore.Setup(oreData);
                ore.SetWeight(amountMined);

                player.GiveItem(ore);
                player.GiveExperience(this, Mathf.RoundToInt(amountMined * 10));

                Debug.Log($"Mining Successful, mined {amountMined.ToString("0.0")}kg of {ore.name}");

                if (tool is IDurableItem durableTool && Random.value < damageChance)
                    durableTool.AdjustDurability(-0.1f);

                break; 
            }
        }
    }
}

public interface IMiningTool
{
    float DamageChanceReduction { get; }
}

public interface IMineable
{
}