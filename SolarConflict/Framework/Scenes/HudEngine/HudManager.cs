using Microsoft.Xna.Framework;
using SolarConflict.Framework.Scenes;
using SolarConflict.Framework.Scenes.HudEngine;
using SolarConflict.Framework.Scenes.HudEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework
{
    [Serializable]
    public class HudManager
    {
        public enum PositionType {TLtoBL, BLtoBR,BRtoTR, None}
        private Dictionary<PositionType, List<IHudComponent>> _components;
        //private List<IHudComponent> _components; //TODO: maybe dictionary 
        

        public HudManager()
        {
            _components = new Dictionary<PositionType, List<IHudComponent>>();
            _components[PositionType.None] = new List<IHudComponent>();
            _components[PositionType.TLtoBL] = new List<IHudComponent>();
            _components[PositionType.BLtoBR] = new List<IHudComponent>();
            _components[PositionType.BRtoTR] = new List<IHudComponent>();
            
            AddComponent(new MissionsIndicator());
            AddComponent(new HitpointIndicator());
            AddComponent(new PlayerDamageIndicator());
            AddComponent(new ScreenOverlayColor());
            AddComponent(new RespawnIndicator());
            

           // AddComponent(new StatRings(numOuterRingDots: 18));


            AddComponent(new MetersIndicatorV2(), PositionType.TLtoBL);
           

            AddComponent(new MoneyIndicator(), PositionType.BLtoBR);
            AddComponent(new ItemIndicator(), PositionType.BLtoBR);

            AddComponent(new MiniMap(), PositionType.BRtoTR);
            //AddComponent(new StarDateIndicator(), PositionType.BRtoTR);
            

            
            // _components.Add(new StatRings(numOuterRingDots: 18));

            // _components.Add(new AnnouncerEffects());
            //_components.Add(new PlayerHighlight());
            // _components.Add(new StatRings(numOuterRingDots: 18));
        }        

        public void AddComponent(IHudComponent component, PositionType positionType = PositionType.None)
        {
            _components[positionType].Add(component);
        }

        //public void Remove(Type type)
        //{
        //    for (int i = _components.Count - 1; i >= 0; i--)
        //    {
        //        if (_components[i].GetType() == type)
        //            _components.Remove(_components[i]);
        //    }
        //}

        public void Update(Scene scene, Agent player)
        {
            foreach (var list in _components.Values)
            {
                foreach (var item in list)
                {
                    item.Update(scene, player);
                }
            }
            
        }

        public void Draw(Scene scene, Agent player)
        {            
            int space = 10;
            Vector2 pos = Vector2.One * 10;
            foreach (var item in _components[PositionType.TLtoBL])
            {
                var size = item.GetSize();
                pos.X -= size.X; //Offset 
                pos.Y -= size.Y;
                item.Draw(scene.Camera.SpriteBatch, scene, player, pos);
                pos.X = 10;
                pos.Y += size.Height;
            }
            pos = new Vector2(space, ActivityManager.ScreenSize.Y - space);
            foreach (var item in _components[PositionType.BLtoBR])
            {

                var size = item.GetSize();
                pos.X -= size.X; //Offset 
                pos.Y += size.Y;
                item.Draw(scene.Camera.SpriteBatch, scene, player, pos);
                pos.Y = ActivityManager.ScreenSize.Y - space;
                pos.X += size.Width;
            }
            pos = new Vector2(ActivityManager.ScreenSize.X - space, ActivityManager.ScreenSize.Y - space);
            foreach (var item in _components[PositionType.BRtoTR])
            {
                var size = item.GetSize();
                pos.X += size.X; //Offset 
                pos.Y += size.Y;
                item.Draw(scene.Camera.SpriteBatch, scene, player, pos);
                pos.X = ActivityManager.ScreenSize.X - space;
                pos.Y -= size.Height;
            }

            foreach (var item in _components[PositionType.None])
            {
                item.Draw(scene.Camera.SpriteBatch, scene, player, Vector2.Zero);            
            }

            //foreach (var list in _components.Values)
            //{
            //    foreach (var item in list)
            //    {
            //        item.Draw(scene.Camera.SpriteBatch, scene, player , Vector2.Zero);
            //    }
            //}
        }
           
    }
}
