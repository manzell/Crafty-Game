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

public class Mining : PlayerAction
{
    [SerializeField] float miningRate = 10f;
    [SerializeField] List<miningOreTypeRate> mineables;

    IMiningTool tool;

    struct miningOreTypeRate
    {
        public ItemData item;
        public float gatherRate; 
    }

    public override bool Can(Player player, Zone zone)
    {
        if (!zone.DropTable.Table.ContainsKey(this))
            return false;

        List<string> failMessages = new();

        if (zone.Inputs.FirstOrDefault(input => input is IMiningTool) == null)
            failMessages.Add("No Mining Tool!");
        if (zone.DropTable.Table[this].Count(x => x.data.Clone() is IMineable) == 0)
            failMessages.Add("No Mineables");

        // Debug.Log($"Mining {failMessages.Count} - {failMessages.Count == 0}"); 

        return failMessages.Count == 0; 
    }

    public override void Prepare(Player player, Zone zone)
    {
        Interval = Mathf.Max(Interval - player.GetLevel(this) * 0.1f, 5f);
        tool = zone.Inputs.First(input => input is IMiningTool && (input as IMiningTool).Durability > 0) as IMiningTool;
        player.SetCurrentAction(this);
        prepareEvent.Invoke(player, zone); 
    }

    public override IEnumerator Progress(Player player, Zone zone)
    {
        yield return new WaitForSeconds(Interval);

        while (player.currentAction == this && Can(player, zone))
        {
            float seed = Random.Range(0, mineables.Sum(gatherable => gatherable.gatherRate));
            float damageChance = Mathf.Clamp(.5f - (player.GetLevel(this) * 0.02f), 0.075f, .5f) - tool.DamageChanceReduction;
            float amountMined = miningRate * tool.MiningEfficiency;

            foreach (miningOreTypeRate mineable in mineables)
            {
                seed -= mineable.gatherRate;

                if (seed <= 0)
                {
                    Ore ore = new Ore(mineable.item);
                    ore.SetWeight(amountMined);

                    Debug.Log($"Mining Successful, mined {ore.Weight.ToString("0.0")}kg of {ore.name}");

                    player.GiveItem(ore);
                    player.GiveExperience(this, Mathf.RoundToInt(10 * amountMined / Interval));

                    break;
                }
            }

            if (Random.value < damageChance)
                tool.AdjustDurability(-1f);

            progressEvent.Invoke(player, zone);

            yield return new WaitForSeconds(Interval);
        }

        Complete(player, zone); 
    }

    public override void Complete(Player player, Zone zone)
    {
        Debug.Log($"{name} Complete(player)");
        completeEvent.Invoke(player, zone); 
        player.SetCurrentAction(null); 
    }
}