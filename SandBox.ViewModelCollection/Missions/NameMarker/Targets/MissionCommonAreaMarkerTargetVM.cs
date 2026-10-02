using System;
using SandBox.Objects.AreaMarkers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Missions.NameMarker.Targets
{
	// Token: 0x02000039 RID: 57
	public class MissionCommonAreaMarkerTargetVM : MissionNameMarkerTargetVM<CommonAreaMarker>
	{
		// Token: 0x06000411 RID: 1041 RVA: 0x00010E60 File Offset: 0x0000F060
		public MissionCommonAreaMarkerTargetVM(CommonAreaMarker target)
			: base(target)
		{
			base.NameType = "Passage";
			base.IconType = "common_area";
			this.TargetAlley = Hero.MainHero.CurrentSettlement.Alleys[target.AreaIndex - 1];
			this.UpdateAlleyStatus();
			CampaignEvents.AlleyOwnerChanged.AddNonSerializedListener(this, new Action<Alley, Hero, Hero>(this.OnAlleyOwnerChanged));
			this.RefreshValues();
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00010ECF File Offset: 0x0000F0CF
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00010EE2 File Offset: 0x0000F0E2
		private void OnAlleyOwnerChanged(Alley alley, Hero newOwner, Hero oldOwner)
		{
			if (this.TargetAlley == alley && (newOwner == Hero.MainHero || oldOwner == Hero.MainHero))
			{
				this.UpdateAlleyStatus();
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00010F03 File Offset: 0x0000F103
		public override void UpdatePosition(Camera missionCamera)
		{
			base.UpdatePositionWith(missionCamera, base.Target.GetPosition() + MissionNameMarkerHelper.DefaultHeightOffset);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00010F21 File Offset: 0x0000F121
		protected override TextObject GetName()
		{
			return base.Target.GetName();
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00010F30 File Offset: 0x0000F130
		private void UpdateAlleyStatus()
		{
			if (this.TargetAlley != null)
			{
				Hero owner = this.TargetAlley.Owner;
				if (owner != null)
				{
					if (owner == Hero.MainHero)
					{
						base.NameType = "Friendly";
						base.IsFriendly = true;
						base.IsEnemy = false;
						return;
					}
					base.NameType = "Passage";
					base.IsFriendly = false;
					base.IsEnemy = true;
					return;
				}
				else
				{
					base.NameType = "Normal";
					base.IsFriendly = false;
					base.IsEnemy = false;
				}
			}
		}

		// Token: 0x04000219 RID: 537
		public readonly Alley TargetAlley;
	}
}
