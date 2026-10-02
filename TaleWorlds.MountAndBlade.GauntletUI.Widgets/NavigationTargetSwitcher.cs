using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000032 RID: 50
	public class NavigationTargetSwitcher : Widget
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x000096BF File Offset: 0x000078BF
		public NavigationTargetSwitcher(UIContext context)
			: base(context)
		{
			base.WidthSizePolicy = SizePolicy.Fixed;
			base.HeightSizePolicy = SizePolicy.Fixed;
			base.SuggestedHeight = 0f;
			base.SuggestedWidth = 0f;
			base.IsVisible = false;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x000096F3 File Offset: 0x000078F3
		private void OnFromTargetNavigationIndexUpdated(PropertyOwnerObject propertyOwner, string propertyName, int value)
		{
			if (propertyName == "GamepadNavigationIndex" && this.ToTarget != null)
			{
				this.TransferGamepadNavigation();
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00009710 File Offset: 0x00007910
		private void TransferGamepadNavigation()
		{
			if (!this._isTransferingNavigationIndices)
			{
				this._isTransferingNavigationIndices = true;
				int gamepadNavigationIndex = this.FromTarget.GamepadNavigationIndex;
				this.ToTarget.GamepadNavigationIndex = gamepadNavigationIndex;
				this.FromTarget.GamepadNavigationIndex = -1;
				if (this.FromTarget.OnGamepadNavigationFocusGained != null)
				{
					Widget toTarget = this.ToTarget;
					toTarget.OnGamepadNavigationFocusGained = (Action<Widget>)Delegate.Combine(toTarget.OnGamepadNavigationFocusGained, this.FromTarget.OnGamepadNavigationFocusGained);
					this.FromTarget.OnGamepadNavigationFocusGained = null;
				}
				this._isTransferingNavigationIndices = false;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00009796 File Offset: 0x00007996
		// (set) Token: 0x060002FD RID: 765 RVA: 0x0000979E File Offset: 0x0000799E
		public Widget ToTarget
		{
			get
			{
				return this._toTarget;
			}
			set
			{
				if (value != this._toTarget)
				{
					this._toTarget = value;
					if (this._toTarget != null && this.FromTarget != null && this.FromTarget.GamepadNavigationIndex != -1)
					{
						this.TransferGamepadNavigation();
					}
				}
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060002FE RID: 766 RVA: 0x000097D4 File Offset: 0x000079D4
		// (set) Token: 0x060002FF RID: 767 RVA: 0x000097DC File Offset: 0x000079DC
		public Widget FromTarget
		{
			get
			{
				return this._fromTarget;
			}
			set
			{
				if (value != this._fromTarget)
				{
					if (this._fromTarget != null)
					{
						this._fromTarget.intPropertyChanged -= this.OnFromTargetNavigationIndexUpdated;
					}
					this._fromTarget = value;
					this._fromTarget.intPropertyChanged += this.OnFromTargetNavigationIndexUpdated;
				}
			}
		}

		// Token: 0x04000135 RID: 309
		private bool _isTransferingNavigationIndices;

		// Token: 0x04000136 RID: 310
		private Widget _toTarget;

		// Token: 0x04000137 RID: 311
		private Widget _fromTarget;
	}
}
