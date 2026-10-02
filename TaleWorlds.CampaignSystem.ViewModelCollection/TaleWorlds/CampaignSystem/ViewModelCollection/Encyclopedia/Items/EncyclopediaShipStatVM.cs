using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000EA RID: 234
	public class EncyclopediaShipStatVM : ViewModel
	{
		// Token: 0x060015B8 RID: 5560 RVA: 0x000556B0 File Offset: 0x000538B0
		public EncyclopediaShipStatVM(string statId, TextObject name, string value, Func<List<TooltipProperty>> getTooltipProperties = null)
		{
			this._nameTextObj = name;
			this.ValueText = value;
			this.StatId = statId;
			if (getTooltipProperties != null)
			{
				this.Tooltip = new BasicTooltipViewModel(getTooltipProperties);
			}
			else
			{
				this.Tooltip = new BasicTooltipViewModel(() => GameTexts.FindText("str_ship_stat_explanation", this.StatId).ToString());
			}
			this.RefreshValues();
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x00055708 File Offset: 0x00053908
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject textObject = GameTexts.FindText("str_LEFT_colon", null);
			textObject.SetTextVariable("LEFT", this._nameTextObj.ToString());
			this.Name = textObject.ToString();
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060015BA RID: 5562 RVA: 0x0005574A File Offset: 0x0005394A
		// (set) Token: 0x060015BB RID: 5563 RVA: 0x00055752 File Offset: 0x00053952
		[DataSourceProperty]
		public string StatId
		{
			get
			{
				return this._statId;
			}
			set
			{
				if (value != this._statId)
				{
					this._statId = value;
					base.OnPropertyChangedWithValue<string>(value, "StatId");
				}
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060015BC RID: 5564 RVA: 0x00055775 File Offset: 0x00053975
		// (set) Token: 0x060015BD RID: 5565 RVA: 0x0005577D File Offset: 0x0005397D
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

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x000557A0 File Offset: 0x000539A0
		// (set) Token: 0x060015BF RID: 5567 RVA: 0x000557A8 File Offset: 0x000539A8
		[DataSourceProperty]
		public string ValueText
		{
			get
			{
				return this._valueText;
			}
			set
			{
				if (value != this._valueText)
				{
					this._valueText = value;
					base.OnPropertyChangedWithValue<string>(value, "ValueText");
				}
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x060015C0 RID: 5568 RVA: 0x000557CB File Offset: 0x000539CB
		// (set) Token: 0x060015C1 RID: 5569 RVA: 0x000557D3 File Offset: 0x000539D3
		[DataSourceProperty]
		public BasicTooltipViewModel Tooltip
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				if (value != this._tooltip)
				{
					this._tooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Tooltip");
				}
			}
		}

		// Token: 0x040009DF RID: 2527
		private readonly TextObject _nameTextObj;

		// Token: 0x040009E0 RID: 2528
		private string _statId;

		// Token: 0x040009E1 RID: 2529
		private string _name;

		// Token: 0x040009E2 RID: 2530
		private string _valueText;

		// Token: 0x040009E3 RID: 2531
		private BasicTooltipViewModel _tooltip;
	}
}
