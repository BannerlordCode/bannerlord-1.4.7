using System;
using TaleWorlds.MountAndBlade.Missions.Hints;

namespace TaleWorlds.MountAndBlade.Missions.MissionLogics
{
	// Token: 0x020003EA RID: 1002
	public class MissionHintLogic : MissionLogic
	{
		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06003705 RID: 14085 RVA: 0x000E384C File Offset: 0x000E1A4C
		// (remove) Token: 0x06003706 RID: 14086 RVA: 0x000E3884 File Offset: 0x000E1A84
		public event MissionHintLogic.MissionHintChangedDelegate OnActiveHintChanged;

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06003707 RID: 14087 RVA: 0x000E38B9 File Offset: 0x000E1AB9
		// (set) Token: 0x06003708 RID: 14088 RVA: 0x000E38C1 File Offset: 0x000E1AC1
		public MissionHint ActiveHint { get; private set; }

		// Token: 0x06003709 RID: 14089 RVA: 0x000E38CC File Offset: 0x000E1ACC
		public void SetActiveHint(MissionHint hint)
		{
			MissionHint activeHint = this.ActiveHint;
			this.ActiveHint = hint;
			MissionHintLogic.MissionHintChangedDelegate onActiveHintChanged = this.OnActiveHintChanged;
			if (onActiveHintChanged == null)
			{
				return;
			}
			onActiveHintChanged(activeHint, this.ActiveHint);
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x000E38FE File Offset: 0x000E1AFE
		public void Clear()
		{
			this.SetActiveHint(null);
		}

		// Token: 0x0200069A RID: 1690
		// (Invoke) Token: 0x060041AD RID: 16813
		public delegate void MissionHintChangedDelegate(MissionHint previousHint, MissionHint newHint);
	}
}
