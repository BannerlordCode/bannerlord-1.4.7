using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection
{
	// Token: 0x0200001E RID: 30
	public class SelectableItemPropertyVM : ViewModel
	{
		// Token: 0x060001C5 RID: 453 RVA: 0x0000C7A6 File Offset: 0x0000A9A6
		public SelectableItemPropertyVM(string name, string value, bool isWarning = false, BasicTooltipViewModel hint = null)
		{
			this.Name = name;
			this.Value = value;
			this.Hint = hint;
			this.Type = 0;
			this.IsWarning = isWarning;
			this.RefreshValues();
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ColonText = GameTexts.FindText("str_colon", null).ToString();
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000C7F6 File Offset: 0x0000A9F6
		private void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000C808 File Offset: 0x0000AA08
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000C810 File Offset: 0x0000AA10
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000C82E File Offset: 0x0000AA2E
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000C836 File Offset: 0x0000AA36
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000C854 File Offset: 0x0000AA54
		// (set) Token: 0x060001CD RID: 461 RVA: 0x0000C85C File Offset: 0x0000AA5C
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

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000C87F File Offset: 0x0000AA7F
		// (set) Token: 0x060001CF RID: 463 RVA: 0x0000C887 File Offset: 0x0000AA87
		[DataSourceProperty]
		public string Value
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
					base.OnPropertyChangedWithValue<string>(value, "Value");
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000C8AA File Offset: 0x0000AAAA
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x0000C8B2 File Offset: 0x0000AAB2
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x0000C8D8 File Offset: 0x0000AAD8
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

		// Token: 0x040000D5 RID: 213
		private int _type;

		// Token: 0x040000D6 RID: 214
		private bool _isWarning;

		// Token: 0x040000D7 RID: 215
		private string _name;

		// Token: 0x040000D8 RID: 216
		private string _value;

		// Token: 0x040000D9 RID: 217
		private BasicTooltipViewModel _hint;

		// Token: 0x040000DA RID: 218
		private string _colonText;

		// Token: 0x0200017A RID: 378
		public enum PropertyType
		{
			// Token: 0x0400102D RID: 4141
			None,
			// Token: 0x0400102E RID: 4142
			Wall,
			// Token: 0x0400102F RID: 4143
			Garrison,
			// Token: 0x04001030 RID: 4144
			Militia,
			// Token: 0x04001031 RID: 4145
			Prosperity,
			// Token: 0x04001032 RID: 4146
			Food,
			// Token: 0x04001033 RID: 4147
			Loyalty,
			// Token: 0x04001034 RID: 4148
			Security,
			// Token: 0x04001035 RID: 4149
			Shipyard,
			// Token: 0x04001036 RID: 4150
			Patrol,
			// Token: 0x04001037 RID: 4151
			CoastalPatrol
		}
	}
}
