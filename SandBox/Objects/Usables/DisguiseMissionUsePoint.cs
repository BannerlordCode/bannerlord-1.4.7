using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects.Usables
{
	// Token: 0x0200004B RID: 75
	public class DisguiseMissionUsePoint : UsableMissionObject
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x0000FD4C File Offset: 0x0000DF4C
		public DisguiseMissionUsePoint()
			: base(false)
		{
			TextObject textObject = new TextObject("{=!}Steal", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			this.ActionMessage = textObject;
			this.DescriptionMessage = new TextObject("{=!}Information.", null);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000FDA6 File Offset: 0x0000DFA6
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=!}Steal the information", null);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000FDB3 File Offset: 0x0000DFB3
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			base.OnUse(userAgent, agentBoneIndex);
			bool isMainAgent = userAgent.IsMainAgent;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000FDC4 File Offset: 0x0000DFC4
		public override void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			base.OnUseStopped(userAgent, isSuccessful, preferenceIndex);
			if (this.LockUserFrames || this.LockUserPositions)
			{
				userAgent.ClearTargetFrame();
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000FDE5 File Offset: 0x0000DFE5
		public override bool IsDisabledForAgent(Agent agent)
		{
			return !agent.IsMainAgent;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000FDF0 File Offset: 0x0000DFF0
		public override bool IsUsableByAgent(Agent userAgent)
		{
			return userAgent.Position.Distance(base.GameEntity.GlobalPosition) < 2f;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000FE20 File Offset: 0x0000E020
		public override WorldFrame GetUserFrameForAgent(Agent agent)
		{
			return agent.GetWorldFrame();
		}

		// Token: 0x04000132 RID: 306
		public const float InteractionPointDistance = 2f;
	}
}
