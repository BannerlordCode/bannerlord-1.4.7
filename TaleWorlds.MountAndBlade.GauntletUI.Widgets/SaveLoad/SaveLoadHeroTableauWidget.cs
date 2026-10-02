using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.SaveLoad
{
	// Token: 0x0200005B RID: 91
	public class SaveLoadHeroTableauWidget : TextureWidget
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x0000F67C File Offset: 0x0000D87C
		public bool IsVersionCompatible
		{
			get
			{
				bool? textureProviderProperty = base.GetTextureProviderProperty<bool>("IsVersionCompatible");
				bool flag = true;
				return (textureProviderProperty.GetValueOrDefault() == flag) & (textureProviderProperty != null);
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0000F6A9 File Offset: 0x0000D8A9
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x0000F6B1 File Offset: 0x0000D8B1
		[Editor(false)]
		public string HeroVisualCode
		{
			get
			{
				return this._heroVisualCode;
			}
			set
			{
				if (value != this._heroVisualCode)
				{
					this._heroVisualCode = value;
					base.OnPropertyChanged<string>(value, "HeroVisualCode");
					base.SetTextureProviderProperty("HeroVisualCode", value);
				}
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0000F6E0 File Offset: 0x0000D8E0
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x0000F6E8 File Offset: 0x0000D8E8
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

		// Token: 0x060004FB RID: 1275 RVA: 0x0000F717 File Offset: 0x0000D917
		public SaveLoadHeroTableauWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "SaveLoadHeroTableauTextureProvider";
			this._isRenderRequestedPreviousFrame = true;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0000F732 File Offset: 0x0000D932
		protected override void OnMousePressed()
		{
			base.SetTextureProviderProperty("CurrentlyRotating", true);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0000F745 File Offset: 0x0000D945
		protected override void OnMouseReleased(bool isFromInput)
		{
			base.SetTextureProviderProperty("CurrentlyRotating", false);
		}

		// Token: 0x04000222 RID: 546
		private string _heroVisualCode;

		// Token: 0x04000223 RID: 547
		private string _bannerCode;
	}
}
