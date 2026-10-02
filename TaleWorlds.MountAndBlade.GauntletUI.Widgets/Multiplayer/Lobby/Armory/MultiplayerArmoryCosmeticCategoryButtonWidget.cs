using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B4 RID: 180
	public class MultiplayerArmoryCosmeticCategoryButtonWidget : ButtonWidget
	{
		// Token: 0x06000958 RID: 2392 RVA: 0x0001A545 File Offset: 0x00018745
		public MultiplayerArmoryCosmeticCategoryButtonWidget(UIContext context)
			: base(context)
		{
			this.CosmeticTypeName = string.Empty;
			this.CosmeticCategoryName = string.Empty;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0001A564 File Offset: 0x00018764
		private void UpdateCategorySprite()
		{
			if (string.IsNullOrEmpty(this.CosmeticCategoryName) || string.IsNullOrEmpty(this.CosmeticTypeName))
			{
				return;
			}
			Sprite sprite = null;
			if (this.CosmeticTypeName == "Clothing")
			{
				sprite = this.GetClothingCategorySprite(this.CosmeticCategoryName);
			}
			else if (this.CosmeticTypeName == "Taunt")
			{
				sprite = this.GetTauntCategorySprite(this.CosmeticCategoryName);
			}
			if (sprite != null)
			{
				base.Brush.DefaultLayer.Sprite = sprite;
				base.Brush.Sprite = sprite;
			}
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0001A5EE File Offset: 0x000187EE
		private Sprite GetClothingCategorySprite(string clothingCategory)
		{
			Brush clothingCategorySpriteBrush = this.ClothingCategorySpriteBrush;
			if (clothingCategorySpriteBrush == null)
			{
				return null;
			}
			BrushLayer layer = clothingCategorySpriteBrush.GetLayer(clothingCategory);
			if (layer == null)
			{
				return null;
			}
			return layer.Sprite;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0001A60D File Offset: 0x0001880D
		private Sprite GetTauntCategorySprite(string tauntCategory)
		{
			Brush tauntCategorySpriteBrush = this.TauntCategorySpriteBrush;
			if (tauntCategorySpriteBrush == null)
			{
				return null;
			}
			BrushLayer layer = tauntCategorySpriteBrush.GetLayer(tauntCategory);
			if (layer == null)
			{
				return null;
			}
			return layer.Sprite;
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0001A62C File Offset: 0x0001882C
		// (set) Token: 0x0600095D RID: 2397 RVA: 0x0001A634 File Offset: 0x00018834
		[DataSourceProperty]
		public Brush ClothingCategorySpriteBrush
		{
			get
			{
				return this._clothingCategorySpriteBrush;
			}
			set
			{
				if (value != this._clothingCategorySpriteBrush)
				{
					this._clothingCategorySpriteBrush = value;
					base.OnPropertyChanged<Brush>(value, "ClothingCategorySpriteBrush");
					this.UpdateCategorySprite();
				}
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0001A658 File Offset: 0x00018858
		// (set) Token: 0x0600095F RID: 2399 RVA: 0x0001A660 File Offset: 0x00018860
		[DataSourceProperty]
		public Brush TauntCategorySpriteBrush
		{
			get
			{
				return this._tauntCategorySpriteBrush;
			}
			set
			{
				if (value != this._tauntCategorySpriteBrush)
				{
					this._tauntCategorySpriteBrush = value;
					base.OnPropertyChanged<Brush>(value, "TauntCategorySpriteBrush");
					this.UpdateCategorySprite();
				}
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0001A684 File Offset: 0x00018884
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x0001A68C File Offset: 0x0001888C
		[DataSourceProperty]
		public string CosmeticTypeName
		{
			get
			{
				return this._cosmeticTypeName;
			}
			set
			{
				if (value != this._cosmeticTypeName)
				{
					this._cosmeticTypeName = value;
					base.OnPropertyChanged<string>(value, "CosmeticTypeName");
					this.UpdateCategorySprite();
				}
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0001A6B5 File Offset: 0x000188B5
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x0001A6BD File Offset: 0x000188BD
		[DataSourceProperty]
		public string CosmeticCategoryName
		{
			get
			{
				return this._cosmeticCategoryName;
			}
			set
			{
				if (value != this._cosmeticCategoryName)
				{
					this._cosmeticCategoryName = value;
					base.OnPropertyChanged<string>(value, "CosmeticCategoryName");
					this.UpdateCategorySprite();
				}
			}
		}

		// Token: 0x04000437 RID: 1079
		private const string _clothingTypeName = "Clothing";

		// Token: 0x04000438 RID: 1080
		private const string _tauntTypeName = "Taunt";

		// Token: 0x04000439 RID: 1081
		private Brush _clothingCategorySpriteBrush;

		// Token: 0x0400043A RID: 1082
		private Brush _tauntCategorySpriteBrush;

		// Token: 0x0400043B RID: 1083
		private string _cosmeticTypeName;

		// Token: 0x0400043C RID: 1084
		private string _cosmeticCategoryName;
	}
}
