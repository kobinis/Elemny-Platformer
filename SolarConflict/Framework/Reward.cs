using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework
{
    [Serializable]
    public class Reward : IEmitter
    {
        public string ID { get; set; }
        public int Money;
        public List<Tuple<string,int>> Items { get; private set; }
        public FactionType Faction;
        public float ReputationDelta;
        public string EmitterID { get; set; }
        public string Data;

        public Reward()
        {
            Items = new List<Tuple<string, int>>();
        }

        public Reward(int money):this()
        {
            Money = money;
        }

        public string GetTag()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Reward:");
            if(Money != 0)
                sb.Append("\nCredits: " + Color.Gold.ToTag(Money.ToString()) + "#image{coin}");
            if (ReputationDelta != 0)
            {
                Color color = ReputationDelta < 0 ? Color.Red : Color.Green;
                sb.Append("\nReputation: " + color.ToTag(((int)(ReputationDelta * 1000)).ToString()));
            }
            foreach (var tuple in Items)
            {
                var item = ContentBank.Inst.GetItem(tuple.Item1, false);
                int num = tuple.Item2;
                sb.Append("\n"+ num.ToString() + " x " + item.IconTag + item.Tag);
            }
          
            return sb.ToString();
        }

        public bool AddReward(GameEngine gameEngine, FactionType giverFaction = FactionType.None)
        {
            GameObject player = gameEngine.PlayerAgent;
            return AddRewardToShip(gameEngine, giverFaction, player);
        }

        public bool AddRewardToShip(GameEngine gameEngine, FactionType giverFaction, GameObject recivingShip)
        {
            //Items:            
            if (recivingShip == null)
                recivingShip = gameEngine.GetFaction(FactionType.Player).Mothership;
            if (recivingShip != null)
            {
                foreach (var itemPair in Items)
                {
                    Item item = ContentBank.Inst.GetItem(itemPair.Item1, true);
                    if (itemPair.Item2 > 0)
                        item.Stack = itemPair.Item2;
                    if (!recivingShip.AddItemToInventory(item))
                    {
                        item.Position = recivingShip.Position;
                        gameEngine.AddList.Add(item);
                    }
                }
            }
            else
            {
                return false;
            }

            //Money
            gameEngine.GetFaction(FactionType.Player).AddValueToMeter(MeterType.Money, Money);

            //Reputation
            if (Faction == FactionType.None)
            {
                giverFaction = Faction;
            }

            float relations = gameEngine.GetFaction(giverFaction).GetRelationToFaction(FactionType.Player);
            gameEngine.GetFaction(giverFaction).SetRelationToFaction(gameEngine, FactionType.Player, relations + ReputationDelta);

            IEmitter emitter = null;
            ContentBank.Inst.TryGetEmitter(EmitterID);
            emitter?.Emit(gameEngine, recivingShip, recivingShip.GetFactionType(), recivingShip.Position, Vector2.Zero, recivingShip.Rotation, param:gameEngine.Level);
            
            return true;
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            throw new NotImplementedException();
        }
    }
}
