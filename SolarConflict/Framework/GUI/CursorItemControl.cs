using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;

namespace SolarConflict.Framework.GUI
{
    public delegate bool ItemFilter(Item item);
    /// <summary>
    /// For Cursor item in inventory and other activitys
    /// </summary>
    [Serializable]
    public class CursorItemControl:ItemControl
    {
        /// <summary>
        /// Filter and ItemUnderCourserTimeout
        /// </summary>
        const int FilterTimeout = 2;

        public Item ItemUnderCursor
        {
            get { return _filterCooldown <= FilterTimeout ? _itemUnderCursor : null; }
            set { _itemUnderCursorCooldown = 0; _itemUnderCursor = value; }
        }

        public ItemFilter Filter
        {
            get { return _filterCooldown <= FilterTimeout ? _filter : null; }
            set { _filterCooldown = 0; _filter = value; }
        }

        private Item _itemUnderCursor;
        private int _itemUnderCursorCooldown;

        private ItemFilter _filter;
        private int _filterCooldown;

        public CursorItemControl(Vector2 size):base(null, Vector2.Zero, size)
        {
        }

        public Item GetItemToCheck()
        {
            if (Item != null)
                return Item;
            return ItemUnderCursor;     
        }

        public override void Update(InputState inputState)
        {
            //base.Update(inputState);    
            Position = inputState.Cursor.Position;
            _itemUnderCursorCooldown++;
            _filterCooldown++;
        }


    }
}
