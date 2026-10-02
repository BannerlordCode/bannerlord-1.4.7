using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x02000026 RID: 38
	public class BoolItemWithActionVM : ViewModel
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00005BA0 File Offset: 0x00003DA0
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00005BA8 File Offset: 0x00003DA8
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00005BC6 File Offset: 0x00003DC6
		public void ExecuteAction()
		{
			this._onExecute(this.Identifier);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00005BD9 File Offset: 0x00003DD9
		public BoolItemWithActionVM(Action<object> onExecute, bool isActive, object identifier)
		{
			this._onExecute = onExecute;
			this.Identifier = identifier;
			this.IsActive = isActive;
		}

		// Token: 0x040000AF RID: 175
		public object Identifier;

		// Token: 0x040000B0 RID: 176
		protected Action<object> _onExecute;

		// Token: 0x040000B1 RID: 177
		private bool _isActive;
	}
}
