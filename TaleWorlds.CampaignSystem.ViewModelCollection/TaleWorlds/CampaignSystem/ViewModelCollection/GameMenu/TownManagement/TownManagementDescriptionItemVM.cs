using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000A8 RID: 168
	public class TownManagementDescriptionItemVM : ViewModel
	{
		// Token: 0x0600103D RID: 4157 RVA: 0x00042864 File Offset: 0x00040A64
		public TownManagementDescriptionItemVM(TextObject title, int value, int valueChange, TownManagementDescriptionItemVM.DescriptionType type, BasicTooltipViewModel hint = null)
		{
			this._titleObj = title;
			this.Value = value;
			this.ValueChange = valueChange;
			this.Type = (int)type;
			this.Hint = hint ?? new BasicTooltipViewModel();
			this.RefreshValues();
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x000428B2 File Offset: 0x00040AB2
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Title = this._titleObj.ToString();
			this.RefreshIsWarning();
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x000428D4 File Offset: 0x00040AD4
		private void RefreshIsWarning()
		{
			int type = this.Type;
			if (type == 1)
			{
				this.IsWarning = this.Value < 1;
				return;
			}
			if (type == 5)
			{
				this.IsWarning = this.Value < Campaign.Current.Models.SettlementLoyaltyModel.RebelliousStateStartLoyaltyThreshold;
				return;
			}
			if (type != 7)
			{
				this.IsWarning = false;
				return;
			}
			this.IsWarning = this.Value < 1;
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x00042940 File Offset: 0x00040B40
		// (set) Token: 0x06001041 RID: 4161 RVA: 0x00042948 File Offset: 0x00040B48
		[DataSourceProperty]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x00042966 File Offset: 0x00040B66
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x0004296E File Offset: 0x00040B6E
		[DataSourceProperty]
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				if (value != this._title)
				{
					this._title = value;
					base.OnPropertyChangedWithValue<string>(value, "Title");
				}
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x00042991 File Offset: 0x00040B91
		// (set) Token: 0x06001045 RID: 4165 RVA: 0x00042999 File Offset: 0x00040B99
		[DataSourceProperty]
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue(value, "Value");
				}
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x000429B7 File Offset: 0x00040BB7
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x000429BF File Offset: 0x00040BBF
		[DataSourceProperty]
		public int ValueChange
		{
			get
			{
				return this._valueChange;
			}
			set
			{
				if (value != this._valueChange)
				{
					this._valueChange = value;
					base.OnPropertyChangedWithValue(value, "ValueChange");
				}
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x000429DD File Offset: 0x00040BDD
		// (set) Token: 0x06001049 RID: 4169 RVA: 0x000429E5 File Offset: 0x00040BE5
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint && value != null)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x00042A06 File Offset: 0x00040C06
		// (set) Token: 0x0600104B RID: 4171 RVA: 0x00042A0E File Offset: 0x00040C0E
		[DataSourceProperty]
		public bool IsWarning
		{
			get
			{
				return this._isWarning;
			}
			set
			{
				if (value != this._isWarning)
				{
					this._isWarning = value;
					base.OnPropertyChangedWithValue(value, "IsWarning");
				}
			}
		}

		// Token: 0x0400076C RID: 1900
		private readonly TextObject _titleObj;

		// Token: 0x0400076D RID: 1901
		private int _type = -1;

		// Token: 0x0400076E RID: 1902
		private string _title;

		// Token: 0x0400076F RID: 1903
		private int _value;

		// Token: 0x04000770 RID: 1904
		private int _valueChange;

		// Token: 0x04000771 RID: 1905
		private BasicTooltipViewModel _hint;

		// Token: 0x04000772 RID: 1906
		private bool _isWarning;

		// Token: 0x0200021A RID: 538
		public enum DescriptionType
		{
			// Token: 0x040011D2 RID: 4562
			Gold,
			// Token: 0x040011D3 RID: 4563
			Production,
			// Token: 0x040011D4 RID: 4564
			Militia,
			// Token: 0x040011D5 RID: 4565
			Prosperity,
			// Token: 0x040011D6 RID: 4566
			Food,
			// Token: 0x040011D7 RID: 4567
			Loyalty,
			// Token: 0x040011D8 RID: 4568
			Security,
			// Token: 0x040011D9 RID: 4569
			Garrison
		}
	}
}
