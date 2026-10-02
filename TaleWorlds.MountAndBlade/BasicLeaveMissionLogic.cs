using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000274 RID: 628
	public class BasicLeaveMissionLogic : MissionLogic
	{
		// Token: 0x06002334 RID: 9012 RVA: 0x0007CDCB File Offset: 0x0007AFCB
		public BasicLeaveMissionLogic()
			: this(false)
		{
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x0007CDD4 File Offset: 0x0007AFD4
		public BasicLeaveMissionLogic(bool askBeforeLeave)
			: this(askBeforeLeave, 5)
		{
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0007CDDE File Offset: 0x0007AFDE
		public BasicLeaveMissionLogic(bool askBeforeLeave, int minRetreatDistance)
		{
			this._askBeforeLeave = askBeforeLeave;
			this._minRetreatDistance = minRetreatDistance;
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0007CDF4 File Offset: 0x0007AFF4
		public override bool MissionEnded(ref MissionResult missionResult)
		{
			return base.Mission.MainAgent != null && !base.Mission.MainAgent.IsActive();
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0007CE18 File Offset: 0x0007B018
		public override InquiryData OnEndMissionRequest(out bool canPlayerLeave)
		{
			canPlayerLeave = true;
			if (base.Mission.MainAgent != null && base.Mission.MainAgent.IsActive() && (float)this._minRetreatDistance > 0f && base.Mission.IsPlayerCloseToAnEnemy((float)this._minRetreatDistance))
			{
				canPlayerLeave = false;
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_can_not_retreat", null), 0, null, null, "");
			}
			else if (this._askBeforeLeave)
			{
				return new InquiryData("", GameTexts.FindText("str_give_up_fight", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(base.Mission.OnEndMissionResult), null, "", 0f, null, null, null);
			}
			return null;
		}

		// Token: 0x04000D7F RID: 3455
		private readonly bool _askBeforeLeave;

		// Token: 0x04000D80 RID: 3456
		private readonly int _minRetreatDistance;
	}
}
