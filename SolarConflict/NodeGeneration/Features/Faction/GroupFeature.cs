using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using XnaUtils;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Session.World.Generation.Features;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Utils;
using System.Diagnostics;

namespace SolarConflict.Session.World.Generation.Profiles {
    
    /// <summary>Chooses subfeatures, places them in a cluster</summary>
    /// <warning>KLUDGE: _should_ work by making a bunch of GenerationFeature clones, adopting them, and letting the base GenerationFeature.Generate() method process them. GenerationFeature isn't
    /// cloneable, though, so instead it _semi-adopts_ a single feature (meaning it sets itself as its parent, but doesn't add it as its child), moves it around a bunch, and calls it repeatedly.
    /// This is incredibly ugly.</warning>
    [Serializable]
    class GroupFeature : GenerationFeature {

        public int NumFeatures;

        public Cluster Cluster;

        public GenerationFeature[] PossibleFeatures;

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator) {
            Refresh();

            Cluster.Transforms(NumFeatures).Do(t => {
                // Choose, adopt, position, and execute feature
                var feature = PossibleFeatures.Choice(Rand);

                feature.LocalPosition = t.Position;
                feature.LocalRotation = t.Rotation;

                feature.Parent = this;

                feature.GenerationLogic(scene, generator);
            });

            return null;
        }

        /// <summary>Updates cluster based on parent, and size and other stats based on pattern</summary>
        void Refresh() {
            var ring = Cluster as Ring;
            if (ring != null) {
                if (ring?.Radius == -1) {
                    // Ring radius defaults to just big enough to keep child features from overlapping our parent (but not necessarily each other)                
                    if (Parent != null)
                        ring.Radius = Parent.Size + PossibleFeatures.Max(f => f.Size);
                }
                Size = ring.Radius + PossibleFeatures.Max(f => f.Size);
            }
        }        
    }
}
