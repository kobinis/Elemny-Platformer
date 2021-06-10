using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.MetaGame.World;
using SolarConflict.GameContent;
using XnaUtils;

namespace SolarConflict.GameContent.Activities
{
    /// <summary>
    /// Hold game session with current scene, metadata, and galaxy data
    /// </summary>
    [Serializable]
    class GameActivity : Activity
    {
        private Scene _scene;
        private GalaxyMap _galaxyMap;
        private MetaWorld _metaWorld;
       

        public GameActivity(MetaWorld metaworld, GalaxyMap galaxyMap)
        {
            _metaWorld = metaworld;
            _galaxyMap = galaxyMap;          
        }

        protected override void Init(ActivityParameters parameters)
        {
            base.Init(parameters);
        }

        public override void Update(InputState inputState)
        {
            _scene?.Update(inputState);
        }

        public override void Draw(SpriteBatch sb)
        {
            _scene?.Draw(sb);
        }

        public static Activity ActivityProvider(string param)
        {
            return new GameActivity(null, null);
        }

    }
}
