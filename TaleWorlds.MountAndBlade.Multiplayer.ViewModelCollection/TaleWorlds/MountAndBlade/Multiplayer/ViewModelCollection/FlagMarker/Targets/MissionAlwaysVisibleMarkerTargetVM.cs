using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets
{
	// Token: 0x02000097 RID: 151
	public class MissionAlwaysVisibleMarkerTargetVM : MissionMarkerTargetVM
	{
		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06000EEE RID: 3822 RVA: 0x0002E2B2 File Offset: 0x0002C4B2
		// (set) Token: 0x06000EEF RID: 3823 RVA: 0x0002E2BA File Offset: 0x0002C4BA
		public MissionPeer TargetPeer { get; private set; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x0002E2C3 File Offset: 0x0002C4C3
		public override Vec3 WorldPosition
		{
			get
			{
				return this._position;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0002E2CB File Offset: 0x0002C4CB
		protected override float HeightOffset
		{
			get
			{
				return 0.75f;
			}
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0002E2D2 File Offset: 0x0002C4D2
		public MissionAlwaysVisibleMarkerTargetVM(MissionPeer peer, Vec3 position, Action<MissionAlwaysVisibleMarkerTargetVM> onRemove)
			: base(MissionMarkerType.Peer)
		{
			this.TargetPeer = peer;
			this._position = position;
			this._onRemove = onRemove;
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0002E2F0 File Offset: 0x0002C4F0
		public void ExecuteRemove()
		{
			Action<MissionAlwaysVisibleMarkerTargetVM> onRemove = this._onRemove;
			if (onRemove == null)
			{
				return;
			}
			onRemove(this);
		}

		// Token: 0x040006E3 RID: 1763
		private Vec3 _position;

		// Token: 0x040006E4 RID: 1764
		private Action<MissionAlwaysVisibleMarkerTargetVM> _onRemove;
	}
}
