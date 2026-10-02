using System;
using System.Collections.Generic;
using StoryMode.GameComponents.CampaignBehaviors;
using StoryMode.StoryModeObjects;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace StoryMode
{
	// Token: 0x0200000C RID: 12
	public class MainStoryLine
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002B98 File Offset: 0x00000D98
		public bool IsPlayerInteractionRestricted
		{
			get
			{
				return !this.TutorialPhase.IsCompleted && !this.IsOnImperialQuestLine && !this.IsOnAntiImperialQuestLine;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002BBC File Offset: 0x00000DBC
		public bool IsOnImperialQuestLine
		{
			get
			{
				return this.MainStoryLineSide == MainStoryLineSide.CreateImperialKingdom || this.MainStoryLineSide == MainStoryLineSide.SupportImperialKingdom;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002BD2 File Offset: 0x00000DD2
		public bool IsOnAntiImperialQuestLine
		{
			get
			{
				return this.MainStoryLineSide == MainStoryLineSide.CreateAntiImperialKingdom || this.MainStoryLineSide == MainStoryLineSide.SupportAntiImperialKingdom;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002BE8 File Offset: 0x00000DE8
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002BF0 File Offset: 0x00000DF0
		[SaveableProperty(2)]
		public TutorialPhase TutorialPhase { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002BF9 File Offset: 0x00000DF9
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002C01 File Offset: 0x00000E01
		[SaveableProperty(3)]
		public FirstPhase FirstPhase { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002C0A File Offset: 0x00000E0A
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002C12 File Offset: 0x00000E12
		[SaveableProperty(4)]
		public SecondPhase SecondPhase { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002C1B File Offset: 0x00000E1B
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002C23 File Offset: 0x00000E23
		[SaveableProperty(5)]
		public ThirdPhase ThirdPhase { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002C2C File Offset: 0x00000E2C
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002C34 File Offset: 0x00000E34
		[SaveableProperty(8)]
		public Kingdom PlayerSupportedKingdom { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002C3D File Offset: 0x00000E3D
		public bool IsCompleted
		{
			get
			{
				return StoryModeManager.Current.MainStoryLine.ThirdPhase != null && StoryModeManager.Current.MainStoryLine.ThirdPhase.IsCompleted;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002C66 File Offset: 0x00000E66
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002C6E File Offset: 0x00000E6E
		public ItemObject DragonBanner { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002C77 File Offset: 0x00000E77
		public bool IsFirstPhaseCompleted
		{
			get
			{
				return this.SecondPhase != null;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002C82 File Offset: 0x00000E82
		public bool IsSecondPhaseCompleted
		{
			get
			{
				return this.ThirdPhase != null;
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002C8D File Offset: 0x00000E8D
		public MainStoryLine()
		{
			this.MainStoryLineSide = MainStoryLineSide.None;
			this.TutorialPhase = new TutorialPhase();
			this._tutorialScores = new Dictionary<string, float>();
			this.FamilyRescued = false;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002CB9 File Offset: 0x00000EB9
		public void OnSessionLaunched()
		{
			this.DragonBanner = Campaign.Current.ObjectManager.GetObject<ItemObject>("dragon_banner");
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002CD5 File Offset: 0x00000ED5
		public void SetTutorialScores(Dictionary<string, float> scores)
		{
			this._tutorialScores = new Dictionary<string, float>(scores);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002CE3 File Offset: 0x00000EE3
		public Dictionary<string, float> GetTutorialScores()
		{
			return new Dictionary<string, float>(this._tutorialScores);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002CF0 File Offset: 0x00000EF0
		public void SetStoryLineSide(MainStoryLineSide side)
		{
			this.MainStoryLineSide = side;
			this.PlayerSupportedKingdom = Clan.PlayerClan.Kingdom;
			StoryModeEvents.Instance.OnMainStoryLineSideChosen(this.MainStoryLineSide);
			DisableHeroAction.Apply(StoryModeHeroes.ImperialMentor);
			DisableHeroAction.Apply(StoryModeHeroes.AntiImperialMentor);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002D2D File Offset: 0x00000F2D
		public void SetMentorSettlements(Settlement imperialMentorSettlement, Settlement antiImperialMentorSettlement)
		{
			this.ImperialMentorSettlement = imperialMentorSettlement;
			this.AntiImperialMentorSettlement = antiImperialMentorSettlement;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002D40 File Offset: 0x00000F40
		public void CompleteTutorialPhase(bool isSkipped)
		{
			this.TutorialPhase.CompleteTutorial(isSkipped);
			this.FirstPhase = new FirstPhase();
			TutorialPhaseCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<TutorialPhaseCampaignBehavior>();
			if (campaignBehavior != null)
			{
				campaignBehavior.FinalizeTutorialPhase();
			}
			StoryModeEvents.Instance.OnStoryModeTutorialEnded();
			StoryModeManager.Current.MainStoryLine.FirstPhase.CollectBannerPiece();
			Campaign.Current.CampaignBehaviorManager.RemoveBehavior<TutorialPhaseCampaignBehavior>();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002DA5 File Offset: 0x00000FA5
		public void CompleteFirstPhase()
		{
			this.SecondPhase = new SecondPhase();
			Campaign.Current.CampaignBehaviorManager.RemoveBehavior<FirstPhaseCampaignBehavior>();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002DC1 File Offset: 0x00000FC1
		public void CompleteSecondPhase()
		{
			this.ThirdPhase = new ThirdPhase();
			StoryModeEvents.Instance.OnConspiracyActivated();
			Campaign.Current.CampaignBehaviorManager.RemoveBehavior<SecondPhaseCampaignBehavior>();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002DE7 File Offset: 0x00000FE7
		public void CancelSecondAndThirdPhase()
		{
			if (this.SecondPhase != null)
			{
				Campaign.Current.CampaignBehaviorManager.RemoveBehavior<SecondPhaseCampaignBehavior>();
			}
			Campaign.Current.CampaignBehaviorManager.RemoveBehavior<ThirdPhaseCampaignBehavior>();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002E0F File Offset: 0x0000100F
		internal static void AutoGeneratedStaticCollectObjectsMainStoryLine(object o, List<object> collectedObjects)
		{
			((MainStoryLine)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002E20 File Offset: 0x00001020
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this.ImperialMentorSettlement);
			collectedObjects.Add(this.AntiImperialMentorSettlement);
			collectedObjects.Add(this._tutorialScores);
			collectedObjects.Add(this.TutorialPhase);
			collectedObjects.Add(this.FirstPhase);
			collectedObjects.Add(this.SecondPhase);
			collectedObjects.Add(this.ThirdPhase);
			collectedObjects.Add(this.PlayerSupportedKingdom);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002E8D File Offset: 0x0000108D
		internal static object AutoGeneratedGetMemberValueTutorialPhase(object o)
		{
			return ((MainStoryLine)o).TutorialPhase;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002E9A File Offset: 0x0000109A
		internal static object AutoGeneratedGetMemberValueFirstPhase(object o)
		{
			return ((MainStoryLine)o).FirstPhase;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002EA7 File Offset: 0x000010A7
		internal static object AutoGeneratedGetMemberValueSecondPhase(object o)
		{
			return ((MainStoryLine)o).SecondPhase;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002EB4 File Offset: 0x000010B4
		internal static object AutoGeneratedGetMemberValueThirdPhase(object o)
		{
			return ((MainStoryLine)o).ThirdPhase;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002EC1 File Offset: 0x000010C1
		internal static object AutoGeneratedGetMemberValuePlayerSupportedKingdom(object o)
		{
			return ((MainStoryLine)o).PlayerSupportedKingdom;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002ECE File Offset: 0x000010CE
		internal static object AutoGeneratedGetMemberValueMainStoryLineSide(object o)
		{
			return ((MainStoryLine)o).MainStoryLineSide;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002EE0 File Offset: 0x000010E0
		internal static object AutoGeneratedGetMemberValueImperialMentorSettlement(object o)
		{
			return ((MainStoryLine)o).ImperialMentorSettlement;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002EED File Offset: 0x000010ED
		internal static object AutoGeneratedGetMemberValueAntiImperialMentorSettlement(object o)
		{
			return ((MainStoryLine)o).AntiImperialMentorSettlement;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002EFA File Offset: 0x000010FA
		internal static object AutoGeneratedGetMemberValueFamilyRescued(object o)
		{
			return ((MainStoryLine)o).FamilyRescued;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002F0C File Offset: 0x0000110C
		internal static object AutoGeneratedGetMemberValue_tutorialScores(object o)
		{
			return ((MainStoryLine)o)._tutorialScores;
		}

		// Token: 0x0400001B RID: 27
		public const int MainStoryLineDialogOptionPriority = 150;

		// Token: 0x0400001C RID: 28
		public const string DragonBannerItemStringId = "dragon_banner";

		// Token: 0x0400001D RID: 29
		public const string DragonBannerPart1ItemStringId = "dragon_banner_center";

		// Token: 0x0400001E RID: 30
		public const string DragonBannerPart2ItemStringId = "dragon_banner_dragonhead";

		// Token: 0x0400001F RID: 31
		public const string DragonBannerPart3ItemStringId = "dragon_banner_handle";

		// Token: 0x04000020 RID: 32
		[SaveableField(1)]
		public MainStoryLineSide MainStoryLineSide;

		// Token: 0x04000025 RID: 37
		[SaveableField(6)]
		public Settlement ImperialMentorSettlement;

		// Token: 0x04000026 RID: 38
		[SaveableField(7)]
		public Settlement AntiImperialMentorSettlement;

		// Token: 0x04000028 RID: 40
		[SaveableField(9)]
		private Dictionary<string, float> _tutorialScores;

		// Token: 0x04000029 RID: 41
		[SaveableField(10)]
		public bool FamilyRescued;
	}
}
