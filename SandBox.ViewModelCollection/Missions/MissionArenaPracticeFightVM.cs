using System;
using SandBox.Missions.MissionLogics.Arena;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions
{
	// Token: 0x0200002D RID: 45
	public class MissionArenaPracticeFightVM : ViewModel
	{
		// Token: 0x060003A8 RID: 936 RVA: 0x0000FAF5 File Offset: 0x0000DCF5
		public MissionArenaPracticeFightVM(ArenaPracticeFightMissionController practiceMissionController)
		{
			this._practiceMissionController = practiceMissionController;
			this._mission = practiceMissionController.Mission;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000FB10 File Offset: 0x0000DD10
		public void Tick()
		{
			this.IsPlayerPracticing = this._practiceMissionController.IsPlayerPracticing;
			Agent mainAgent = this._mission.MainAgent;
			if (mainAgent != null && mainAgent.IsActive())
			{
				int killCount = this._mission.MainAgent.KillCount;
				GameTexts.SetVariable("BEATEN_OPPONENT_COUNT", killCount);
				this.OpponentsBeatenText = GameTexts.FindText("str_beaten_opponent", null).ToString();
			}
			int remainingOpponentCount = this._practiceMissionController.RemainingOpponentCount;
			GameTexts.SetVariable("REMAINING_OPPONENT_COUNT", remainingOpponentCount);
			this.OpponentsRemainingText = GameTexts.FindText("str_remaining_opponent", null).ToString();
			this.UpdatePrizeText();
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000FBAC File Offset: 0x0000DDAC
		public void UpdatePrizeText()
		{
			bool remainingOpponentCount = this._practiceMissionController.RemainingOpponentCount != 0;
			int opponentCountBeatenByPlayer = this._practiceMissionController.OpponentCountBeatenByPlayer;
			int num = 0;
			if (!remainingOpponentCount)
			{
				num = 250;
			}
			else if (opponentCountBeatenByPlayer >= 3)
			{
				if (opponentCountBeatenByPlayer < 6)
				{
					num = 5;
				}
				else if (opponentCountBeatenByPlayer < 10)
				{
					num = 10;
				}
				else if (opponentCountBeatenByPlayer < 20)
				{
					num = 25;
				}
				else
				{
					num = 60;
				}
			}
			GameTexts.SetVariable("DENAR_AMOUNT", num);
			GameTexts.SetVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			this.PrizeText = GameTexts.FindText("str_earned_denar", null).ToString();
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003AB RID: 939 RVA: 0x0000FC2F File Offset: 0x0000DE2F
		// (set) Token: 0x060003AC RID: 940 RVA: 0x0000FC37 File Offset: 0x0000DE37
		[DataSourceProperty]
		public string OpponentsBeatenText
		{
			get
			{
				return this._opponentsBeatenText;
			}
			set
			{
				if (this._opponentsBeatenText != value)
				{
					this._opponentsBeatenText = value;
					base.OnPropertyChangedWithValue<string>(value, "OpponentsBeatenText");
				}
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0000FC5A File Offset: 0x0000DE5A
		// (set) Token: 0x060003AE RID: 942 RVA: 0x0000FC62 File Offset: 0x0000DE62
		[DataSourceProperty]
		public string PrizeText
		{
			get
			{
				return this._prizeText;
			}
			set
			{
				if (this._prizeText != value)
				{
					this._prizeText = value;
					base.OnPropertyChangedWithValue<string>(value, "PrizeText");
				}
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000FC85 File Offset: 0x0000DE85
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x0000FC8D File Offset: 0x0000DE8D
		[DataSourceProperty]
		public string OpponentsRemainingText
		{
			get
			{
				return this._opponentsRemainingText;
			}
			set
			{
				if (this._opponentsRemainingText != value)
				{
					this._opponentsRemainingText = value;
					base.OnPropertyChangedWithValue<string>(value, "OpponentsRemainingText");
				}
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000FCB0 File Offset: 0x0000DEB0
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0000FCB8 File Offset: 0x0000DEB8
		public bool IsPlayerPracticing
		{
			get
			{
				return this._isPlayerPracticing;
			}
			set
			{
				if (this._isPlayerPracticing != value)
				{
					this._isPlayerPracticing = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerPracticing");
				}
			}
		}

		// Token: 0x040001DD RID: 477
		private readonly Mission _mission;

		// Token: 0x040001DE RID: 478
		private readonly ArenaPracticeFightMissionController _practiceMissionController;

		// Token: 0x040001DF RID: 479
		private string _opponentsBeatenText;

		// Token: 0x040001E0 RID: 480
		private string _opponentsRemainingText;

		// Token: 0x040001E1 RID: 481
		private bool _isPlayerPracticing;

		// Token: 0x040001E2 RID: 482
		private string _prizeText;
	}
}
