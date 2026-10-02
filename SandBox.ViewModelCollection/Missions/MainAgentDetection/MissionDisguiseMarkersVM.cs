using System;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Missions.MainAgentDetection
{
	// Token: 0x02000043 RID: 67
	public class MissionDisguiseMarkersVM : ViewModel
	{
		// Token: 0x0600045E RID: 1118 RVA: 0x000118F8 File Offset: 0x0000FAF8
		public MissionDisguiseMarkersVM()
		{
			this.HostileAgents = new MBBindingList<MissionDisguiseMarkerItemVM>();
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0001190B File Offset: 0x0000FB0B
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x00011913 File Offset: 0x0000FB13
		[DataSourceProperty]
		public MissionDisguiseMarkerItemVM TargetAgent
		{
			get
			{
				return this._targetAgent;
			}
			set
			{
				if (value != this._targetAgent)
				{
					this._targetAgent = value;
					base.OnPropertyChangedWithValue<MissionDisguiseMarkerItemVM>(value, "TargetAgent");
				}
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00011931 File Offset: 0x0000FB31
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x00011939 File Offset: 0x0000FB39
		[DataSourceProperty]
		public MBBindingList<MissionDisguiseMarkerItemVM> HostileAgents
		{
			get
			{
				return this._hostileAgents;
			}
			set
			{
				if (value != this._hostileAgents)
				{
					this._hostileAgents = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionDisguiseMarkerItemVM>>(value, "HostileAgents");
				}
			}
		}

		// Token: 0x04000239 RID: 569
		private MissionDisguiseMarkerItemVM _targetAgent;

		// Token: 0x0400023A RID: 570
		private MBBindingList<MissionDisguiseMarkerItemVM> _hostileAgents;
	}
}
