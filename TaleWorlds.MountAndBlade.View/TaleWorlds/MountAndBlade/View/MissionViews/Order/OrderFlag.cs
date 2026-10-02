using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screens;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Order
{
	// Token: 0x020000A9 RID: 169
	public class OrderFlag
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x000288B5 File Offset: 0x00026AB5
		// (set) Token: 0x060005A9 RID: 1449 RVA: 0x000288BD File Offset: 0x00026ABD
		public IOrderable FocusedOrderableObject { get; private set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x000288C6 File Offset: 0x00026AC6
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x000288CE File Offset: 0x00026ACE
		public int LatestUpdateFrameNo { get; private set; }

		// Token: 0x060005AC RID: 1452 RVA: 0x000288D8 File Offset: 0x00026AD8
		public OrderFlag(Mission mission, MissionScreen missionScreen, float flagScale = 10f)
		{
			this._mission = mission;
			this._missionScreen = missionScreen;
			this._entity = GameEntity.CreateEmpty(this._mission.Scene, true, true, true);
			this._flag = GameEntity.CreateEmpty(this._mission.Scene, true, true, true);
			this._gear = GameEntity.CreateEmpty(this._mission.Scene, true, true, true);
			this._arrow = GameEntity.CreateEmpty(this._mission.Scene, true, true, true);
			this._width = GameEntity.CreateEmpty(this._mission.Scene, true, true, true);
			this._attack = GameEntity.CreateEmpty(this._mission.Scene, true, true, true);
			this._flagUnavailable = GameEntity.CreateEmpty(this._mission.Scene, true, true, true);
			this._widthLeft = GameEntity.CreateEmpty(this._mission.Scene, true, true, true);
			this._widthRight = GameEntity.CreateEmpty(this._mission.Scene, true, true, true);
			this._entity.EntityFlags |= EntityFlags.NotAffectedBySeason;
			this._flag.EntityFlags |= EntityFlags.NotAffectedBySeason;
			this._gear.EntityFlags |= EntityFlags.NotAffectedBySeason;
			this._arrow.EntityFlags |= EntityFlags.NotAffectedBySeason;
			this._width.EntityFlags |= EntityFlags.NotAffectedBySeason;
			this._attack.EntityFlags |= EntityFlags.NotAffectedBySeason;
			this._flagUnavailable.EntityFlags |= EntityFlags.NotAffectedBySeason;
			this._widthLeft.EntityFlags |= EntityFlags.NotAffectedBySeason;
			this._widthRight.EntityFlags |= EntityFlags.NotAffectedBySeason;
			this._flag.AddComponent(MetaMesh.GetCopy("order_flag_a", true, false));
			MatrixFrame frame = this._flag.GetFrame();
			Vec3 vec = Vec3.One * flagScale;
			frame.Scale(in vec);
			this._flag.SetFrame(ref frame, true);
			this._gear.AddComponent(MetaMesh.GetCopy("order_gear", true, false));
			MatrixFrame frame2 = this._gear.GetFrame();
			vec = Vec3.One * flagScale;
			frame2.Scale(in vec);
			this._gear.SetFrame(ref frame2, true);
			this._arrow.AddComponent(MetaMesh.GetCopy("order_arrow_a", true, false));
			this._widthLeft.AddComponent(MetaMesh.GetCopy("order_arrow_a", true, false));
			this._widthRight.AddComponent(MetaMesh.GetCopy("order_arrow_a", true, false));
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			matrixFrame.rotation.RotateAboutUp(-1.5707964f);
			this._widthLeft.SetFrame(ref matrixFrame, true);
			matrixFrame = MatrixFrame.Identity;
			matrixFrame.rotation.RotateAboutUp(1.5707964f);
			this._widthRight.SetFrame(ref matrixFrame, true);
			this._width.AddChild(this._widthLeft, false);
			this._width.AddChild(this._widthRight, false);
			MetaMesh copy = MetaMesh.GetCopy("destroy_icon", true, false);
			copy.RecomputeBoundingBox(true);
			MatrixFrame frame3 = copy.Frame;
			vec = new Vec3(0.15f, 0.15f, 0.15f, -1f);
			frame3.Scale(in vec);
			frame3.Elevate(10f);
			copy.Frame = frame3;
			this._attack.AddMultiMesh(copy, true);
			this._flagUnavailable.AddComponent(MetaMesh.GetCopy("order_unavailable", true, false));
			this._entity.AddChild(this._flag, false);
			this._entity.AddChild(this._gear, false);
			this._entity.AddChild(this._arrow, false);
			this._entity.AddChild(this._width, false);
			this._entity.AddChild(this._attack, false);
			this._entity.AddChild(this._flagUnavailable, false);
			this._flag.SetVisibilityExcludeParents(false);
			this._gear.SetVisibilityExcludeParents(false);
			this._arrow.SetVisibilityExcludeParents(false);
			this._width.SetVisibilityExcludeParents(false);
			this._attack.SetVisibilityExcludeParents(false);
			this._flagUnavailable.SetVisibilityExcludeParents(false);
			this.SetActiveVisualEntity(this._flag);
			BoundingBox boundingBox = this._arrow.GetMetaMesh(0).GetBoundingBox();
			this._arrowLength = boundingBox.max.y - boundingBox.min.y;
			bool flag;
			this.UpdateFrame(out flag, false, Vec3.Invalid);
			this._orderablesWithInteractionArea = this._mission.MissionObjects.OfType<IOrderableWithInteractionArea>();
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00028D80 File Offset: 0x00026F80
		public void Tick(float dt)
		{
			this.FocusedOrderableObject = null;
			WeakGameEntity weakGameEntity = WeakGameEntity.Invalid;
			bool flag = false;
			bool flag2 = !this._mission.IsNavalBattle && !this._mission.IsNavalRaidBattle;
			Vec3 invalid = Vec3.Invalid;
			if (flag2)
			{
				weakGameEntity = this.GetCollidedEntity(out invalid);
				if (weakGameEntity.IsValid)
				{
					BattleSideEnum side = Mission.Current.PlayerTeam.Side;
					IOrderable orderable = (IOrderable)weakGameEntity.GetScriptComponents().First<ScriptComponentBehavior>(delegate(ScriptComponentBehavior sc)
					{
						IOrderable orderable2;
						return (orderable2 = sc as IOrderable) != null && orderable2.GetOrder(side) > OrderType.None;
					});
					if (orderable.GetOrder(side) != OrderType.None)
					{
						this.FocusedOrderableObject = orderable;
					}
				}
			}
			this.UpdateFrame(out flag, weakGameEntity.IsValid, invalid);
			this.LatestUpdateFrameNo = Utilities.EngineFrameNo;
			if (!this.IsVisible)
			{
				return;
			}
			if (flag2 && this.FocusedOrderableObject == null)
			{
				this.FocusedOrderableObject = this._orderablesWithInteractionArea.FirstOrDefault<IOrderableWithInteractionArea>((IOrderableWithInteractionArea o) => ((ScriptComponentBehavior)o).GameEntity.IsVisibleIncludeParents() && o.IsPointInsideInteractionArea(this.Position));
				ScriptComponentBehavior scriptComponentBehavior;
				if ((scriptComponentBehavior = this.FocusedOrderableObject as ScriptComponentBehavior) != null && scriptComponentBehavior.GameEntity.Scene == null)
				{
					this.FocusedOrderableObject = null;
				}
			}
			this.UpdateCurrentMesh(flag);
			if (this._activeVisualEntity == this._flag || this._activeVisualEntity == this._flagUnavailable)
			{
				MatrixFrame frame = this._flag.GetFrame();
				float num = MathF.Sin(MBCommon.GetApplicationTime() * 2f) + 1f;
				num *= 0.25f;
				frame.origin.z = num;
				this._flag.SetFrame(ref frame, true);
				this._flagUnavailable.SetFrame(ref frame, true);
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00028F30 File Offset: 0x00027130
		private void SetActiveVisualEntity(GameEntity entity)
		{
			this._activeVisualEntity = entity;
			this._flag.SetVisibilityExcludeParents(false);
			this._gear.SetVisibilityExcludeParents(false);
			this._arrow.SetVisibilityExcludeParents(false);
			this._width.SetVisibilityExcludeParents(false);
			this._attack.SetVisibilityExcludeParents(false);
			this._flagUnavailable.SetVisibilityExcludeParents(false);
			this._activeVisualEntity.SetVisibilityExcludeParents(true);
			if (this._activeVisualEntity == this._arrow || this._activeVisualEntity == this._flagUnavailable)
			{
				this._flag.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00028FCC File Offset: 0x000271CC
		private void UpdateCurrentMesh(bool isOnValidGround)
		{
			if (this.FocusedOrderableObject != null)
			{
				BattleSideEnum side = Mission.Current.PlayerTeam.Side;
				if (this.FocusedOrderableObject.GetOrder(side) == OrderType.AttackEntity)
				{
					this.SetActiveVisualEntity(this._attack);
					return;
				}
				OrderType order = this.FocusedOrderableObject.GetOrder(side);
				if (order == OrderType.Use || order == OrderType.FollowEntity)
				{
					this.SetActiveVisualEntity(this._gear);
					return;
				}
			}
			if (this._isArrowVisible)
			{
				this.SetActiveVisualEntity(this._arrow);
				return;
			}
			if (this._isWidthVisible)
			{
				this.SetActiveVisualEntity(this._width);
				return;
			}
			this.SetActiveVisualEntity(isOnValidGround ? this._flag : this._flagUnavailable);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00029072 File Offset: 0x00027272
		public void SetArrowVisibility(bool isVisible, Vec2 arrowDirection)
		{
			this._isArrowVisible = isVisible;
			this._arrowDirection = arrowDirection;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00029084 File Offset: 0x00027284
		protected virtual Vec3 GetFlagPosition(out bool isOnValidGround, bool checkForTargetEntity, Vec3 targetCollisionPoint)
		{
			Vec3 vec;
			Vec3 vec2;
			if (this._missionScreen.GetProjectedMousePositionOnGround(out vec, out vec2, BodyFlags.BodyOwnerFlora, true))
			{
				if (!this.IsVisible)
				{
					isOnValidGround = false;
				}
				else if (checkForTargetEntity)
				{
					if (targetCollisionPoint.IsValid)
					{
						vec = targetCollisionPoint;
					}
					else
					{
						Debug.FailedAssert("Collision point for target entity is invalid.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.View\\MissionViews\\Order\\OrderFlag.cs", "GetFlagPosition", 349);
					}
					WorldPosition worldPosition = new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, vec, false);
					isOnValidGround = Mission.Current.IsOrderPositionAvailable(in worldPosition, Mission.Current.PlayerTeam);
				}
				else
				{
					WorldPosition worldPosition2 = new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, vec, false);
					isOnValidGround = this.IsPositionOnValidGround(worldPosition2);
				}
				return vec;
			}
			isOnValidGround = false;
			return new Vec3(0f, 0f, -100000f, -1f);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00029154 File Offset: 0x00027354
		protected virtual void UpdateFrame(out bool isOnValidGround, bool checkForTargetEntity, Vec3 targetCollisionPoint)
		{
			MissionScreen missionScreen = this._missionScreen;
			if (((missionScreen != null) ? missionScreen.SceneView : null) == null)
			{
				isOnValidGround = false;
				return;
			}
			Vec3 flagPosition = this.GetFlagPosition(out isOnValidGround, checkForTargetEntity, targetCollisionPoint);
			if (!flagPosition.IsValid)
			{
				return;
			}
			this.Position = flagPosition;
			Vec3 vec;
			Vec3 vec2;
			this._missionScreen.ScreenPointToWorldRay(Vec2.One * 0.5f, out vec, out vec2);
			float num;
			if (this._missionScreen.LastFollowedAgent != null)
			{
				vec2 = vec - this.Position;
				num = vec2.AsVec2.RotationInRadians;
			}
			else
			{
				num = this._missionScreen.CombatCamera.Frame.rotation.f.RotationZ;
			}
			float num2 = num;
			MatrixFrame frame = this._entity.GetFrame();
			frame.rotation = Mat3.Identity;
			frame.rotation.RotateAboutUp(num2);
			this._entity.SetFrame(ref frame, true);
			if (this._isArrowVisible)
			{
				num2 = this._arrowDirection.RotationInRadians;
				Mat3 identity = Mat3.Identity;
				identity.RotateAboutUp(num2);
				MatrixFrame identity2 = MatrixFrame.Identity;
				identity2.rotation = frame.rotation.TransformToLocal(in identity);
				identity2.Advance(-this._arrowLength);
				this._arrow.SetFrame(ref identity2, true);
			}
			if (this._isWidthVisible)
			{
				this._widthLeft.SetLocalPosition(Vec3.Side * (this._customWidth * 0.5f - 0f));
				this._widthRight.SetLocalPosition(Vec3.Side * (this._customWidth * -0.5f + 0f));
				this._widthLeft.SetLocalPosition(Vec3.Side * (this._customWidth * 0.5f - this._arrowLength));
				this._widthRight.SetLocalPosition(Vec3.Side * (this._customWidth * -0.5f + this._arrowLength));
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00029341 File Offset: 0x00027541
		public virtual bool IsPositionOnValidGround(WorldPosition worldPosition)
		{
			return Mission.Current.IsFormationUnitPositionAvailable(ref worldPosition, Mission.Current.PlayerTeam);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00029359 File Offset: 0x00027559
		public static bool IsOrderPositionValid(WorldPosition orderPosition)
		{
			return Mission.Current.IsOrderPositionAvailable(in orderPosition, Mission.Current.PlayerTeam);
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00029371 File Offset: 0x00027571
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x00029380 File Offset: 0x00027580
		public Vec3 Position
		{
			get
			{
				return this._entity.GlobalPosition;
			}
			private set
			{
				MatrixFrame frame = this._entity.GetFrame();
				frame.origin = value;
				this._entity.SetFrame(ref frame, true);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x000293AF File Offset: 0x000275AF
		public MatrixFrame Frame
		{
			get
			{
				return this._entity.GetGlobalFrame();
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x000293BC File Offset: 0x000275BC
		// (set) Token: 0x060005B9 RID: 1465 RVA: 0x000293C9 File Offset: 0x000275C9
		public bool IsVisible
		{
			get
			{
				return this._entity.IsVisibleIncludeParents();
			}
			set
			{
				this._entity.SetVisibilityExcludeParents(value);
				if (!value)
				{
					this.FocusedOrderableObject = null;
				}
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x000293E4 File Offset: 0x000275E4
		private WeakGameEntity GetCollidedEntity(out Vec3 closestPoint)
		{
			Vec2 vec = ((this._mission.Mode == MissionMode.Deployment) ? Input.MousePositionRanged : new Vec2(0.5f, 0.5f));
			Vec3 eyeGlobalPosition;
			Vec3 vec2;
			this._missionScreen.ScreenPointToWorldRay(vec, out eyeGlobalPosition, out vec2);
			Vec3 vec3 = (vec2 - eyeGlobalPosition).NormalizedCopy();
			vec2 = eyeGlobalPosition + vec3 * 10000f;
			eyeGlobalPosition = Agent.Main.GetEyeGlobalPosition();
			float num;
			WeakGameEntity parent;
			this._mission.Scene.RayCastForClosestEntityOrTerrain(eyeGlobalPosition, vec2, out num, out closestPoint, out parent, 0.3f, BodyFlags.Disabled | BodyFlags.AILimiter | BodyFlags.Barrier | BodyFlags.Barrier3D | BodyFlags.Ragdoll | BodyFlags.RagdollLimiter | BodyFlags.DoNotCollideWithRaycast | BodyFlags.BodyOwnerFlora);
			while (parent.IsValid)
			{
				if (parent.GetScriptComponents().Any<ScriptComponentBehavior>(delegate(ScriptComponentBehavior sc)
				{
					IOrderable orderable;
					return (orderable = sc as IOrderable) != null && orderable.GetOrder(Mission.Current.PlayerTeam.Side) > OrderType.None;
				}))
				{
					break;
				}
				parent = parent.Parent;
			}
			return parent;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x000294B9 File Offset: 0x000276B9
		public void SetWidthVisibility(bool isVisible, float width)
		{
			this._isWidthVisible = isVisible;
			this._customWidth = width;
		}

		// Token: 0x0400030E RID: 782
		private readonly GameEntity _entity;

		// Token: 0x0400030F RID: 783
		private readonly GameEntity _flag;

		// Token: 0x04000310 RID: 784
		private readonly GameEntity _gear;

		// Token: 0x04000311 RID: 785
		private readonly GameEntity _arrow;

		// Token: 0x04000312 RID: 786
		private readonly GameEntity _width;

		// Token: 0x04000313 RID: 787
		private readonly GameEntity _attack;

		// Token: 0x04000314 RID: 788
		private readonly GameEntity _flagUnavailable;

		// Token: 0x04000315 RID: 789
		private readonly GameEntity _widthLeft;

		// Token: 0x04000316 RID: 790
		private readonly GameEntity _widthRight;

		// Token: 0x04000317 RID: 791
		public bool IsTroop = true;

		// Token: 0x04000318 RID: 792
		private bool _isWidthVisible;

		// Token: 0x04000319 RID: 793
		private float _customWidth;

		// Token: 0x0400031A RID: 794
		private GameEntity _activeVisualEntity;

		// Token: 0x0400031C RID: 796
		protected readonly IEnumerable<IOrderableWithInteractionArea> _orderablesWithInteractionArea;

		// Token: 0x0400031D RID: 797
		protected readonly Mission _mission;

		// Token: 0x0400031E RID: 798
		protected readonly MissionScreen _missionScreen;

		// Token: 0x0400031F RID: 799
		private readonly float _arrowLength;

		// Token: 0x04000320 RID: 800
		private bool _isArrowVisible;

		// Token: 0x04000321 RID: 801
		private Vec2 _arrowDirection;
	}
}
