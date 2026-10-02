using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000102 RID: 258
	public class CraftingPieceVM : ViewModel
	{
		// Token: 0x0600176F RID: 5999 RVA: 0x0005A4E9 File Offset: 0x000586E9
		public CraftingPieceVM()
		{
			this.ImageIdentifier = new CraftingPieceImageIdentifierVM(null, string.Empty);
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x0005A50C File Offset: 0x0005870C
		public CraftingPieceVM(Action<CraftingPieceVM> selectWeaponPart, string templateId, WeaponDesignElement usableCraftingPiece, int pieceType, int index, bool isOpened)
		{
			this._selectWeaponPiece = selectWeaponPart;
			this.CraftingPiece = usableCraftingPiece;
			this.Tier = usableCraftingPiece.CraftingPiece.PieceTier;
			this.TierText = Common.ToRoman(this.Tier);
			this.ImageIdentifier = new CraftingPieceImageIdentifierVM(usableCraftingPiece.CraftingPiece, templateId);
			this.PieceType = pieceType;
			this.Index = index;
			this.PlayerHasPiece = isOpened;
			this.ItemAttributeIcons = new MBBindingList<CraftingItemFlagVM>();
			this.IsEmpty = string.IsNullOrEmpty(this.CraftingPiece.CraftingPiece.MeshName);
			this.RefreshFlagIcons();
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x0005A5AC File Offset: 0x000587AC
		public void RefreshFlagIcons()
		{
			this.ItemAttributeIcons.Clear();
			foreach (Tuple<string, TextObject> tuple in CampaignUIHelper.GetItemFlagDetails(this.CraftingPiece.CraftingPiece.AdditionalItemFlags))
			{
				this.ItemAttributeIcons.Add(new CraftingItemFlagVM(tuple.Item1, tuple.Item2, true));
			}
			foreach (ValueTuple<string, TextObject> valueTuple in CampaignUIHelper.GetWeaponFlagDetails(this.CraftingPiece.CraftingPiece.AdditionalWeaponFlags, null))
			{
				this.ItemAttributeIcons.Add(new CraftingItemFlagVM(valueTuple.Item1, valueTuple.Item2, true));
			}
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x0005A698 File Offset: 0x00058898
		public void ExecuteOpenTooltip()
		{
			InformationManager.ShowTooltip(typeof(WeaponDesignElement), new object[] { this.CraftingPiece });
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0005A6B8 File Offset: 0x000588B8
		public void ExecuteCloseTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x0005A6BF File Offset: 0x000588BF
		public void ExecuteSelect()
		{
			this._selectWeaponPiece(this);
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06001775 RID: 6005 RVA: 0x0005A6CD File Offset: 0x000588CD
		// (set) Token: 0x06001776 RID: 6006 RVA: 0x0005A6D5 File Offset: 0x000588D5
		[DataSourceProperty]
		public bool IsFilteredOut
		{
			get
			{
				return this._isFilteredOut;
			}
			set
			{
				if (value != this._isFilteredOut)
				{
					this._isFilteredOut = value;
					base.OnPropertyChangedWithValue(value, "IsFilteredOut");
				}
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06001777 RID: 6007 RVA: 0x0005A6F3 File Offset: 0x000588F3
		// (set) Token: 0x06001778 RID: 6008 RVA: 0x0005A6FB File Offset: 0x000588FB
		[DataSourceProperty]
		public MBBindingList<CraftingItemFlagVM> ItemAttributeIcons
		{
			get
			{
				return this._itemAttributeIcons;
			}
			set
			{
				if (value != this._itemAttributeIcons)
				{
					this._itemAttributeIcons = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingItemFlagVM>>(value, "ItemAttributeIcons");
				}
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x0005A719 File Offset: 0x00058919
		// (set) Token: 0x0600177A RID: 6010 RVA: 0x0005A721 File Offset: 0x00058921
		[DataSourceProperty]
		public bool PlayerHasPiece
		{
			get
			{
				return this._playerHasPiece;
			}
			set
			{
				if (this._playerHasPiece != value)
				{
					this._playerHasPiece = value;
					base.OnPropertyChangedWithValue(value, "PlayerHasPiece");
				}
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x0600177B RID: 6011 RVA: 0x0005A73F File Offset: 0x0005893F
		// (set) Token: 0x0600177C RID: 6012 RVA: 0x0005A747 File Offset: 0x00058947
		[DataSourceProperty]
		public bool IsEmpty
		{
			get
			{
				return this._isEmpty;
			}
			set
			{
				if (this._isEmpty != value)
				{
					this._isEmpty = value;
					base.OnPropertyChangedWithValue(value, "IsEmpty");
				}
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x0600177D RID: 6013 RVA: 0x0005A765 File Offset: 0x00058965
		// (set) Token: 0x0600177E RID: 6014 RVA: 0x0005A76D File Offset: 0x0005896D
		[DataSourceProperty]
		public string TierText
		{
			get
			{
				return this._tierText;
			}
			set
			{
				if (this._tierText != value)
				{
					this._tierText = value;
					base.OnPropertyChangedWithValue<string>(value, "TierText");
				}
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x0600177F RID: 6015 RVA: 0x0005A790 File Offset: 0x00058990
		// (set) Token: 0x06001780 RID: 6016 RVA: 0x0005A798 File Offset: 0x00058998
		[DataSourceProperty]
		public int Tier
		{
			get
			{
				return this._tier;
			}
			set
			{
				if (this._tier != value)
				{
					this._tier = value;
					base.OnPropertyChangedWithValue(value, "Tier");
				}
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x0005A7B6 File Offset: 0x000589B6
		// (set) Token: 0x06001782 RID: 6018 RVA: 0x0005A7BE File Offset: 0x000589BE
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this._isSelected != value)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06001783 RID: 6019 RVA: 0x0005A7DC File Offset: 0x000589DC
		// (set) Token: 0x06001784 RID: 6020 RVA: 0x0005A7E4 File Offset: 0x000589E4
		[DataSourceProperty]
		public CraftingPieceImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (this._imageIdentifier != value)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<CraftingPieceImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06001785 RID: 6021 RVA: 0x0005A802 File Offset: 0x00058A02
		// (set) Token: 0x06001786 RID: 6022 RVA: 0x0005A80A File Offset: 0x00058A0A
		[DataSourceProperty]
		public int PieceType
		{
			get
			{
				return this._pieceType;
			}
			set
			{
				if (this._pieceType != value)
				{
					this._pieceType = value;
					base.OnPropertyChangedWithValue(value, "PieceType");
				}
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06001787 RID: 6023 RVA: 0x0005A828 File Offset: 0x00058A28
		// (set) Token: 0x06001788 RID: 6024 RVA: 0x0005A830 File Offset: 0x00058A30
		[DataSourceProperty]
		public bool IsNewlyUnlocked
		{
			get
			{
				return this._isNewlyUnlocked;
			}
			set
			{
				if (value != this._isNewlyUnlocked)
				{
					this._isNewlyUnlocked = value;
					base.OnPropertyChangedWithValue(value, "IsNewlyUnlocked");
				}
			}
		}

		// Token: 0x04000AB6 RID: 2742
		public WeaponDesignElement CraftingPiece;

		// Token: 0x04000AB7 RID: 2743
		public int Index;

		// Token: 0x04000AB8 RID: 2744
		private readonly Action<CraftingPieceVM> _selectWeaponPiece;

		// Token: 0x04000AB9 RID: 2745
		private bool _isFilteredOut;

		// Token: 0x04000ABA RID: 2746
		public CraftingPieceImageIdentifierVM _imageIdentifier;

		// Token: 0x04000ABB RID: 2747
		public int _pieceType = -1;

		// Token: 0x04000ABC RID: 2748
		public int _tier;

		// Token: 0x04000ABD RID: 2749
		public bool _isSelected;

		// Token: 0x04000ABE RID: 2750
		public bool _playerHasPiece;

		// Token: 0x04000ABF RID: 2751
		private bool _isEmpty;

		// Token: 0x04000AC0 RID: 2752
		public string _tierText;

		// Token: 0x04000AC1 RID: 2753
		private MBBindingList<CraftingItemFlagVM> _itemAttributeIcons;

		// Token: 0x04000AC2 RID: 2754
		private bool _isNewlyUnlocked;
	}
}
