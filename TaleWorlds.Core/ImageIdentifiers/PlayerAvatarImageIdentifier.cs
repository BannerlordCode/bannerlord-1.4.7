using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E9 RID: 233
	public class PlayerAvatarImageIdentifier : ImageIdentifier
	{
		// Token: 0x06000B97 RID: 2967 RVA: 0x000256EB File Offset: 0x000238EB
		public PlayerAvatarImageIdentifier(PlayerId playerId, int forcedAvatarIndex)
		{
			base.Id = playerId.ToString();
			base.AdditionalArgs = string.Format("{0}", forcedAvatarIndex);
			base.TextureProviderName = "PlayerAvatarImageTextureProvider";
		}
	}
}
