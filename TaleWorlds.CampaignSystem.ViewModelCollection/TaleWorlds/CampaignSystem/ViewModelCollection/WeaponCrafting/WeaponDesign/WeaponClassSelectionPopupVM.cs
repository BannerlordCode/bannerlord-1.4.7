using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000105 RID: 261
	public class WeaponClassSelectionPopupVM : ViewModel
	{
		// Token: 0x06001796 RID: 6038 RVA: 0x0005A9B4 File Offset: 0x00058BB4
		public WeaponClassSelectionPopupVM(ICraftingCampaignBehavior craftingBehavior, List<CraftingTemplate> templatesList, Action<int> onSelect, Func<CraftingTemplate, int> getUnlockedPiecesCount)
		{
			this.WeaponClasses = new MBBindingList<WeaponClassVM>();
			this._craftingBehavior = craftingBehavior;
			this._onSelect = onSelect;
			this._templatesList = templatesList;
			this._getUnlockedPiecesCount = getUnlockedPiecesCount;
			foreach (CraftingTemplate craftingTemplate in this._templatesList)
			{
				this.WeaponClasses.Add(new WeaponClassVM(this._templatesList.IndexOf(craftingTemplate), craftingTemplate, new Action<int>(this.ExecuteSelectWeaponClass)));
			}
			this.RefreshList();
			this.RefreshValues();
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x0005AA64 File Offset: 0x00058C64
		private void RefreshList()
		{
			foreach (WeaponClassVM weaponClassVM in this.WeaponClasses)
			{
				WeaponClassVM weaponClassVM2 = weaponClassVM;
				Func<CraftingTemplate, int> getUnlockedPiecesCount = this._getUnlockedPiecesCount;
				weaponClassVM2.UnlockedPiecesCount = ((getUnlockedPiecesCount != null) ? getUnlockedPiecesCount(weaponClassVM.Template) : 0);
				weaponClassVM.HasNewlyUnlockedPieces = weaponClassVM.NewlyUnlockedPieceCount > 0;
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x0005AAD8 File Offset: 0x00058CD8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PopupHeader = new TextObject("{=wZGj3qO1}Choose What to Craft", null).ToString();
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x0005AAF8 File Offset: 0x00058CF8
		public void UpdateNewlyUnlockedPiecesCount(List<CraftingPiece> newlyUnlockedPieces)
		{
			for (int i = 0; i < this.WeaponClasses.Count; i++)
			{
				WeaponClassVM weaponClassVM = this.WeaponClasses[i];
				int num = 0;
				for (int j = 0; j < newlyUnlockedPieces.Count; j++)
				{
					CraftingPiece craftingPiece = newlyUnlockedPieces[j];
					if (weaponClassVM.Template.IsPieceTypeUsable(craftingPiece.PieceType))
					{
						CraftingPiece craftingPiece2 = this.FindPieceInTemplate(weaponClassVM.Template, craftingPiece);
						if (craftingPiece2 != null && !craftingPiece2.IsHiddenOnDesigner && this._craftingBehavior.IsOpened(craftingPiece2, weaponClassVM.Template))
						{
							num++;
						}
					}
				}
				weaponClassVM.NewlyUnlockedPieceCount = num;
			}
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x0005AB98 File Offset: 0x00058D98
		private CraftingPiece FindPieceInTemplate(CraftingTemplate template, CraftingPiece piece)
		{
			foreach (CraftingPiece craftingPiece in template.Pieces)
			{
				if (piece.StringId == craftingPiece.StringId)
				{
					return craftingPiece;
				}
			}
			return null;
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x0005AC00 File Offset: 0x00058E00
		public void ExecuteSelectWeaponClass(int index)
		{
			if (this.WeaponClasses[index].IsSelected)
			{
				this.ExecuteClosePopup();
				return;
			}
			Action<int> onSelect = this._onSelect;
			if (onSelect != null)
			{
				onSelect(index);
			}
			this.ExecuteClosePopup();
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x0005AC34 File Offset: 0x00058E34
		public void ExecuteClosePopup()
		{
			this.IsVisible = false;
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x0005AC3D File Offset: 0x00058E3D
		public void ExecuteOpenPopup()
		{
			this.IsVisible = true;
			this.RefreshList();
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x0600179E RID: 6046 RVA: 0x0005AC4C File Offset: 0x00058E4C
		// (set) Token: 0x0600179F RID: 6047 RVA: 0x0005AC54 File Offset: 0x00058E54
		[DataSourceProperty]
		public string PopupHeader
		{
			get
			{
				return this._popupHeader;
			}
			set
			{
				if (value != this._popupHeader)
				{
					this._popupHeader = value;
					base.OnPropertyChangedWithValue<string>(value, "PopupHeader");
				}
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x060017A0 RID: 6048 RVA: 0x0005AC77 File Offset: 0x00058E77
		// (set) Token: 0x060017A1 RID: 6049 RVA: 0x0005AC7F File Offset: 0x00058E7F
		[DataSourceProperty]
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value != this._isVisible)
				{
					this._isVisible = value;
					base.OnPropertyChangedWithValue(value, "IsVisible");
					Game game = Game.Current;
					if (game == null)
					{
						return;
					}
					game.EventManager.TriggerEvent<CraftingWeaponClassSelectionOpenedEvent>(new CraftingWeaponClassSelectionOpenedEvent(this._isVisible));
				}
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x060017A2 RID: 6050 RVA: 0x0005ACBC File Offset: 0x00058EBC
		// (set) Token: 0x060017A3 RID: 6051 RVA: 0x0005ACC4 File Offset: 0x00058EC4
		[DataSourceProperty]
		public MBBindingList<WeaponClassVM> WeaponClasses
		{
			get
			{
				return this._weaponClasses;
			}
			set
			{
				if (value != this._weaponClasses)
				{
					this._weaponClasses = value;
					base.OnPropertyChangedWithValue<MBBindingList<WeaponClassVM>>(value, "WeaponClasses");
				}
			}
		}

		// Token: 0x04000ACB RID: 2763
		private readonly ICraftingCampaignBehavior _craftingBehavior;

		// Token: 0x04000ACC RID: 2764
		private readonly Action<int> _onSelect;

		// Token: 0x04000ACD RID: 2765
		private readonly List<CraftingTemplate> _templatesList;

		// Token: 0x04000ACE RID: 2766
		private readonly Func<CraftingTemplate, int> _getUnlockedPiecesCount;

		// Token: 0x04000ACF RID: 2767
		private string _popupHeader;

		// Token: 0x04000AD0 RID: 2768
		private bool _isVisible;

		// Token: 0x04000AD1 RID: 2769
		private MBBindingList<WeaponClassVM> _weaponClasses;
	}
}
