using System;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Conversation
{
	// Token: 0x0200011A RID: 282
	public class PersuasionVM : ViewModel
	{
		// Token: 0x06001A18 RID: 6680 RVA: 0x00062D9B File Offset: 0x00060F9B
		public PersuasionVM(ConversationManager manager)
		{
			this.PersuasionProgress = new MBBindingList<BoolItemWithActionVM>();
			this._manager = manager;
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x00062DB8 File Offset: 0x00060FB8
		public void OnPersuasionProgress(Tuple<PersuasionOptionArgs, PersuasionOptionResult> selectedOption)
		{
			this.ProgressText = "";
			string text = null;
			string text2 = null;
			switch (selectedOption.Item2)
			{
			case PersuasionOptionResult.CriticalFailure:
				text = new TextObject("{=ocSW4WA2}Critical Fail!", null).ToString();
				text2 = "<a style=\"Conversation.Persuasion.Negative\"><b>{TEXT}</b></a>";
				break;
			case PersuasionOptionResult.Failure:
			case PersuasionOptionResult.Miss:
				text = new TextObject("{=JYOcl7Ox}Ineffective!", null).ToString();
				text2 = "<a style=\"Conversation.Persuasion.Neutral\"><b>{TEXT}</b></a>";
				break;
			case PersuasionOptionResult.Success:
				text = new TextObject("{=3F0y3ugx}Success!", null).ToString();
				text2 = "<a style=\"Conversation.Persuasion.Positive\"><b>{TEXT}</b></a>";
				break;
			case PersuasionOptionResult.CriticalSuccess:
				text = new TextObject("{=4U9EnZt5}Critical Success!", null).ToString();
				text2 = "<a style=\"Conversation.Persuasion.Positive\"><b>{TEXT}</b></a>";
				break;
			}
			this.ProgressText = text2.Replace("{TEXT}", text);
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x00062E6B File Offset: 0x0006106B
		public override void RefreshValues()
		{
			base.RefreshValues();
			PersuasionOptionVM currentPersuasionOption = this.CurrentPersuasionOption;
			if (currentPersuasionOption == null)
			{
				return;
			}
			currentPersuasionOption.RefreshValues();
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x00062E83 File Offset: 0x00061083
		public void SetCurrentOption(PersuasionOptionVM option)
		{
			if (this.CurrentPersuasionOption != option)
			{
				this.CurrentPersuasionOption = option;
			}
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x00062E98 File Offset: 0x00061098
		public void RefreshPersusasion()
		{
			this.CurrentCritFailChance = 0;
			this.CurrentFailChance = 0;
			this.CurrentCritSuccessChance = 0;
			this.CurrentSuccessChance = 0;
			this.IsPersuasionActive = ConversationManager.GetPersuasionIsActive();
			this.PersuasionProgress.Clear();
			this.PersuasionHint = new BasicTooltipViewModel();
			if (this.IsPersuasionActive)
			{
				int num = (int)ConversationManager.GetPersuasionProgress();
				int num2 = (int)ConversationManager.GetPersuasionGoalValue();
				for (int i = 1; i <= num2; i++)
				{
					bool flag = i <= num;
					this.PersuasionProgress.Add(new BoolItemWithActionVM(null, flag, null));
				}
				if (this.CurrentPersuasionOption != null)
				{
					this.CurrentCritFailChance = this._currentPersuasionOption.CritFailChance;
					this.CurrentFailChance = this._currentPersuasionOption.FailChance;
					this.CurrentCritSuccessChance = this._currentPersuasionOption.CritSuccessChance;
					this.CurrentSuccessChance = this._currentPersuasionOption.SuccessChance;
				}
				this.PersuasionHint = new BasicTooltipViewModel(() => this.GetPersuasionTooltip());
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x00062F85 File Offset: 0x00061185
		private string GetPersuasionTooltip()
		{
			if (ConversationManager.GetPersuasionIsActive())
			{
				GameTexts.SetVariable("CURRENT_PROGRESS", (int)ConversationManager.GetPersuasionProgress());
				GameTexts.SetVariable("TARGET_PROGRESS", (int)ConversationManager.GetPersuasionGoalValue());
				return GameTexts.FindText("str_persuasion_tooltip", null).ToString();
			}
			return "";
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x00062FC4 File Offset: 0x000611C4
		private void RefreshChangeValues()
		{
			float num;
			float num2;
			float num3;
			this._manager.GetPersuasionChanceValues(out num, out num2, out num3);
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06001A1F RID: 6687 RVA: 0x00062FE2 File Offset: 0x000611E2
		// (set) Token: 0x06001A20 RID: 6688 RVA: 0x00062FEA File Offset: 0x000611EA
		[DataSourceProperty]
		public BasicTooltipViewModel PersuasionHint
		{
			get
			{
				return this._persuasionHint;
			}
			set
			{
				if (this._persuasionHint != value)
				{
					this._persuasionHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "PersuasionHint");
				}
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06001A21 RID: 6689 RVA: 0x00063008 File Offset: 0x00061208
		// (set) Token: 0x06001A22 RID: 6690 RVA: 0x00063010 File Offset: 0x00061210
		[DataSourceProperty]
		public string ProgressText
		{
			get
			{
				return this._progressText;
			}
			set
			{
				if (this._progressText != value)
				{
					this._progressText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProgressText");
				}
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06001A23 RID: 6691 RVA: 0x00063033 File Offset: 0x00061233
		// (set) Token: 0x06001A24 RID: 6692 RVA: 0x0006303B File Offset: 0x0006123B
		[DataSourceProperty]
		public MBBindingList<BoolItemWithActionVM> PersuasionProgress
		{
			get
			{
				return this._persuasionProgress;
			}
			set
			{
				if (value != this._persuasionProgress)
				{
					this._persuasionProgress = value;
					base.OnPropertyChangedWithValue<MBBindingList<BoolItemWithActionVM>>(value, "PersuasionProgress");
				}
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x00063059 File Offset: 0x00061259
		// (set) Token: 0x06001A26 RID: 6694 RVA: 0x00063061 File Offset: 0x00061261
		[DataSourceProperty]
		public bool IsPersuasionActive
		{
			get
			{
				return this._isPersuasionActive;
			}
			set
			{
				if (value != this._isPersuasionActive)
				{
					if (value)
					{
						this.RefreshChangeValues();
					}
					this._isPersuasionActive = value;
					base.OnPropertyChangedWithValue(value, "IsPersuasionActive");
				}
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x00063088 File Offset: 0x00061288
		// (set) Token: 0x06001A28 RID: 6696 RVA: 0x00063090 File Offset: 0x00061290
		[DataSourceProperty]
		public int CurrentSuccessChance
		{
			get
			{
				return this._currentSuccessChance;
			}
			set
			{
				if (this._currentSuccessChance != value)
				{
					this._currentSuccessChance = value;
					base.OnPropertyChangedWithValue(value, "CurrentSuccessChance");
				}
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06001A29 RID: 6697 RVA: 0x000630AE File Offset: 0x000612AE
		// (set) Token: 0x06001A2A RID: 6698 RVA: 0x000630B6 File Offset: 0x000612B6
		[DataSourceProperty]
		public PersuasionOptionVM CurrentPersuasionOption
		{
			get
			{
				return this._currentPersuasionOption;
			}
			set
			{
				if (this._currentPersuasionOption != value)
				{
					this._currentPersuasionOption = value;
					base.OnPropertyChangedWithValue<PersuasionOptionVM>(value, "CurrentPersuasionOption");
				}
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06001A2B RID: 6699 RVA: 0x000630D4 File Offset: 0x000612D4
		// (set) Token: 0x06001A2C RID: 6700 RVA: 0x000630DC File Offset: 0x000612DC
		[DataSourceProperty]
		public int CurrentFailChance
		{
			get
			{
				return this._currentFailChance;
			}
			set
			{
				if (this._currentFailChance != value)
				{
					this._currentFailChance = value;
					base.OnPropertyChangedWithValue(value, "CurrentFailChance");
				}
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06001A2D RID: 6701 RVA: 0x000630FA File Offset: 0x000612FA
		// (set) Token: 0x06001A2E RID: 6702 RVA: 0x00063102 File Offset: 0x00061302
		[DataSourceProperty]
		public int CurrentCritSuccessChance
		{
			get
			{
				return this._currentCritSuccessChance;
			}
			set
			{
				if (this._currentCritSuccessChance != value)
				{
					this._currentCritSuccessChance = value;
					base.OnPropertyChangedWithValue(value, "CurrentCritSuccessChance");
				}
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x00063120 File Offset: 0x00061320
		// (set) Token: 0x06001A30 RID: 6704 RVA: 0x00063128 File Offset: 0x00061328
		[DataSourceProperty]
		public int CurrentCritFailChance
		{
			get
			{
				return this._currentCritFailChance;
			}
			set
			{
				if (this._currentCritFailChance != value)
				{
					this._currentCritFailChance = value;
					base.OnPropertyChangedWithValue(value, "CurrentCritFailChance");
				}
			}
		}

		// Token: 0x04000BFD RID: 3069
		internal const string PositiveText = "<a style=\"Conversation.Persuasion.Positive\"><b>{TEXT}</b></a>";

		// Token: 0x04000BFE RID: 3070
		internal const string NegativeText = "<a style=\"Conversation.Persuasion.Negative\"><b>{TEXT}</b></a>";

		// Token: 0x04000BFF RID: 3071
		internal const string NeutralText = "<a style=\"Conversation.Persuasion.Neutral\"><b>{TEXT}</b></a>";

		// Token: 0x04000C00 RID: 3072
		private ConversationManager _manager;

		// Token: 0x04000C01 RID: 3073
		private MBBindingList<BoolItemWithActionVM> _persuasionProgress;

		// Token: 0x04000C02 RID: 3074
		private bool _isPersuasionActive;

		// Token: 0x04000C03 RID: 3075
		private int _currentCritFailChance;

		// Token: 0x04000C04 RID: 3076
		private int _currentFailChance;

		// Token: 0x04000C05 RID: 3077
		private int _currentSuccessChance;

		// Token: 0x04000C06 RID: 3078
		private int _currentCritSuccessChance;

		// Token: 0x04000C07 RID: 3079
		private string _progressText;

		// Token: 0x04000C08 RID: 3080
		private PersuasionOptionVM _currentPersuasionOption;

		// Token: 0x04000C09 RID: 3081
		private BasicTooltipViewModel _persuasionHint;
	}
}
