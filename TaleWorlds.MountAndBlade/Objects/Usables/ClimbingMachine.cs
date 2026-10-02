using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003A7 RID: 935
	public class ClimbingMachine : UsableMachine
	{
		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06003515 RID: 13589 RVA: 0x000DA3D0 File Offset: 0x000D85D0
		public override float SinkingReferenceOffset
		{
			get
			{
				return base.GameEntity.GetGlobalScale().z * -1.5f;
			}
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x000DA3F6 File Offset: 0x000D85F6
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = new TextObject("{=fEQAPJ2e}{KEY} Use", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x000DA425 File Offset: 0x000D8625
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=mUIew6a6}Climbing Net", null);
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x000DA434 File Offset: 0x000D8634
		protected internal override void OnInit()
		{
			base.OnInit();
			this._climbingLoop = ActionIndexCache.Create("act_climb_net");
			this._climbingEnd = ActionIndexCache.Create("act_climb_net_ending");
			this._climbingEndContinue = ActionIndexCache.Create("act_climb_net_ending_continue");
			this._climbEndingPoint = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("climb_end"));
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.AutoEquipWeaponsOnUseStopped = true;
				this._standingPointUsageDurations.Add(0f);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x000DA4F8 File Offset: 0x000D86F8
		private void OnUseAction(Agent userAgent)
		{
			userAgent.SetForceAttachedEntity(base.GameEntity);
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x000DA506 File Offset: 0x000D8706
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick;
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x000DA50C File Offset: 0x000D870C
		public override void OnDeploymentFinished()
		{
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.AddComponent(new ResetGravityExclusionAndEntityAttachmentOnStopUsageComponent(new Action<Agent>(this.OnUseAction)));
				standingPoint.AddComponent(new ResetAnimationOnStopUsageComponent(ActionIndexCache.act_none, false));
				standingPoint.SetAreUserPositionsUpdatedInTheMachineTick(true);
			}
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x000DA588 File Offset: 0x000D8788
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			bool flag = false;
			for (int i = 0; i < base.StandingPoints.Count; i++)
			{
				StandingPoint standingPoint = base.StandingPoints[i];
				float num = this._standingPointUsageDurations[i];
				if (standingPoint.HasUser)
				{
					Agent userAgent = standingPoint.UserAgent;
					ActionIndexCache currentAction = userAgent.GetCurrentAction(0);
					num += dt;
					if (currentAction == this._climbingLoop || currentAction == this._climbingEnd)
					{
						MatrixFrame globalFrame = standingPoint.GameEntity.GetGlobalFrame();
						Vec3 vec = globalFrame.origin + globalFrame.rotation.u.NormalizedCopy() * num * 1.4f;
						Vec2 vec3;
						if (currentAction == this._climbingEnd)
						{
							Vec3 vec2 = vec;
							vec3 = globalFrame.rotation.f.AsVec2;
							vec = vec2 + new Vec3(vec3.Normalized() * (Math.Min(userAgent.GetCurrentActionProgress(0) * 1f, 1f) * 0.3f), 0f, -1f);
						}
						userAgent.SetTargetZ(vec.z);
						Agent agent = userAgent;
						vec3 = vec.AsVec2;
						Vec3 vec4 = globalFrame.rotation.f.NormalizedCopy();
						agent.SetTargetPositionAndDirection(in vec3, in vec4);
						Vec3 vec5 = globalFrame.rotation.u.NormalizedCopy();
						userAgent.SetTargetUp(in vec5);
						float num2 = Vec3.DotProduct(vec5, this._climbEndingPoint.GlobalPosition - vec);
						if (currentAction == this._climbingLoop && num2 < 1.8f && !userAgent.SetActionChannel(0, in this._climbingEnd, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
						{
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							num = 0f;
						}
						else if (num2 > Vec3.DotProduct(vec5, this._climbEndingPoint.GlobalPosition - globalFrame.origin) - 1.5f)
						{
							flag = true;
						}
					}
					else if (currentAction == this._climbingEndContinue)
					{
						userAgent.ClearTargetFrame();
						userAgent.SetTargetZ(this._climbEndingPoint.GlobalPosition.z);
						userAgent.SetTargetUp(in Vec3.Zero);
						if (userAgent.GetCurrentActionProgress(0) > 0.95f)
						{
							userAgent.SetExcludedFromGravity(false, true);
							userAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							num = 0f;
						}
					}
					else if (userAgent.SetActionChannel(0, in this._climbingLoop, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
					{
						userAgent.SetExcludedFromGravity(true, false);
						flag = true;
					}
					else
					{
						userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						num = 0f;
					}
				}
				else
				{
					num = 0f;
				}
				this._standingPointUsageDurations[i] = num;
			}
			foreach (StandingPoint standingPoint2 in base.StandingPoints)
			{
				if (!standingPoint2.HasUser)
				{
					standingPoint2.IsDeactivated = flag;
					flag = true;
				}
			}
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x000DA8D4 File Offset: 0x000D8AD4
		public override void OnMissionEnded()
		{
		}

		// Token: 0x04001688 RID: 5768
		private const float ClimbingEndDisplacement = 0.3f;

		// Token: 0x04001689 RID: 5769
		private const float ClimbingSpeed = 1.4f;

		// Token: 0x0400168A RID: 5770
		private const float EndingAnimationTriggerDifference = 1.8f;

		// Token: 0x0400168B RID: 5771
		private const string ClimbingLoopActionName = "act_climb_net";

		// Token: 0x0400168C RID: 5772
		private const string ClimbingEndActionName = "act_climb_net_ending";

		// Token: 0x0400168D RID: 5773
		private const string ClimbingEndContinueActionName = "act_climb_net_ending_continue";

		// Token: 0x0400168E RID: 5774
		private ActionIndexCache _climbingLoop;

		// Token: 0x0400168F RID: 5775
		private ActionIndexCache _climbingEnd;

		// Token: 0x04001690 RID: 5776
		private ActionIndexCache _climbingEndContinue;

		// Token: 0x04001691 RID: 5777
		private GameEntity _climbEndingPoint;

		// Token: 0x04001692 RID: 5778
		private List<float> _standingPointUsageDurations = new List<float>();
	}
}
