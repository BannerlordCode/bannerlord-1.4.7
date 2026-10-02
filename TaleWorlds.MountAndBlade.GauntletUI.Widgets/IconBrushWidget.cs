using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000025 RID: 37
	public class IconBrushWidget : ButtonWidget
	{
		// Token: 0x060001EE RID: 494 RVA: 0x00007445 File Offset: 0x00005645
		public IconBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007450 File Offset: 0x00005650
		private void UpdateIcon()
		{
			if (this.IconBrush == null || string.IsNullOrEmpty(this.IconID))
			{
				return;
			}
			BrushLayer layer = this.IconBrush.GetLayer(this.IconID);
			if (base.Brush != null)
			{
				Sprite sprite = ((layer != null) ? layer.Sprite : null);
				base.Brush.Sprite = sprite;
				if (sprite != null && this.UseIconSize)
				{
					base.SuggestedWidth = (float)sprite.Width;
					base.SuggestedHeight = (float)sprite.Height;
				}
				foreach (BrushLayer brushLayer in base.Brush.Layers)
				{
					if (this.UseStylesFromSourceIcon && layer != null)
					{
						brushLayer.FillFrom(layer);
					}
					else
					{
						brushLayer.Sprite = sprite;
					}
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00007530 File Offset: 0x00005730
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x00007538 File Offset: 0x00005738
		public Brush IconBrush
		{
			get
			{
				return this._iconBrush;
			}
			set
			{
				if (value != this._iconBrush)
				{
					this._iconBrush = value;
					base.OnPropertyChanged<Brush>(value, "IconBrush");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000755C File Offset: 0x0000575C
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x00007564 File Offset: 0x00005764
		public string IconID
		{
			get
			{
				return this._iconId;
			}
			set
			{
				if (value != this._iconId)
				{
					this._iconId = value;
					base.OnPropertyChanged<string>(value, "IconID");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000758D File Offset: 0x0000578D
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x00007595 File Offset: 0x00005795
		public bool UseStylesFromSourceIcon
		{
			get
			{
				return this._useStylesFromSourceIcon;
			}
			set
			{
				if (value != this._useStylesFromSourceIcon)
				{
					this._useStylesFromSourceIcon = value;
					base.OnPropertyChanged(value, "UseStylesFromSourceIcon");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x000075B9 File Offset: 0x000057B9
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x000075C1 File Offset: 0x000057C1
		public bool UseIconSize
		{
			get
			{
				return this._useIconSize;
			}
			set
			{
				if (value != this._useIconSize)
				{
					this._useIconSize = value;
					base.OnPropertyChanged(value, "UseIconSize");
					this.UpdateIcon();
				}
			}
		}

		// Token: 0x040000EB RID: 235
		private Brush _iconBrush;

		// Token: 0x040000EC RID: 236
		private string _iconId;

		// Token: 0x040000ED RID: 237
		private bool _useStylesFromSourceIcon;

		// Token: 0x040000EE RID: 238
		private bool _useIconSize;
	}
}
