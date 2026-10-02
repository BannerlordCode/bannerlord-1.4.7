using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options.Gamepad
{
	// Token: 0x0200007C RID: 124
	public class OptionsGamepadOptionItemListPanel : ListPanel
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060006C9 RID: 1737 RVA: 0x00013A4C File Offset: 0x00011C4C
		// (remove) Token: 0x060006CA RID: 1738 RVA: 0x00013A84 File Offset: 0x00011C84
		public event OptionsGamepadOptionItemListPanel.OnActionTextChangeEvent OnActionTextChanged;

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00013AB9 File Offset: 0x00011CB9
		// (set) Token: 0x060006CC RID: 1740 RVA: 0x00013AC1 File Offset: 0x00011CC1
		public OptionsGamepadKeyLocationWidget TargetKey { get; private set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00013ACA File Offset: 0x00011CCA
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x00013AD2 File Offset: 0x00011CD2
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (this._actionText != value)
				{
					this._actionText = value;
					OptionsGamepadOptionItemListPanel.OnActionTextChangeEvent onActionTextChanged = this.OnActionTextChanged;
					if (onActionTextChanged == null)
					{
						return;
					}
					onActionTextChanged();
				}
			}
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00013AF9 File Offset: 0x00011CF9
		public OptionsGamepadOptionItemListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00013B02 File Offset: 0x00011D02
		public void SetKeyProperties(OptionsGamepadKeyLocationWidget currentTarget, Widget parentAreaWidget)
		{
			this.TargetKey = currentTarget;
			this.TargetKey.SetKeyProperties(this.ActionText, parentAreaWidget);
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00013B1D File Offset: 0x00011D1D
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x00013B25 File Offset: 0x00011D25
		public int KeyId
		{
			get
			{
				return this._keyId;
			}
			set
			{
				if (value != this._keyId)
				{
					this._keyId = value;
				}
			}
		}

		// Token: 0x040002EF RID: 751
		private string _actionText;

		// Token: 0x040002F0 RID: 752
		private int _keyId;

		// Token: 0x020001B2 RID: 434
		// (Invoke) Token: 0x06001526 RID: 5414
		public delegate void OnActionTextChangeEvent();
	}
}
