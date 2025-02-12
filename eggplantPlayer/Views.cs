using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eggplantPlayer;
using Terminal.Gui;

namespace EggplantPlayer
{
    public class CommandTextView : TextView
    {
        public Action<string> OnCommandEntered;

        protected override bool OnKeyDown(Key keyEvent)
        {
            if (keyEvent == Key.Enter)
            {
                string command = this.Text.ToString().Trim(); // Get input
                if (!string.IsNullOrWhiteSpace(command))
                {
                    OnCommandEntered?.Invoke(command); // Trigger command execution
                }

                this.Text = string.Empty; // Clear input after execution
                return true; // Mark event as handled
                             //Process Command here 
            }
            else
            {
                return base.OnKeyDown(keyEvent);
            }
        }

    }

    public class AnimationTextView : TextView
    {
        private Animation animation;
        public AnimationTextView() {

        }
        public void SetAnimation() { }
    }
}
