using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.CustomBattle.CustomBattle
{
	// Token: 0x0200001A RID: 26
	public class CustomBattleSiegeMachineVM : ViewModel
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000090C6 File Offset: 0x000072C6
		// (set) Token: 0x06000129 RID: 297 RVA: 0x000090CE File Offset: 0x000072CE
		public SiegeEngineType SiegeEngineType { get; private set; }

		// Token: 0x0600012A RID: 298 RVA: 0x000090D7 File Offset: 0x000072D7
		public CustomBattleSiegeMachineVM(SiegeEngineType machineType, Action<CustomBattleSiegeMachineVM> onSelection, Action<CustomBattleSiegeMachineVM> onResetSelection)
		{
			this._onSelection = onSelection;
			this._onResetSelection = onResetSelection;
			this.SetMachineType(machineType);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000090F4 File Offset: 0x000072F4
		public void SetMachineType(SiegeEngineType machine)
		{
			this.SiegeEngineType = machine;
			this.Name = ((machine != null) ? machine.StringId : "");
			this.IsRanged = machine != null && machine.IsRanged;
			this.MachineID = ((machine != null) ? machine.StringId : "");
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00009146 File Offset: 0x00007346
		private void OnSelection()
		{
			this._onSelection(this);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00009154 File Offset: 0x00007354
		private void OnResetSelection()
		{
			this._onResetSelection(this);
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00009162 File Offset: 0x00007362
		// (set) Token: 0x0600012F RID: 303 RVA: 0x0000916A File Offset: 0x0000736A
		[DataSourceProperty]
		public bool IsRanged
		{
			get
			{
				return this._isRanged;
			}
			set
			{
				if (value != this._isRanged)
				{
					this._isRanged = value;
					base.OnPropertyChangedWithValue(value, "IsRanged");
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00009188 File Offset: 0x00007388
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00009190 File Offset: 0x00007390
		[DataSourceProperty]
		public string MachineID
		{
			get
			{
				return this._machineID;
			}
			set
			{
				if (value != this._machineID)
				{
					this._machineID = value;
					base.OnPropertyChangedWithValue<string>(value, "MachineID");
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000132 RID: 306 RVA: 0x000091B3 File Offset: 0x000073B3
		// (set) Token: 0x06000133 RID: 307 RVA: 0x000091BB File Offset: 0x000073BB
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

		// Token: 0x040000CD RID: 205
		private Action<CustomBattleSiegeMachineVM> _onSelection;

		// Token: 0x040000CE RID: 206
		private Action<CustomBattleSiegeMachineVM> _onResetSelection;

		// Token: 0x040000D0 RID: 208
		private string _name;

		// Token: 0x040000D1 RID: 209
		private bool _isRanged;

		// Token: 0x040000D2 RID: 210
		private string _machineID;
	}
}
