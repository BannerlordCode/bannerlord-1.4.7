using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200003E RID: 62
	[ApplicationInterfaceBase]
	internal interface IMusic
	{
		// Token: 0x06000646 RID: 1606
		[EngineMethod("get_free_music_channel_index", false, null, false)]
		int GetFreeMusicChannelIndex();

		// Token: 0x06000647 RID: 1607
		[EngineMethod("load_clip", false, null, false)]
		void LoadClip(int index, string pathToClip);

		// Token: 0x06000648 RID: 1608
		[EngineMethod("unload_clip", false, null, false)]
		void UnloadClip(int index);

		// Token: 0x06000649 RID: 1609
		[EngineMethod("is_clip_loaded", false, null, false)]
		bool IsClipLoaded(int index);

		// Token: 0x0600064A RID: 1610
		[EngineMethod("play_music", false, null, false)]
		void PlayMusic(int index);

		// Token: 0x0600064B RID: 1611
		[EngineMethod("play_delayed", false, null, false)]
		void PlayDelayed(int index, int delayMilliseconds);

		// Token: 0x0600064C RID: 1612
		[EngineMethod("is_music_playing", false, null, false)]
		bool IsMusicPlaying(int index);

		// Token: 0x0600064D RID: 1613
		[EngineMethod("pause_music", false, null, false)]
		void PauseMusic(int index);

		// Token: 0x0600064E RID: 1614
		[EngineMethod("stop_music", false, null, false)]
		void StopMusic(int index);

		// Token: 0x0600064F RID: 1615
		[EngineMethod("set_volume", false, null, false)]
		void SetVolume(int index, float volume);
	}
}
