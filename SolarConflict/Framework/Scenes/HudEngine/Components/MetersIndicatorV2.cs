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
    public class MetersIndicatorV2 : IHudComponent
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
            public Rectangle sourceRectangle;
            public Rectangle fillRectangle;

            public int heightOffset;
            public Vector2 fillOffset;
            public Vector2 textOffset;
            public Color textColor;

            public bool showText;


            public MeterDisplayPropertiys(Rectangle sourceRec, Rectangle fillRec, int hOffset, Vector2 fOffset)
            {
                IsHideWhenZero = false;
                showText = false;
                UnitScaling = 1;
                sourceRectangle = sourceRec;
                fillRectangle = fillRec;
                heightOffset = hOffset;
                fillOffset = fOffset;
            }

            public MeterDisplayPropertiys(Rectangle sourceRec, Rectangle fillRec, int hOffset, Vector2 fOffset, Color tColor, Vector2 tOffset)
            {
                IsHideWhenZero = false;
                showText = true;
                UnitScaling = 1;
                sourceRectangle = sourceRec;
                fillRectangle = fillRec;
                heightOffset = hOffset;
                fillOffset = fOffset;

                textOffset = tOffset;
                textColor = tColor;
            }
        }

        private Dictionary<MeterType, MeterDisplayPropertiys> _meterDisplayPropertiysMap;
        public MeterDisplayPropertiys _defaultMeterDisplayPropertiys;
        private List<MeterType> _metersToShow;
        private Dictionary<MeterType, Meter> _meters; 

        public MetersIndicatorV2()
        {
            _meterDisplayPropertiysMap = new Dictionary<MeterType, MeterDisplayPropertiys>();
            _defaultMeterDisplayPropertiys = new MeterDisplayPropertiys(Rectangle.Empty, Rectangle.Empty, 0, Vector2.Zero);
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
                    if (meter != null)
                    {
                        //To render
                        _meters[item] = meter;
                    }
                }
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

                if (_meterDisplayPropertiysMap.ContainsKey(meterKeyValue.Key))
                {
                    meterDisplayPropertiys = _meterDisplayPropertiysMap[meterKeyValue.Key];
                }

                //we pull source region accordign to type, we need region for underlay and region for fill
                if (meterKeyValue.Value.MaxValue == 0 || (meterDisplayPropertiys.IsHideWhenZero && meterKeyValue.Value.Value == 0))
                    continue;
                Color color = meterDisplayPropertiys.Color;
                Rectangle targetRec = meterDisplayPropertiys.sourceRectangle;
                Rectangle sourceRec = targetRec;

                Rectangle filltarRec = meterDisplayPropertiys.fillRectangle;
                Rectangle fillRec = filltarRec;

                int spacing = 35;

                Vector2 pos = position + new Vector2(0, 0 + index * spacing);

                targetRec.X = (int)pos.X ;
                targetRec.Y = (int)pos.Y + meterDisplayPropertiys.heightOffset;

                pos += meterDisplayPropertiys.fillOffset;

                filltarRec.X = (int)pos.X;
                filltarRec.Y = (int)pos.Y;

                float value = 1;
                //get percentage
                if (meterKeyValue.Value.MaxValue < float.MaxValue)
                {
                    value = meterKeyValue.Value.Value / meterKeyValue.Value.MaxValue;
                }

                //Get correct fill rectangle according to percentage value
                filltarRec.Width = (int)((float)filltarRec.Width * value);
                fillRec.Width = filltarRec.Width;

                HudUtils.MeterDisplayV2(sb, targetRec, sourceRec, filltarRec, fillRec);
                

                if(meterDisplayPropertiys.showText)
                { 
                    string text =((int)Math.Round( meterKeyValue.Value.Value)).ToString();
                    if (meterKeyValue.Value.MaxValue < float.MaxValue)
                        text += "/" + ((int)Math.Round( meterKeyValue.Value.MaxValue)).ToString();

                    //This isn't cheapest however not most expensive either and we can do handle this few times per frame
                    Vector2 textPos = Game1.orbitron12.MeasureString(text);

                    sb.DrawString(Game1.orbitron12, text, pos + meterDisplayPropertiys.textOffset - textPos / 2f, meterDisplayPropertiys.textColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
                index++;
            }
        }

        private void Init()
        {
            _metersToShow.Add(MeterType.Hitpoints);
            _metersToShow.Add(MeterType.Shield);
            _metersToShow.Add(MeterType.Energy);
            //MeterDisplayPropertiys stunProp; // = new MeterDisplayPropertiys(Color.Violet); // TO DO
            //stunProp.IsHideWhenZero = true;
            //stunProp.UnitScaling = 1 / 60f;
            //_meterDisplayPropertiysMap.Add(MeterType.StunTime, stunProp);
            _meterDisplayPropertiysMap.Add(MeterType.Hitpoints, new MeterDisplayPropertiys(new Rectangle(0, 0, 223, 32), new Rectangle(0, 123, 184, 26), 0, new Vector2(36,2), new Color(139,254,242), new Vector2(100,15)));
            _meterDisplayPropertiysMap.Add(MeterType.Shield, new MeterDisplayPropertiys(new Rectangle(0, 39, 223, 35), new Rectangle(0, 153, 184, 29), -6, new Vector2(36,-4), new Color(157,203,255), new Vector2(100,17)));
            _meterDisplayPropertiysMap.Add(MeterType.Energy, new MeterDisplayPropertiys(new Rectangle(0, 77, 223, 19), new Rectangle(0, 104, 184, 15), -9, new Vector2(36,-7)));
        }

        public Rectangle GetSize() {

            //To Do: Modify to resolve variable size of bars depending on their source rectangles
            int index = 0;
            int spacing = 32;
            foreach (var meterKeyValue in _meters)
            {
                Vector2 pos = new Vector2(0, 0 + index * spacing);        
                index++;
            }
            return new Rectangle(0,0,223, (int)index * spacing);
        }
    }
}
