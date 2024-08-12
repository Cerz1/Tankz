using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tankz_2021

{
    class Groundable : GameObject
    {
        public bool IsGrounded
        {
            // IsGravityAffected is true while we're falling and became false as we land on ground
            get { return !RigidBody.IsGravityAffected; }
            set { RigidBody.IsGravityAffected = !value; }
        }

        public Groundable(string textureName, DrawLayer layer = DrawLayer.Playground, float w = 0, float h = 0) : base(textureName, layer, w, h)
        {
            RigidBody = new RigidBody(this);
        }

        public virtual void OnGrounded()
        {
            IsGrounded = true;
            RigidBody.Velocity.Y = 0.0f;
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if(collisionInfo.Collider is Groundable)
            {
                // Horizontal Collision
                if(collisionInfo.Delta.X < collisionInfo.Delta.Y)
                {
                    // Collision from Left
                    if(X < collisionInfo.Collider.X)
                    {
                        collisionInfo.Delta.X = -collisionInfo.Delta.X;
                    }
                    X += collisionInfo.Delta.X;
                    RigidBody.Velocity.X = 0.0f;
                }
                else
                {
                    // Vertical Collision
                    if(!IsGrounded && ((Groundable)collisionInfo.Collider).IsGrounded)
                    {
                        if(Y < collisionInfo.Collider.Y)
                        {
                            // Collision from Top
                            collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                            OnGrounded();
                            Y += collisionInfo.Delta.Y;
                        }
                    }
                }
            }
        }

        public override void Update()
        {
            if(IsActive)
            {
                IsGrounded = false;

                float groundY = ((PlayScene)Game.CurrentScene).GroundY;

                if(Position.Y + HalfHeight > groundY)
                {
                    Y = groundY - HalfHeight;
                    OnGrounded();
                }
            }
        }
    }
}
