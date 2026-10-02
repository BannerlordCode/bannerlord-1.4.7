using System;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.SaveLoad
{
	// Token: 0x02000013 RID: 19
	public class SavedGameGroupVM : ViewModel
	{
		// Token: 0x0600019F RID: 415 RVA: 0x0000819A File Offset: 0x0000639A
		public SavedGameGroupVM()
		{
			this.SavedGamesList = new MBBindingList<SavedGameVM>();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000081AD File Offset: 0x000063AD
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SavedGamesList.ApplyActionOnAllItems(delegate(SavedGameVM s)
			{
				s.RefreshValues();
			});
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000081DF File Offset: 0x000063DF
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x000081E7 File Offset: 0x000063E7
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

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00008205 File Offset: 0x00006405
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x0000820D File Offset: 0x0000640D
		[DataSourceProperty]
		public MBBindingList<SavedGameVM> SavedGamesList
		{
			get
			{
				return this._savedGamesList;
			}
			set
			{
				if (value != this._savedGamesList)
				{
					this._savedGamesList = value;
					base.OnPropertyChangedWithValue<MBBindingList<SavedGameVM>>(value, "SavedGamesList");
				}
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000822B File Offset: 0x0000642B
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00008233 File Offset: 0x00006433
		[DataSourceProperty]
		public string IdentifierID
		{
			get
			{
				return this._identifierID;
			}
			set
			{
				if (value != this._identifierID)
				{
					this._identifierID = value;
					base.OnPropertyChangedWithValue<string>(value, "IdentifierID");
				}
			}
		}

		// Token: 0x040000B3 RID: 179
		private bool _isFilteredOut;

		// Token: 0x040000B4 RID: 180
		private MBBindingList<SavedGameVM> _savedGamesList;

		// Token: 0x040000B5 RID: 181
		private string _identifierID;
	}
}
