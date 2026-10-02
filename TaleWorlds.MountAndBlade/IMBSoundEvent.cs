using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B2 RID: 434
	[ScriptingInterfaceBase]
	internal interface IMBSoundEvent
	{
		// Token: 0x060018AF RID: 6319
		[EngineMethod("create_event_from_external_file", false, null, false)]
		int CreateEventFromExternalFile(string programmerSoundEventName, string filePath, UIntPtr scene, bool is3d, bool isBlocking);

		// Token: 0x060018B0 RID: 6320
		[EngineMethod("create_event_from_sound_buffer", false, null, false)]
		int CreateEventFromSoundBuffer(string programmerSoundEventName, byte[] soundBuffer, UIntPtr scene, bool is3d, bool isBlocking);

		// Token: 0x060018B1 RID: 6321
		[EngineMethod("play_sound", false, null, false)]
		bool PlaySound(int fmodEventIndex, in Vec3 position);

		// Token: 0x060018B2 RID: 6322
		[EngineMethod("play_sound_with_int_param", false, null, false)]
		bool PlaySoundWithIntParam(int fmodEventIndex, int paramIndex, float paramVal, in Vec3 position);

		// Token: 0x060018B3 RID: 6323
		[EngineMethod("play_sound_with_str_param", false, null, false)]
		bool PlaySoundWithStrParam(int fmodEventIndex, string paramName, float paramVal, in Vec3 position);

		// Token: 0x060018B4 RID: 6324
		[EngineMethod("play_sound_with_param", false, null, false)]
		bool PlaySoundWithParam(int soundCodeId, SoundEventParameter parameter, in Vec3 position);
	}
}
