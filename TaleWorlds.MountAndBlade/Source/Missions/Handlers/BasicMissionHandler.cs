using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Missions.Handlers
{
	// Token: 0x020003D8 RID: 984
	public class BasicMissionHandler : MissionLogic
	{
		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06003674 RID: 13940 RVA: 0x000E174B File Offset: 0x000DF94B
		// (set) Token: 0x06003675 RID: 13941 RVA: 0x000E1753 File Offset: 0x000DF953
		public bool IsWarningWidgetOpened { get; private set; }

		// Token: 0x06003676 RID: 13942 RVA: 0x000E175C File Offset: 0x000DF95C
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this.IsWarningWidgetOpened = false;
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x000E176B File Offset: 0x000DF96B
		public void CreateWarningWidgetForResult(BattleEndLogic.ExitResult result)
		{
			if (!GameNetwork.IsClient)
			{
				MBCommon.PauseGameEngine();
			}
			this._isSurrender = result == BattleEndLogic.ExitResult.SurrenderSiege;
			InformationManager.ShowInquiry(this._isSurrender ? this.GetSurrenderPopupData() : this.GetRetreatPopUpData(), true, false);
			this.IsWarningWidgetOpened = true;
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x000E17A7 File Offset: 0x000DF9A7
		private void CloseSelectionWidget()
		{
			if (!this.IsWarningWidgetOpened)
			{
				return;
			}
			this.IsWarningWidgetOpened = false;
			if (!GameNetwork.IsClient)
			{
				MBCommon.UnPauseGameEngine();
			}
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x000E17C5 File Offset: 0x000DF9C5
		private void OnEventCancelSelectionWidget()
		{
			this.CloseSelectionWidget();
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x000E17D0 File Offset: 0x000DF9D0
		private void OnEventAcceptSelectionWidget()
		{
			MissionLogic[] array = base.Mission.MissionLogics.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnBattleEnded();
			}
			this.CloseSelectionWidget();
			if (this._isSurrender)
			{
				base.Mission.SurrenderMission();
				return;
			}
			base.Mission.RetreatMission();
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x000E182C File Offset: 0x000DFA2C
		private InquiryData GetRetreatPopUpData()
		{
			return new InquiryData("", GameTexts.FindText("str_retreat_question", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(this.OnEventAcceptSelectionWidget), new Action(this.OnEventCancelSelectionWidget), "", 0f, null, null, null);
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x000E189C File Offset: 0x000DFA9C
		private InquiryData GetSurrenderPopupData()
		{
			return new InquiryData(GameTexts.FindText("str_surrender", null).ToString(), GameTexts.FindText("str_surrender_question", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(this.OnEventAcceptSelectionWidget), new Action(this.OnEventCancelSelectionWidget), "", 0f, null, null, null);
		}

		// Token: 0x0400176D RID: 5997
		private bool _isSurrender;
	}
}
