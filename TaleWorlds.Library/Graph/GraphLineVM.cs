using System;

namespace TaleWorlds.Library.Graph
{
	// Token: 0x020000B6 RID: 182
	public class GraphLineVM : ViewModel
	{
		// Token: 0x060006CB RID: 1739 RVA: 0x000170AB File Offset: 0x000152AB
		public GraphLineVM(string ID, string name)
		{
			this.Points = new MBBindingList<GraphLinePointVM>();
			this.Name = name;
			this.ID = ID;
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x000170CC File Offset: 0x000152CC
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x000170D4 File Offset: 0x000152D4
		[DataSourceProperty]
		public MBBindingList<GraphLinePointVM> Points
		{
			get
			{
				return this._points;
			}
			set
			{
				if (value != this._points)
				{
					this._points = value;
					base.OnPropertyChangedWithValue<MBBindingList<GraphLinePointVM>>(value, "Points");
				}
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x000170F2 File Offset: 0x000152F2
		// (set) Token: 0x060006CF RID: 1743 RVA: 0x000170FA File Offset: 0x000152FA
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0001711D File Offset: 0x0001531D
		// (set) Token: 0x060006D1 RID: 1745 RVA: 0x00017125 File Offset: 0x00015325
		[DataSourceProperty]
		public string ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if (value != this._ID)
				{
					this._ID = value;
					base.OnPropertyChangedWithValue<string>(value, "ID");
				}
			}
		}

		// Token: 0x04000211 RID: 529
		private MBBindingList<GraphLinePointVM> _points;

		// Token: 0x04000212 RID: 530
		private string _name;

		// Token: 0x04000213 RID: 531
		private string _ID;
	}
}
