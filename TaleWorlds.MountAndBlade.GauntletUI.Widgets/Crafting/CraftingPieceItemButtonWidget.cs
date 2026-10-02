using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000167 RID: 359
	public class CraftingPieceItemButtonWidget : ButtonWidget
	{
		// Token: 0x060012F0 RID: 4848 RVA: 0x00033D60 File Offset: 0x00031F60
		public CraftingPieceItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x00033D69 File Offset: 0x00031F69
		private void UpdateSelfBrush()
		{
			if (this.DontHavePieceBrush == null || this.HasPieceBrush == null)
			{
				return;
			}
			base.Brush = (this.PlayerHasPiece ? this.HasPieceBrush : this.DontHavePieceBrush);
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x00033D98 File Offset: 0x00031F98
		private void UpdateMaterialBrush()
		{
			if (this.DontHavePieceMaterialBrush == null || this.HasPieceMaterialBrush == null || this.ImageIdentifier == null)
			{
				return;
			}
			this.ImageIdentifier.Brush = (this.PlayerHasPiece ? this.HasPieceMaterialBrush : this.DontHavePieceMaterialBrush);
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x00033DD4 File Offset: 0x00031FD4
		// (set) Token: 0x060012F4 RID: 4852 RVA: 0x00033DDC File Offset: 0x00031FDC
		public ImageIdentifierWidget ImageIdentifier
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
					this.UpdateMaterialBrush();
				}
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x00033DF4 File Offset: 0x00031FF4
		// (set) Token: 0x060012F6 RID: 4854 RVA: 0x00033DFC File Offset: 0x00031FFC
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
					this.UpdateSelfBrush();
					this.UpdateMaterialBrush();
				}
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x00033E1A File Offset: 0x0003201A
		// (set) Token: 0x060012F8 RID: 4856 RVA: 0x00033E22 File Offset: 0x00032022
		public Brush HasPieceBrush
		{
			get
			{
				return this._hasPieceBrush;
			}
			set
			{
				if (this._hasPieceBrush != value)
				{
					this._hasPieceBrush = value;
					this.UpdateSelfBrush();
				}
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x00033E3A File Offset: 0x0003203A
		// (set) Token: 0x060012FA RID: 4858 RVA: 0x00033E42 File Offset: 0x00032042
		public Brush DontHavePieceBrush
		{
			get
			{
				return this._dontHavePieceBrush;
			}
			set
			{
				if (this._dontHavePieceBrush != value)
				{
					this._dontHavePieceBrush = value;
					this.UpdateSelfBrush();
				}
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x00033E5A File Offset: 0x0003205A
		// (set) Token: 0x060012FC RID: 4860 RVA: 0x00033E62 File Offset: 0x00032062
		public Brush HasPieceMaterialBrush
		{
			get
			{
				return this._hasPieceMaterialBrush;
			}
			set
			{
				if (this._hasPieceMaterialBrush != value)
				{
					this._hasPieceMaterialBrush = value;
					this.UpdateMaterialBrush();
				}
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x00033E7A File Offset: 0x0003207A
		// (set) Token: 0x060012FE RID: 4862 RVA: 0x00033E82 File Offset: 0x00032082
		public Brush DontHavePieceMaterialBrush
		{
			get
			{
				return this._dontHavePieceMaterialBrush;
			}
			set
			{
				if (this._dontHavePieceMaterialBrush != value)
				{
					this._dontHavePieceMaterialBrush = value;
					this.UpdateMaterialBrush();
				}
			}
		}

		// Token: 0x04000899 RID: 2201
		private ImageIdentifierWidget _imageIdentifier;

		// Token: 0x0400089A RID: 2202
		private bool _playerHasPiece;

		// Token: 0x0400089B RID: 2203
		private Brush _hasPieceBrush;

		// Token: 0x0400089C RID: 2204
		private Brush _dontHavePieceBrush;

		// Token: 0x0400089D RID: 2205
		private Brush _hasPieceMaterialBrush;

		// Token: 0x0400089E RID: 2206
		private Brush _dontHavePieceMaterialBrush;
	}
}
