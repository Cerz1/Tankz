using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    enum BulletType { StdBullet, Rocket, LAST }

    abstract class Bullet : GameObject
    {
        protected int damage = 25;
        protected float maxSpeed = 10;
        protected SoundEmitter shootSound;

        public BulletType Type { get; protected set; }

        public Bullet(string texturePath, int spriteWidth=0, int spriteHeight=0) : base(texturePath)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            RigidBody.IsGravityAffected = true;
        }

        public void Shoot(Vector2 shootPos, Vector2 shootVelocity)
        {
            sprite.position = shootPos;
            RigidBody.Velocity = shootVelocity * maxSpeed;
            shootSound.Play(shootVelocity.Length);
        }

        public virtual void Reset()
        {
            IsActive = false;
        }

        public override void Update()
        {
            if (IsActive)
            {
                Vector2 cameraDist = Position - CameraMngr.MainCamera.position;

                if (cameraDist.LengthSquared > Game.HalfDiagonalSquared)
                {
                    BulletMngr.RestoreBullet(this);
                    return;
                }

                if (RigidBody.Velocity != Vector2.Zero)
                {
                    Forward = RigidBody.Velocity;
                }
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Player)
            {
                ((Player)collisionInfo.Collider).AddDamage(damage);
            }

            BulletMngr.RestoreBullet(this);

            Explosion1 exp = (Explosion1)GfxMngr.GetSpecialFX(SpecialFX.Explosion_1);

            if (exp != null)
            {
                exp.Position = this.Position;
                exp.Play();
            }
        }
    }
}
