using System;
using SandBox.Objects;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets
{
	// Token: 0x0200003B RID: 59
	public class MissionPassageUsePointNameMarkerTargetVM : MissionNameMarkerTargetVM<PassageUsePoint>
	{
		// Token: 0x0600041B RID: 1051 RVA: 0x00011028 File Offset: 0x0000F228
		public MissionPassageUsePointNameMarkerTargetVM(PassageUsePoint target)
			: base(target)
		{
			base.NameType = "Passage";
			base.IconType = ((base.Target.ToLocation == null && base.Target.IsMissionExit) ? "center" : base.Target.ToLocation.StringId);
			base.Quests = new MBBindingList<QuestMarkerVM>();
			this.RefreshValues();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00011090 File Offset: 0x0000F290
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, base.Target.GameEntity.GlobalPosition + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000110C1 File Offset: 0x0000F2C1
		protected override TextObject GetName()
		{
			if (base.Target.ToLocation == null && base.Target.IsMissionExit)
			{
				return GameTexts.FindText("str_mission_exit", null);
			}
			return base.Target.ToLocation.Name;
		}
	}
}
