using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x0200004F RID: 79
	public class MissionAgentLockItemVM : ViewModel
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x00018359 File Offset: 0x00016559
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x00018361 File Offset: 0x00016561
		public Agent TrackedAgent { get; private set; }

		// Token: 0x0600069E RID: 1694 RVA: 0x0001836A File Offset: 0x0001656A
		public MissionAgentLockItemVM(Agent agent, MissionAgentLockItemVM.LockStates initialLockState)
		{
			this.TrackedAgent = agent;
			this.LockState = (int)initialLockState;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00018387 File Offset: 0x00016587
		public void SetLockState(MissionAgentLockItemVM.LockStates lockState)
		{
			this.LockState = (int)lockState;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00018390 File Offset: 0x00016590
		public void UpdatePosition(Vec2 position)
		{
			this.Position = position;
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00018399 File Offset: 0x00016599
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x000183A1 File Offset: 0x000165A1
		[DataSourceProperty]
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value != this._position)
				{
					this._position = value;
					base.OnPropertyChangedWithValue(value, "Position");
				}
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x000183C4 File Offset: 0x000165C4
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x000183CC File Offset: 0x000165CC
		[DataSourceProperty]
		public int LockState
		{
			get
			{
				return this._lockState;
			}
			set
			{
				if (value != this._lockState)
				{
					this._lockState = value;
					base.OnPropertyChangedWithValue(value, "LockState");
				}
			}
		}

		// Token: 0x040002F0 RID: 752
		private Vec2 _position;

		// Token: 0x040002F1 RID: 753
		private int _lockState = -1;

		// Token: 0x020000EA RID: 234
		public enum LockStates
		{
			// Token: 0x04000656 RID: 1622
			Possible,
			// Token: 0x04000657 RID: 1623
			Active
		}
	}
}
