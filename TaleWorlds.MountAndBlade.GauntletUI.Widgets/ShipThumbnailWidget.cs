using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000040 RID: 64
	public class ShipThumbnailWidget : Widget
	{
		// Token: 0x060003B4 RID: 948 RVA: 0x0000BB48 File Offset: 0x00009D48
		public ShipThumbnailWidget(UIContext context)
			: base(context)
		{
			base.ClipContents = true;
			base.DoNotPassEventsToChildren = true;
			base.UpdateChildrenStates = true;
			this._childWidget = new BrushWidget(context)
			{
				IsVisible = false,
				WidthSizePolicy = SizePolicy.Fixed,
				HeightSizePolicy = SizePolicy.Fixed,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};
			base.AddChild(this._childWidget);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000BBAC File Offset: 0x00009DAC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.Size != this._previousSize)
			{
				this._previousSize = base.Size;
				this._shouldUpdateSprite = true;
			}
			if (this._shouldUpdateSprite && (base.Size.X != 0f || base.WidthSizePolicy == SizePolicy.CoverChildren) && (base.Size.Y != 0f || base.HeightSizePolicy == SizePolicy.CoverChildren))
			{
				this._shouldUpdateSprite = false;
				this.UpdateSprite();
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000BC3C File Offset: 0x00009E3C
		private void UpdateSprite()
		{
			object obj;
			if (!string.IsNullOrEmpty(this.PrefabId))
			{
				Brush spriteBrush = this.SpriteBrush;
				if (spriteBrush == null)
				{
					obj = null;
				}
				else
				{
					BrushLayer layer = spriteBrush.GetLayer(this.PrefabId);
					obj = ((layer != null) ? layer.Sprite : null);
				}
			}
			else
			{
				obj = null;
			}
			object obj2;
			if ((obj2 = obj) == null)
			{
				Brush spriteBrush2 = this.SpriteBrush;
				if (spriteBrush2 == null)
				{
					obj2 = null;
				}
				else
				{
					BrushLayer defaultLayer = spriteBrush2.DefaultLayer;
					obj2 = ((defaultLayer != null) ? defaultLayer.Sprite : null);
				}
			}
			Sprite sprite = obj2;
			this._childWidget.Brush.DefaultLayer.Sprite = sprite;
			if (sprite != null)
			{
				this._childWidget.IsVisible = true;
				float num;
				float num2;
				if (base.WidthSizePolicy == SizePolicy.CoverChildren && base.HeightSizePolicy == SizePolicy.CoverChildren)
				{
					num = (float)sprite.Width;
					num2 = (float)sprite.Height;
				}
				else if (base.WidthSizePolicy == SizePolicy.CoverChildren)
				{
					num2 = base.Size.Y * base._inverseScaleToUse;
					num = num2 * (float)sprite.Width / (float)sprite.Height;
				}
				else if (base.HeightSizePolicy == SizePolicy.CoverChildren)
				{
					num = base.Size.X * base._inverseScaleToUse;
					num2 = num * (float)sprite.Height / (float)sprite.Width;
				}
				else
				{
					float num3 = base.Size.X * base._inverseScaleToUse;
					float num4 = base.Size.Y * base._inverseScaleToUse;
					float num5 = num3 / (float)sprite.Width;
					float num6 = num4 / (float)sprite.Height;
					float num7 = Math.Max(num5, num6);
					num = (float)sprite.Width * num7;
					num2 = (float)sprite.Height * num7;
				}
				this._childWidget.SuggestedWidth = num;
				this._childWidget.SuggestedHeight = num2;
				return;
			}
			this._childWidget.IsVisible = false;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000BDD2 File Offset: 0x00009FD2
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x0000BDDA File Offset: 0x00009FDA
		[Editor(false)]
		public string PrefabId
		{
			get
			{
				return this._prefabId;
			}
			set
			{
				if (this._prefabId != value)
				{
					this._prefabId = value;
					base.OnPropertyChanged<string>(value, "PrefabId");
					this._shouldUpdateSprite = true;
				}
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0000BE04 File Offset: 0x0000A004
		// (set) Token: 0x060003BA RID: 954 RVA: 0x0000BE0C File Offset: 0x0000A00C
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
					this._shouldUpdateSprite = true;
				}
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0000BE31 File Offset: 0x0000A031
		// (set) Token: 0x060003BC RID: 956 RVA: 0x0000BE39 File Offset: 0x0000A039
		[Editor(false)]
		public Brush StyleBrush
		{
			get
			{
				return this._styleBrush;
			}
			set
			{
				if (this._styleBrush != value)
				{
					this._styleBrush = value;
					base.OnPropertyChanged<Brush>(value, "StyleBrush");
					this._childWidget.Brush = value;
					this._shouldUpdateSprite = true;
				}
			}
		}

		// Token: 0x0400018C RID: 396
		private readonly BrushWidget _childWidget;

		// Token: 0x0400018D RID: 397
		private bool _shouldUpdateSprite;

		// Token: 0x0400018E RID: 398
		private Vec2 _previousSize;

		// Token: 0x0400018F RID: 399
		private string _prefabId;

		// Token: 0x04000190 RID: 400
		private Brush _spriteBrush;

		// Token: 0x04000191 RID: 401
		private Brush _styleBrush;
	}
}
