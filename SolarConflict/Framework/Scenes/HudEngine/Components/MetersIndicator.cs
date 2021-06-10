using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.Scenes.HudEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.Scenes
{
    /// <summary>
    /// Shows meter bars that display info like: hitpoints, shield, energy...
    /// </summary>
    [Serializable]
    public class MetersIndicator : IHudComponent
    {
        [Serializable]
        public class MeterDisplayPropertiys
        {
            public Color Color;
            /// <summary>
            /// Scale the meter value to other unit (for example frames to sec)
            /// </summary>
            public float UnitScaling;
            public bool IsHideWhenZero;
            //float defaultMaxValue is zero then the value is the max

            public MeterDisplayPropertiys(Color color)
            {
                IsHideWhenZero = false;
                UnitScaling = 1;
                Color = color;
            }
        }

        private Dictionary<MeterType, MeterDisplayPropertiys> _meterDisplayPropertiysMap;
        public MeterDisplayPropertiys _defaultMeterDisplayPropertiys;
        private List<MeterType> _metersToShow;
        private Dictionary<MeterType, Meter> _meters; //Not sure we need it        

        public MetersIndicator()
        {
            _meterDisplayPropertiysMap = new Dictionary<MeterType, MeterDisplayPropertiys>();
            _defaultMeterDisplayPropertiys = new MeterDisplayPropertiys(Color.White);
            _metersToShow = new List<MeterType>();
            _meters = new Dictionary<MeterType, Meter>();
            Init();
        }

        public void Update(Scene scene, Agent player)
        {
            if (player != null)
            {
                _meters.Clear();              
                foreach (var item in _metersToShow)
                {
                    Meter meter = player.GetMeter(item);
                    if (meter != null) //&& meter.Value > 0
                    {
                        _meters[item] = meter;
                    }
                }
            //     Meter speedMeter = new Meter(player.Velocity.Length());//Player 
            //    _meters.Add(MeterType.Speed, speedMeter);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            SpriteBatch sb = Game1.sb;
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            DrawMeters(Game1.sb,pos);
            sb.End();
        }

        private void DrawMeters(SpriteBatch sb, Vector2 position)
        {
            int index = 0;
            
            foreach (var meterKeyValue in _meters)
            {
                MeterDisplayPropertiys meterDisplayPropertiys = _defaultMeterDisplayPropertiys;
                if(_meterDisplayPropertiysMap.ContainsKey(meterKeyValue.Key))
                {
                    meterDisplayPropertiys = _meterDisplayPropertiysMap[meterKeyValue.Key];
                }
                if (meterKeyValue.Value.MaxValue == 0 || (meterDisplayPropertiys.IsHideWhenZero && meterKeyValue.Value.Value == 0))
                    continue;
                Color color = meterDisplayPropertiys.Color;// new Color(0, 224 / 2, 216 / 2);

                float value = 0;
                if (meterKeyValue.Value.MaxValue < float.MaxValue)
                {
                    value = meterKeyValue.Value.Value / meterKeyValue.Value.MaxValue;
                }
                else
                {
                    value = meterKeyValue.Value.Value / 1000f; //TODO:
                }

                int spacing = 35;
                Vector2 pos = position + new Vector2(0, 0 + index * spacing);

                HudUtils.MeterDisplay(sb, value, new Rectangle((int)pos.X, (int)pos.Y, 300, 40), color);

                string text = //meterKeyValue.Key.ToString() + ":" + 
                    ((int)Math.Round( meterKeyValue.Value.Value)).ToString();
                if (meterKeyValue.Value.MaxValue < float.MaxValue)
                    text += "/" + ((int)Math.Round( meterKeyValue.Value.MaxValue)).ToString();

                sb.DrawString(Game1.font, text, pos + new Vector2(10, 2), new Color(0, 0, 0, 255),0, Vector2.Zero, 1, SpriteEffects.None, 0);
                index++;
            }
        }

        private void Init()
        {
            _metersToShow.Add(MeterType.Hitpoints);
            _metersToShow.Add(MeterType.Shield);
            _metersToShow.Add(MeterType.Energy);
            MeterDisplayPropertiys stunProp = new MeterDisplayPropertiys(Color.Violet);
            stunProp.IsHideWhenZero = true;
            stunProp.UnitScaling = 1 / 60f;
            _meterDisplayPropertiysMap.Add(MeterType.StunTime, stunProp);
            _meterDisplayPropertiysMap.Add(MeterType.Hitpoints, new MeterDisplayPropertiys(Palette.Hitpoints));
            _meterDisplayPropertiysMap.Add(MeterType.Shield, new MeterDisplayPropertiys(Palette.Shield));
            _meterDisplayPropertiysMap.Add(MeterType.Energy, new MeterDisplayPropertiys(Palette.Energy));
            //_meterDisplayPropertiysMap.Add()
            //  _metersToShow.Add(MeterType.Money);
            //  _metersToShow.Add(MeterType.WarpFuel);
            //_metersToShow.Add(MeterType.Money);
            // _metersToShow.Add(MeterType.ControlPoints);
            //if (DebugUtils.IsDebug)
            //    _metersToShow.Add(MeterType.Money);
            //    _metersToShow.Add(MeterType.StunTime);
            ///*metersToShow.Add(MeterType.Cloak);
            //metersToShow.Add(MeterType.Ammo1);
            //metersToShow.Add(MeterType.Ammo2);
            //metersToShow.Add(MeterType.Ammo3);
            //_metersToShow.Add(MeterType.ControlPoints);
            //_metersToShow.Add(MeterType.MaxControlPoints);
            ////metersToShow.Add(MeterType.Kills);*/
            //_metersToShow.Add(MeterType.GlobalRepairCooldown);
            //_metersToShow.Add(MeterType.FactionKills);
        }

        public Rectangle GetSize() {
            int index = 0;
            int spacing = 35;
            foreach (var meterKeyValue in _meters)
            {
                
                Vector2 pos = new Vector2(0, 0 + index * spacing);        
                index++;
            }
            return new Rectangle(0,0,300, (int)index * spacing);
        }
    }
}
