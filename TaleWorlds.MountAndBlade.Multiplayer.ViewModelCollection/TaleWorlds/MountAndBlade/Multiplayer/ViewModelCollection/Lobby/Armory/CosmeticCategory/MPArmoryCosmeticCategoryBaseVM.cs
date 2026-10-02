using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticCategory
{
	// Token: 0x02000082 RID: 130
	public abstract class MPArmoryCosmeticCategoryBaseVM : ViewModel
	{
		// Token: 0x06000CF4 RID: 3316 RVA: 0x000281EC File Offset: 0x000263EC
		public MPArmoryCosmeticCategoryBaseVM(CosmeticsManager.CosmeticType cosmeticType)
		{
			this.AvailableCosmetics = new MBBindingList<MPArmoryCosmeticItemBaseVM>();
			this.CosmeticType = cosmeticType;
			this.CosmeticTypeName = cosmeticType.ToString();
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00028219 File Offset: 0x00026419
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.AvailableCosmetics.ApplyActionOnAllItems(delegate(MPArmoryCosmeticItemBaseVM c)
			{
				c.RefreshValues();
			});
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0002824B File Offset: 0x0002644B
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.AvailableCosmetics.ApplyActionOnAllItems(delegate(MPArmoryCosmeticItemBaseVM c)
			{
				c.OnFinalize();
			});
		}

		// Token: 0x06000CF7 RID: 3319
		protected abstract void ExecuteSelectCategory();

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0002827D File Offset: 0x0002647D
		public void Sort(MPArmoryCosmeticsVM.CosmeticItemComparer comparer)
		{
			this.AvailableCosmetics.Sort(comparer);
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x0002828B File Offset: 0x0002648B
		// (set) Token: 0x06000CFA RID: 3322 RVA: 0x00028293 File Offset: 0x00026493
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
					base.OnPropertyChangedWithValue<string>(value, "CosmeticTypeName");
				}
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x000282B6 File Offset: 0x000264B6
		// (set) Token: 0x06000CFC RID: 3324 RVA: 0x000282BE File Offset: 0x000264BE
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
					base.OnPropertyChangedWithValue<string>(value, "CosmeticCategoryName");
				}
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x000282E1 File Offset: 0x000264E1
		// (set) Token: 0x06000CFE RID: 3326 RVA: 0x000282E9 File Offset: 0x000264E9
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00028307 File Offset: 0x00026507
		// (set) Token: 0x06000D00 RID: 3328 RVA: 0x0002830F File Offset: 0x0002650F
		[DataSourceProperty]
		public MBBindingList<MPArmoryCosmeticItemBaseVM> AvailableCosmetics
		{
			get
			{
				return this._availableCosmetics;
			}
			set
			{
				if (value != this._availableCosmetics)
				{
					this._availableCosmetics = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPArmoryCosmeticItemBaseVM>>(value, "AvailableCosmetics");
				}
			}
		}

		// Token: 0x040005DE RID: 1502
		public readonly CosmeticsManager.CosmeticType CosmeticType;

		// Token: 0x040005DF RID: 1503
		private string _cosmeticTypeName;

		// Token: 0x040005E0 RID: 1504
		private string _cosmeticCategoryName;

		// Token: 0x040005E1 RID: 1505
		private bool _isSelected;

		// Token: 0x040005E2 RID: 1506
		private MBBindingList<MPArmoryCosmeticItemBaseVM> _availableCosmetics;
	}
}
