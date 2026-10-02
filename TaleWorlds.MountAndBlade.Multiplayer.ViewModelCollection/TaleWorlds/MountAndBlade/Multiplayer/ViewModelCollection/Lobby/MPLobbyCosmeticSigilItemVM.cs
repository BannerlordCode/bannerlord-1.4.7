using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000029 RID: 41
	public class MPLobbyCosmeticSigilItemVM : MPLobbySigilItemVM
	{
		// Token: 0x06000301 RID: 769 RVA: 0x0000BD26 File Offset: 0x00009F26
		public MPLobbyCosmeticSigilItemVM(int iconID, int rarity, int cost, string cosmeticID)
			: base(iconID, null)
		{
			this.Rarity = rarity;
			this.Cost = cost;
			this.CosmeticID = cosmeticID;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000BD46 File Offset: 0x00009F46
		public static void SetOnSelectionCallback(Action<MPLobbyCosmeticSigilItemVM> onSelection)
		{
			MPLobbyCosmeticSigilItemVM._onSelection = onSelection;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000BD4E File Offset: 0x00009F4E
		public static void ResetOnSelectionCallback()
		{
			MPLobbyCosmeticSigilItemVM._onSelection = null;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000BD56 File Offset: 0x00009F56
		public static void SetOnObtainRequestedCallback(Action<MPLobbyCosmeticSigilItemVM> onObtainRequested)
		{
			MPLobbyCosmeticSigilItemVM._onObtainRequested = onObtainRequested;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000BD5E File Offset: 0x00009F5E
		public static void ResetOnObtainRequestedCallback()
		{
			MPLobbyCosmeticSigilItemVM._onObtainRequested = null;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000BD66 File Offset: 0x00009F66
		private void ExecuteSelection()
		{
			if (this.IsUnlocked)
			{
				Action<MPLobbyCosmeticSigilItemVM> onSelection = MPLobbyCosmeticSigilItemVM._onSelection;
				if (onSelection == null)
				{
					return;
				}
				onSelection(this);
				return;
			}
			else
			{
				Action<MPLobbyCosmeticSigilItemVM> onObtainRequested = MPLobbyCosmeticSigilItemVM._onObtainRequested;
				if (onObtainRequested == null)
				{
					return;
				}
				onObtainRequested(this);
				return;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000BD91 File Offset: 0x00009F91
		// (set) Token: 0x06000308 RID: 776 RVA: 0x0000BD99 File Offset: 0x00009F99
		[DataSourceProperty]
		public bool IsUnlocked
		{
			get
			{
				return this._isUnlocked;
			}
			set
			{
				if (value != this._isUnlocked)
				{
					this._isUnlocked = value;
					base.OnPropertyChangedWithValue(value, "IsUnlocked");
				}
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000BDB7 File Offset: 0x00009FB7
		// (set) Token: 0x0600030A RID: 778 RVA: 0x0000BDBF File Offset: 0x00009FBF
		[DataSourceProperty]
		public bool IsUsed
		{
			get
			{
				return this._isUsed;
			}
			set
			{
				if (value != this._isUsed)
				{
					this._isUsed = value;
					base.OnPropertyChangedWithValue(value, "IsUsed");
				}
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600030B RID: 779 RVA: 0x0000BDDD File Offset: 0x00009FDD
		// (set) Token: 0x0600030C RID: 780 RVA: 0x0000BDE5 File Offset: 0x00009FE5
		[DataSourceProperty]
		public int Rarity
		{
			get
			{
				return this._rarity;
			}
			set
			{
				if (value != this._rarity)
				{
					this._rarity = value;
					base.OnPropertyChangedWithValue(value, "Rarity");
				}
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000BE03 File Offset: 0x0000A003
		// (set) Token: 0x0600030E RID: 782 RVA: 0x0000BE0B File Offset: 0x0000A00B
		[DataSourceProperty]
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				if (value != this._cost)
				{
					this._cost = value;
					base.OnPropertyChangedWithValue(value, "Cost");
				}
			}
		}

		// Token: 0x04000190 RID: 400
		public readonly string CosmeticID;

		// Token: 0x04000191 RID: 401
		private static Action<MPLobbyCosmeticSigilItemVM> _onSelection;

		// Token: 0x04000192 RID: 402
		private static Action<MPLobbyCosmeticSigilItemVM> _onObtainRequested;

		// Token: 0x04000193 RID: 403
		private bool _isUnlocked;

		// Token: 0x04000194 RID: 404
		private bool _isUsed;

		// Token: 0x04000195 RID: 405
		private int _rarity;

		// Token: 0x04000196 RID: 406
		private int _cost;
	}
}
