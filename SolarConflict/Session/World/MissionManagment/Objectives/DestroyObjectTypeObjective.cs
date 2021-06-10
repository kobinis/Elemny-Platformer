//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils.Graphics;

//namespace SolarConflict.Session.World.MissionManagment.Objectives
//{
//    [Serializable]
//    class DestroyObjectTypeObjective : MissionObjective
//    {
//        public float RadiusOffset;
//        public float RadiusMult;
//        GameObjectType ObjectType;
//        FactionType? FromFaction;
//        public int UpdateCooldownTime;
//        //public CollisionType ListType = CollisionType.CollideAll;

//        private GameObject target; //TODO: add minDis// the first
//        private int time;

//        public DestroyObjectTypeObjective(GameObjectType objectType, FactionType? fromFaction, float radiusOffset = 200, float radiusMult = 1)
//        {
//            ObjectType = objectType;
//            FromFaction = fromFaction;
//            RadiusOffset = radiusOffset;
//            RadiusMult = radiusMult;
//            UpdateCooldownTime = 60;
//        }

//        public override string GetObjectiveText()
//        {
//            if (target != null && target.IsActive)
//                return " " + Sprite.Get(Status.ToString()).ToTag() + "Go to " + target.Name + target.GetSprite().ToTag();
//            else
//                return " " + Sprite.Get(Status.ToString()).ToTag();
//        }

//        public override Vector2? GetPosition()
//        {
//            return target?.Position;
//        }

//        public override float GetRadius()
//        {
//            float size = 0;
//            if (target != null)
//                size = target.Size * RadiusMult;
//            return size + RadiusOffset;
//        }

//        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
//        {
//            var player = scene.PlayerAgent;
//            if (player != null && player.IsActive)
//            {
//                if (target == null || target.IsNotActive && time % UpdateCooldownTime == 0)
//                {

//                    List<GameObject> searchObject = scene.GameEngine._collideAllParticles; //NOW: select from a list
//                    float minDis = float.MaxValue;

//                    foreach (var gameObject in searchObject)
//                    {
//                        if ((gameObject.GetObjectType() & ObjectType) > 0 && (FromFaction == null || gameObject.GetFactionType() == FromFaction.Value))
//                        {
//                            float dis = (gameObject.Position - player.Position).LengthSquared();
//                            if (dis < minDis)
//                            {
//                                minDis = dis;
//                                target = gameObject;
//                            }
//                        }
//                    }
//                }


//                if (target != null && target.IsActive && GameObject.DistanceFromEdge(player.Position, target.Position, player.Size, GetRadius()) <= 0)
//                {
//                    return ObjectiveStatus.Completed;
//                }
//            }
//            time++;
//            return ObjectiveStatus.Ongoing;
//        }
//    }
//}


