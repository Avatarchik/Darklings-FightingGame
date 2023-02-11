using UnityEngine;

public class TauntState : State
{
    public override void UpdateLogic(PlayerNetwork player)
    {
        if (!player.enter)
        {
            if (player.otherPlayer.health <= 0)
            {
                player.comboLocked = true;
            }
            else
            {
                player.comboLocked = false;
            }
            player.enter = true;
            player.animationFrames = 0;
            player.animation = "Taunt";
        }
        player.velocity = DemonicsVector2.Zero;
        player.animationFrames++;
        ToIdleState(player);
    }
    private void ToIdleState(PlayerNetwork player)
    {
        if (player.animationFrames >= 160 && !player.comboLocked)
        {
            GameSimulation.Run = true;
            EnterState(player, "Idle");
        }
    }
}