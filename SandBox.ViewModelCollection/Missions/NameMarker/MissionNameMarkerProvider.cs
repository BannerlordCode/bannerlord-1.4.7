using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Missions.NameMarker
{
	// Token: 0x02000031 RID: 49
	public abstract class MissionNameMarkerProvider
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x00010138 File Offset: 0x0000E338
		public MissionNameMarkerProvider()
		{
		}

		// Token: 0x060003CA RID: 970
		public abstract void CreateMarkers(List<MissionNameMarkerTargetBaseVM> markers);

		// Token: 0x060003CB RID: 971 RVA: 0x00010140 File Offset: 0x0000E340
		public void Initialize(Mission mission, Action onSetMarkersDirty)
		{
			this.OnInitialize(mission);
			this._initialized = true;
			this._onSetMarkersDirty = onSetMarkersDirty;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00010157 File Offset: 0x0000E357
		public void Destroy(Mission mission)
		{
			this.OnDestroy(mission);
			this._initialized = false;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00010167 File Offset: 0x0000E367
		public void Tick(float dt)
		{
			this.OnTick(dt);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00010170 File Offset: 0x0000E370
		protected virtual void OnInitialize(Mission mission)
		{
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00010172 File Offset: 0x0000E372
		protected virtual void OnDestroy(Mission mission)
		{
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00010174 File Offset: 0x0000E374
		protected virtual void OnTick(float dt)
		{
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00010176 File Offset: 0x0000E376
		protected void SetMarkersDirty()
		{
			this._onSetMarkersDirty();
		}

		// Token: 0x040001FF RID: 511
		private Action _onSetMarkersDirty;

		// Token: 0x04000200 RID: 512
		private bool _initialized;
	}
}
