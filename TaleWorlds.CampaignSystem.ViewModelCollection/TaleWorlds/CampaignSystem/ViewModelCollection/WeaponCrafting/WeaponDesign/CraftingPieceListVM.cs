using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000101 RID: 257
	public class CraftingPieceListVM : ViewModel
	{
		// Token: 0x06001762 RID: 5986 RVA: 0x0005A3BB File Offset: 0x000585BB
		public CraftingPieceListVM(MBBindingList<CraftingPieceVM> pieceList, CraftingPiece.PieceTypes pieceType, Action<CraftingPiece.PieceTypes, bool> onSelect)
		{
			this.Pieces = pieceList;
			this.PieceType = pieceType;
			this._onSelect = onSelect;
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x0005A3D8 File Offset: 0x000585D8
		public void ExecuteSelect()
		{
			Action<CraftingPiece.PieceTypes, bool> onSelect = this._onSelect;
			if (onSelect != null)
			{
				onSelect(this.PieceType, true);
			}
			this.HasNewlyUnlockedPieces = false;
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x0005A3F9 File Offset: 0x000585F9
		public void Refresh()
		{
			this.HasNewlyUnlockedPieces = this.Pieces.Any<CraftingPieceVM>((CraftingPieceVM x) => x.IsNewlyUnlocked);
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06001765 RID: 5989 RVA: 0x0005A42B File Offset: 0x0005862B
		// (set) Token: 0x06001766 RID: 5990 RVA: 0x0005A433 File Offset: 0x00058633
		[DataSourceProperty]
		public bool HasNewlyUnlockedPieces
		{
			get
			{
				return this._hasNewlyUnlockedPieces;
			}
			set
			{
				if (value != this._hasNewlyUnlockedPieces)
				{
					this._hasNewlyUnlockedPieces = value;
					base.OnPropertyChangedWithValue(value, "HasNewlyUnlockedPieces");
				}
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06001767 RID: 5991 RVA: 0x0005A451 File Offset: 0x00058651
		// (set) Token: 0x06001768 RID: 5992 RVA: 0x0005A459 File Offset: 0x00058659
		[DataSourceProperty]
		public MBBindingList<CraftingPieceVM> Pieces
		{
			get
			{
				return this._pieces;
			}
			set
			{
				if (value != this._pieces)
				{
					this._pieces = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingPieceVM>>(value, "Pieces");
				}
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06001769 RID: 5993 RVA: 0x0005A477 File Offset: 0x00058677
		// (set) Token: 0x0600176A RID: 5994 RVA: 0x0005A47F File Offset: 0x0005867F
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

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x0600176B RID: 5995 RVA: 0x0005A49D File Offset: 0x0005869D
		// (set) Token: 0x0600176C RID: 5996 RVA: 0x0005A4A5 File Offset: 0x000586A5
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x0600176D RID: 5997 RVA: 0x0005A4C3 File Offset: 0x000586C3
		// (set) Token: 0x0600176E RID: 5998 RVA: 0x0005A4CB File Offset: 0x000586CB
		[DataSourceProperty]
		public CraftingPieceVM SelectedPiece
		{
			get
			{
				return this._selectedPiece;
			}
			set
			{
				if (value != this._selectedPiece)
				{
					this._selectedPiece = value;
					base.OnPropertyChangedWithValue<CraftingPieceVM>(value, "SelectedPiece");
				}
			}
		}

		// Token: 0x04000AAF RID: 2735
		public CraftingPiece.PieceTypes PieceType;

		// Token: 0x04000AB0 RID: 2736
		private Action<CraftingPiece.PieceTypes, bool> _onSelect;

		// Token: 0x04000AB1 RID: 2737
		private bool _hasNewlyUnlockedPieces;

		// Token: 0x04000AB2 RID: 2738
		private MBBindingList<CraftingPieceVM> _pieces;

		// Token: 0x04000AB3 RID: 2739
		private bool _isSelected;

		// Token: 0x04000AB4 RID: 2740
		private bool _isEnabled;

		// Token: 0x04000AB5 RID: 2741
		private CraftingPieceVM _selectedPiece;
	}
}
