using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Tankz_2021
{
    class Player : Tank
    {
        // References
        protected Controller controller;
        protected ProgressBar loadingBar;
        protected TextObject playerName;
        //protected TextObject playerName;
        protected StateMachine stateMachine;
        
        // Variables
        //protected int playerId;
        protected bool isLoading;
        protected float currentLoadingValue;
        protected float loadIncrease = 50;
        protected float maxLoadingValue = 100;
        protected Vector2 loadingBarOffset;
        private bool isFirePressed;
        private bool isChangeWeaponPressed;

        // Properties
        public override int Energy { get => base.Energy; set { base.Energy = value; energyBar.Scale((float)value / (float)base.MaxEnergy); } }
        //public int MaxEnergy { get { return base.MaxEnergy; } }
        //public bool IsGrounded { get { return RigidBody.Velocity.Y == 0; } }

        protected WeaponsGUI weaponsGUI;

        public Player(Controller ctrl, int id = 0) : base()
        {
            // Misc
            controller = ctrl;
            maxSpeed = 6;
            bulletType = BulletType.StdBullet;
            IsActive = true;
            
            // RB
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Tile);
            RigidBody.IsGravityAffected = true;

            // Loading Bar
            float unitDist = Game.PixelsToUnits(4);
            loadingBar = new ProgressBar("barFrame", "blueBar", new Vector2(unitDist, unitDist));
            loadingBar.IsActive = false;
            loadingBar.Camera = CameraMngr.MainCamera;
            loadingBarOffset = new Vector2(loadingBar.Width*-0.5f, -1.3f);

            // Energy Bar and Player Name
            energyBar.Position = new Vector2(id * 3 + 0.2f, 0.5f);
            playerName = new TextObject(new Vector2(energyBar.Position.X, 0.25f),$"Player {id+1}");

            Reset();

            //FSM
            stateMachine = new StateMachine(this);
            stateMachine.AddState(StateEnum.WAIT, new WaitState());
            stateMachine.AddState(StateEnum.PLAY, new PlayState());
            stateMachine.AddState(StateEnum.SHOOT, new ShootState());

            weaponsGUI = new WeaponsGUI(new Vector2(energyBar.Position.X, 1f));
            weaponsGUI.IsActive = true;
        }

        public virtual void Play()
        {
            stateMachine.GoTo(StateEnum.PLAY);
        }

        public virtual void UpdateStateMachine()
        {
            stateMachine.Update();
        }

        protected virtual void StartLoading()
        {
            currentLoadingValue = 0;
            loadIncrease = Math.Abs(loadIncrease);

            loadingBar.Position = Position + loadingBarOffset;
            loadingBar.IsActive = true;
            isLoading = true;
        }

        public virtual void StopLoading()
        {
            if (isLoading)
            {
                isLoading = false;
                loadingBar.IsActive = false;
                Shoot(currentLoadingValue / maxLoadingValue);

                if (LastShotBullet != null)
                {
                    CameraMngr.SetTarget(LastShotBullet);

                    bulletType = weaponsGUI.DecrementBullets();
                }

                stateMachine.GoTo(StateEnum.SHOOT);
            }
        }

        public void Input()
        {
            //movement
            float directionX = controller.GetHorizontal();

            if (directionX != 0 && !isLoading)
            {
                RigidBody.Velocity.X = directionX * maxSpeed;
            }
            else
            {
                RigidBody.Velocity.X = 0;
            }

            //cannon rotation
            float directionY = controller.GetVertical();
            if (directionY != 0)
            {
                cannon.Rotation += directionY * Game.DeltaTime;
                cannon.Rotation = MathHelper.Clamp(cannon.Rotation, maxCannonAngle, 0);
            }

            // shoot
            if (controller.IsFirePressed())
            {
                if (!isFirePressed)
                {
                    isFirePressed = true;
                    StartLoading();
                }
            }
            else if (isFirePressed)
            {
                isFirePressed = false;
                StopLoading();
            }

            //weapon change
            if (controller.NextWeapon())
            {
                if (!isChangeWeaponPressed)
                {
                    isChangeWeaponPressed = true;
                    bulletType = weaponsGUI.NextWeapon();
                }
            }
            else if (controller.PrevWeapon())
            {
                if (!isChangeWeaponPressed)
                {
                    isChangeWeaponPressed = true;
                    bulletType = weaponsGUI.NextWeapon(-1);
                }
            }
            else
            {
                isChangeWeaponPressed = false;
            }
        }

        public override void OnDie()
        {
            IsActive = false;
            energyBar.IsActive = false;
            ((PlayScene)Game.CurrentScene).OnPlayerDies();
        }

        public override void Update()
        {
            if (IsActive)
            {
                base.Update();

                if (isLoading)
                {
                    currentLoadingValue += loadIncrease * Game.DeltaTime;

                    if(currentLoadingValue > maxLoadingValue)
                    {
                        currentLoadingValue = maxLoadingValue;
                        loadIncrease = -loadIncrease;
                    }
                    else if(currentLoadingValue < 0)
                    {
                        currentLoadingValue = 0;
                        loadIncrease = -loadIncrease;
                    }

                    loadingBar.Scale(currentLoadingValue / maxLoadingValue);
                }
            }
        }
    }
}
