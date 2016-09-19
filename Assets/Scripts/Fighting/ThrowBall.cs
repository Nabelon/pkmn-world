using UnityEngine;
using System.Collections;
namespace fight
{
    public class ThrowBall : Action {
        private string name;
        private int ballStrength;
        private bag.Item ballItem;
        public ThrowBall(bag.Item item)
        {
            name = item.ToString();
            ballItem = item;
            switch (item)
            {
                case bag.Item.MonsterBall: ballStrength = 1; break;
                case bag.Item.MegaBall: ballStrength = 2; break;
                case bag.Item.BestBall: ballStrength = 3; break;
            }
        }
        public override int getPriority()
        {
            return 6;
        }
        public override void doAction()
        {
            if (FightingManager.isTrainerFight())
            {
                //dont do that
                TextBox.addText("You Thief!");
                return;
            }
            bag.ItemBag.getBag().removeItem(ballItem);
            Monster prey = FightingManager.defender;
            int rarity = MonsterInfo.getMonsterInfo().getRarity(prey.id);
            int maxCatchRate = (3 + 5) * 2 * 2 + 4; //(bestBallStrength + lowestRarity) * statusAligmentMax * fullHp + puffer
            int randInt = Random.Range(0, maxCatchRate);
            int diff = randInt - (int) (ballStrength * rarity * 1.5f * (2.0f - prey.getMCurrHp() / (float) prey.getMMaxHp())); //TODO: status aligment; 
            if (diff < 0)
            {
                //caught
                FightingManager.fightIsOver = true;
                TextBox.addText("Gratulations, you caught " + prey.name);
                if (!MonsterDex.getMonsterDex().wasCaught(prey.id))
                {
                    TextBox.addText("Added " + prey.name + " to the monsterdex!");
                    MonsterDex.getMonsterDex().addToCaugth(prey.id);
                }
                bag.MonsterBag.getBag().addMonster(prey.bagmonster);
            }
            else
            {
                //not caught
                if(diff < 8) {
                    TextBox.addText("Close...");
                }
                else if (diff < 16)
                {
                    TextBox.addText("Could be worse");
                }
                else 
                {
                    TextBox.addText("Did you even try to aim the ball???");
                }
            }
        }
    }
}
