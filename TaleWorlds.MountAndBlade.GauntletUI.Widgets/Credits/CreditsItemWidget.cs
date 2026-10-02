using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Credits
{
	// Token: 0x0200015F RID: 351
	public class CreditsItemWidget : Widget
	{
		// Token: 0x0600128E RID: 4750 RVA: 0x000330BF File Offset: 0x000312BF
		public CreditsItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x000330C8 File Offset: 0x000312C8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.RefreshItemWidget();
				this._initialized = true;
			}
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x000330E8 File Offset: 0x000312E8
		private void RefreshItemWidget()
		{
			if (!string.IsNullOrEmpty(this.ItemType))
			{
				if (this.CategoryWidget != null)
				{
					this.CategoryWidget.IsVisible = this.ItemType == "Category";
				}
				if (this.SectionWidget != null)
				{
					this.SectionWidget.IsVisible = this.ItemType == "Section";
				}
				if (this.EntryWidget != null)
				{
					this.EntryWidget.IsVisible = this.ItemType == "Entry";
				}
				if (this.EmptyLineWidget != null)
				{
					this.EmptyLineWidget.IsVisible = this.ItemType == "EmptyLine";
				}
				if (this.ImageWidget != null)
				{
					this.ImageWidget.IsVisible = this.ItemType == "Image";
					if (this.ImageWidget.Sprite != null)
					{
						this.ImageWidget.SuggestedWidth = (float)this.ImageWidget.Sprite.Width;
						this.ImageWidget.SuggestedHeight = (float)this.ImageWidget.Sprite.Height;
					}
				}
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x000331F9 File Offset: 0x000313F9
		// (set) Token: 0x06001292 RID: 4754 RVA: 0x00033201 File Offset: 0x00031401
		[Editor(false)]
		public string ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (this._itemType != value)
				{
					this._itemType = value;
					base.OnPropertyChanged<string>(value, "ItemType");
				}
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001293 RID: 4755 RVA: 0x00033224 File Offset: 0x00031424
		// (set) Token: 0x06001294 RID: 4756 RVA: 0x0003322C File Offset: 0x0003142C
		[Editor(false)]
		public Widget CategoryWidget
		{
			get
			{
				return this._categoryWidget;
			}
			set
			{
				if (this._categoryWidget != value)
				{
					this._categoryWidget = value;
					base.OnPropertyChanged<Widget>(value, "CategoryWidget");
				}
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x0003324A File Offset: 0x0003144A
		// (set) Token: 0x06001296 RID: 4758 RVA: 0x00033252 File Offset: 0x00031452
		[Editor(false)]
		public Widget ImageWidget
		{
			get
			{
				return this._imageWidget;
			}
			set
			{
				if (this._imageWidget != value)
				{
					this._imageWidget = value;
					base.OnPropertyChanged<Widget>(value, "ImageWidget");
				}
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06001297 RID: 4759 RVA: 0x00033270 File Offset: 0x00031470
		// (set) Token: 0x06001298 RID: 4760 RVA: 0x00033278 File Offset: 0x00031478
		[Editor(false)]
		public Widget SectionWidget
		{
			get
			{
				return this._sectionWidget;
			}
			set
			{
				if (this._sectionWidget != value)
				{
					this._sectionWidget = value;
					base.OnPropertyChanged<Widget>(value, "SectionWidget");
				}
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x00033296 File Offset: 0x00031496
		// (set) Token: 0x0600129A RID: 4762 RVA: 0x0003329E File Offset: 0x0003149E
		[Editor(false)]
		public Widget EntryWidget
		{
			get
			{
				return this._entryWidget;
			}
			set
			{
				if (this._entryWidget != value)
				{
					this._entryWidget = value;
					base.OnPropertyChanged<Widget>(value, "EntryWidget");
				}
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x0600129B RID: 4763 RVA: 0x000332BC File Offset: 0x000314BC
		// (set) Token: 0x0600129C RID: 4764 RVA: 0x000332C4 File Offset: 0x000314C4
		[Editor(false)]
		public Widget EmptyLineWidget
		{
			get
			{
				return this._emptyLineWidget;
			}
			set
			{
				if (this._emptyLineWidget != value)
				{
					this._emptyLineWidget = value;
					base.OnPropertyChanged<Widget>(value, "EmptyLineWidget");
				}
			}
		}

		// Token: 0x0400086C RID: 2156
		private bool _initialized;

		// Token: 0x0400086D RID: 2157
		private string _itemType;

		// Token: 0x0400086E RID: 2158
		private Widget _categoryWidget;

		// Token: 0x0400086F RID: 2159
		private Widget _sectionWidget;

		// Token: 0x04000870 RID: 2160
		private Widget _entryWidget;

		// Token: 0x04000871 RID: 2161
		private Widget _emptyLineWidget;

		// Token: 0x04000872 RID: 2162
		private Widget _imageWidget;
	}
}
