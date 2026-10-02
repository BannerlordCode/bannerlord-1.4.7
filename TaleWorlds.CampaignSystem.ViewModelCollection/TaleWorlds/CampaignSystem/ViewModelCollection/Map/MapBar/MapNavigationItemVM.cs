using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar
{
	// Token: 0x0200005D RID: 93
	public class MapNavigationItemVM : ViewModel
	{
		// Token: 0x060006B0 RID: 1712 RVA: 0x00021B34 File Offset: 0x0001FD34
		public MapNavigationItemVM(INavigationElement navigationElement)
		{
			this.NavigationElement = navigationElement;
			this.Tooltip = new BasicTooltipViewModel(() => this.GetTooltip());
			this.AlertTooltip = new BasicTooltipViewModel(() => this.GetAlertTooltip());
			this.ItemId = this.NavigationElement.StringId;
			this.RefreshStates(true);
			this.RefreshValues();
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00021B9C File Offset: 0x0001FD9C
		private string GetTooltip()
		{
			NavigationPermissionItem permission = this.NavigationElement.Permission;
			if (permission.IsAuthorized || this.NavigationElement.IsActive)
			{
				TextObject tooltip = this.NavigationElement.Tooltip;
				if (tooltip == null)
				{
					return null;
				}
				return tooltip.ToString();
			}
			else
			{
				TextObject reasonString = permission.ReasonString;
				if (reasonString == null)
				{
					return null;
				}
				return reasonString.ToString();
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00021BF4 File Offset: 0x0001FDF4
		private string GetAlertTooltip()
		{
			TextObject alertTooltip = this.NavigationElement.AlertTooltip;
			if (alertTooltip == null)
			{
				return null;
			}
			return alertTooltip.ToString();
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00021C0C File Offset: 0x0001FE0C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AlertText = GameTexts.FindText("str_map_bar_alert", null).ToString();
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00021C2C File Offset: 0x0001FE2C
		public void RefreshStates(bool forceRefresh = false)
		{
			this.IsActive = this.NavigationElement.IsActive;
			this.HasAlert = this.NavigationElement.HasAlert;
			this.IsEnabled = this.NavigationElement.Permission.IsAuthorized;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00021C74 File Offset: 0x0001FE74
		public void ExecuteOpen()
		{
			this.NavigationElement.OpenView();
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00021C81 File Offset: 0x0001FE81
		public void ExecuteGoToLink()
		{
			this.NavigationElement.GoToLink();
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00021C8E File Offset: 0x0001FE8E
		// (set) Token: 0x060006B8 RID: 1720 RVA: 0x00021C96 File Offset: 0x0001FE96
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

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00021CB4 File Offset: 0x0001FEB4
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x00021CBC File Offset: 0x0001FEBC
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x00021CDA File Offset: 0x0001FEDA
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x00021CE2 File Offset: 0x0001FEE2
		[DataSourceProperty]
		public bool HasAlert
		{
			get
			{
				return this._hasAlert;
			}
			set
			{
				if (value != this._hasAlert)
				{
					this._hasAlert = value;
					base.OnPropertyChangedWithValue(value, "HasAlert");
				}
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x00021D00 File Offset: 0x0001FF00
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x00021D08 File Offset: 0x0001FF08
		[DataSourceProperty]
		public string ItemId
		{
			get
			{
				return this._itemId;
			}
			set
			{
				if (value != this._itemId)
				{
					this._itemId = value;
					base.OnPropertyChangedWithValue<string>(value, "ItemId");
				}
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x00021D2B File Offset: 0x0001FF2B
		// (set) Token: 0x060006C0 RID: 1728 RVA: 0x00021D33 File Offset: 0x0001FF33
		[DataSourceProperty]
		public string AlertText
		{
			get
			{
				return this._alertText;
			}
			set
			{
				if (value != this._alertText)
				{
					this._alertText = value;
					base.OnPropertyChangedWithValue<string>(value, "AlertText");
				}
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00021D56 File Offset: 0x0001FF56
		// (set) Token: 0x060006C2 RID: 1730 RVA: 0x00021D5E File Offset: 0x0001FF5E
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

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00021D7C File Offset: 0x0001FF7C
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x00021D84 File Offset: 0x0001FF84
		[DataSourceProperty]
		public BasicTooltipViewModel AlertTooltip
		{
			get
			{
				return this._alertTooltip;
			}
			set
			{
				if (value != this._alertTooltip)
				{
					this._alertTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "AlertTooltip");
				}
			}
		}

		// Token: 0x040002EC RID: 748
		public readonly INavigationElement NavigationElement;

		// Token: 0x040002ED RID: 749
		private bool _isEnabled;

		// Token: 0x040002EE RID: 750
		private bool _isActive;

		// Token: 0x040002EF RID: 751
		private bool _hasAlert;

		// Token: 0x040002F0 RID: 752
		private string _itemId;

		// Token: 0x040002F1 RID: 753
		private string _alertText;

		// Token: 0x040002F2 RID: 754
		private BasicTooltipViewModel _tooltip;

		// Token: 0x040002F3 RID: 755
		private BasicTooltipViewModel _alertTooltip;
	}
}
