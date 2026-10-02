using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000027 RID: 39
	public class ImageIdentifierWidget : TextureWidget
	{
		// Token: 0x06000205 RID: 517 RVA: 0x0000776C File Offset: 0x0000596C
		public ImageIdentifierWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "";
			this._calculateSizeFirstFrame = false;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00007788 File Offset: 0x00005988
		protected override void OnContextActivated()
		{
			base.OnContextActivated();
			string imageId = this.ImageId;
			this.ImageId = string.Empty;
			this.ImageId = imageId;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000077B4 File Offset: 0x000059B4
		protected override void OnContextDeactivated()
		{
			base.OnContextDeactivated();
			base.SetTextureProviderProperty("IsReleased", true);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000077CD File Offset: 0x000059CD
		private void RefreshVisibility()
		{
			if (this.HideWhenNull)
			{
				base.IsVisible = !string.IsNullOrEmpty(this.ImageId);
				return;
			}
			base.IsVisible = true;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000077F3 File Offset: 0x000059F3
		public override void OnClearTextureProvider()
		{
			base.SetTextureProviderProperty("IsReleased", true);
			base.OnClearTextureProvider();
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000780C File Offset: 0x00005A0C
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00007814 File Offset: 0x00005A14
		[Editor(false)]
		public string ImageId
		{
			get
			{
				return this._imageId;
			}
			set
			{
				if (this._imageId != value)
				{
					if (!string.IsNullOrEmpty(this._imageId))
					{
						base.SetTextureProviderProperty("IsReleased", true);
					}
					this._imageId = value;
					base.OnPropertyChanged<string>(value, "ImageId");
					base.SetTextureProviderProperty("ImageId", value);
					if (!string.IsNullOrEmpty(this._imageId))
					{
						base.SetTextureProviderProperty("IsReleased", false);
					}
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00007890 File Offset: 0x00005A90
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00007898 File Offset: 0x00005A98
		[Editor(false)]
		public string AdditionalArgs
		{
			get
			{
				return this._additionalArgs;
			}
			set
			{
				if (this._additionalArgs != value)
				{
					base.SetTextureProviderProperty("IsReleased", true);
					this._additionalArgs = value;
					base.OnPropertyChanged<string>(value, "AdditionalArgs");
					base.SetTextureProviderProperty("AdditionalArgs", value);
					if (!string.IsNullOrEmpty(this._additionalArgs))
					{
						base.SetTextureProviderProperty("IsReleased", false);
					}
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00007907 File Offset: 0x00005B07
		// (set) Token: 0x0600020F RID: 527 RVA: 0x00007910 File Offset: 0x00005B10
		[Editor(false)]
		public bool IsBig
		{
			get
			{
				return this._isBig;
			}
			set
			{
				if (this._isBig != value)
				{
					base.SetTextureProviderProperty("IsReleased", true);
					base.SetTextureProviderProperty("IsReleased", false);
					this._isBig = value;
					base.OnPropertyChanged(value, "IsBig");
					base.SetTextureProviderProperty("IsBig", value);
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00007972 File Offset: 0x00005B72
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000797A File Offset: 0x00005B7A
		[Editor(false)]
		public bool HideWhenNull
		{
			get
			{
				return this._hideWhenNull;
			}
			set
			{
				if (this._hideWhenNull != value)
				{
					this._hideWhenNull = value;
					base.OnPropertyChanged(value, "HideWhenNull");
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x040000F4 RID: 244
		private string _imageId;

		// Token: 0x040000F5 RID: 245
		private string _additionalArgs;

		// Token: 0x040000F6 RID: 246
		private bool _isBig;

		// Token: 0x040000F7 RID: 247
		private bool _hideWhenNull;
	}
}
