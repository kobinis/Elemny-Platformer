using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Utils.QuickStart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class AoeRepairKit1
    {
        static float lifetime = 6f;
        static float totalRegen = 480f;

        public static Item Make()
        {
            MakeAura();

            var data = new KitData("Regeneration Field Kit", "AoeRepairKit", MeterType.None, 0f, Utility.Frames(8f), ControlSignals.None, isGlobalCooldown: true);
            data.ActivationEmitterID = "AoeRepairKit_Aura";
            data.ItemData.BuyPrice = 600;
            var result = KitQuickStart.Make(data);
            result.Profile.DescriptionText = $"Generates an aura that regenerates {totalRegen} shields and hitpoints over {lifetime} seconds to all ships in range";

            return result;
        }

        static void MakeAura() {
            var outerEmitter = new GroupEmitter();
            outerEmitter.EmitType = GroupEmitter.EmitterType.All;            

            var auraEmitter = new ProjectileProfile();
            auraEmitter.TextureID = "add14";
            auraEmitter.DrawType = DrawType.Additive;         

            auraEmitter.VelocityInertia = 0f;                        

            auraEmitter.InitSize = new InitFloatConst(20f);
            auraEmitter.UpdateSize = new UpdateSizeGrow(0f, 1.05f, 600f);

            auraEmitter.ColorLogic = ColorUpdater.FadeOutSlow;

            auraEmitter.InitMaxLifetime = new InitFloatConst(Utility.Frames(lifetime));

            var innerEmitter = new ProjectileProfile(); // kludge to allow aura to collide with parent
            innerEmitter.InitMaxLifetimeID = "1";
            innerEmitter.TimeOutEmitter = auraEmitter;

            var regenPerFrame = totalRegen / Utility.Frames(lifetime);

            auraEmitter.IsDestroyedOnCollision = false;

            auraEmitter.CollisionSpec = new CollisionSpec();
            auraEmitter.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies;

            auraEmitter.CollisionSpec.ImpactList.Add(new MeterCollisionSpec(MeterType.Hitpoints, regenPerFrame));
            auraEmitter.CollisionSpec.ImpactList.Add(new MeterCollisionSpec(MeterType.Shield, regenPerFrame));            

            outerEmitter.AddEmitter(innerEmitter);
            outerEmitter.AddEmitter("sound_tensor_charge");

            outerEmitter.ID = "AoeRepairKit_Aura";

            ContentBank.Inst.AddContent(outerEmitter);
        }

        
    }
}
