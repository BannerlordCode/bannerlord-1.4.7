using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.General;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.Personal;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed
{
	// Token: 0x02000089 RID: 137
	public class MPKillFeedVM : ViewModel
	{
		// Token: 0x06000D62 RID: 3426 RVA: 0x0002933C File Offset: 0x0002753C
		public MPKillFeedVM()
		{
			this.GeneralCasualty = new MPGeneralKillNotificationVM();
			this.PersonalCasualty = new MPPersonalKillNotificationVM();
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0002935C File Offset: 0x0002755C
		public void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, bool isPersonalFeedEnabled)
		{
			Agent assistedAgent = this.GetAssistedAgent(affectedAgent, affectorAgent);
			if (assistedAgent != null && assistedAgent.IsMainAgent && isPersonalFeedEnabled)
			{
				string text = affectedAgent.Name;
				if (affectedAgent.MissionPeer != null)
				{
					text = affectedAgent.MissionPeer.DisplayedName;
				}
				this.OnPersonalAssist(text);
			}
			this.GeneralCasualty.OnAgentRemoved(affectedAgent, affectorAgent, assistedAgent);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x000293B2 File Offset: 0x000275B2
		private void OnPersonalAssist(string victimAgentName)
		{
			this.PersonalCasualty.OnPersonalAssist(victimAgentName);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x000293C0 File Offset: 0x000275C0
		public void OnPersonalDamage(int damageAmount, bool isFatal, bool isMountDamage, bool isFriendlyDamage, bool isHeadshot, string killedAgentName)
		{
			this.PersonalCasualty.OnPersonalHit(damageAmount, isFatal, isMountDamage, isFriendlyDamage, isHeadshot, killedAgentName);
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x000293D6 File Offset: 0x000275D6
		private Agent GetAssistedAgent(Agent affectedAgent, Agent affectorAgent)
		{
			if (affectedAgent == null)
			{
				return null;
			}
			Agent.Hitter assistingHitter = affectedAgent.GetAssistingHitter((affectorAgent != null) ? affectorAgent.MissionPeer : null);
			if (assistingHitter == null)
			{
				return null;
			}
			MissionPeer hitterPeer = assistingHitter.HitterPeer;
			if (hitterPeer == null)
			{
				return null;
			}
			return hitterPeer.ControlledAgent;
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06000D67 RID: 3431 RVA: 0x00029405 File Offset: 0x00027605
		// (set) Token: 0x06000D68 RID: 3432 RVA: 0x0002940D File Offset: 0x0002760D
		[DataSourceProperty]
		public MPGeneralKillNotificationVM GeneralCasualty
		{
			get
			{
				return this._generalCasualty;
			}
			set
			{
				if (value != this._generalCasualty)
				{
					this._generalCasualty = value;
					base.OnPropertyChangedWithValue<MPGeneralKillNotificationVM>(value, "GeneralCasualty");
				}
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06000D69 RID: 3433 RVA: 0x0002942B File Offset: 0x0002762B
		// (set) Token: 0x06000D6A RID: 3434 RVA: 0x00029433 File Offset: 0x00027633
		[DataSourceProperty]
		public MPPersonalKillNotificationVM PersonalCasualty
		{
			get
			{
				return this._personalCasualty;
			}
			set
			{
				if (value != this._personalCasualty)
				{
					this._personalCasualty = value;
					base.OnPropertyChangedWithValue<MPPersonalKillNotificationVM>(value, "PersonalCasualty");
				}
			}
		}

		// Token: 0x0400061F RID: 1567
		private MPGeneralKillNotificationVM _generalCasualty;

		// Token: 0x04000620 RID: 1568
		private MPPersonalKillNotificationVM _personalCasualty;
	}
}
