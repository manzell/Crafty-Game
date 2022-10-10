using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TreeEditor;
using UnityEngine;
using UnityEngine.Events;

/* How mining works:
 * 
 * The amount of Ore mined is 1 KG + random(0.1 kg per level)
 * The amount of time it takes is 10 seconds, -0.1 seconds per level
 * the quality of Ore is random and determined by Vein
 * The chance of durability loss is 50% - 2% per level
 * Higher quality gathering items have more durability
 */

[CreateAssetMenu]
public class Mining : OngoingAction
{
    [SerializeField] float miningRate = 10f;
    [SerializeField] List<(ItemData item, float gatherRate)> gatherables;

    IMiningTool tool;

    public override bool Can(Player player)
    {
        bool playerIdle = player.currentAction == null;

        Debug.Log(Inputs);

        bool toolAvailable = Inputs.Any(input => input is IMiningTool && (input as IMiningTool).Durability > 0);

        return playerIdle && toolAvailable;
    }

    public override void Prepare(Player player)
    {
        interval = Mathf.Max(interval - player.GetLevel(this) * 0.1f, 5f);
        tool = Inputs.First(input => input is IMiningTool) as IMiningTool;
        prepareEvent.Invoke(player); 
    }

    public override IEnumerator Progress(Player player)
    {
        while(player.currentAction == this)
        {
            yield return new WaitForSeconds(interval);

            float seed = Random.Range(0, gatherables.Sum(gatherable => gatherable.gatherRate));
            float damageChance = Mathf.Clamp(.5f - (player.GetLevel(this) * 0.02f), 0.075f, .5f) - tool.DamageChanceReduction;
            float amountMined = miningRate * tool.MiningEfficiency;
            ItemData minedItem;

            foreach ((ItemData item, float gatherRate) gatherable in gatherables)
            {
                seed -= gatherable.gatherRate;

                if (seed <= 0)
                {
                    minedItem = gatherable.item;
                    Ore ore = new Ore(minedItem);

                    ore.SetWeight(amountMined);
                    player.GiveItem(ore);
                    player.GiveExperience(this, Mathf.RoundToInt(10 * amountMined / interval));

                    Debug.Log($"Mining Successful, mined 10kg of {ore.name}");

                    break;
                }
            }

            if (Random.value < damageChance)
                tool.AdjustDurability(-1f);

            progressEvent.Invoke(player);
        }  
    }

    public override void Complete(Player player)
    {
        completeEvent.Invoke(player); 
        player.SetCurrentAction(null); 
    }
}