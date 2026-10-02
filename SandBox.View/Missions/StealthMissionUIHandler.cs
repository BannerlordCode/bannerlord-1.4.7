using System;
using SandBox.Objects.Usables;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x02000027 RID: 39
	public class StealthMissionUIHandler : MissionView
	{
		// Token: 0x0600010E RID: 270 RVA: 0x0000CA59 File Offset: 0x0000AC59
		public override void OnObjectUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			base.OnObjectUsed(userAgent, usedObject);
			if (usedObject is StealthAreaUsePoint)
			{
				this.CameraFadeInFadeOut(0.5f, 0.5f, 1f);
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000CA80 File Offset: 0x0000AC80
		private void CameraFadeInFadeOut(float fadeOutTime, float blackTime, float fadeInTime)
		{
			if (!ScreenFadeController.IsFadeActive)
			{
				ScreenFadeController.BeginFadeOutAndIn(fadeOutTime, blackTime, fadeInTime);
			}
		}
	}
}
