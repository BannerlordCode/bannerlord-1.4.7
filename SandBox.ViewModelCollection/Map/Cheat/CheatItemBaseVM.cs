using System;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map.Cheat
{
	// Token: 0x02000050 RID: 80
	public abstract class CheatItemBaseVM : ViewModel
	{
		// Token: 0x060004EA RID: 1258 RVA: 0x00012D27 File Offset: 0x00010F27
		public CheatItemBaseVM()
		{
		}

		// Token: 0x060004EB RID: 1259
		public abstract void ExecuteAction();

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00012D2F File Offset: 0x00010F2F
		// (set) Token: 0x060004ED RID: 1261 RVA: 0x00012D37 File Offset: 0x00010F37
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

		// Token: 0x0400026E RID: 622
		private string _name;
	}
}
