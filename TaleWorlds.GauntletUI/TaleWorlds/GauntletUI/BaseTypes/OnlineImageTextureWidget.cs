using System;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000061 RID: 97
	public class OnlineImageTextureWidget : TextureWidget
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0001BCB8 File Offset: 0x00019EB8
		// (set) Token: 0x0600067A RID: 1658 RVA: 0x0001BCC0 File Offset: 0x00019EC0
		public OnlineImageTextureWidget.ImageSizePolicies ImageSizePolicy { get; set; }

		// Token: 0x0600067B RID: 1659 RVA: 0x0001BCC9 File Offset: 0x00019EC9
		public OnlineImageTextureWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "OnlineImageTextureProvider";
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001BCDD File Offset: 0x00019EDD
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.UpdateSizePolicy();
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001BCEC File Offset: 0x00019EEC
		private void UpdateSizePolicy()
		{
			Texture texture = base.Texture;
			bool flag = texture != null && texture.IsValid;
			if (this.ImageSizePolicy == OnlineImageTextureWidget.ImageSizePolicies.OriginalSize)
			{
				if (flag)
				{
					base.WidthSizePolicy = SizePolicy.Fixed;
					base.HeightSizePolicy = SizePolicy.Fixed;
					base.SuggestedWidth = (float)base.Texture.Width;
					base.SuggestedHeight = (float)base.Texture.Height;
					return;
				}
			}
			else
			{
				if (this.ImageSizePolicy == OnlineImageTextureWidget.ImageSizePolicies.Stretch)
				{
					base.WidthSizePolicy = SizePolicy.StretchToParent;
					base.HeightSizePolicy = SizePolicy.StretchToParent;
					return;
				}
				if (this.ImageSizePolicy == OnlineImageTextureWidget.ImageSizePolicies.ScaleToBiggerDimension && flag)
				{
					base.WidthSizePolicy = SizePolicy.Fixed;
					base.HeightSizePolicy = SizePolicy.Fixed;
					float num;
					if (base.Texture.Width > base.Texture.Height)
					{
						num = base.ParentWidget.Size.Y / (float)base.Texture.Height;
						if (num * (float)base.Texture.Width < base.ParentWidget.Size.X)
						{
							num = base.ParentWidget.Size.X / (float)base.Texture.Width;
						}
					}
					else
					{
						num = base.ParentWidget.Size.X / (float)base.Texture.Width;
						if (num * (float)base.Texture.Height < base.ParentWidget.Size.Y)
						{
							num = base.ParentWidget.Size.Y / (float)base.Texture.Height;
						}
					}
					base.ScaledSuggestedWidth = num * (float)base.Texture.Width;
					base.ScaledSuggestedHeight = num * (float)base.Texture.Height;
				}
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001BE87 File Offset: 0x0001A087
		// (set) Token: 0x0600067F RID: 1663 RVA: 0x0001BE8F File Offset: 0x0001A08F
		[Editor(false)]
		public string OnlineImageSourceUrl
		{
			get
			{
				return this._onlineImageSourceUrl;
			}
			set
			{
				if (this._onlineImageSourceUrl != value)
				{
					this._onlineImageSourceUrl = value;
					base.OnPropertyChanged<string>(value, "OnlineImageSourceUrl");
					base.SetTextureProviderProperty("OnlineSourceUrl", value);
					this.RefreshState();
				}
			}
		}

		// Token: 0x04000308 RID: 776
		private string _onlineImageSourceUrl;

		// Token: 0x02000096 RID: 150
		public enum ImageSizePolicies
		{
			// Token: 0x04000495 RID: 1173
			Stretch,
			// Token: 0x04000496 RID: 1174
			OriginalSize,
			// Token: 0x04000497 RID: 1175
			ScaleToBiggerDimension
		}
	}
}
