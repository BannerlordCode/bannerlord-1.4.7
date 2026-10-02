using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200008E RID: 142
	[EngineStruct("Managed_sound_event_parameter", false, null)]
	public struct SoundEventParameter
	{
		// Token: 0x06000CBB RID: 3259 RVA: 0x0000E345 File Offset: 0x0000C545
		public SoundEventParameter(string paramName, float value)
		{
			this.ParamName = paramName;
			this.Value = value;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0000E355 File Offset: 0x0000C555
		public void Update(string paramName, float value)
		{
			this.ParamName = paramName;
			this.Value = value;
		}

		// Token: 0x040001C8 RID: 456
		[CustomEngineStructMemberData("str_id")]
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		internal string ParamName;

		// Token: 0x040001C9 RID: 457
		internal float Value;
	}
}
