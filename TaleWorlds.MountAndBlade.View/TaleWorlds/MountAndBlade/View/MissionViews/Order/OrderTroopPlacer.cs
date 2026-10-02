using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Order
{
	// Token: 0x020000AA RID: 170
	public class OrderTroopPlacer : MissionView
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x000294FC File Offset: 0x000276FC
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x00029504 File Offset: 0x00027704
		public bool SuspendTroopPlacer
		{
			get
			{
				return this._suspendTroopPlacer;
			}
			set
			{
				this._suspendTroopPlacer = value;
				if (value)
				{
					this.HideOrderPositionEntities();
				}
				else
				{
					this._formationDrawingStartingPosition = null;
				}
				this.Reset();
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x0002952A File Offset: 0x0002772A
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x00029532 File Offset: 0x00027732
		public OrderFlag OrderFlag { get; private set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0002953B File Offset: 0x0002773B
		private Team _playerTeam
		{
			get
			{
				return base.Mission.PlayerTeam;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00029548 File Offset: 0x00027748
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x00029550 File Offset: 0x00027750
		private protected OrderTroopPlacer.CursorState ActiveCursorState { protected get; private set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00029559 File Offset: 0x00027759
		protected OrderController OrderController
		{
			get
			{
				if (this._orderController != null)
				{
					return this._orderController;
				}
				return Mission.Current.PlayerTeam.PlayerOrderController;
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00029579 File Offset: 0x00027779
		public OrderTroopPlacer(OrderController orderController)
		{
			this._orderController = orderController;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00029588 File Offset: 0x00027788
		protected virtual OrderFlag CreateOrderFlag()
		{
			return new OrderFlag(base.Mission, base.MissionScreen, 10f);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000295A0 File Offset: 0x000277A0
		protected virtual bool CanUpdate()
		{
			return this.OrderController.SelectedFormations.Count > 0;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000295B5 File Offset: 0x000277B5
		protected virtual bool HasSelectedFormations()
		{
			return this.OrderController.SelectedFormations.Count > 0;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000295CC File Offset: 0x000277CC
		protected virtual OrderTroopPlacer.CursorState GetCursorState()
		{
			OrderTroopPlacer.CursorState cursorState = OrderTroopPlacer.CursorState.Invisible;
			if (this.HasSelectedFormations())
			{
				WorldPosition worldPosition;
				float num;
				WeakGameEntity weakGameEntity;
				if (!this.TryGetScreenMiddleToWorldPosition(out worldPosition, out num, out weakGameEntity))
				{
					num = 1000f;
				}
				if (cursorState == OrderTroopPlacer.CursorState.Invisible && num < 1000f)
				{
					if (!this._formationDrawingMode && !weakGameEntity.IsValid)
					{
						for (int i = 0; i < this._orderRotationEntities.Count; i++)
						{
							GameEntity gameEntity = this._orderRotationEntities[i];
							if (gameEntity.IsVisibleIncludeParents() && weakGameEntity == gameEntity)
							{
								this._mouseOverFormation = this.OrderController.SelectedFormations.ElementAt<Formation>(i / 2);
								this._mouseOverDirection = 1 - (i & 1);
								cursorState = OrderTroopPlacer.CursorState.Rotation;
								break;
							}
						}
					}
					if (cursorState == OrderTroopPlacer.CursorState.Invisible)
					{
						OrderFlag orderFlag = base.MissionScreen.OrderFlag;
						if (((orderFlag != null) ? orderFlag.FocusedOrderableObject : null) != null)
						{
							cursorState = OrderTroopPlacer.CursorState.OrderableEntity;
						}
					}
					if (cursorState == OrderTroopPlacer.CursorState.Invisible)
					{
						cursorState = this.GetGroundOrNormalCursor();
					}
				}
			}
			if (cursorState != OrderTroopPlacer.CursorState.Ground && cursorState != OrderTroopPlacer.CursorState.Rotation)
			{
				this._mouseOverDirection = 0;
			}
			return cursorState;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000296BA File Offset: 0x000278BA
		protected virtual Vec3 GetGroundedVec3(WorldPosition worldPosition)
		{
			return worldPosition.GetGroundVec3();
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000296C4 File Offset: 0x000278C4
		protected virtual bool TryGetScreenMiddleToWorldPosition(out WorldPosition worldPosition, out float collisionDistance, out WeakGameEntity collidedEntity)
		{
			Vec3 vec;
			Vec3 vec2;
			base.MissionScreen.ScreenPointToWorldRay(this.GetScreenPoint(), out vec, out vec2);
			float num;
			WeakGameEntity weakGameEntity;
			if (base.Mission.Scene.RayCastForClosestEntityOrTerrain(vec, vec2, out num, out weakGameEntity, 0.3f, BodyFlags.Disabled | BodyFlags.AILimiter | BodyFlags.Barrier | BodyFlags.Barrier3D | BodyFlags.Ragdoll | BodyFlags.RagdollLimiter | BodyFlags.DoNotCollideWithRaycast | BodyFlags.BodyOwnerFlora))
			{
				Vec3 vec3 = vec2 - vec;
				vec3.Normalize();
				collisionDistance = num;
				collidedEntity = weakGameEntity;
				worldPosition = new WorldPosition(base.Mission.Scene, UIntPtr.Zero, vec + vec3 * collisionDistance, false);
				return true;
			}
			worldPosition = WorldPosition.Invalid;
			collisionDistance = 0f;
			collidedEntity = WeakGameEntity.Invalid;
			return false;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00029770 File Offset: 0x00027970
		protected bool TryGetScreenMiddleToWorldPosition(out WorldPosition worldPosition, out float collisionDistance)
		{
			WeakGameEntity weakGameEntity;
			return this.TryGetScreenMiddleToWorldPosition(out worldPosition, out collisionDistance, out weakGameEntity);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00029788 File Offset: 0x00027988
		protected bool TryGetScreenMiddleToWorldPosition(out WorldPosition worldPosition, out WeakGameEntity collidedEntity)
		{
			float num;
			return this.TryGetScreenMiddleToWorldPosition(out worldPosition, out num, out collidedEntity);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000297A0 File Offset: 0x000279A0
		protected bool TryGetScreenMiddleToWorldPosition(out WorldPosition worldPosition)
		{
			float num;
			WeakGameEntity weakGameEntity;
			return this.TryGetScreenMiddleToWorldPosition(out worldPosition, out num, out weakGameEntity);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000297B8 File Offset: 0x000279B8
		protected Vec2 GetScreenPoint()
		{
			if (!base.MissionScreen.MouseVisible)
			{
				return new Vec2(0.5f, 0.5f) + this._deltaMousePosition;
			}
			return base.Input.GetMousePositionRanged() + this._deltaMousePosition;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000297F8 File Offset: 0x000279F8
		public OrderTroopPlacer.CursorState GetGroundOrNormalCursor()
		{
			if (!this._formationDrawingMode)
			{
				return OrderTroopPlacer.CursorState.Normal;
			}
			return OrderTroopPlacer.CursorState.Ground;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00029808 File Offset: 0x00027A08
		public override void AfterStart()
		{
			base.AfterStart();
			this.OrderFlag = this.CreateOrderFlag();
			this._formationDrawingStartingPosition = null;
			this._formationDrawingStartingPointOfMouse = null;
			this._formationDrawingStartingTime = null;
			this._orderRotationEntities = new List<GameEntity>();
			this._orderPositionEntities = new List<GameEntity>();
			this.formationDrawTimer = new Timer(MBCommon.GetApplicationTime(), 0.033333335f, true);
			this._widthEntityLeft = GameEntity.CreateEmpty(base.Mission.Scene, true, true, true);
			this._widthEntityLeft.AddComponent(MetaMesh.GetCopy("order_arrow_a", true, false));
			this._widthEntityLeft.SetVisibilityExcludeParents(false);
			this._widthEntityRight = GameEntity.CreateEmpty(base.Mission.Scene, true, true, true);
			this._widthEntityRight.AddComponent(MetaMesh.GetCopy("order_arrow_a", true, false));
			this._widthEntityRight.SetVisibilityExcludeParents(false);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000298F0 File Offset: 0x00027AF0
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (!this._initialized)
			{
				MissionPeer missionPeer = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>() : null);
				if (base.Mission.PlayerTeam != null || (missionPeer != null && (missionPeer.Team == base.Mission.AttackerTeam || missionPeer.Team == base.Mission.DefenderTeam)))
				{
					this._initialized = true;
				}
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0002995E File Offset: 0x00027B5E
		public void RestrictOrdersToDeploymentBoundaries(bool enabled)
		{
			this._restrictOrdersToDeploymentBoundaries = enabled;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00029968 File Offset: 0x00027B68
		private void UpdateFormationDrawingForFacingOrder(bool giveOrder)
		{
			this._isDrawnThisFrame = true;
			Vec3 vec = base.MissionScreen.GetOrderFlagPosition();
			Vec2 asVec = vec.AsVec2;
			Vec2 orderLookAtDirection = OrderController.GetOrderLookAtDirection(this.OrderController.SelectedFormations, asVec);
			List<WorldPosition> list;
			this.OrderController.SimulateNewFacingOrder(orderLookAtDirection, out list);
			int num = 0;
			this.HideOrderPositionEntities();
			foreach (WorldPosition worldPosition in list)
			{
				int num2 = num;
				vec = this.GetGroundedVec3(worldPosition);
				this.AddOrderPositionEntity(num2, in vec, giveOrder, -1f);
				num++;
			}
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00029A14 File Offset: 0x00027C14
		private void UpdateFormationDrawingForDestination(bool giveOrder)
		{
			this._isDrawnThisFrame = true;
			List<WorldPosition> list;
			this.OrderController.SimulateDestinationFrames(out list, 3f);
			int num = 0;
			this.HideOrderPositionEntities();
			foreach (WorldPosition worldPosition in list)
			{
				int num2 = num;
				Vec3 groundedVec = this.GetGroundedVec3(worldPosition);
				this.AddOrderPositionEntity(num2, in groundedVec, giveOrder, 0.7f);
				num++;
			}
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00029A98 File Offset: 0x00027C98
		private void UpdateFormationDrawingForFormingOrder(bool giveOrder)
		{
			this._isDrawnThisFrame = true;
			MatrixFrame orderFlagFrame = base.MissionScreen.GetOrderFlagFrame();
			Vec3 origin = orderFlagFrame.origin;
			Vec2 asVec = orderFlagFrame.rotation.f.AsVec2;
			float orderFormCustomWidth = OrderController.GetOrderFormCustomWidth(this.OrderController.SelectedFormations, origin);
			List<WorldPosition> list;
			this.OrderController.SimulateNewCustomWidthOrder(orderFormCustomWidth, out list);
			Formation formation = this.OrderController.SelectedFormations.MaxBy<Formation, int>((Formation f) => f.CountOfUnits);
			int num = 0;
			this.HideOrderPositionEntities();
			foreach (WorldPosition worldPosition in list)
			{
				worldPosition.GetNavMesh();
				int num2 = num;
				Vec3 vec = this.GetGroundedVec3(worldPosition);
				this.AddOrderPositionEntity(num2, in vec, giveOrder, -1f);
				num++;
			}
			float unitDiameter = formation.UnitDiameter;
			float interval = formation.Interval;
			int num3 = MathF.Max(0, (int)((orderFormCustomWidth - unitDiameter) / (interval + unitDiameter) + 1E-05f)) + 1;
			float num4 = (float)(num3 - 1) * (interval + unitDiameter);
			for (int i = 0; i < num3; i++)
			{
				Vec2 vec2 = new Vec2((float)i * (interval + unitDiameter) - num4 / 2f, 0f);
				Vec2 vec3 = asVec.TransformToParentUnitF(vec2);
				WorldPosition worldPosition2 = new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, origin, false);
				worldPosition2.SetVec2(worldPosition2.AsVec2 + vec3);
				int num5 = num++;
				Vec3 vec = this.GetGroundedVec3(worldPosition2);
				this.AddOrderPositionEntity(num5, in vec, false, -1f);
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00029C54 File Offset: 0x00027E54
		public void UpdateFormationDrawing(bool giveOrder)
		{
			this._isDrawnThisFrame = true;
			this.HideOrderPositionEntities();
			if (this._formationDrawingStartingPosition == null)
			{
				return;
			}
			WorldPosition worldPosition = WorldPosition.Invalid;
			bool flag = false;
			if (base.MissionScreen.MouseVisible && this._formationDrawingStartingPointOfMouse != null)
			{
				Vec2 vec = this._formationDrawingStartingPointOfMouse.Value - base.Input.GetMousePositionPixel();
				if (MathF.Abs(vec.x) < 10f && MathF.Abs(vec.y) < 10f)
				{
					flag = true;
					worldPosition = this._formationDrawingStartingPosition.Value;
				}
			}
			if (base.MissionScreen.MouseVisible && this._formationDrawingStartingTime != null && base.Mission.CurrentTime - this._formationDrawingStartingTime.Value < 0.3f)
			{
				flag = true;
				worldPosition = this._formationDrawingStartingPosition.Value;
			}
			if (!flag)
			{
				WorldPosition worldPosition2;
				if (!this.TryGetScreenMiddleToWorldPosition(out worldPosition2))
				{
					return;
				}
				worldPosition = worldPosition2;
			}
			WorldPosition worldPosition3;
			if (this._mouseOverDirection == 1)
			{
				worldPosition3 = worldPosition;
				worldPosition = this._formationDrawingStartingPosition.Value;
			}
			else
			{
				worldPosition3 = this._formationDrawingStartingPosition.Value;
			}
			if (!this.OrderFlag.IsPositionOnValidGround(worldPosition3))
			{
				return;
			}
			Vec2 vec2;
			if (this._restrictOrdersToDeploymentBoundaries && base.Mission.DeploymentPlan.HasDeploymentBoundaries(base.Mission.PlayerTeam))
			{
				IMissionDeploymentPlan deploymentPlan = base.Mission.DeploymentPlan;
				Team playerTeam = base.Mission.PlayerTeam;
				vec2 = worldPosition3.AsVec2;
				if (!deploymentPlan.IsPositionInsideDeploymentBoundaries(playerTeam, in vec2))
				{
					return;
				}
			}
			bool flag2 = !base.DebugInput.IsControlDown();
			this.UpdateFormationDrawingForMovementOrder(giveOrder, worldPosition3, worldPosition, flag2);
			Vec2 deltaMousePosition = this._deltaMousePosition;
			float num = 1f;
			vec2 = base.Input.GetMousePositionRanged() - this._lastMousePosition;
			this._deltaMousePosition = deltaMousePosition * MathF.Max(num - vec2.Length * 10f, 0f);
			this._lastMousePosition = base.Input.GetMousePositionRanged();
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00029E3C File Offset: 0x0002803C
		private void UpdateFormationDrawingForMovementOrder(bool giveOrder, WorldPosition formationRealStartingPosition, WorldPosition formationRealEndingPosition, bool isFormationLayoutVertical)
		{
			this._isDrawnThisFrame = true;
			List<WorldPosition> list;
			this.OrderController.SimulateNewOrderWithPositionAndDirection(formationRealStartingPosition, formationRealEndingPosition, out list, isFormationLayoutVertical);
			if (giveOrder)
			{
				if (!isFormationLayoutVertical)
				{
					this.OrderController.SetOrderWithTwoPositions(OrderType.MoveToLineSegmentWithHorizontalLayout, formationRealStartingPosition, formationRealEndingPosition);
				}
				else
				{
					this.OrderController.SetOrderWithTwoPositions(OrderType.MoveToLineSegment, formationRealStartingPosition, formationRealEndingPosition);
				}
			}
			int num = 0;
			foreach (WorldPosition worldPosition in list)
			{
				int num2 = num;
				Vec3 groundedVec = this.GetGroundedVec3(worldPosition);
				this.AddOrderPositionEntity(num2, in groundedVec, giveOrder, -1f);
				num++;
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00029EE0 File Offset: 0x000280E0
		private void HandleMouseDown()
		{
			if (this.HasSelectedFormations())
			{
				switch (this.ActiveCursorState)
				{
				case OrderTroopPlacer.CursorState.Invisible:
				case OrderTroopPlacer.CursorState.Ground:
					break;
				case OrderTroopPlacer.CursorState.Normal:
				{
					this._formationDrawingMode = true;
					WorldPosition worldPosition;
					if (this.TryGetScreenMiddleToWorldPosition(out worldPosition))
					{
						this._formationDrawingStartingPosition = new WorldPosition?(worldPosition);
						this._formationDrawingStartingPointOfMouse = new Vec2?(base.Input.GetMousePositionPixel());
						this._formationDrawingStartingTime = new float?(base.Mission.CurrentTime);
						return;
					}
					this._formationDrawingStartingPosition = null;
					this._formationDrawingStartingPointOfMouse = null;
					this._formationDrawingStartingTime = null;
					return;
				}
				case OrderTroopPlacer.CursorState.Rotation:
					if (this._mouseOverFormation.CountOfUnits > 0)
					{
						this.HideNonSelectedOrderRotationEntities(this._mouseOverFormation);
						this.OrderController.ClearSelectedFormations();
						this.OrderController.SelectFormation(this._mouseOverFormation);
						this._formationDrawingMode = true;
						WorldPosition worldPosition2 = this._mouseOverFormation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
						Vec2 direction = this._mouseOverFormation.Direction;
						direction.RotateCCW(-1.5707964f);
						this._formationDrawingStartingPosition = new WorldPosition?(worldPosition2);
						this._formationDrawingStartingPosition.Value.SetVec2(this._formationDrawingStartingPosition.Value.AsVec2 + direction * ((this._mouseOverDirection == 1) ? 0.5f : (-0.5f)) * this._mouseOverFormation.Width);
						WorldPosition worldPosition3 = worldPosition2;
						worldPosition3.SetVec2(worldPosition3.AsVec2 + direction * ((this._mouseOverDirection == 1) ? (-0.5f) : 0.5f) * this._mouseOverFormation.Width);
						Vec2 vec = base.MissionScreen.SceneView.WorldPointToScreenPoint(this.GetGroundedVec3(worldPosition3));
						Vec2 screenPoint = this.GetScreenPoint();
						this._deltaMousePosition = vec - screenPoint;
						this._lastMousePosition = base.Input.GetMousePositionRanged();
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0002A0D4 File Offset: 0x000282D4
		private void HandleMouseUp()
		{
			if (this.ActiveCursorState == OrderTroopPlacer.CursorState.Ground)
			{
				if (this.IsDrawingFacing || this._wasDrawingFacing)
				{
					this.UpdateFormationDrawingForFacingOrder(true);
				}
				else if (this.IsDrawingForming || this._wasDrawingForming)
				{
					this.UpdateFormationDrawingForFormingOrder(true);
				}
				else
				{
					this.UpdateFormationDrawing(true);
				}
				if (this.IsDeployment)
				{
					Action onUnitDeployed = this.OnUnitDeployed;
					if (onUnitDeployed != null)
					{
						onUnitDeployed();
					}
					UISoundsHelper.PlayUISound("event:/ui/mission/deploy");
				}
			}
			this._formationDrawingMode = false;
			this._deltaMousePosition = Vec2.Zero;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0002A158 File Offset: 0x00028358
		private void AddOrderPositionEntity(int entityIndex, in Vec3 groundPosition, bool fadeOut, float alpha = -1f)
		{
			while (this._orderPositionEntities.Count <= entityIndex)
			{
				GameEntity gameEntity = GameEntity.CreateEmpty(base.Mission.Scene, true, true, true);
				gameEntity.EntityFlags |= EntityFlags.NotAffectedBySeason;
				MetaMesh copy = MetaMesh.GetCopy("order_flag_small", true, false);
				gameEntity.AddComponent(copy);
				gameEntity.SetVisibilityExcludeParents(false);
				this._orderPositionEntities.Add(gameEntity);
			}
			GameEntity gameEntity2 = this._orderPositionEntities[entityIndex];
			Mat3 identity = Mat3.Identity;
			MatrixFrame matrixFrame = new MatrixFrame(in identity, in groundPosition);
			gameEntity2.SetFrame(ref matrixFrame, true);
			if (alpha != -1f)
			{
				gameEntity2.SetVisibilityExcludeParents(true);
				gameEntity2.SetAlpha(alpha);
				return;
			}
			if (fadeOut)
			{
				gameEntity2.FadeOut(0.3f, false);
				return;
			}
			gameEntity2.FadeIn(true);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0002A218 File Offset: 0x00028418
		private void HideNonSelectedOrderRotationEntities(Formation formation)
		{
			for (int i = 0; i < this._orderRotationEntities.Count; i++)
			{
				GameEntity gameEntity = this._orderRotationEntities[i];
				if (gameEntity == null && gameEntity.IsVisibleIncludeParents() && this.OrderController.SelectedFormations.ElementAt<Formation>(i / 2) != formation)
				{
					gameEntity.SetVisibilityExcludeParents(false);
					gameEntity.BodyFlag |= BodyFlags.Disabled;
				}
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0002A284 File Offset: 0x00028484
		private void HideOrderPositionEntities()
		{
			foreach (GameEntity gameEntity in this._orderPositionEntities)
			{
				gameEntity.HideIfNotFadingOut();
			}
			for (int i = 0; i < this._orderRotationEntities.Count; i++)
			{
				GameEntity gameEntity2 = this._orderRotationEntities[i];
				gameEntity2.SetVisibilityExcludeParents(false);
				gameEntity2.BodyFlag |= BodyFlags.Disabled;
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0002A30C File Offset: 0x0002850C
		[Conditional("DEBUG")]
		private void DebugTick(float dt)
		{
			bool initialized = this._initialized;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0002A318 File Offset: 0x00028518
		private void Reset()
		{
			this._isMouseDown = false;
			this._formationDrawingMode = false;
			this._formationDrawingStartingPosition = null;
			this._formationDrawingStartingPointOfMouse = null;
			this._formationDrawingStartingTime = null;
			this._mouseOverFormation = null;
			this.ActiveCursorState = this.GetCursorState();
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0002A36C File Offset: 0x0002856C
		public override void OnMissionScreenTick(float dt)
		{
			if (!this._initialized)
			{
				return;
			}
			this.ActiveCursorState = this.GetCursorState();
			base.OnMissionScreenTick(dt);
			if (!this.CanUpdate())
			{
				return;
			}
			this._isDrawnThisFrame = false;
			if (this.SuspendTroopPlacer)
			{
				return;
			}
			if (base.Input.IsKeyPressed(InputKey.LeftMouseButton) || base.Input.IsKeyPressed(InputKey.ControllerRTrigger))
			{
				this._isMouseDown = true;
				this.HandleMouseDown();
			}
			if ((base.Input.IsKeyReleased(InputKey.LeftMouseButton) || base.Input.IsKeyReleased(InputKey.ControllerRTrigger)) && this._isMouseDown)
			{
				this._isMouseDown = false;
				this.HandleMouseUp();
			}
			else if ((base.Input.IsKeyDown(InputKey.LeftMouseButton) || base.Input.IsKeyDown(InputKey.ControllerRTrigger)) && this._isMouseDown)
			{
				if (this.formationDrawTimer.Check(MBCommon.GetApplicationTime()) && !this.IsDrawingFacing && !this.IsDrawingForming && this.ActiveCursorState == OrderTroopPlacer.CursorState.Ground && this.GetGroundOrNormalCursor() == OrderTroopPlacer.CursorState.Ground)
				{
					this.UpdateFormationDrawing(false);
				}
			}
			else if (this.IsDrawingForced)
			{
				if (this.formationDrawTimer.Check(MBCommon.GetApplicationTime()))
				{
					this.Reset();
					this.HandleMouseDown();
					this.UpdateFormationDrawing(false);
				}
			}
			else if (this.IsDrawingFacing || this._wasDrawingFacing)
			{
				if (this.IsDrawingFacing)
				{
					this.Reset();
					this.UpdateFormationDrawingForFacingOrder(false);
				}
			}
			else if (this.IsDrawingForming || this._wasDrawingForming)
			{
				if (this.IsDrawingForming)
				{
					this.Reset();
					this.UpdateFormationDrawingForFormingOrder(false);
				}
			}
			else if (this._wasDrawingForced)
			{
				this.Reset();
			}
			else
			{
				this.UpdateFormationDrawingForDestination(false);
			}
			if (!base.Input.IsKeyDown(InputKey.LeftMouseButton) && !base.Input.IsKeyDown(InputKey.ControllerRTrigger) && this._isMouseDown)
			{
				this.Reset();
			}
			foreach (GameEntity gameEntity in this._orderPositionEntities)
			{
				gameEntity.SetPreviousFrameInvalid();
			}
			foreach (GameEntity gameEntity2 in this._orderRotationEntities)
			{
				gameEntity2.SetPreviousFrameInvalid();
			}
			this._wasDrawingForced = this.IsDrawingForced;
			this._wasDrawingFacing = this.IsDrawingFacing;
			this._wasDrawingForming = this.IsDrawingForming;
			this._wasDrawnPreviousFrame = this._isDrawnThisFrame;
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0002A610 File Offset: 0x00028810
		private bool IsDeployment
		{
			get
			{
				Mission mission = base.Mission;
				return mission != null && mission.Mode == MissionMode.Deployment;
			}
		}

		// Token: 0x04000323 RID: 803
		private bool _suspendTroopPlacer;

		// Token: 0x04000325 RID: 805
		public bool IsDrawingForced;

		// Token: 0x04000326 RID: 806
		public bool IsDrawingFacing;

		// Token: 0x04000327 RID: 807
		public bool IsDrawingForming;

		// Token: 0x04000328 RID: 808
		public Action OnUnitDeployed;

		// Token: 0x04000329 RID: 809
		private bool _isMouseDown;

		// Token: 0x0400032A RID: 810
		private List<GameEntity> _orderPositionEntities;

		// Token: 0x0400032B RID: 811
		private List<GameEntity> _orderRotationEntities;

		// Token: 0x0400032C RID: 812
		private bool _formationDrawingMode;

		// Token: 0x0400032D RID: 813
		private Formation _mouseOverFormation;

		// Token: 0x0400032E RID: 814
		private Vec2 _lastMousePosition;

		// Token: 0x0400032F RID: 815
		private Vec2 _deltaMousePosition;

		// Token: 0x04000330 RID: 816
		private int _mouseOverDirection;

		// Token: 0x04000331 RID: 817
		private WorldPosition? _formationDrawingStartingPosition;

		// Token: 0x04000332 RID: 818
		private Vec2? _formationDrawingStartingPointOfMouse;

		// Token: 0x04000333 RID: 819
		private float? _formationDrawingStartingTime;

		// Token: 0x04000334 RID: 820
		private bool _restrictOrdersToDeploymentBoundaries;

		// Token: 0x04000335 RID: 821
		private bool _initialized;

		// Token: 0x04000337 RID: 823
		private Timer formationDrawTimer;

		// Token: 0x04000338 RID: 824
		private bool _wasDrawingForced;

		// Token: 0x04000339 RID: 825
		private bool _wasDrawingFacing;

		// Token: 0x0400033A RID: 826
		private bool _wasDrawingForming;

		// Token: 0x0400033B RID: 827
		private GameEntity _widthEntityLeft;

		// Token: 0x0400033C RID: 828
		private GameEntity _widthEntityRight;

		// Token: 0x0400033D RID: 829
		private bool _isDrawnThisFrame;

		// Token: 0x0400033E RID: 830
		private bool _wasDrawnPreviousFrame;

		// Token: 0x0400033F RID: 831
		private OrderController _orderController;

		// Token: 0x020000F0 RID: 240
		public enum CursorState
		{
			// Token: 0x0400041B RID: 1051
			Invisible,
			// Token: 0x0400041C RID: 1052
			Normal,
			// Token: 0x0400041D RID: 1053
			Ground,
			// Token: 0x0400041E RID: 1054
			Rotation,
			// Token: 0x0400041F RID: 1055
			Count,
			// Token: 0x04000420 RID: 1056
			OrderableEntity
		}
	}
}
