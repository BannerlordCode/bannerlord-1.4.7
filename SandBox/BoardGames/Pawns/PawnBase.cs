using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.BoardGames.Pawns
{
	// Token: 0x020000F9 RID: 249
	public abstract class PawnBase
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x0005D4DE File Offset: 0x0005B6DE
		// (set) Token: 0x06000C7B RID: 3195 RVA: 0x0005D4E5 File Offset: 0x0005B6E5
		public static int PawnMoveSoundCodeID { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x0005D4ED File Offset: 0x0005B6ED
		// (set) Token: 0x06000C7D RID: 3197 RVA: 0x0005D4F4 File Offset: 0x0005B6F4
		public static int PawnSelectSoundCodeID { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x0005D4FC File Offset: 0x0005B6FC
		// (set) Token: 0x06000C7F RID: 3199 RVA: 0x0005D503 File Offset: 0x0005B703
		public static int PawnTapSoundCodeID { get; set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0005D50B File Offset: 0x0005B70B
		// (set) Token: 0x06000C81 RID: 3201 RVA: 0x0005D512 File Offset: 0x0005B712
		public static int PawnRemoveSoundCodeID { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000C82 RID: 3202
		public abstract bool IsPlaced { get; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0005D51A File Offset: 0x0005B71A
		// (set) Token: 0x06000C84 RID: 3204 RVA: 0x0005D522 File Offset: 0x0005B722
		public virtual Vec3 PosBeforeMoving
		{
			get
			{
				return this.PosBeforeMovingBase;
			}
			protected set
			{
				this.PosBeforeMovingBase = value;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0005D52B File Offset: 0x0005B72B
		public GameEntity Entity { get; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0005D533 File Offset: 0x0005B733
		protected List<Vec3> GoalPositions { get; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x0005D53B File Offset: 0x0005B73B
		// (set) Token: 0x06000C88 RID: 3208 RVA: 0x0005D543 File Offset: 0x0005B743
		private protected Vec3 CurrentPos { protected get; private set; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x0005D54C File Offset: 0x0005B74C
		// (set) Token: 0x06000C8A RID: 3210 RVA: 0x0005D554 File Offset: 0x0005B754
		public bool Captured { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x0005D55D File Offset: 0x0005B75D
		// (set) Token: 0x06000C8C RID: 3212 RVA: 0x0005D565 File Offset: 0x0005B765
		public bool MovingToDifferentTile { get; set; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0005D56E File Offset: 0x0005B76E
		// (set) Token: 0x06000C8E RID: 3214 RVA: 0x0005D576 File Offset: 0x0005B776
		public bool Moving { get; private set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0005D57F File Offset: 0x0005B77F
		// (set) Token: 0x06000C90 RID: 3216 RVA: 0x0005D587 File Offset: 0x0005B787
		public bool PlayerOne { get; private set; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x0005D590 File Offset: 0x0005B790
		public bool HasAnyGoalPosition
		{
			get
			{
				bool flag = false;
				if (this.GoalPositions != null)
				{
					flag = !this.GoalPositions.IsEmpty<Vec3>();
				}
				return flag;
			}
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0005D5B8 File Offset: 0x0005B7B8
		protected PawnBase(GameEntity entity, bool playerOne)
		{
			this.Entity = entity;
			this.PlayerOne = playerOne;
			this.CurrentPos = this.Entity.GetGlobalFrame().origin;
			this.PosBeforeMoving = this.CurrentPos;
			this.Moving = false;
			this._dragged = false;
			this.Captured = false;
			this._movePauseDuration = 0.3f;
			entity.CreateVariableRatePhysics(true);
			this.GoalPositions = new List<Vec3>();
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0005D630 File Offset: 0x0005B830
		public virtual void Reset()
		{
			this.ClearGoalPositions();
			this.Moving = false;
			this.MovingToDifferentTile = false;
			this._movePauseDuration = 0.3f;
			this._movePauseTimer = 0f;
			this._moveTiming = false;
			this._dragged = false;
			this.Captured = false;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0005D67C File Offset: 0x0005B87C
		public virtual void AddGoalPosition(Vec3 goal)
		{
			this.GoalPositions.Add(goal);
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0005D68C File Offset: 0x0005B88C
		public virtual void SetPawnAtPosition(Vec3 position)
		{
			MatrixFrame globalFrame = this.Entity.GetGlobalFrame();
			globalFrame.origin = position;
			this.Entity.SetGlobalFrame(in globalFrame, true);
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0005D6BC File Offset: 0x0005B8BC
		public virtual void MovePawnToGoalPositions(bool instantMove, float speed, bool dragged = false)
		{
			this.PosBeforeMoving = this.Entity.GlobalPosition;
			this._moveSpeed = speed;
			this._currentGoalPos = 0;
			this._movePauseTimer = 0f;
			this._dtCounter = 0f;
			this._moveTiming = false;
			this._dragged = dragged;
			if (this.GoalPositions.Count == 1 && this.PosBeforeMoving.Equals(this.GoalPositions[0]))
			{
				instantMove = true;
			}
			if (instantMove)
			{
				MatrixFrame globalFrame = this.Entity.GetGlobalFrame();
				globalFrame.origin = this.GoalPositions[this.GoalPositions.Count - 1];
				this.Entity.SetGlobalFrame(in globalFrame, true);
				this.ClearGoalPositions();
				return;
			}
			this.Moving = true;
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0005D78E File Offset: 0x0005B98E
		public virtual void EnableCollisionBody()
		{
			this.Entity.BodyFlag &= ~BodyFlags.Disabled;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0005D7A4 File Offset: 0x0005B9A4
		public virtual void DisableCollisionBody()
		{
			this.Entity.BodyFlag |= BodyFlags.Disabled;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0005D7BC File Offset: 0x0005B9BC
		public void Tick(float dt)
		{
			if (this._moveTiming)
			{
				this._movePauseTimer += dt;
				if (this._movePauseTimer >= this._movePauseDuration)
				{
					this._moveTiming = false;
					this._movePauseTimer = 0f;
				}
				return;
			}
			if (this.Moving && dt > 0f)
			{
				Vec3 vec = new Vec3(0f, 0f, 0f, -1f);
				Vec3 vec2 = this.GoalPositions[this._currentGoalPos] - this.PosBeforeMoving;
				float num = vec2.Normalize();
				float num2 = num / this._moveSpeed;
				float num3 = this._dtCounter / num2;
				if (this._dtCounter.Equals(0f))
				{
					float x = (this.Entity.GlobalBoxMax - this.Entity.GlobalBoxMin).x;
					float z = (this.Entity.GlobalBoxMax - this.Entity.GlobalBoxMin).z;
					Vec3 vec3 = new Vec3(0f, 0f, z / 2f, -1f);
					Vec3 vec4 = this.Entity.GetGlobalFrame().origin + vec3 + vec2 * (x / 1.8f);
					Vec3 vec5 = this.GoalPositions[this._currentGoalPos] + vec3;
					float num4;
					if (Mission.Current.Scene.RayCastForClosestEntityOrTerrain(vec4, vec5, out num4, 0.001f, BodyFlags.None))
					{
						this._freePathToDestination = false;
						num = num4;
					}
					else
					{
						this._freePathToDestination = true;
						if (!this._dragged)
						{
							this.PlayPawnMoveSound();
						}
						else
						{
							this.PlayPawnTapSound();
						}
					}
				}
				if (!this._freePathToDestination)
				{
					float num5 = MathF.Sin(num3 * 3.1415927f);
					float num6 = num / 6f;
					num5 *= num6;
					vec += new Vec3(0f, 0f, num5, -1f);
				}
				float dtCounter = this._dtCounter;
				this._dtCounter += dt;
				Vec3 vec8;
				if (num3 >= 1f)
				{
					this._dtCounter = 0f;
					this.CurrentPos = this.GoalPositions[this._currentGoalPos];
					vec = Vec3.Zero;
					if (!this._freePathToDestination && this.IsPlaced)
					{
						this.PlayPawnTapSound();
					}
					else if (!this.IsPlaced)
					{
						this.PlayPawnRemovedTapSound();
					}
					Vec3 vec6 = this.GoalPositions[this._currentGoalPos];
					bool flag = true;
					while (this._currentGoalPos < this.GoalPositions.Count - 1)
					{
						this._currentGoalPos++;
						Vec3 vec7 = this.GoalPositions[this._currentGoalPos];
						vec8 = vec6 - vec7;
						if (vec8.LengthSquared > 0f)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						Action<PawnBase, Vec3, Vec3> onArrivedFinalGoalPosition = this.OnArrivedFinalGoalPosition;
						if (onArrivedFinalGoalPosition != null)
						{
							onArrivedFinalGoalPosition(this, this.PosBeforeMoving, this.CurrentPos);
						}
						this.Moving = false;
						this.ClearGoalPositions();
					}
					else
					{
						Action<PawnBase, Vec3, Vec3> onArrivedIntermediateGoalPosition = this.OnArrivedIntermediateGoalPosition;
						if (onArrivedIntermediateGoalPosition != null)
						{
							onArrivedIntermediateGoalPosition(this, this.PosBeforeMoving, this.CurrentPos);
						}
						this._movePauseDuration = 0.3f;
						this._moveTiming = true;
					}
					this.PosBeforeMoving = this.CurrentPos;
				}
				else
				{
					this.Moving = true;
					this.CurrentPos = MBMath.Lerp(this.PosBeforeMoving, this.GoalPositions[this._currentGoalPos], num3, 0.005f);
				}
				ref MatrixFrame ptr = ref this.Entity.GetGlobalFrame();
				vec8 = this.CurrentPos + vec;
				MatrixFrame matrixFrame = new MatrixFrame(in ptr.rotation, in vec8);
				this.Entity.SetGlobalFrame(in matrixFrame, true);
			}
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0005DB64 File Offset: 0x0005BD64
		public void MovePawnToGoalPositionsDelayed(bool instantMove, float speed, bool dragged, float delay)
		{
			if (this.GoalPositions.Count > 0)
			{
				if (this.GoalPositions.Count == 1 && this.PosBeforeMoving.Equals(this.GoalPositions[0]))
				{
					this.ClearGoalPositions();
					return;
				}
				this.MovePawnToGoalPositions(instantMove, speed, dragged);
				this._movePauseDuration = delay;
				this._moveTiming = delay > 0f;
			}
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0005DBDB File Offset: 0x0005BDDB
		public void SetPlayerOne(bool playerOne)
		{
			this.PlayerOne = playerOne;
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0005DBE4 File Offset: 0x0005BDE4
		public void ClearGoalPositions()
		{
			this.MovingToDifferentTile = false;
			this.GoalPositions.Clear();
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0005DBF8 File Offset: 0x0005BDF8
		public void UpdatePawnPosition()
		{
			this.PosBeforeMoving = this.Entity.GlobalPosition;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0005DC0B File Offset: 0x0005BE0B
		public void PlayPawnSelectSound()
		{
			Mission.Current.MakeSound(PawnBase.PawnSelectSoundCodeID, this.CurrentPos, true, false, -1, -1);
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x0005DC26 File Offset: 0x0005BE26
		private void PlayPawnTapSound()
		{
			Mission.Current.MakeSound(PawnBase.PawnTapSoundCodeID, this.CurrentPos, true, false, -1, -1);
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0005DC41 File Offset: 0x0005BE41
		private void PlayPawnRemovedTapSound()
		{
			Mission.Current.MakeSound(PawnBase.PawnRemoveSoundCodeID, this.CurrentPos, true, false, -1, -1);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0005DC5C File Offset: 0x0005BE5C
		private void PlayPawnMoveSound()
		{
			Mission.Current.MakeSound(PawnBase.PawnMoveSoundCodeID, this.CurrentPos, true, false, -1, -1);
		}

		// Token: 0x0400056E RID: 1390
		public Action<PawnBase, Vec3, Vec3> OnArrivedIntermediateGoalPosition;

		// Token: 0x0400056F RID: 1391
		public Action<PawnBase, Vec3, Vec3> OnArrivedFinalGoalPosition;

		// Token: 0x04000570 RID: 1392
		protected Vec3 PosBeforeMovingBase;

		// Token: 0x04000571 RID: 1393
		private int _currentGoalPos;

		// Token: 0x04000572 RID: 1394
		private float _dtCounter;

		// Token: 0x04000573 RID: 1395
		private float _movePauseDuration;

		// Token: 0x04000574 RID: 1396
		private float _movePauseTimer;

		// Token: 0x04000575 RID: 1397
		private float _moveSpeed;

		// Token: 0x04000576 RID: 1398
		private bool _moveTiming;

		// Token: 0x04000577 RID: 1399
		private bool _dragged;

		// Token: 0x04000578 RID: 1400
		private bool _freePathToDestination;
	}
}
