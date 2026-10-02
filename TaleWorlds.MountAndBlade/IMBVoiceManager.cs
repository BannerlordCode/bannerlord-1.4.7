using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B3 RID: 435
	[ScriptingInterfaceBase]
	internal interface IMBVoiceManager
	{
		// Token: 0x060018B5 RID: 6325
		[EngineMethod("get_voice_type_index", false, null, false)]
		int GetVoiceTypeIndex(string voiceType);

		// Token: 0x060018B6 RID: 6326
		[EngineMethod("get_voice_definition_count_with_monster_sound_and_collision_info_class_name", false, null, false)]
		int GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassName(string className);

		// Token: 0x060018B7 RID: 6327
		[EngineMethod("get_voice_definitions_with_monster_sound_and_collision_info_class_name", false, null, false)]
		void GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassName(string className, int[] definitionIndices);
	}
}
