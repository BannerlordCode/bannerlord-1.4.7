using System;
using TaleWorlds.CampaignSystem;

namespace StoryMode
{
	// Token: 0x02000014 RID: 20
	public class StoryModeEvents : CampaignEventReceiver
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004FD6 File Offset: 0x000031D6
		public static StoryModeEvents Instance
		{
			get
			{
				StoryModeManager storyModeManager = StoryModeManager.Current;
				if (storyModeManager == null)
				{
					return null;
				}
				return storyModeManager.StoryModeEvents;
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004FE8 File Offset: 0x000031E8
		public override void RemoveListeners(object obj)
		{
			this._onMainStoryLineSideChosenEvent.ClearListeners(obj);
			this._onStoryModeTutorialEndedEvent.ClearListeners(obj);
			this._onStealthTutorialActivatedEvent.ClearListeners(obj);
			this._onBannerPieceCollectedEvent.ClearListeners(obj);
			this._onConspiracyActivatedEvent.ClearListeners(obj);
			this._onTravelToVillageTutorialQuestStartedEvent.ClearListeners(obj);
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000503D File Offset: 0x0000323D
		public static IMbEvent<MainStoryLineSide> OnMainStoryLineSideChosenEvent
		{
			get
			{
				return StoryModeEvents.Instance._onMainStoryLineSideChosenEvent;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005049 File Offset: 0x00003249
		public void OnMainStoryLineSideChosen(MainStoryLineSide side)
		{
			StoryModeEvents.Instance._onMainStoryLineSideChosenEvent.Invoke(side);
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000505B File Offset: 0x0000325B
		public static IMbEvent OnStoryModeTutorialEndedEvent
		{
			get
			{
				return StoryModeEvents.Instance._onStoryModeTutorialEndedEvent;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005067 File Offset: 0x00003267
		public void OnStoryModeTutorialEnded()
		{
			StoryModeEvents.Instance._onStoryModeTutorialEndedEvent.Invoke();
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00005078 File Offset: 0x00003278
		public static IMbEvent OnStealthTutorialActivatedEvent
		{
			get
			{
				return StoryModeEvents.Instance._onStealthTutorialActivatedEvent;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005084 File Offset: 0x00003284
		public void OnStealthTutorialActivated()
		{
			StoryModeEvents.Instance._onStealthTutorialActivatedEvent.Invoke();
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00005095 File Offset: 0x00003295
		public static IMbEvent OnBannerPieceCollectedEvent
		{
			get
			{
				return StoryModeEvents.Instance._onBannerPieceCollectedEvent;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000050A1 File Offset: 0x000032A1
		public void OnBannerPieceCollected()
		{
			StoryModeEvents.Instance._onBannerPieceCollectedEvent.Invoke();
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000050B2 File Offset: 0x000032B2
		public static IMbEvent OnConspiracyActivatedEvent
		{
			get
			{
				return StoryModeEvents.Instance._onConspiracyActivatedEvent;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000050BE File Offset: 0x000032BE
		public void OnConspiracyActivated()
		{
			StoryModeEvents.Instance._onConspiracyActivatedEvent.Invoke();
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000050CF File Offset: 0x000032CF
		public static IMbEvent OnTravelToVillageTutorialQuestStartedEvent
		{
			get
			{
				return StoryModeEvents.Instance._onTravelToVillageTutorialQuestStartedEvent;
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000050DB File Offset: 0x000032DB
		public void OnTravelToVillageTutorialQuestStarted()
		{
			StoryModeEvents.Instance._onTravelToVillageTutorialQuestStartedEvent.Invoke();
		}

		// Token: 0x0400003A RID: 58
		private readonly MbEvent<MainStoryLineSide> _onMainStoryLineSideChosenEvent = new MbEvent<MainStoryLineSide>();

		// Token: 0x0400003B RID: 59
		private readonly MbEvent _onStoryModeTutorialEndedEvent = new MbEvent();

		// Token: 0x0400003C RID: 60
		private readonly MbEvent _onStealthTutorialActivatedEvent = new MbEvent();

		// Token: 0x0400003D RID: 61
		private readonly MbEvent _onBannerPieceCollectedEvent = new MbEvent();

		// Token: 0x0400003E RID: 62
		private readonly MbEvent _onConspiracyActivatedEvent = new MbEvent();

		// Token: 0x0400003F RID: 63
		private readonly MbEvent _onTravelToVillageTutorialQuestStartedEvent = new MbEvent();
	}
}
