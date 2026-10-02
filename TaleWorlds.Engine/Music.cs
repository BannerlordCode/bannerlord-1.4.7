using System;

namespace TaleWorlds.Engine
{
	// Token: 0x0200006E RID: 110
	public class Music
	{
		// Token: 0x06000A3E RID: 2622 RVA: 0x0000A695 File Offset: 0x00008895
		public static int GetFreeMusicChannelIndex()
		{
			return EngineApplicationInterface.IMusic.GetFreeMusicChannelIndex();
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0000A6A1 File Offset: 0x000088A1
		public static void LoadClip(int index, string pathToClip)
		{
			EngineApplicationInterface.IMusic.LoadClip(index, pathToClip);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0000A6AF File Offset: 0x000088AF
		public static void UnloadClip(int index)
		{
			EngineApplicationInterface.IMusic.UnloadClip(index);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0000A6BC File Offset: 0x000088BC
		public static bool IsClipLoaded(int index)
		{
			return EngineApplicationInterface.IMusic.IsClipLoaded(index);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0000A6C9 File Offset: 0x000088C9
		public static void PlayMusic(int index)
		{
			EngineApplicationInterface.IMusic.PlayMusic(index);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0000A6D6 File Offset: 0x000088D6
		public static void PlayDelayed(int index, int deltaMilliseconds)
		{
			EngineApplicationInterface.IMusic.PlayDelayed(index, deltaMilliseconds);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0000A6E4 File Offset: 0x000088E4
		public static bool IsMusicPlaying(int index)
		{
			return EngineApplicationInterface.IMusic.IsMusicPlaying(index);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0000A6F1 File Offset: 0x000088F1
		public static void PauseMusic(int index)
		{
			EngineApplicationInterface.IMusic.PauseMusic(index);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0000A6FE File Offset: 0x000088FE
		public static void StopMusic(int index)
		{
			EngineApplicationInterface.IMusic.StopMusic(index);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0000A70B File Offset: 0x0000890B
		public static void SetVolume(int index, float volume)
		{
			EngineApplicationInterface.IMusic.SetVolume(index, volume);
		}
	}
}
