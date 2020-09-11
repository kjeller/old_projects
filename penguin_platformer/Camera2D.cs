using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer_Slutprojekt
{
    class Camera2D
    {
        readonly Viewport viewport;
        public Vector2 position { get; set; } // Camera position
        public Vector2 origin { get; set; } // Camera origin
        public float zoom { get; set; } // Camera zoom
        public float rotation { get; set;} // Camera rotation

        public Camera2D(Viewport viewport)
        {
            this.viewport = viewport;

            origin = new Vector2(viewport.Width / 2, viewport.Height / 2);
            position = Vector2.Zero;
            rotation = 0;
            zoom = 1;
        }

        public void Update(Entity entity)
        {
            position = new Vector2(entity.position.X, viewport.Height / 2);
        }

        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-position, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(zoom, zoom, 1) *
                Matrix.CreateTranslation(new Vector3(origin, 0));    
        }
    }
}
