using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200002A RID: 42
	public class ItemTableauWidget : TextureWidget
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00007DD0 File Offset: 0x00005FD0
		// (set) Token: 0x0600022B RID: 555 RVA: 0x00007DD8 File Offset: 0x00005FD8
		[Editor(false)]
		public string ItemModifierId
		{
			get
			{
				return this._itemModifierId;
			}
			set
			{
				if (value != this._itemModifierId)
				{
					this._itemModifierId = value;
					base.OnPropertyChanged<string>(value, "ItemModifierId");
					base.SetTextureProviderProperty("ItemModifierId", value);
				}
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00007E07 File Offset: 0x00006007
		// (set) Token: 0x0600022D RID: 557 RVA: 0x00007E0F File Offset: 0x0000600F
		[Editor(false)]
		public string StringId
		{
			get
			{
				return this._stringId;
			}
			set
			{
				if (value != this._stringId)
				{
					this._stringId = value;
					base.OnPropertyChanged<string>(value, "StringId");
					if (value != null)
					{
						base.SetTextureProviderProperty("StringId", value);
					}
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00007E41 File Offset: 0x00006041
		// (set) Token: 0x0600022F RID: 559 RVA: 0x00007E49 File Offset: 0x00006049
		[Editor(false)]
		public float InitialTiltRotation
		{
			get
			{
				return this._initialTiltRotation;
			}
			set
			{
				if (value != this._initialTiltRotation)
				{
					this._initialTiltRotation = value;
					base.OnPropertyChanged(value, "InitialTiltRotation");
					base.SetTextureProviderProperty("InitialTiltRotation", value);
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00007E78 File Offset: 0x00006078
		// (set) Token: 0x06000231 RID: 561 RVA: 0x00007E80 File Offset: 0x00006080
		[Editor(false)]
		public float InitialPanRotation
		{
			get
			{
				return this._initialPanRotation;
			}
			set
			{
				if (value != this._initialPanRotation)
				{
					this._initialPanRotation = value;
					base.OnPropertyChanged(value, "InitialPanRotation");
					base.SetTextureProviderProperty("InitialPanRotation", value);
				}
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00007EAF File Offset: 0x000060AF
		// (set) Token: 0x06000233 RID: 563 RVA: 0x00007EB7 File Offset: 0x000060B7
		[Editor(false)]
		public string BannerCode
		{
			get
			{
				return this._bannerCode;
			}
			set
			{
				if (value != this._bannerCode)
				{
					this._bannerCode = value;
					base.OnPropertyChanged<string>(value, "BannerCode");
					base.SetTextureProviderProperty("BannerCode", value);
				}
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00007EE6 File Offset: 0x000060E6
		public ItemTableauWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "ItemTableauTextureProvider";
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00007EFA File Offset: 0x000060FA
		protected override bool OnPreviewMouseScroll()
		{
			return true;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00007EFD File Offset: 0x000060FD
		protected override void OnMouseScroll()
		{
			base.OnMouseScroll();
			base.SetTextureProviderProperty("CurrentZoom", Input.DeltaMouseScroll * 0.1f);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00007F20 File Offset: 0x00006120
		protected override void OnMousePressed()
		{
			base.SetTextureProviderProperty("CurrentlyRotating", true);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00007F33 File Offset: 0x00006133
		protected override void OnRightStickMovement()
		{
			base.OnRightStickMovement();
			base.SetTextureProviderProperty("RotateItemVertical", base.EventManager.RightStickVerticalScrollAmount);
			base.SetTextureProviderProperty("RotateItemHorizontal", base.EventManager.RightStickHorizontalScrollAmount);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00007F71 File Offset: 0x00006171
		protected override void OnMouseReleased(bool isFromInput)
		{
			base.SetTextureProviderProperty("CurrentlyRotating", false);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00007F84 File Offset: 0x00006184
		protected override bool OnPreviewRightStickMovement()
		{
			return true;
		}

		// Token: 0x04000107 RID: 263
		private string _itemModifierId;

		// Token: 0x04000108 RID: 264
		private string _stringId;

		// Token: 0x04000109 RID: 265
		private float _initialTiltRotation;

		// Token: 0x0400010A RID: 266
		private float _initialPanRotation;

		// Token: 0x0400010B RID: 267
		private string _bannerCode;
	}
}
