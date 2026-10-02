using System;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003D6 RID: 982
	public class MissionOptionsComponent : MissionLogic
	{
		// Token: 0x140000AC RID: 172
		// (add) Token: 0x0600366C RID: 13932 RVA: 0x000E1574 File Offset: 0x000DF774
		// (remove) Token: 0x0600366D RID: 13933 RVA: 0x000E15AC File Offset: 0x000DF7AC
		public event OnMissionAddOptionsDelegate OnOptionsAdded;

		// Token: 0x0600366E RID: 13934 RVA: 0x000E15E1 File Offset: 0x000DF7E1
		public void OnAddOptionsUIHandler()
		{
			if (this.OnOptionsAdded != null)
			{
				this.OnOptionsAdded();
			}
		}
	}
}
