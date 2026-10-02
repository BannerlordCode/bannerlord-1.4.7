using System;
using TaleWorlds.Library.EventSystem;

namespace SandBox.ViewModelCollection.Missions.NameMarker
{
	// Token: 0x02000035 RID: 53
	public class MissionNameMarkerToggleEvent : EventBase
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00010930 File Offset: 0x0000EB30
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x00010938 File Offset: 0x0000EB38
		public bool NewState { get; private set; }

		// Token: 0x06000406 RID: 1030 RVA: 0x00010941 File Offset: 0x0000EB41
		public MissionNameMarkerToggleEvent(bool newState)
		{
			this.NewState = newState;
		}
	}
}
