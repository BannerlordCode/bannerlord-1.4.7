using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x0200006E RID: 110
	public class KingdomDiplomacyProposalActionItemVM : ViewModel
	{
		// Token: 0x060008E5 RID: 2277 RVA: 0x00027C68 File Offset: 0x00025E68
		public KingdomDiplomacyProposalActionItemVM(TextObject nameText, TextObject explanationText, int influenceCost, bool isEnabled, TextObject hintText, Action action)
		{
			this._nameText = nameText;
			this._explanationText = explanationText;
			this._action = action;
			this.InfluenceCost = influenceCost;
			this.IsEnabled = isEnabled;
			this.Hint = new HintViewModel(hintText, null);
			this.RefreshValues();
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00027CB4 File Offset: 0x00025EB4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameText.ToString();
			this.Explanation = this._explanationText.ToString();
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00027CDE File Offset: 0x00025EDE
		public void ExecuteAction()
		{
			Action action = this._action;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x00027CF0 File Offset: 0x00025EF0
		// (set) Token: 0x060008E9 RID: 2281 RVA: 0x00027CF8 File Offset: 0x00025EF8
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x00027D1B File Offset: 0x00025F1B
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x00027D23 File Offset: 0x00025F23
		[DataSourceProperty]
		public string Explanation
		{
			get
			{
				return this._explanation;
			}
			set
			{
				if (value != this._explanation)
				{
					this._explanation = value;
					base.OnPropertyChangedWithValue<string>(value, "Explanation");
				}
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x00027D46 File Offset: 0x00025F46
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x00027D4E File Offset: 0x00025F4E
		[DataSourceProperty]
		public int InfluenceCost
		{
			get
			{
				return this._influenceCost;
			}
			set
			{
				if (value != this._influenceCost)
				{
					this._influenceCost = value;
					base.OnPropertyChangedWithValue(value, "InfluenceCost");
				}
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x00027D6C File Offset: 0x00025F6C
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x00027D74 File Offset: 0x00025F74
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00027D92 File Offset: 0x00025F92
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x00027D9A File Offset: 0x00025F9A
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x040003EC RID: 1004
		private readonly TextObject _nameText;

		// Token: 0x040003ED RID: 1005
		private readonly TextObject _explanationText;

		// Token: 0x040003EE RID: 1006
		private readonly Action _action;

		// Token: 0x040003EF RID: 1007
		private string _name;

		// Token: 0x040003F0 RID: 1008
		private string _explanation;

		// Token: 0x040003F1 RID: 1009
		private bool _isEnabled;

		// Token: 0x040003F2 RID: 1010
		private int _influenceCost;

		// Token: 0x040003F3 RID: 1011
		private HintViewModel _hint;
	}
}
