using System;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets
{
	// Token: 0x02000038 RID: 56
	public class MissionBasicAreaIndicatorMarkerTargetVM : MissionNameMarkerTargetVM<BasicAreaIndicator>
	{
		// Token: 0x0600040E RID: 1038 RVA: 0x00010DE4 File Offset: 0x0000EFE4
		public MissionBasicAreaIndicatorMarkerTargetVM(BasicAreaIndicator target, Vec3 position)
			: base(target)
		{
			base.NameType = "Passage";
			base.IconType = (string.IsNullOrEmpty(base.Target.Type) ? "common_area" : base.Target.Type);
			this._position = position;
			this.RefreshValues();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00010E3A File Offset: 0x0000F03A
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, this._position + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00010E53 File Offset: 0x0000F053
		protected override TextObject GetName()
		{
			return base.Target.GetName();
		}

		// Token: 0x04000218 RID: 536
		private readonly Vec3 _position;
	}
}
