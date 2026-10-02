using System;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003A3 RID: 931
	public class TutorialState : GameState
	{
		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x0600359F RID: 13727 RVA: 0x000DA0BC File Offset: 0x000D82BC
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x000DA0E2 File Offset: 0x000D82E2
		protected override void OnActivate()
		{
			base.OnActivate();
			this.MenuContext.Refresh();
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x000DA0F5 File Offset: 0x000D82F5
		protected override void OnFinalize()
		{
			this.MenuContext.Destroy();
			this._objectManager.UnregisterObject(this.MenuContext);
			this.MenuContext = null;
			base.OnFinalize();
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x000DA120 File Offset: 0x000D8320
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			this.MenuContext.OnTick(dt);
		}

		// Token: 0x04000F4F RID: 3919
		private MBObjectManager _objectManager = MBObjectManager.Instance;

		// Token: 0x04000F50 RID: 3920
		public MenuContext MenuContext = MBObjectManager.Instance.CreateObject<MenuContext>();
	}
}
