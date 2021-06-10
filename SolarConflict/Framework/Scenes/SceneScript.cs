using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;

namespace SolarConflict
{
    /// <summary>
    /// Used to exstend Scene insted on inheratince
    /// </summary>
    [Serializable]
    public abstract class SceneScript
    {

        /// <summary>
        /// Called after scene init was called
        /// </summary>
        /// <param name="scene"></param>
        public virtual void InitScript(Scene scene)
        {

        }

        ///// <summary>
        ///// Called every update frame of scene before scene was updated
        ///// </summary>
        ///// <param name="scene"></param>

        /// <summary>
        /// Called every update frame of scene before scene was updated
        /// </summary>
        /// <param name="scene"></param>
        /// <returns> returns true once script is over</returns>
        public virtual bool UpdateScript(Scene scene)
        {
            return false;
        }

        /// <summary>
        /// Only updates when game is not paused
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public virtual void UpdateWithGameEngine(Scene scene)
        {
            
        }


        public virtual void Draw(Scene scene)
        {

        }

        public virtual void OnBack(Scene scene)
        {
        }


    }
}
