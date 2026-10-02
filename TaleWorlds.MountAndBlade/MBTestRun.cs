using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E0 RID: 480
	public class MBTestRun
	{
		// Token: 0x06001C4A RID: 7242 RVA: 0x00061130 File Offset: 0x0005F330
		public static bool EnterEditMode()
		{
			return MBAPI.IMBTestRun.EnterEditMode();
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x0006113C File Offset: 0x0005F33C
		public static bool NewScene()
		{
			return MBAPI.IMBTestRun.NewScene();
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x00061148 File Offset: 0x0005F348
		public static bool LeaveEditMode()
		{
			return MBAPI.IMBTestRun.LeaveEditMode();
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x00061154 File Offset: 0x0005F354
		public static bool OpenScene(string sceneName)
		{
			return MBAPI.IMBTestRun.OpenScene(sceneName);
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x00061161 File Offset: 0x0005F361
		public static bool CloseScene()
		{
			return MBAPI.IMBTestRun.CloseScene();
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x0006116D File Offset: 0x0005F36D
		public static bool SaveScene()
		{
			return false;
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x00061170 File Offset: 0x0005F370
		public static bool OpenDefaultScene()
		{
			return false;
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x00061173 File Offset: 0x0005F373
		public static int GetFPS()
		{
			return MBAPI.IMBTestRun.GetFPS();
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x0006117F File Offset: 0x0005F37F
		public static void StartMission()
		{
			MBAPI.IMBTestRun.StartMission();
		}
	}
}
