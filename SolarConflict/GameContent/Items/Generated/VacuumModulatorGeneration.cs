using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Generated
{
    class VacuumModulatorGeneration
    {
        public static IEmitter Make()
        {
            int maxLevel = 5;
            int levelSkip = 1;
            for (int i = 1; i <= maxLevel; i+=levelSkip)
                ContentBank.Inst.AddContent(MakeVacuumModulator(i,maxLevel));
            return null;
        }

        public static Item MakeVacuumModulator(int level, int maxLevel)
        {
            ItemData data = new ItemData(ScalingUtils.NameFromLevel("Vacuum Modulator", level));
            data.IconID = "VacuumModulator1b";
            data.BreaksClocking = false;                            
            data.SlotType = SlotType.Utility;            
            data.Category = ItemCategory.Utility | ItemCategory.AsteroidMiningGear;
            data.Level = level;
            data.SecounderyIconID = $"lvl{data.Level}";
            data.BuyPrice = (int)(ScalingUtils.ScaleCost(level) * 0.5f);
            int range = Math.Min( 800 + level / 2 * 200, 2500);
            float force = level * 0.3f + 0.4f;
            bool needsFreeSpace = false;// level == 1;
            //Description
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Palette.ItemDescription.ToTag("Pulls collectibles to your ship"));
            if (level == maxLevel)
            {
                sb.AppendLine(Palette.ItemDescription.ToTag("State of the art in vacuum technology"));
                data.IsWorkingFromInventory = true;
            }
            if (needsFreeSpace)
                sb.AppendLine(Palette.ItemNegetiveStat.ToTag("Needs free inventory slot to pull"));
            sb.AppendLine("Range: " + Palette.Highlight.ToTag(range.ToString()));
            sb.AppendLine("Force: " + Palette.Highlight.ToTag(force.ToString()));

            Item item = ItemQuickStart.Make(data);
            item.ID = $"VacuumModulator{level}";
            item.Profile.IsShownOnHUD = false;
           

            EmitterCallerSystem system = new EmitterCallerSystem();
            if (needsFreeSpace)
                system.ActivationCheck = new ActivationCheck(ControlSignals.OnInventoryHasRoom);
            else
                system.ActivationCheck = null;
            system.Emitter = MakeItemPullingAOE(range, force);
            system.MaxLifetime = 30;
            system.CooldownTime = 30;
            system.EmitterSpeed = 0;
            //system.
            item.System = system;
            item.Profile.IsActivatable = false;
            item.Profile.DescriptionText = sb.ToString();
            item.Profile.BreaksCloaking = false;
            item.Profile.Category |= ItemCategory.NonAI;
            return item;
        }

        public static ProjectileProfile MakeItemPullingAOE(int range, float force)
        {
            ProjectileProfile profile = new ProjectileProfile(); //Graphics: add background warp shader as effect
            profile.CollisionType = CollisionType.CollideAll;
            profile.DrawType = DrawType.Alpha;
            profile.TextureID = "shockwave2";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSize = new InitFloatConst(range);
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            profile.InitMaxLifetime = new InitFloatConst(30);
            profile.Mass = 0.1f;
            profile.CollisionSpec = new CollisionSpec(0, -force);
            profile.CollisionSpec.IsDamaging = false;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.CollideWithMask = GameObjectType.Item | GameObjectType.Collectible;
            profile.DrawType = DrawType.None;
            
         //   profile.IsPotentialTarget = false;
            
            return profile;
        }

    }
}
