using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viking_Warrior___Slutprojekt
{
    static class CollisionHandler
    {
        /// <summary>
        /// Checks if rectangle1 is ontop of rectangle2
        /// </summary>
        /// <param name="rectangle1">the rectangle "above"</param>
        /// <param name="rectangle2">the rectangle "under"</param>
        public static bool onTop(this Rectangle rectangle1, Rectangle rectangle2)
        {
            return (rectangle1.Bottom >= rectangle2.Top -4 &&
                    rectangle1.Bottom <= rectangle2.Top + (rectangle2.Height / 2) &&
                    rectangle1.Right >= rectangle2.Left + (rectangle2.Width / 5) && //rectangle1 can't "climb" on the side of a tile from the right
                    rectangle1.Left <= rectangle2.Right - (rectangle2.Width / 5) //rectangle1 can't "climb" on the side of a tile from left
                   );
        }
        /// <summary>
        /// Checks if rectangle1 is below rectangle2
        /// </summary>
        /// <param name="rectangle1">the rectangle "below"</param>
        /// <param name="rectangle2">the rectangle "above"</param>
        /// <returns></returns>
        public static bool onBottom(this Rectangle rectangle1, Rectangle rectangle2)
        {
            return (rectangle1.Top <= rectangle2.Bottom + (rectangle2.Height /5) &&
                    rectangle1.Top >= rectangle2.Bottom &&
                    rectangle1.Right >= rectangle2.Left + (rectangle2.Width / 5) &&
                    rectangle1.Left <= rectangle2.Right - (rectangle2.Width / 5)
                );
        }
    }
}
