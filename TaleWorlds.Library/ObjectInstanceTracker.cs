using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x02000074 RID: 116
	public class ObjectInstanceTracker
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x0000E973 File Offset: 0x0000CB73
		public static void RegisterTrackedInstance(string name, WeakReference instance)
		{
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000E978 File Offset: 0x0000CB78
		public static bool CheckBlacklistedTypeCounts(Dictionary<string, int> typeNameCounts, ref string outputLog)
		{
			bool flag = false;
			foreach (string text in typeNameCounts.Keys)
			{
				int num = 0;
				int num2 = typeNameCounts[text];
				List<WeakReference> list;
				if (ObjectInstanceTracker.TrackedInstances.TryGetValue(text, out list))
				{
					using (List<WeakReference>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.Target != null)
							{
								num++;
							}
						}
					}
				}
				if (num != num2)
				{
					flag = true;
					outputLog = string.Concat(new object[] { outputLog, "Type(", text, ") has ", num, " alive instance, but its should be ", num2, "\n" });
				}
			}
			return flag;
		}

		// Token: 0x0400014A RID: 330
		private static Dictionary<string, List<WeakReference>> TrackedInstances = new Dictionary<string, List<WeakReference>>();
	}
}
