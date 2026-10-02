using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000109 RID: 265
	public class VictoryComponent : AgentComponent
	{
		// Token: 0x06000D90 RID: 3472 RVA: 0x0001A048 File Offset: 0x00018248
		public VictoryComponent(Agent agent, RandomTimer timer)
			: base(agent)
		{
			this._timer = timer;
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0001A058 File Offset: 0x00018258
		public bool CheckTimer()
		{
			return this._timer.Check(Mission.Current.CurrentTime);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0001A06F File Offset: 0x0001826F
		public void ChangeTimerDuration(float min, float max)
		{
			this._timer.ChangeDuration(min, max);
		}

		// Token: 0x04000310 RID: 784
		private readonly RandomTimer _timer;
	}
}
