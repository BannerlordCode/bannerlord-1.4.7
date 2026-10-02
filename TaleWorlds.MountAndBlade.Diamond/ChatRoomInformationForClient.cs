using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000FF RID: 255
	[Serializable]
	public class ChatRoomInformationForClient
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x00006E2D File Offset: 0x0000502D
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x00006E35 File Offset: 0x00005035
		[JsonProperty]
		public Guid RoomId { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00006E3E File Offset: 0x0000503E
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x00006E46 File Offset: 0x00005046
		[JsonProperty]
		public string Name { get; private set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00006E4F File Offset: 0x0000504F
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x00006E57 File Offset: 0x00005057
		[JsonProperty]
		public string Endpoint { get; private set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00006E60 File Offset: 0x00005060
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x00006E68 File Offset: 0x00005068
		[JsonProperty]
		public string RoomColor { get; private set; }

		// Token: 0x0600056A RID: 1386 RVA: 0x00006E71 File Offset: 0x00005071
		public ChatRoomInformationForClient()
		{
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00006E79 File Offset: 0x00005079
		public ChatRoomInformationForClient(Guid roomId, string name, string endpoint, string color)
		{
			this.RoomId = roomId;
			this.Name = name;
			this.Endpoint = endpoint;
			this.RoomColor = color;
		}
	}
}
