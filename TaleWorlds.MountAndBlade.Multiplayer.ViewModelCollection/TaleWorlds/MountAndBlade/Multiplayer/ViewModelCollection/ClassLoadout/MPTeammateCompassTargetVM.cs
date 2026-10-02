using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.Compass;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000A7 RID: 167
	public class MPTeammateCompassTargetVM : CompassTargetVM
	{
		// Token: 0x06000FEA RID: 4074 RVA: 0x00030E7C File Offset: 0x0002F07C
		public MPTeammateCompassTargetVM(TargetIconType iconType, uint color, uint color2, Banner banner, bool isAlly)
			: base(iconType, color, color2, banner, false, isAlly)
		{
			base.IconType = iconType.ToString();
			base.IsFlag = false;
			base.Banner = ((banner != null) ? new BannerImageIdentifierVM(banner, false) : new BannerImageIdentifierVM(null, false));
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00030ECC File Offset: 0x0002F0CC
		public void RefreshTargetIconType(TargetIconType targetIconType)
		{
			base.IconType = targetIconType.ToString();
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00030EE1 File Offset: 0x0002F0E1
		public void RefreshTeam(Banner banner, bool isAlly)
		{
			base.Banner = ((banner != null) ? new BannerImageIdentifierVM(banner, false) : new BannerImageIdentifierVM(null, false));
			base.IsEnemy = !isAlly;
		}
	}
}
