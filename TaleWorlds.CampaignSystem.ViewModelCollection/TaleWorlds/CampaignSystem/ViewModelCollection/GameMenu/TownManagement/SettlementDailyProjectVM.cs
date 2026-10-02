using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000A3 RID: 163
	public class SettlementDailyProjectVM : SettlementProjectVM
	{
		// Token: 0x06000FDF RID: 4063 RVA: 0x0004183B File Offset: 0x0003FA3B
		public SettlementDailyProjectVM(Action<SettlementProjectVM, bool> onSelection, Action<SettlementProjectVM> onSetAsCurrent, Action onResetCurrent, Building building, Settlement settlement)
			: base(onSelection, onSetAsCurrent, onResetCurrent, building, settlement)
		{
			base.IsDaily = true;
			this.RefreshValues();
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00041857 File Offset: 0x0003FA57
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DefaultText = GameTexts.FindText("str_default", null).ToString();
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00041875 File Offset: 0x0003FA75
		public override void RefreshProductionText()
		{
			base.RefreshProductionText();
			base.ProductionText = new TextObject("{=bd7oAQq6}Daily", null).ToString();
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00041893 File Offset: 0x0003FA93
		public override void ExecuteAddRemoveToQueue()
		{
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x00041895 File Offset: 0x0003FA95
		public override void ExecuteSetAsActiveDevelopment()
		{
			this._onSelection(this, false);
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x000418A4 File Offset: 0x0003FAA4
		public override void ExecuteSetAsCurrent()
		{
			Action<SettlementProjectVM> onSetAsCurrent = this._onSetAsCurrent;
			if (onSetAsCurrent == null)
			{
				return;
			}
			onSetAsCurrent(this);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x000418B7 File Offset: 0x0003FAB7
		public override void ExecuteResetCurrent()
		{
			Action onResetCurrent = this._onResetCurrent;
			if (onResetCurrent == null)
			{
				return;
			}
			onResetCurrent();
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x000418C9 File Offset: 0x0003FAC9
		public override void ExecuteToggleSelected()
		{
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x000418CB File Offset: 0x0003FACB
		// (set) Token: 0x06000FE8 RID: 4072 RVA: 0x000418D3 File Offset: 0x0003FAD3
		[DataSourceProperty]
		public bool IsDefault
		{
			get
			{
				return this._isDefault;
			}
			set
			{
				if (value != this._isDefault)
				{
					this._isDefault = value;
					base.OnPropertyChangedWithValue(value, "IsDefault");
				}
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x000418F1 File Offset: 0x0003FAF1
		// (set) Token: 0x06000FEA RID: 4074 RVA: 0x000418F9 File Offset: 0x0003FAF9
		[DataSourceProperty]
		public string DefaultText
		{
			get
			{
				return this._defaultText;
			}
			set
			{
				if (value != this._defaultText)
				{
					this._defaultText = value;
					base.OnPropertyChangedWithValue<string>(value, "DefaultText");
				}
			}
		}

		// Token: 0x04000741 RID: 1857
		private bool _isDefault;

		// Token: 0x04000742 RID: 1858
		private string _defaultText;
	}
}
