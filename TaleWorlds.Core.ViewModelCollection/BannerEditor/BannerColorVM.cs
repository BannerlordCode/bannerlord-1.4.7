using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.BannerEditor
{
	// Token: 0x0200002C RID: 44
	public class BannerColorVM : ViewModel
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00005F11 File Offset: 0x00004111
		public int ColorID { get; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00005F19 File Offset: 0x00004119
		public uint Color { get; }

		// Token: 0x060001DD RID: 477 RVA: 0x00005F24 File Offset: 0x00004124
		public BannerColorVM(int colorID, uint color, Action<BannerColorVM> onSelection)
		{
			this.Color = color;
			this.ColorAsStr = TaleWorlds.Library.Color.FromUint(this.Color).ToString();
			this.ColorID = colorID;
			this._onSelection = onSelection;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00005F6B File Offset: 0x0000416B
		public void ExecuteSelectIcon()
		{
			this._onSelection(this);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00005F79 File Offset: 0x00004179
		public void SetOnSelectionAction(Action<BannerColorVM> onSelection)
		{
			this._onSelection = onSelection;
			this.IsSelected = false;
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00005F89 File Offset: 0x00004189
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00005F91 File Offset: 0x00004191
		[DataSourceProperty]
		public string ColorAsStr
		{
			get
			{
				return this._colorAsStr;
			}
			set
			{
				if (value != this._colorAsStr)
				{
					this._colorAsStr = value;
					base.OnPropertyChangedWithValue<string>(value, "ColorAsStr");
				}
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00005FB4 File Offset: 0x000041B4
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x00005FBC File Offset: 0x000041BC
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

		// Token: 0x040000C7 RID: 199
		private Action<BannerColorVM> _onSelection;

		// Token: 0x040000C8 RID: 200
		private string _colorAsStr;

		// Token: 0x040000C9 RID: 201
		private bool _isSelected;
	}
}
