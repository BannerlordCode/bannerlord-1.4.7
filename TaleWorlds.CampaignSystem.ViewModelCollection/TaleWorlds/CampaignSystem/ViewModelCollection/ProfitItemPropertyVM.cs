using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection
{
	// Token: 0x0200001C RID: 28
	public class ProfitItemPropertyVM : ViewModel
	{
		// Token: 0x060001B0 RID: 432 RVA: 0x0000C5AE File Offset: 0x0000A7AE
		public ProfitItemPropertyVM(string name, int value, ProfitItemPropertyVM.PropertyType type = ProfitItemPropertyVM.PropertyType.None, CharacterImageIdentifierVM governorVisual = null, BasicTooltipViewModel hint = null)
		{
			this.Name = name;
			this.Value = value;
			this.Type = (int)type;
			this.GovernorVisual = governorVisual;
			this.Hint = hint;
			this.RefreshValues();
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000C5E1 File Offset: 0x0000A7E1
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ColonText = GameTexts.FindText("str_colon", null).ToString();
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000C5FF File Offset: 0x0000A7FF
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x0000C607 File Offset: 0x0000A807
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
					this.ShowGovernorPortrait = this._type == 5;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000C634 File Offset: 0x0000A834
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x0000C63C File Offset: 0x0000A83C
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000C65F File Offset: 0x0000A85F
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x0000C667 File Offset: 0x0000A867
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
					this.ValueString = this._value.ToString("+0;-#");
					base.OnPropertyChangedWithValue(value, "Value");
				}
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000C69B File Offset: 0x0000A89B
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x0000C6A3 File Offset: 0x0000A8A3
		[DataSourceProperty]
		public string ValueString
		{
			get
			{
				return this._valueString;
			}
			private set
			{
				if (value != this._valueString)
				{
					this._valueString = value;
					base.OnPropertyChangedWithValue<string>(value, "ValueString");
				}
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000C6C6 File Offset: 0x0000A8C6
		// (set) Token: 0x060001BB RID: 443 RVA: 0x0000C6CE File Offset: 0x0000A8CE
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
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
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000C6EC File Offset: 0x0000A8EC
		// (set) Token: 0x060001BD RID: 445 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
		[DataSourceProperty]
		public string ColonText
		{
			get
			{
				return this._colonText;
			}
			set
			{
				if (value != this._colonText)
				{
					this._colonText = value;
					base.OnPropertyChangedWithValue<string>(value, "ColonText");
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000C717 File Offset: 0x0000A917
		// (set) Token: 0x060001BF RID: 447 RVA: 0x0000C71F File Offset: 0x0000A91F
		[DataSourceProperty]
		public CharacterImageIdentifierVM GovernorVisual
		{
			get
			{
				return this._governorVisual;
			}
			set
			{
				if (value != this._governorVisual)
				{
					this._governorVisual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "GovernorVisual");
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000C73D File Offset: 0x0000A93D
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x0000C745 File Offset: 0x0000A945
		[DataSourceProperty]
		public bool ShowGovernorPortrait
		{
			get
			{
				return this._showGovernorPortrait;
			}
			private set
			{
				if (value != this._showGovernorPortrait)
				{
					this._showGovernorPortrait = value;
					base.OnPropertyChangedWithValue(value, "ShowGovernorPortrait");
				}
			}
		}

		// Token: 0x040000CC RID: 204
		private int _type;

		// Token: 0x040000CD RID: 205
		private string _name;

		// Token: 0x040000CE RID: 206
		private int _value;

		// Token: 0x040000CF RID: 207
		private string _valueString;

		// Token: 0x040000D0 RID: 208
		private BasicTooltipViewModel _hint;

		// Token: 0x040000D1 RID: 209
		private string _colonText;

		// Token: 0x040000D2 RID: 210
		private CharacterImageIdentifierVM _governorVisual;

		// Token: 0x040000D3 RID: 211
		private bool _showGovernorPortrait;

		// Token: 0x02000179 RID: 377
		public enum PropertyType
		{
			// Token: 0x04001026 RID: 4134
			None,
			// Token: 0x04001027 RID: 4135
			Tax,
			// Token: 0x04001028 RID: 4136
			Tariff,
			// Token: 0x04001029 RID: 4137
			Garrison,
			// Token: 0x0400102A RID: 4138
			Village,
			// Token: 0x0400102B RID: 4139
			Governor
		}
	}
}
