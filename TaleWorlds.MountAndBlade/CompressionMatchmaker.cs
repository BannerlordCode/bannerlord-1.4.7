using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002FF RID: 767
	public static class CompressionMatchmaker
	{
		// Token: 0x040010F6 RID: 4342
		public static CompressionInfo.Integer KillDeathAssistCountCompressionInfo = new CompressionInfo.Integer(-1000, 100000, true);

		// Token: 0x040010F7 RID: 4343
		public static CompressionInfo.Float MissionTimeCompressionInfo = new CompressionInfo.Float(-5f, 86400f, 20);

		// Token: 0x040010F8 RID: 4344
		public static CompressionInfo.Float MissionTimeLowPrecisionCompressionInfo = new CompressionInfo.Float(-5f, 12, 4f);

		// Token: 0x040010F9 RID: 4345
		public static CompressionInfo.Integer MissionCurrentStateCompressionInfo = new CompressionInfo.Integer(0, 6);

		// Token: 0x040010FA RID: 4346
		public static CompressionInfo.Integer ScoreCompressionInfo = new CompressionInfo.Integer(-1000000, 21);
	}
}
