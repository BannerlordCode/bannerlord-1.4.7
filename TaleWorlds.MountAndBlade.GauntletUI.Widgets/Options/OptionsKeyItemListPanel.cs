using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options
{
	// Token: 0x02000078 RID: 120
	public class OptionsKeyItemListPanel : ListPanel
	{
		// Token: 0x06000687 RID: 1671 RVA: 0x00013218 File Offset: 0x00011418
		public OptionsKeyItemListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00013221 File Offset: 0x00011421
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this._screenWidget = this.FindScreenWidget(base.ParentWidget);
				this._initialized = true;
			}
			if (!this._eventsRegistered)
			{
				this.RegisterHoverEvents();
				this._eventsRegistered = true;
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00013260 File Offset: 0x00011460
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			this.SetCurrentOption(false, false, -1);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00013271 File Offset: 0x00011471
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			this.ResetCurrentOption();
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00013280 File Offset: 0x00011480
		private OptionsScreenWidget FindScreenWidget(Widget parent)
		{
			OptionsScreenWidget optionsScreenWidget;
			if ((optionsScreenWidget = parent as OptionsScreenWidget) != null)
			{
				return optionsScreenWidget;
			}
			if (parent == null)
			{
				return null;
			}
			return this.FindScreenWidget(parent.ParentWidget);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x000132AA File Offset: 0x000114AA
		private void SetCurrentOption(bool fromHoverOverDropdown, bool fromBooleanSelection, int hoverDropdownItemIndex = -1)
		{
			OptionsScreenWidget screenWidget = this._screenWidget;
			if (screenWidget == null)
			{
				return;
			}
			screenWidget.SetCurrentOption(this, null);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x000132BE File Offset: 0x000114BE
		private void ResetCurrentOption()
		{
			OptionsScreenWidget screenWidget = this._screenWidget;
			if (screenWidget == null)
			{
				return;
			}
			screenWidget.SetCurrentOption(null, null);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000132D4 File Offset: 0x000114D4
		private void RegisterHoverEvents()
		{
			List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
			for (int i = 0; i < allChildrenRecursive.Count; i++)
			{
				allChildrenRecursive[i].boolPropertyChanged += this.Child_PropertyChanged;
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00013312 File Offset: 0x00011512
		private void Child_PropertyChanged(PropertyOwnerObject childWidget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				if (propertyValue)
				{
					this.SetCurrentOption(false, false, -1);
					return;
				}
				this.ResetCurrentOption();
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x00013334 File Offset: 0x00011534
		// (set) Token: 0x06000691 RID: 1681 RVA: 0x0001333C File Offset: 0x0001153C
		public string OptionTitle
		{
			get
			{
				return this._optionTitle;
			}
			set
			{
				if (this._optionTitle != value)
				{
					this._optionTitle = value;
				}
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00013353 File Offset: 0x00011553
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x0001335B File Offset: 0x0001155B
		public string OptionDescription
		{
			get
			{
				return this._optionDescription;
			}
			set
			{
				if (this._optionDescription != value)
				{
					this._optionDescription = value;
				}
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x00013372 File Offset: 0x00011572
		// (set) Token: 0x06000695 RID: 1685 RVA: 0x0001337A File Offset: 0x0001157A
		public string OptionExtraInformation
		{
			get
			{
				return this._optionExtraInformation;
			}
			set
			{
				if (this._optionExtraInformation != value)
				{
					this._optionExtraInformation = value;
				}
			}
		}

		// Token: 0x040002CC RID: 716
		private OptionsScreenWidget _screenWidget;

		// Token: 0x040002CD RID: 717
		private bool _eventsRegistered;

		// Token: 0x040002CE RID: 718
		private bool _initialized;

		// Token: 0x040002CF RID: 719
		private string _optionDescription;

		// Token: 0x040002D0 RID: 720
		private string _optionTitle;

		// Token: 0x040002D1 RID: 721
		private string _optionExtraInformation;
	}
}
