using System;
using TaleWorlds.Library.Http;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E4 RID: 740
	public class CommunityClient
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x000A4901 File Offset: 0x000A2B01
		// (set) Token: 0x06002ACA RID: 10954 RVA: 0x000A4909 File Offset: 0x000A2B09
		public bool IsInGame { get; private set; }

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002ACB RID: 10955 RVA: 0x000A4912 File Offset: 0x000A2B12
		// (set) Token: 0x06002ACC RID: 10956 RVA: 0x000A491A File Offset: 0x000A2B1A
		public ICommunityClientHandler Handler { get; set; }

		// Token: 0x06002ACD RID: 10957 RVA: 0x000A4923 File Offset: 0x000A2B23
		public CommunityClient()
		{
			this._httpDriver = HttpDriverManager.GetDefaultHttpDriver();
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x000A4936 File Offset: 0x000A2B36
		public void QuitFromGame()
		{
			if (this.IsInGame)
			{
				this.IsInGame = false;
				ICommunityClientHandler handler = this.Handler;
				if (handler == null)
				{
					return;
				}
				handler.OnQuitFromGame();
			}
		}

		// Token: 0x04001047 RID: 4167
		private IHttpDriver _httpDriver;
	}
}
