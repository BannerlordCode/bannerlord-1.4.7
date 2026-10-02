using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000016 RID: 22
	public class DefaultLayerSpriteChangerBrushWidget : BrushWidget
	{
		// Token: 0x06000129 RID: 297 RVA: 0x000051C0 File Offset: 0x000033C0
		public DefaultLayerSpriteChangerBrushWidget(UIContext context)
			: base(context)
		{
			this.UpdateDefaultLayerSprite();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000051D0 File Offset: 0x000033D0
		private void UpdateDefaultLayerSprite()
		{
			Sprite sprite;
			if (this.SpriteBrushLayerName == null)
			{
				sprite = null;
			}
			else
			{
				Brush spriteBrush = this.SpriteBrush;
				if (spriteBrush == null)
				{
					sprite = null;
				}
				else
				{
					BrushLayer layer = spriteBrush.GetLayer(this.SpriteBrushLayerName);
					sprite = ((layer != null) ? layer.Sprite : null);
				}
			}
			Sprite sprite2 = sprite;
			base.IsVisible = sprite2 != null;
			if (base.IsVisible)
			{
				base.Brush.Sprite = sprite2;
				base.Brush.DefaultLayer.Sprite = sprite2;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000523C File Offset: 0x0000343C
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00005244 File Offset: 0x00003444
		[Editor(false)]
		public Brush SpriteBrush
		{
			get
			{
				return this._spriteBrush;
			}
			set
			{
				if (this._spriteBrush != value)
				{
					this._spriteBrush = value;
					base.OnPropertyChanged<Brush>(value, "SpriteBrush");
					this.UpdateDefaultLayerSprite();
				}
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00005268 File Offset: 0x00003468
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00005270 File Offset: 0x00003470
		[Editor(false)]
		public string SpriteBrushLayerName
		{
			get
			{
				return this._spriteBrushLayerName;
			}
			set
			{
				if (this._spriteBrushLayerName != value)
				{
					this._spriteBrushLayerName = value;
					base.OnPropertyChanged<string>(value, "SpriteBrushLayerName");
					this.UpdateDefaultLayerSprite();
				}
			}
		}

		// Token: 0x04000090 RID: 144
		private Brush _spriteBrush;

		// Token: 0x04000091 RID: 145
		private string _spriteBrushLayerName;
	}
}
