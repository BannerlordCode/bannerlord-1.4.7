using System;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets
{
	// Token: 0x0200003C RID: 60
	public class MissionWorkshopNameMarkerTargetVM : MissionNameMarkerTargetVM<Workshop>
	{
		// Token: 0x0600041E RID: 1054 RVA: 0x000110F9 File Offset: 0x0000F2F9
		public MissionWorkshopNameMarkerTargetVM(Workshop target, Vec3 signPosition)
			: base(target)
		{
			base.NameType = "Passage";
			base.IconType = target.WorkshopType.StringId;
			this._signPosition = signPosition;
			this.RefreshValues();
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001112B File Offset: 0x0000F32B
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, this._signPosition + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00011144 File Offset: 0x0000F344
		protected override TextObject GetName()
		{
			return base.Target.WorkshopType.Name;
		}

		// Token: 0x0400021D RID: 541
		private readonly Vec3 _signPosition;
	}
}
