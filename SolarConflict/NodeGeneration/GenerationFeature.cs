using Microsoft.Xna.Framework;
using SolarConflict.GameWorld;
using SolarConflict.NodeGeneration.Features;
using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.World.Generation
{    
    public class GenerationFeature //TODO: add rotation
    {
        protected int? localLevel;
        public int Level
        {
            get
            {
                if (localLevel.HasValue)
                    return localLevel.Value;
                return Parent.Level;
            }            
        }

        public void SetLevel(int? level)
        {
            this.localLevel = level;
        }


        protected FactionType? localFaction;
        public FactionType Faction
        {
            get
            {
                if (localFaction.HasValue)
                    return localFaction.Value;
                return Parent.Faction;
            }
        }

        public void SetFaction(FactionType? faction)
        {
            localFaction = faction;
        }

        public float Size;
        public GenerationFeature Parent;
        public Vector2 LocalPosition;
        public float LocalRotation;
        /// <summary>Affects the order in which features are generated (lower is earlier)</summary>
        /// <remarks>Note that features are generated depth-first</remarks>
        public virtual float Priority => 100f;
        public Random _rand;
        public Random Rand { get { return _rand ?? Parent?.Rand; } set { _rand = value; } }

        protected List<GenerationFeature> children;

        public float Rotation
        {
            get
            {
                if (Parent == null)
                    return LocalRotation;
                else
                    return LocalRotation + Parent.Rotation;
            }
            set
            {
                if (Parent == null)
                    LocalRotation = value;
                else
                    LocalRotation = value - Parent.Rotation;
            }
        }
        public Vector2 Position
        {
            get
            {
                if (Parent == null)
                    return LocalPosition;
                else
                    return LocalPosition + Parent.Position;
            }
            set
            {
                if (Parent == null)
                    LocalPosition = value;
                else
                    LocalPosition = value - Parent.Position;
            }
        }



        public GenerationFeature(Random rand = null)
        {
            _rand = rand;
            children = new List<GenerationFeature>();
        }


        public virtual void AddChild(GenerationFeature child)
        {
            child.Parent = this;
            children.Add(child);           
        }

        public virtual void RemoveChild(GenerationFeature child)
        {
            child.Parent = child;
            children.Remove(child);
        }

        public virtual GameObject Generation(Scene scene, SceneGenerator generator)
        {
            var go = GenerationLogic(scene, generator);            
            foreach (var child in children.OrderBy(c => c.Priority))
            {
                child.Generation(scene, generator);
            }
            return go;
        }

        public virtual GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            return null;
        }

        public virtual void SetParentObject(GameObject gameobject)
        {

        }


        public static implicit operator GenerationFeature(string emitterID)
        {
            return new EmitterFeature(ContentBank.Inst.GetEmitter(emitterID));
        }      
    }
}
