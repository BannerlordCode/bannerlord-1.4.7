using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000CC RID: 204
	public class MultiplayerClassLoadoutItemTabListPanel : ListPanel
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000AA4 RID: 2724 RVA: 0x0001DDEC File Offset: 0x0001BFEC
		// (remove) Token: 0x06000AA5 RID: 2725 RVA: 0x0001DE24 File Offset: 0x0001C024
		public event Action OnInitialized;

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0001DE59 File Offset: 0x0001C059
		public MultiplayerClassLoadoutItemTabListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0001DE62 File Offset: 0x0001C062
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isInitialized)
			{
				this._isInitialized = true;
				Action onInitialized = this.OnInitialized;
				if (onInitialized == null)
				{
					return;
				}
				onInitialized();
			}
		}

		// Token: 0x040004DA RID: 1242
		private bool _isInitialized;
	}
}
