using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021
{
    class Tile : Groundable
    {
        protected RandomizeSoundEmitter crackSound;

        public Tile(string textureName = "earth", DrawLayer layer = DrawLayer.Playground) : base(textureName)
        {
            //RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Tile;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Tile | RigidBodyType.PlayerBullet);
            IsActive = true;
            RigidBody.IsGravityAffected = true;

            crackSound = new RandomizeSoundEmitter(this);
            crackSound.AddClip("crack_1");
            crackSound.AddClip("crack_2");

            components.Add(ComponentType.RandomizeSoundEmitter, crackSound);

        }

        public override void OnCollide(Collision collisionInfo)
        {
            if(collisionInfo.Collider is Bullet)
            {
                IsActive = false;
                crackSound.Play();
            }
            else
            {
                base.OnCollide(collisionInfo);
            }
        }
    }
}
