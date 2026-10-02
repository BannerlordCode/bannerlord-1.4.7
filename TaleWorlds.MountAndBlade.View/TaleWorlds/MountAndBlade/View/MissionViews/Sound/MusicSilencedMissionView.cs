using System;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Sound
{
	// Token: 0x02000087 RID: 135
	public class MusicSilencedMissionView : MissionView, IMusicHandler
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0002610C File Offset: 0x0002430C
		bool IMusicHandler.IsPausable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0002610F File Offset: 0x0002430F
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			MBMusicManager.Current.DeactivateCurrentMode();
			MBMusicManager.Current.OnSilencedMusicHandlerInit(this);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0002612C File Offset: 0x0002432C
		public override void OnMissionScreenFinalize()
		{
			MBMusicManager.Current.OnSilencedMusicHandlerFinalize();
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00026138 File Offset: 0x00024338
		void IMusicHandler.OnUpdated(float dt)
		{
		}
	}
}
