using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200011A RID: 282
	[Serializable]
	public class GameLog
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x00007FB6 File Offset: 0x000061B6
		// (set) Token: 0x06000624 RID: 1572 RVA: 0x00007FBE File Offset: 0x000061BE
		public int Id { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00007FC7 File Offset: 0x000061C7
		// (set) Token: 0x06000626 RID: 1574 RVA: 0x00007FCF File Offset: 0x000061CF
		public GameLogType Type { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x00007FD8 File Offset: 0x000061D8
		// (set) Token: 0x06000628 RID: 1576 RVA: 0x00007FE0 File Offset: 0x000061E0
		public PlayerId Player { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x00007FE9 File Offset: 0x000061E9
		// (set) Token: 0x0600062A RID: 1578 RVA: 0x00007FF1 File Offset: 0x000061F1
		public float GameTime { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x00007FFA File Offset: 0x000061FA
		// (set) Token: 0x0600062C RID: 1580 RVA: 0x00008002 File Offset: 0x00006202
		public Dictionary<string, string> Data { get; set; }

		// Token: 0x0600062D RID: 1581 RVA: 0x0000800B File Offset: 0x0000620B
		public GameLog()
		{
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00008013 File Offset: 0x00006213
		public GameLog(GameLogType type, PlayerId player, float gameTime)
		{
			this.Type = type;
			this.Player = player;
			this.GameTime = gameTime;
			this.Data = new Dictionary<string, string>();
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0000803C File Offset: 0x0000623C
		public string GetDataAsString()
		{
			string text = "{}";
			try
			{
				text = JsonConvert.SerializeObject(this.Data, Formatting.None);
			}
			catch (Exception)
			{
			}
			return text;
		}
	}
}
