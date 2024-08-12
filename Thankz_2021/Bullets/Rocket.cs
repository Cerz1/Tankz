using Aiv.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    class Rocket : Bullet
    {
        protected float startEngineAngle;
        protected bool engineIsOn;

        protected AudioClip engineStart;

        public Rocket() : base("rocketBullet")
        {
            Type = BulletType.Rocket;
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType(RigidBodyType.Player | RigidBodyType.Tile);
            maxSpeed = 15;

            damage = 30;

            startEngineAngle = -0.174533f;//-10 deg

            shootSound = new SoundEmitter(this, "whistle");
            components.Add(ComponentType.SoundEmitter, shootSound);

            engineStart = GfxMngr.GetClip("engineStart");
        }

        public override void Reset()
        {
            base.Reset();
            engineIsOn = false;
        }

        public override void Update()
        {
            base.Update();

            if (IsActive)
            {
                if(!engineIsOn && (sprite.Rotation > startEngineAngle || sprite.Rotation < -Math.PI - startEngineAngle))
                {
                    engineIsOn = true;
                    RigidBody.Velocity.X = 18 * Math.Sign(RigidBody.Velocity.X);
                    shootSound.Play(1f, 1f, engineStart);
                }
            }
        }
    }
}
