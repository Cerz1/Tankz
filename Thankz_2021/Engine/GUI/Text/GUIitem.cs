using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    class GUIitem : GameObject
    {
        public bool IsSelected;

        protected GameObject owner;
        protected Vector2 offset;

        public GUIitem(Vector2 position, GameObject owner, string textureName, float w = 0, float h = 0) : base(textureName, DrawLayer.GUI, w, h)
        {
            this.owner = owner;
            sprite.position = position;
            sprite.Camera = CameraMngr.GetCamera("GUI");
            offset = position - owner.Position;
        }

        public void SetColor(Vector4 color)
        {
            sprite.SetMultiplyTint(color);
        }
    }
}
