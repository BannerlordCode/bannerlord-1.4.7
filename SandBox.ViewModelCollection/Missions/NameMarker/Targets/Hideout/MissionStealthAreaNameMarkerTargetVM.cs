using System;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets.Hideout
{
	// Token: 0x0200003D RID: 61
	public class MissionStealthAreaNameMarkerTargetVM : MissionNameMarkerTargetVM<StealthAreaMarker>
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x00011156 File Offset: 0x0000F356
		public MissionStealthAreaNameMarkerTargetVM(StealthAreaMarker target, Vec3 position)
			: base(target)
		{
			this._position = position;
			base.NameType = "Passage";
			base.IconType = "stealth_area";
			this.RefreshValues();
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00011182 File Offset: 0x0000F382
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, this._position + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001119B File Offset: 0x0000F39B
		protected override TextObject GetName()
		{
			return new TextObject("{=WcSky2KB}Stealth Area", null);
		}

		// Token: 0x0400021E RID: 542
		private readonly Vec3 _position;
	}
}
