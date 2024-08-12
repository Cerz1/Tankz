using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    class BulletGUIitem : GUIitem
    {
        protected int numBullets;
        protected TextObject numBulletsTxt;
        protected bool isInfinite;
        protected bool isAvailable;

        public bool IsAvailable
        {
            get { return isAvailable; }
            set
            {
                isAvailable = value;
                numBulletsTxt.IsActive = isAvailable;

                if (isAvailable)
                {
                    SetColor(Vector4.One);
                }
                else
                {
                    SetColor(new Vector4(1.0f,0,0,0.4f));
                }
            }
        }

        public bool IsInfinite
        {
            get { return isInfinite; }
            set
            {
                isInfinite = value;
                numBulletsTxt.IsActive = !isInfinite;
            }
        }

        public int NumBullets
        {
            get
            {
                return numBullets;
            }
            set
            {
                numBullets = value;

                if (numBullets <= 0)
                {
                    //no bullets
                    IsAvailable = false;
                }
                else
                {
                    //bullets available
                    numBulletsTxt.Text = numBullets.ToString();

                    if (!isAvailable)
                    {
                        IsAvailable = true;
                    }
                }
            }
        }

        public BulletGUIitem(Vector2 position, GameObject owner, string textureName, int numBullets, float w = 0, float h = 0) : base(position, owner, textureName, w, h)
        {
            numBulletsTxt = new TextObject(new Vector2(position.X - 0.1f, position.Y));
            NumBullets = numBullets;
            IsActive = true;
        }

        public void DecrementBullets()
        {
            NumBullets--;
        }
    }
}
