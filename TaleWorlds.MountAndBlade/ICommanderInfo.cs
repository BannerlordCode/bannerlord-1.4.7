using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000262 RID: 610
	public interface ICommanderInfo : IMissionBehavior
	{
		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06002260 RID: 8800
		// (remove) Token: 0x06002261 RID: 8801
		event Action<BattleSideEnum, float> OnMoraleChangedEvent;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06002262 RID: 8802
		// (remove) Token: 0x06002263 RID: 8803
		event Action OnFlagNumberChangedEvent;

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06002264 RID: 8804
		// (remove) Token: 0x06002265 RID: 8805
		event Action<FlagCapturePoint, Team> OnCapturePointOwnerChangedEvent;

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002266 RID: 8806
		IEnumerable<FlagCapturePoint> AllCapturePoints { get; }

		// Token: 0x06002267 RID: 8807
		Team GetFlagOwner(FlagCapturePoint flag);

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002268 RID: 8808
		bool AreMoralesIndependent { get; }
	}
}
