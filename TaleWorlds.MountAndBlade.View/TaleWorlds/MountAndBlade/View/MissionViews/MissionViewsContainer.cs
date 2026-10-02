using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000082 RID: 130
	public class MissionViewsContainer
	{
		// Token: 0x060004E8 RID: 1256 RVA: 0x00024A94 File Offset: 0x00022C94
		public MissionViewsContainer()
		{
			this._missionViews = new List<MissionView>();
			this._missionViewsCopy = this._missionViews.ToList<MissionView>();
			this._missionViewsCopiedFrame = Utilities.EngineFrameNo;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00024ACA File Offset: 0x00022CCA
		public void Add(MissionView missionView)
		{
			this._missionViews.Add(missionView);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00024AD8 File Offset: 0x00022CD8
		public void Remove(MissionView missionView)
		{
			this._missionViews.Remove(missionView);
			missionView.IsFinalized = true;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00024AEE File Offset: 0x00022CEE
		public bool Contains(MissionView missionView)
		{
			return this._missionViews.Contains(missionView);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00024AFC File Offset: 0x00022CFC
		public bool Any(Func<MissionView, bool> predicate)
		{
			return this._missionViews.Any<MissionView>(predicate);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00024B0C File Offset: 0x00022D0C
		public void ForEach(Action<MissionView> action)
		{
			foreach (MissionView missionView in this.GetMissionViewsCopy())
			{
				if (!missionView.IsFinalized)
				{
					action(missionView);
				}
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00024B68 File Offset: 0x00022D68
		private List<MissionView> GetMissionViewsCopy()
		{
			int engineFrameNo = Utilities.EngineFrameNo;
			if (this._missionViewsCopiedFrame != engineFrameNo)
			{
				this._missionViewsCopy = this._missionViews.ToList<MissionView>();
				this._missionViewsCopiedFrame = engineFrameNo;
			}
			return this._missionViewsCopy;
		}

		// Token: 0x040002C5 RID: 709
		private List<MissionView> _missionViews;

		// Token: 0x040002C6 RID: 710
		private List<MissionView> _missionViewsCopy;

		// Token: 0x040002C7 RID: 711
		private int _missionViewsCopiedFrame = -1;
	}
}
