using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects.Usables
{
	// Token: 0x02000048 RID: 72
	public class ShadowingSecureZoneUsePoint : UsableMissionObject
	{
		// Token: 0x060002AC RID: 684 RVA: 0x0000F8E8 File Offset: 0x0000DAE8
		public ShadowingSecureZoneUsePoint()
			: base(false)
		{
			TextObject textObject = new TextObject("{=!}{KEY} Blend in", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			this.ActionMessage = textObject;
			this.DescriptionMessage = new TextObject("{=!}Blend", null);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000F942 File Offset: 0x0000DB42
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=!}Blend in", null);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000F950 File Offset: 0x0000DB50
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			base.OnUse(userAgent, agentBoneIndex);
			if (userAgent.IsMainAgent)
			{
				userAgent.SetActionChannel(0, in ActionIndexCache.act_idle_unarmed_1, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000F9A0 File Offset: 0x0000DBA0
		public override void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			base.OnUseStopped(userAgent, isSuccessful, preferenceIndex);
			if (userAgent.IsMainAgent)
			{
				userAgent.SetActionChannel(0, in ActionIndexCache.act_none, true, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000F9EF File Offset: 0x0000DBEF
		public override bool IsDisabledForAgent(Agent agent)
		{
			return !agent.IsMainAgent;
		}
	}
}
