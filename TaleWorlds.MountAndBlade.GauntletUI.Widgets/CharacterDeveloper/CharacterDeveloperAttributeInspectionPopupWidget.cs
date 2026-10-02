using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x0200017D RID: 381
	public class CharacterDeveloperAttributeInspectionPopupWidget : Widget
	{
		// Token: 0x060013D2 RID: 5074 RVA: 0x00035EE8 File Offset: 0x000340E8
		public CharacterDeveloperAttributeInspectionPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00035EF4 File Offset: 0x000340F4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.ParentWidget.IsVisible && this._latestMouseUpWidgetWhenActivated != base.EventManager.LatestMouseUpWidget && !base.CheckIsMyChildRecursive(base.EventManager.LatestMouseUpWidget))
			{
				this.Deactivate();
			}
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x00035F41 File Offset: 0x00034141
		private void Activate()
		{
			this._latestMouseUpWidgetWhenActivated = base.EventManager.LatestMouseDownWidget;
			base.ParentWidget.IsVisible = true;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00035F60 File Offset: 0x00034160
		private void Deactivate()
		{
			base.EventFired("Deactivate", Array.Empty<object>());
			base.ParentWidget.IsVisible = false;
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060013D6 RID: 5078 RVA: 0x00035F7E File Offset: 0x0003417E
		// (set) Token: 0x060013D7 RID: 5079 RVA: 0x00035F86 File Offset: 0x00034186
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (this._isActive != value)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
					if (this._isActive)
					{
						this.Activate();
						return;
					}
					this.Deactivate();
				}
			}
		}

		// Token: 0x040008FD RID: 2301
		private Widget _latestMouseUpWidgetWhenActivated;

		// Token: 0x040008FE RID: 2302
		private bool _isActive;
	}
}
