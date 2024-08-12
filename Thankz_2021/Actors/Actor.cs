using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    abstract class Actor : Groundable
    {
        // Variables
        protected BulletType bulletType;
        protected int energy;
        protected float maxSpeed;
        // Properties
        public bool IsAlive { get { return energy > 0; } }
        public int MaxEnergy { get; protected set; }
        public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, MaxEnergy); } }
        // Refs
        protected ProgressBar energyBar;
        public Bullet LastShotBullet { get; protected set; }


        public Actor(string texturePath, DrawLayer layer = DrawLayer.Playground, float w = 0, float h = 0) : base(texturePath, layer, w: w, h: h)
        {
            float unitDist = Game.PixelsToUnits(4);
            energyBar = new ProgressBar("barFrame", "blueBar", new Vector2(unitDist));
            energyBar.Position = new Vector2(1, 1);
            energyBar.IsActive = true;
            MaxEnergy = 100;
        }

        protected virtual void Shoot(Vector2 velocity, Vector2 position)
        {
            Bullet b = BulletMngr.GetBullet(bulletType);

            if (b != null)
            {
                b.IsActive = true;
                position += velocity.Normalized() * (b.HalfWidth);
                b.Shoot(position, velocity);
            }

            LastShotBullet = b;
        }

        public virtual void AddDamage(int dmg)
        {
            Energy -= dmg;

            if (Energy <= 0)
            {
                OnDie();
            }
        }

        public virtual void AddEnergy(int amount)
        {
            Energy = Math.Min(Energy + amount, MaxEnergy);
        }

        public abstract void OnDie();

        public virtual void Reset()
        {
            Energy = MaxEnergy;
        }

    }
}

