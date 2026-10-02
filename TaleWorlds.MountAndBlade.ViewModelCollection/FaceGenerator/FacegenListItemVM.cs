using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.FaceGenerator
{
	// Token: 0x0200007C RID: 124
	public class FacegenListItemVM : ViewModel
	{
		// Token: 0x060009C6 RID: 2502 RVA: 0x000220FA File Offset: 0x000202FA
		public void ExecuteAction()
		{
			this._setSelected(this, true);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x00022109 File Offset: 0x00020309
		public FacegenListItemVM(string imagePath, int index, Action<FacegenListItemVM, bool> setSelected)
		{
			this.ImagePath = imagePath;
			this.Index = index;
			this.IsSelected = false;
			this._setSelected = setSelected;
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0002213B File Offset: 0x0002033B
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x00022143 File Offset: 0x00020343
		[DataSourceProperty]
		public string ImagePath
		{
			get
			{
				return this._imagePath;
			}
			set
			{
				if (value != this._imagePath)
				{
					this._imagePath = value;
					base.OnPropertyChangedWithValue<string>(value, "ImagePath");
				}
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x00022166 File Offset: 0x00020366
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x0002216E File Offset: 0x0002036E
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

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0002218C File Offset: 0x0002038C
		// (set) Token: 0x060009CD RID: 2509 RVA: 0x00022194 File Offset: 0x00020394
		[DataSourceProperty]
		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				if (value != this._index)
				{
					this._index = value;
					base.OnPropertyChangedWithValue(value, "Index");
				}
			}
		}

		// Token: 0x04000451 RID: 1105
		private readonly Action<FacegenListItemVM, bool> _setSelected;

		// Token: 0x04000452 RID: 1106
		private string _imagePath;

		// Token: 0x04000453 RID: 1107
		private bool _isSelected = true;

		// Token: 0x04000454 RID: 1108
		private int _index = -1;
	}
}
