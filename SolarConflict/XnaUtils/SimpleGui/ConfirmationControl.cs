using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XnaUtils.Input;
using XnaUtils.SimpleGui.Controllers;

namespace XnaUtils.SimpleGui {
    
    /// <summary>On invocation, pops up a confirmation, invokes the actual action if that's clicked</summary>
    public class ConfirmationControl : GuiControl {
      
        public string ConfirmationText;

        GuiControl _popup;

        public object TextBank { get; private set; }

        public ConfirmationControl(string confirmationText = "Are you sure?") {
            ConfirmationText = confirmationText;
        }

        void ClosePopup() {
            if (_popup == null)
                // Already closed
                return;

            RemoveChild(_popup);
            _popup = null;
        }

        /// <summary>Call the actual action (without bringing up a confirmation dialogue)</summary>
        public void ConfirmAction(CursorInfo cursorInfo) {
            base.InvokeAction(cursorInfo);
        }

        public override void InvokeAction(CursorInfo cursorLocation) 
        {
            OpenPopup();
        }

        void OpenPopup() {
            if (_popup != null)
                // Already open
                return;
            
            // Create and add the popup
            var layout = new VerticalLayout(Vector2.Zero);
            layout.ShowFrame = true;
            layout.AddChild(new RichTextControl(ConfirmationText));

            var confirm = new RichTextControl("Yes");
            confirm.Action += (source, cursorInfo) => {
                (source.Parent.Parent as ConfirmationControl).ConfirmAction(cursorInfo);
            };
            layout.AddChild(confirm);

            var cancel = new RichTextControl("No");
            cancel.Action += (source, cursorInfo) => {
                (source.Parent.Parent as ConfirmationControl).ClosePopup();
            };
            layout.AddChild(cancel);
                        
            _popup = layout;

            AddChild(layout);
        }
    }
}
