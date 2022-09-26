using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* How mining works:
 * 
 * The amount of Ore mined is 1 KG + random(0.1 kg per level)
 * The amount of time it takes is 10 seconds, -0.1 seconds per level
 * the quality of Ore is random and determined by Vein
 * The chance of durability loss is 50% - 2% per level
 * Higher quality gathering items have more durability
 */

[CreateAssetMenu] 
public class MineOre : PlayerAction, ITakeTime
{
    public float time { get; private set; } = 10f;
    public float interval { get; private set; } = 1.1f;

    protected override bool Can(Player player) => player.currentZone.AvailableItems.Any(item => item.Key is IMineable);

    protected override void Prepare(Player player)
    {
        time = Mathf.Clamp(10f - player.GetLevel(this) * 0.1f, 5f, 10f);
    }

    protected override void Complete(Player player)
    {
        IMiningTool tool = player.Inventory.Where(Item => Item is IMiningTool).First() as IMiningTool;
        int seed = Random.Range(0, player.currentZone.AvailableItems.Where(item => item.Key is IMineable).Sum(item => player.currentZone.AvailableItems[item.Key]));
        float amountMined = 1f + Random.Range(0, 0.1f * player.GetLevel(this));
        float damageChance = Mathf.Clamp(.5f - (player.GetLevel(this) * 0.02f), 0.075f, .5f) - tool.DamageChanceReduction;

        foreach (Ore item in player.currentZone.AvailableItems.Where(item => item.Key is IMineable).Select(item => item.Key))
        {
            seed -= player.currentZone.AvailableItems[item];

            if (seed <= 0)
            {
                Ore awardedItem = Instantiate(item);
                awardedItem.SetWeight(amountMined);

                Debug.Log($"Mining Successful, mined {amountMined.ToString("0.0")}kg of {awardedItem.name}");
                player.GiveItem(awardedItem);
                player.GiveExperience(this, Mathf.RoundToInt(amountMined * 10)); 

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
    float OrePurity { get; }
}