using System;

namespace SandBox.ViewModelCollection.GameOver
{
	// Token: 0x0200005C RID: 92
	public class StatItem
	{
		// Token: 0x0600059E RID: 1438 RVA: 0x00014EF0 File Offset: 0x000130F0
		public StatItem(string id, string value, StatItem.StatType type = StatItem.StatType.None)
		{
			this.ID = id;
			this.Value = value;
			this.Type = type;
		}

		// Token: 0x040002C4 RID: 708
		public readonly string ID;

		// Token: 0x040002C5 RID: 709
		public readonly string Value;

		// Token: 0x040002C6 RID: 710
		public readonly StatItem.StatType Type;

		// Token: 0x020000B9 RID: 185
		public enum StatType
		{
			// Token: 0x0400040C RID: 1036
			None,
			// Token: 0x0400040D RID: 1037
			Influence,
			// Token: 0x0400040E RID: 1038
			Issue,
			// Token: 0x0400040F RID: 1039
			Tournament,
			// Token: 0x04000410 RID: 1040
			Gold,
			// Token: 0x04000411 RID: 1041
			Crime,
			// Token: 0x04000412 RID: 1042
			Kill
		}
	}
}
