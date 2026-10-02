using System;
using System.Collections.Generic;

namespace TaleWorlds.Engine
{
	// Token: 0x02000050 RID: 80
	public class Highlights
	{
		// Token: 0x06000869 RID: 2153 RVA: 0x00006A49 File Offset: 0x00004C49
		public static void Initialize()
		{
			EngineApplicationInterface.IHighlights.Initialize();
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00006A55 File Offset: 0x00004C55
		public static void OpenGroup(string id)
		{
			EngineApplicationInterface.IHighlights.OpenGroup(id);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00006A62 File Offset: 0x00004C62
		public static void CloseGroup(string id, bool destroy = false)
		{
			EngineApplicationInterface.IHighlights.CloseGroup(id, destroy);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00006A70 File Offset: 0x00004C70
		public static void SaveScreenshot(string highlightId, string groupId)
		{
			EngineApplicationInterface.IHighlights.SaveScreenshot(highlightId, groupId);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00006A7E File Offset: 0x00004C7E
		public static void SaveVideo(string highlightId, string groupId, int startDelta, int endDelta)
		{
			EngineApplicationInterface.IHighlights.SaveVideo(highlightId, groupId, startDelta, endDelta);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00006A90 File Offset: 0x00004C90
		public static void OpenSummary(List<string> groups)
		{
			string text = string.Join("::", groups);
			EngineApplicationInterface.IHighlights.OpenSummary(text);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00006AB4 File Offset: 0x00004CB4
		public static void AddHighlight(string id, string name)
		{
			EngineApplicationInterface.IHighlights.AddHighlight(id, name);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00006AC2 File Offset: 0x00004CC2
		public static void RemoveHighlight(string id)
		{
			EngineApplicationInterface.IHighlights.RemoveHighlight(id);
		}

		// Token: 0x020000BF RID: 191
		public enum Significance
		{
			// Token: 0x040003A9 RID: 937
			None,
			// Token: 0x040003AA RID: 938
			ExtremelyBad,
			// Token: 0x040003AB RID: 939
			VeryBad,
			// Token: 0x040003AC RID: 940
			Bad = 4,
			// Token: 0x040003AD RID: 941
			Neutral = 16,
			// Token: 0x040003AE RID: 942
			Good = 256,
			// Token: 0x040003AF RID: 943
			VeryGood = 512,
			// Token: 0x040003B0 RID: 944
			ExtremelyGoods = 1024,
			// Token: 0x040003B1 RID: 945
			Max = 2048
		}

		// Token: 0x020000C0 RID: 192
		public enum Type
		{
			// Token: 0x040003B3 RID: 947
			None,
			// Token: 0x040003B4 RID: 948
			Milestone,
			// Token: 0x040003B5 RID: 949
			Achievement,
			// Token: 0x040003B6 RID: 950
			Incident = 4,
			// Token: 0x040003B7 RID: 951
			StateChange = 8,
			// Token: 0x040003B8 RID: 952
			Unannounced = 16,
			// Token: 0x040003B9 RID: 953
			Max = 32
		}
	}
}
