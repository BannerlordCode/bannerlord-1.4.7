using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000109 RID: 265
	public class WeaponClassVM : ViewModel
	{
		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x060017AD RID: 6061 RVA: 0x0005AD42 File Offset: 0x00058F42
		// (set) Token: 0x060017AE RID: 6062 RVA: 0x0005AD4A File Offset: 0x00058F4A
		public int NewlyUnlockedPieceCount { get; set; }

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x0005AD53 File Offset: 0x00058F53
		public CraftingTemplate Template { get; }

		// Token: 0x060017B0 RID: 6064 RVA: 0x0005AD5C File Offset: 0x00058F5C
		public WeaponClassVM(int selectionIndex, CraftingTemplate template, Action<int> onSelect)
		{
			this._onSelect = onSelect;
			this.SelectionIndex = selectionIndex;
			this.Template = template;
			this._selectedPieces = new Dictionary<CraftingPiece.PieceTypes, string>
			{
				{
					CraftingPiece.PieceTypes.Blade,
					null
				},
				{
					CraftingPiece.PieceTypes.Guard,
					null
				},
				{
					CraftingPiece.PieceTypes.Handle,
					null
				},
				{
					CraftingPiece.PieceTypes.Pommel,
					null
				}
			};
			this.RefreshValues();
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x0005ADB8 File Offset: 0x00058FB8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TemplateName = this.Template.TemplateName.ToString();
			this.UnlockedPiecesLabelText = new TextObject("{=OGbskMfz}Unlocked Parts:", null).ToString();
			this.WeaponType = this.Template.StringId;
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x0005AE08 File Offset: 0x00059008
		public void RegisterSelectedPiece(CraftingPiece.PieceTypes type, string pieceID)
		{
			string text;
			if (this._selectedPieces.TryGetValue(type, out text) && text != pieceID)
			{
				this._selectedPieces[type] = pieceID;
			}
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x0005AE3C File Offset: 0x0005903C
		public string GetSelectedPieceData(CraftingPiece.PieceTypes type)
		{
			string text;
			if (this._selectedPieces.TryGetValue(type, out text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0005AE5C File Offset: 0x0005905C
		public void ExecuteSelect()
		{
			Action<int> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this.SelectionIndex);
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060017B5 RID: 6069 RVA: 0x0005AE74 File Offset: 0x00059074
		// (set) Token: 0x060017B6 RID: 6070 RVA: 0x0005AE7C File Offset: 0x0005907C
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

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060017B7 RID: 6071 RVA: 0x0005AE9A File Offset: 0x0005909A
		// (set) Token: 0x060017B8 RID: 6072 RVA: 0x0005AEA2 File Offset: 0x000590A2
		[DataSourceProperty]
		public string UnlockedPiecesLabelText
		{
			get
			{
				return this._unlockedPiecesLabelText;
			}
			set
			{
				if (value != this._unlockedPiecesLabelText)
				{
					this._unlockedPiecesLabelText = value;
					base.OnPropertyChangedWithValue<string>(value, "UnlockedPiecesLabelText");
				}
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060017B9 RID: 6073 RVA: 0x0005AEC5 File Offset: 0x000590C5
		// (set) Token: 0x060017BA RID: 6074 RVA: 0x0005AECD File Offset: 0x000590CD
		[DataSourceProperty]
		public int UnlockedPiecesCount
		{
			get
			{
				return this._unlockedPiecesCount;
			}
			set
			{
				if (value != this._unlockedPiecesCount)
				{
					this._unlockedPiecesCount = value;
					base.OnPropertyChangedWithValue(value, "UnlockedPiecesCount");
				}
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060017BB RID: 6075 RVA: 0x0005AEEB File Offset: 0x000590EB
		// (set) Token: 0x060017BC RID: 6076 RVA: 0x0005AEF3 File Offset: 0x000590F3
		[DataSourceProperty]
		public string TemplateName
		{
			get
			{
				return this._templateName;
			}
			set
			{
				if (value != this._templateName)
				{
					this._templateName = value;
					base.OnPropertyChangedWithValue<string>(value, "TemplateName");
				}
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x0005AF16 File Offset: 0x00059116
		// (set) Token: 0x060017BE RID: 6078 RVA: 0x0005AF1E File Offset: 0x0005911E
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

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060017BF RID: 6079 RVA: 0x0005AF3C File Offset: 0x0005913C
		// (set) Token: 0x060017C0 RID: 6080 RVA: 0x0005AF44 File Offset: 0x00059144
		[DataSourceProperty]
		public int SelectionIndex
		{
			get
			{
				return this._selectionIndex;
			}
			set
			{
				if (value != this._selectionIndex)
				{
					this._selectionIndex = value;
					base.OnPropertyChangedWithValue(value, "SelectionIndex");
				}
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x0005AF62 File Offset: 0x00059162
		// (set) Token: 0x060017C2 RID: 6082 RVA: 0x0005AF6A File Offset: 0x0005916A
		[DataSourceProperty]
		public string WeaponType
		{
			get
			{
				return this._weaponType;
			}
			set
			{
				if (value != this._weaponType)
				{
					this._weaponType = value;
					base.OnPropertyChangedWithValue<string>(value, "WeaponType");
				}
			}
		}

		// Token: 0x04000AD7 RID: 2775
		private Action<int> _onSelect;

		// Token: 0x04000AD8 RID: 2776
		private Dictionary<CraftingPiece.PieceTypes, string> _selectedPieces;

		// Token: 0x04000AD9 RID: 2777
		private bool _hasNewlyUnlockedPieces;

		// Token: 0x04000ADA RID: 2778
		private string _unlockedPiecesLabelText;

		// Token: 0x04000ADB RID: 2779
		private int _unlockedPiecesCount;

		// Token: 0x04000ADC RID: 2780
		private string _templateName;

		// Token: 0x04000ADD RID: 2781
		private bool _isSelected;

		// Token: 0x04000ADE RID: 2782
		private int _selectionIndex;

		// Token: 0x04000ADF RID: 2783
		private string _weaponType;
	}
}
