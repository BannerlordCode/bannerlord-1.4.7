using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200029E RID: 670
	public class MissionRecorder
	{
		// Token: 0x06002505 RID: 9477 RVA: 0x00086A64 File Offset: 0x00084C64
		public MissionRecorder(Mission mission)
		{
			this._mission = mission;
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x00086A73 File Offset: 0x00084C73
		public void RestartRecord()
		{
			MBAPI.IMBMission.RestartRecord(this._mission.Pointer);
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x00086A8A File Offset: 0x00084C8A
		public void ProcessRecordUntilTime(float time)
		{
			MBAPI.IMBMission.ProcessRecordUntilTime(this._mission.Pointer, time);
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x00086AA2 File Offset: 0x00084CA2
		public bool IsEndOfRecord()
		{
			return MBAPI.IMBMission.EndOfRecord(this._mission.Pointer);
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x00086AB9 File Offset: 0x00084CB9
		public void StartRecording()
		{
			MBAPI.IMBMission.StartRecording();
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x00086AC5 File Offset: 0x00084CC5
		public void RecordCurrentState()
		{
			MBAPI.IMBMission.RecordCurrentState(this._mission.Pointer);
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x00086ADC File Offset: 0x00084CDC
		public void BackupRecordToFile(string fileName, string gameType, string sceneLevels)
		{
			MBAPI.IMBMission.BackupRecordToFile(this._mission.Pointer, fileName, gameType, sceneLevels);
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x00086AF6 File Offset: 0x00084CF6
		public void RestoreRecordFromFile(string fileName)
		{
			MBAPI.IMBMission.RestoreRecordFromFile(this._mission.Pointer, fileName);
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x00086B0E File Offset: 0x00084D0E
		public void ClearRecordBuffers()
		{
			MBAPI.IMBMission.ClearRecordBuffers(this._mission.Pointer);
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x00086B25 File Offset: 0x00084D25
		public static string GetSceneNameForReplay(PlatformFilePath fileName)
		{
			return MBAPI.IMBMission.GetSceneNameForReplay(fileName);
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x00086B32 File Offset: 0x00084D32
		public static string GetGameTypeForReplay(PlatformFilePath fileName)
		{
			return MBAPI.IMBMission.GetGameTypeForReplay(fileName);
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x00086B3F File Offset: 0x00084D3F
		public static string GetSceneLevelsForReplay(PlatformFilePath fileName)
		{
			return MBAPI.IMBMission.GetSceneLevelsForReplay(fileName);
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x00086B4C File Offset: 0x00084D4C
		public static string GetAtmosphereNameForReplay(PlatformFilePath fileName)
		{
			return MBAPI.IMBMission.GetAtmosphereNameForReplay(fileName);
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x00086B59 File Offset: 0x00084D59
		public static int GetAtmosphereSeasonForReplay(PlatformFilePath fileName)
		{
			return MBAPI.IMBMission.GetAtmosphereSeasonForReplay(fileName);
		}

		// Token: 0x04000E54 RID: 3668
		private readonly Mission _mission;
	}
}
