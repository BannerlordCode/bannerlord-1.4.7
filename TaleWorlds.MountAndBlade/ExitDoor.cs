using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000330 RID: 816
	public class ExitDoor : UsableMachine
	{
		// Token: 0x06002DFD RID: 11773 RVA: 0x000B15D0 File Offset: 0x000AF7D0
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = new TextObject("{=gqQPSAQZ}{KEY} Leave Area", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x000B15FF File Offset: 0x000AF7FF
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return null;
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000B1602 File Offset: 0x000AF802
		protected internal override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x000B1616 File Offset: 0x000AF816
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x000B1620 File Offset: 0x000AF820
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				if (standingPoint.HasUser)
				{
					Agent userAgent = standingPoint.UserAgent;
					ActionIndexCache currentAction = userAgent.GetCurrentAction(0);
					ActionIndexCache currentAction2 = userAgent.GetCurrentAction(1);
					if (!(currentAction2 == ActionIndexCache.act_none) || (!(currentAction == ActionIndexCache.act_pickup_middle_begin) && !(currentAction == ActionIndexCache.act_pickup_middle_begin_left_stance)))
					{
						if (currentAction2 == ActionIndexCache.act_none && (currentAction == ActionIndexCache.act_pickup_middle_end || currentAction == ActionIndexCache.act_pickup_middle_end_left_stance))
						{
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							Mission.Current.EndMission();
						}
						else
						{
							if (!(currentAction2 != ActionIndexCache.act_none))
							{
								Agent agent = userAgent;
								int num = 0;
								ActionIndexCache actionIndexCache = (userAgent.GetIsLeftStance() ? ActionIndexCache.act_pickup_middle_begin_left_stance : ActionIndexCache.act_pickup_middle_begin);
								if (agent.SetActionChannel(num, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
								{
									continue;
								}
							}
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
				}
			}
		}
	}
}
