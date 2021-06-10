using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.XnaUtils.Input;
using System;
using System.Collections.Generic;
using XnaUtils.Graphics;

namespace SolarConflict.Framework
{
    /// <summary>
    /// Shows activatable items
    /// </summary>
    [Serializable]
    public class ItemIndicator : IHudComponent
    {
        [Serializable]
        private class HudItem
        {
            public ControlSignals ActivationSignal;
            public Item Item;
            public float ActivationAlpha;

            public HudItem(ControlSignals activationSignal, Item item)
            {
                ActivationSignal = activationSignal;
                Item = item;
                ActivationAlpha = 0;
            }
        }        

        public bool ShowAllItems { get; set; }
        int size = 60;
        int spaceing;
        //private Agent _player;
        private List<HudItem> _hudItems;
        private Dictionary<Item, HudItem> _hudItemsDictioneary;
        [NonSerialized]
        private static Texture2D _backgroundTexture;
             
        public ItemIndicator()
        {
            size = 60;
            spaceing = size + 5;
            ShowAllItems = false;            
            _hudItems = new List<HudItem>();
            _backgroundTexture =  TextureBank.Inst.GetTexture("fullframe");
            _hudItemsDictioneary = new Dictionary<Item, HudItem>();
        }

        public void Update(Scene scene, Agent player)
        {
            UpdateItemList(player);

            for (int i = 0; i < _hudItems.Count; i++) //Replace with gui utils
            {
               
                if(_hudItems[i].Item.WasActive)
                {
                    _hudItems[i].ActivationAlpha = 1;
                }

                _hudItems[i].ActivationAlpha = Math.Max(_hudItems[i].ActivationAlpha - 0.05f, 0);
            } 
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos) //TODO: add some kind of animation and sound when using an item on cooldown or not enough energy
        {
          
            SpriteBatch sb = scene.Camera.SpriteBatch;
            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
       
            int verticalPosition = (int)pos.Y;
            int horizontalPosition = (int)pos.X;// (spaceing*_hudItems.Count)/2;

            for (int i = 0; i < _hudItems.Count; i++) //Replace with gui utils
            {
                Rectangle positionRectangle = new Rectangle(horizontalPosition + i * spaceing, verticalPosition - size, size, size);
                //Rectangle frameRectangle = positionRectangle;
                //frameRectangle.Inflate(2, 2);
                //sb.Draw(backgroundTexture, frameRectangle, Color.CadetBlue);
                _hudItems[i].Item.DrawOnHud(scene.Camera.SpriteBatch, positionRectangle, _hudItems[i].ActivationSignal, _hudItems[i].ActivationAlpha);
            }
            sb.End();
        }

        public static void DrawKeyBinding(SpriteBatch spriteBatch, Vector2 position, ControlSignals activation, bool centered = false, int iconSize = 30) //TODO: move/change
        {
            if (!KeysSettings.Data.KeyBindings.ContainsKey(activation))
            {
                return;
            }

            GKeys key = KeysSettings.Data.KeyBindings[activation];
            Sprite sprite = key.GetIcon();
            if (sprite != null)
            {
                if (centered)
                    position -= new Vector2(iconSize / 2, iconSize / 2);
                Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, iconSize, iconSize);
                spriteBatch.Draw(sprite.Texture, rectangle, Color.White);
            }
            else
            {
                string keyBinding = key.ToString();
                Vector2 stringSize = Game1.font.MeasureString(keyBinding);
                if (centered)
                    position -= stringSize * 0.5f;                
                spriteBatch.DrawString(Game1.font, keyBinding, position, Color.Yellow);
            }
        }

        private void UpdateItemList(Agent player) //Possibly change it to hapen 
        {
            _hudItems.Clear();
            if (player != null && player.Inventory != null)
            {               
                if (player.ItemSlotsContainer != null)
                {
                    int numberOfSlots = player.ItemSlotsContainer.Count;
                    for (int i = 0; i < numberOfSlots; i++)
                    {
                        var item = player.ItemSlotsContainer[i].Item;

                        if (item != null && ( item.Profile.IsShownOnHUD || (item.ItemFlags & ItemFlags.ShowOnHud) == ItemFlags.ShowOnHud || ShowAllItems ))
                        {
                            _hudItems.Add(new HudItem(player.ItemSlotsContainer[i].ActivationSignal, player.ItemSlotsContainer[i].Item));
                        }
                    }
                }

                for (int i = 0; i < Math.Min(player.Inventory.Items.Length, Inventory.QUICK_USE_COUNT); i++)
                {
                    if (player.Inventory.Items[i] != null && player.Inventory.Items[i].Profile.IsWorkingInInventory && player.Inventory.Items[i].Profile.IsActivatable)
                    {
                        _hudItems.Add(new HudItem(Inventory.quickUseBind[i], player.Inventory.Items[i]));
                    }
                }
            }
        }

        public Rectangle GetSize() {            
            return new Rectangle(0,size/2,_hudItems.Count * spaceing, size);
        }

    }
}
