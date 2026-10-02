using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;

namespace SandBox.ViewModelCollection.Missions.NameMarker
{
	// Token: 0x02000032 RID: 50
	public abstract class MissionNameMarkerTargetVM<T> : MissionNameMarkerTargetBaseVM
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00010183 File Offset: 0x0000E383
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x0001018B File Offset: 0x0000E38B
		public T Target { get; private set; }

		// Token: 0x060003D4 RID: 980 RVA: 0x00010194 File Offset: 0x0000E394
		protected MissionNameMarkerTargetVM(T target)
		{
			this.Target = target;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000101A4 File Offset: 0x0000E3A4
		public override bool Equals(MissionNameMarkerTargetBaseVM other)
		{
			MissionNameMarkerTargetVM<T> missionNameMarkerTargetVM;
			if ((missionNameMarkerTargetVM = other as MissionNameMarkerTargetVM<T>) != null)
			{
				T target = missionNameMarkerTargetVM.Target;
				if (target.Equals(this.Target) && this.AreQuestsEqual(missionNameMarkerTargetVM))
				{
					return base.IsPersistent == missionNameMarkerTargetVM.IsPersistent;
				}
			}
			return false;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000101F8 File Offset: 0x0000E3F8
		private bool AreQuestsEqual(MissionNameMarkerTargetVM<T> tOther)
		{
			if (tOther.Quests == null || base.Quests == null)
			{
				return tOther.Quests == null && base.Quests == null;
			}
			if (tOther.Quests.Count != base.Quests.Count)
			{
				return false;
			}
			for (int i = 0; i < base.Quests.Count; i++)
			{
				QuestMarkerVM questMarkerVM = base.Quests[i];
				QuestMarkerVM questMarkerVM2 = tOther.Quests[i];
				if (questMarkerVM.IssueQuestFlag != questMarkerVM2.IssueQuestFlag || questMarkerVM.QuestMarkerType != questMarkerVM2.QuestMarkerType)
				{
					return false;
				}
			}
			return true;
		}
	}
}
