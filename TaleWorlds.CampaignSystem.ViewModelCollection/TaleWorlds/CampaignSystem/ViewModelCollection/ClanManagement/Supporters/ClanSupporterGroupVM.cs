using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Supporters
{
	// Token: 0x02000130 RID: 304
	public class ClanSupporterGroupVM : ViewModel
	{
		// Token: 0x06001C91 RID: 7313 RVA: 0x00069CCB File Offset: 0x00067ECB
		public ClanSupporterGroupVM(TextObject groupName, float influenceBonus, Action<ClanSupporterGroupVM> onSelection)
		{
			this._groupNameText = groupName;
			this._influenceBonus = influenceBonus;
			this._onSelection = onSelection;
			this.Supporters = new MBBindingList<ClanSupporterItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x00069CF9 File Offset: 0x00067EF9
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Refresh();
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x00069D08 File Offset: 0x00067F08
		public void AddSupporter(Hero hero)
		{
			if (!this.Supporters.Any<ClanSupporterItemVM>((ClanSupporterItemVM x) => x.Hero.Hero == hero))
			{
				this.Supporters.Add(new ClanSupporterItemVM(hero));
			}
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x00069D54 File Offset: 0x00067F54
		public void Refresh()
		{
			TextObject textObject = GameTexts.FindText("str_amount_with_influence_icon", null);
			this.TotalInfluenceBonus = (float)this.Supporters.Count * this._influenceBonus;
			TextObject textObject2 = GameTexts.FindText("str_plus_with_number", null);
			textObject2.SetTextVariable("NUMBER", this.TotalInfluenceBonus.ToString("F2"));
			textObject.SetTextVariable("AMOUNT", textObject2.ToString());
			textObject.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
			this.TotalInfluence = textObject.ToString();
			TextObject textObject3 = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null);
			textObject3.SetTextVariable("RANK", this._groupNameText.ToString());
			textObject3.SetTextVariable("NUMBER", this.Supporters.Count);
			this.Name = textObject3.ToString();
			TextObject textObject4 = new TextObject("{=cZCOa00c}{SUPPORTER_RANK} Supporters ({NUM})", null);
			textObject4.SetTextVariable("SUPPORTER_RANK", this._groupNameText.ToString());
			textObject4.SetTextVariable("NUM", this.Supporters.Count);
			this.TitleText = textObject4.ToString();
			TextObject textObject5 = new TextObject("{=jdbT6nc9}Each {SUPPORTER_RANK} supporter provides {INFLUENCE_BONUS} per day.", null);
			textObject5.SetTextVariable("SUPPORTER_RANK", this._groupNameText.ToString());
			textObject5.SetTextVariable("INFLUENCE_BONUS", this._influenceBonus.ToString("F2") + "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
			this.InfluenceBonusDescription = textObject5.ToString();
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x00069EC6 File Offset: 0x000680C6
		public void ExecuteSelect()
		{
			Action<ClanSupporterGroupVM> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06001C96 RID: 7318 RVA: 0x00069ED9 File Offset: 0x000680D9
		// (set) Token: 0x06001C97 RID: 7319 RVA: 0x00069EE1 File Offset: 0x000680E1
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06001C98 RID: 7320 RVA: 0x00069F04 File Offset: 0x00068104
		// (set) Token: 0x06001C99 RID: 7321 RVA: 0x00069F0C File Offset: 0x0006810C
		[DataSourceProperty]
		public float TotalInfluenceBonus
		{
			get
			{
				return this._totalInfluenceBonus;
			}
			private set
			{
				if (value != this._totalInfluenceBonus)
				{
					this._totalInfluenceBonus = value;
					base.OnPropertyChangedWithValue(value, "TotalInfluenceBonus");
				}
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06001C9A RID: 7322 RVA: 0x00069F2A File Offset: 0x0006812A
		// (set) Token: 0x06001C9B RID: 7323 RVA: 0x00069F32 File Offset: 0x00068132
		[DataSourceProperty]
		public string InfluenceBonusDescription
		{
			get
			{
				return this._influenceBonusDescription;
			}
			set
			{
				if (value != this._influenceBonusDescription)
				{
					this._influenceBonusDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "InfluenceBonusDescription");
				}
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x00069F55 File Offset: 0x00068155
		// (set) Token: 0x06001C9D RID: 7325 RVA: 0x00069F5D File Offset: 0x0006815D
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

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06001C9E RID: 7326 RVA: 0x00069F80 File Offset: 0x00068180
		// (set) Token: 0x06001C9F RID: 7327 RVA: 0x00069F88 File Offset: 0x00068188
		[DataSourceProperty]
		public string TotalInfluence
		{
			get
			{
				return this._totalInfluence;
			}
			set
			{
				if (value != this._totalInfluence)
				{
					this._totalInfluence = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalInfluence");
				}
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x00069FAB File Offset: 0x000681AB
		// (set) Token: 0x06001CA1 RID: 7329 RVA: 0x00069FB3 File Offset: 0x000681B3
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x00069FD1 File Offset: 0x000681D1
		// (set) Token: 0x06001CA3 RID: 7331 RVA: 0x00069FD9 File Offset: 0x000681D9
		[DataSourceProperty]
		public MBBindingList<ClanSupporterItemVM> Supporters
		{
			get
			{
				return this._supporters;
			}
			set
			{
				if (value != this._supporters)
				{
					this._supporters = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanSupporterItemVM>>(value, "Supporters");
				}
			}
		}

		// Token: 0x04000D52 RID: 3410
		private TextObject _groupNameText;

		// Token: 0x04000D53 RID: 3411
		private float _influenceBonus;

		// Token: 0x04000D54 RID: 3412
		private Action<ClanSupporterGroupVM> _onSelection;

		// Token: 0x04000D55 RID: 3413
		private string _titleText;

		// Token: 0x04000D56 RID: 3414
		private string _influenceBonusDescription;

		// Token: 0x04000D57 RID: 3415
		private string _name;

		// Token: 0x04000D58 RID: 3416
		private string _totalInfluence;

		// Token: 0x04000D59 RID: 3417
		private bool _isSelected;

		// Token: 0x04000D5A RID: 3418
		private MBBindingList<ClanSupporterItemVM> _supporters;

		// Token: 0x04000D5B RID: 3419
		private float _totalInfluenceBonus;
	}
}
