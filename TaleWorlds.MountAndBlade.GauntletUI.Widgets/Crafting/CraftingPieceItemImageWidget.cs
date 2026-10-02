using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000168 RID: 360
	public class CraftingPieceItemImageWidget : ImageWidget
	{
		// Token: 0x060012FF RID: 4863 RVA: 0x00033E9A File Offset: 0x0003209A
		public CraftingPieceItemImageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00033EA3 File Offset: 0x000320A3
		private void UpdateSelfBrush()
		{
			if (this.DontHavePieceBrush == null || this.HasPieceBrush == null)
			{
				return;
			}
			base.Brush = (this.PlayerHasPiece ? this.HasPieceBrush : this.DontHavePieceBrush);
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00033ED2 File Offset: 0x000320D2
		private void UpdateMaterialBrush()
		{
			if (this.DontHavePieceMaterialBrush == null || this.HasPieceMaterialBrush == null || this.ImageIdentifier == null)
			{
				return;
			}
			this.ImageIdentifier.Brush = (this.PlayerHasPiece ? this.HasPieceMaterialBrush : this.DontHavePieceMaterialBrush);
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x00033F0E File Offset: 0x0003210E
		// (set) Token: 0x06001303 RID: 4867 RVA: 0x00033F16 File Offset: 0x00032116
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

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001304 RID: 4868 RVA: 0x00033F2E File Offset: 0x0003212E
		// (set) Token: 0x06001305 RID: 4869 RVA: 0x00033F36 File Offset: 0x00032136
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

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06001306 RID: 4870 RVA: 0x00033F54 File Offset: 0x00032154
		// (set) Token: 0x06001307 RID: 4871 RVA: 0x00033F5C File Offset: 0x0003215C
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

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x00033F74 File Offset: 0x00032174
		// (set) Token: 0x06001309 RID: 4873 RVA: 0x00033F7C File Offset: 0x0003217C
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

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x00033F94 File Offset: 0x00032194
		// (set) Token: 0x0600130B RID: 4875 RVA: 0x00033F9C File Offset: 0x0003219C
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

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x00033FB4 File Offset: 0x000321B4
		// (set) Token: 0x0600130D RID: 4877 RVA: 0x00033FBC File Offset: 0x000321BC
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

		// Token: 0x0400089F RID: 2207
		private ImageIdentifierWidget _imageIdentifier;

		// Token: 0x040008A0 RID: 2208
		private bool _playerHasPiece;

		// Token: 0x040008A1 RID: 2209
		private Brush _hasPieceBrush;

		// Token: 0x040008A2 RID: 2210
		private Brush _dontHavePieceBrush;

		// Token: 0x040008A3 RID: 2211
		private Brush _hasPieceMaterialBrush;

		// Token: 0x040008A4 RID: 2212
		private Brush _dontHavePieceMaterialBrush;
	}
}
