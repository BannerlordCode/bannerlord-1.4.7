using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects
{
	// Token: 0x0200003F RID: 63
	public class TeleportUsePoint : StandingPoint
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000DECB File Offset: 0x0000C0CB
		public override bool HasAIMovingTo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000DECE File Offset: 0x0000C0CE
		public TeleportUsePoint()
		{
			base.IsInstantUse = true;
			this.LockUserFrames = false;
			this.LockUserFrames = false;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000DEEB File Offset: 0x0000C0EB
		public override bool IsAIMovingTo(Agent agent)
		{
			return false;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000DEF0 File Offset: 0x0000C0F0
		protected override void OnInit()
		{
			this.DescriptionMessage = TextObject.GetEmpty();
			if (this.IsLeave)
			{
				this.ActionMessage = GameTexts.FindText("str_mission_exit", null);
				return;
			}
			switch (this.TypeOfTeleport)
			{
			case TeleportUsePoint.TeleportType.Lair:
				this.ActionMessage = GameTexts.FindText("str_ui_lair", null);
				return;
			case TeleportUsePoint.TeleportType.Door:
				this.ActionMessage = GameTexts.FindText("str_ui_door", null);
				return;
			case TeleportUsePoint.TeleportType.Gate:
				this.ActionMessage = new TextObject("{=6wZUG0ev}Gate", null);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000DF74 File Offset: 0x0000C174
		public override bool IsUsableByAgent(Agent userAgent)
		{
			if (userAgent.IsPlayerControlled && !base.IsDeactivated)
			{
				float num = this.InteractionEntity.GetGlobalFrame().origin.AsVec2.DistanceSquared(userAgent.Position.AsVec2);
				float interactionDistance = this.GetInteractionDistance();
				return num <= interactionDistance * interactionDistance;
			}
			return false;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000DFD4 File Offset: 0x0000C1D4
		public override bool IsDisabledForAgent(Agent agent)
		{
			return !agent.IsPlayerControlled || base.IsDisabledForPlayers || base.IsDeactivated;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000DFEE File Offset: 0x0000C1EE
		protected override void OnTick(float dt)
		{
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000DFF0 File Offset: 0x0000C1F0
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			if (!base.IsDeactivated && (Campaign.Current.GameMode == CampaignGameMode.Campaign || userAgent.IsPlayerControlled))
			{
				base.OnUse(userAgent, agentBoneIndex);
				GameEntity gameEntity = Mission.Current.Scene.FindEntityWithTag(this.TargetPointTag);
				userAgent.TeleportToPosition(gameEntity.GetGlobalFrame().origin.ToWorldPosition().GetGroundVec3());
				userAgent.FadeIn();
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000E05C File Offset: 0x0000C25C
		public void Deactivate()
		{
			base.IsDeactivated = true;
			this.ActionMessage = TextObject.GetEmpty();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000E070 File Offset: 0x0000C270
		public void Activate()
		{
			base.IsDeactivated = false;
			this.OnInit();
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000E07F File Offset: 0x0000C27F
		public override void OnFocusGain(Agent userAgent)
		{
			if (!base.IsDeactivated)
			{
				base.OnFocusGain(userAgent);
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000E090 File Offset: 0x0000C290
		private float GetInteractionDistance()
		{
			if (this.TypeOfTeleport == TeleportUsePoint.TeleportType.Lair)
			{
				return 0.5f;
			}
			return 2.5f;
		}

		// Token: 0x040000ED RID: 237
		public TeleportUsePoint.TeleportType TypeOfTeleport;

		// Token: 0x040000EE RID: 238
		public string TargetPointTag;

		// Token: 0x040000EF RID: 239
		public bool IsLeave;

		// Token: 0x040000F0 RID: 240
		private const float LairInteractionDistance = 0.5f;

		// Token: 0x040000F1 RID: 241
		private const float GateInteractionDistance = 2.5f;

		// Token: 0x02000149 RID: 329
		public enum TeleportType
		{
			// Token: 0x04000680 RID: 1664
			Lair,
			// Token: 0x04000681 RID: 1665
			Door,
			// Token: 0x04000682 RID: 1666
			Gate
		}
	}
}
