using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x0200009E RID: 158
	public class MultiplayerLobbyBadgeButtonWidget : ButtonWidget
	{
		// Token: 0x06000878 RID: 2168 RVA: 0x000184EB File Offset: 0x000166EB
		public MultiplayerLobbyBadgeButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x000184F4 File Offset: 0x000166F4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.EventManager.HoveredWidget == this && Input.IsKeyPressed(InputKey.ControllerRUp))
			{
				this.OnMouseAlternatePressed();
				return;
			}
			if (base.EventManager.HoveredWidget == this && Input.IsKeyReleased(InputKey.ControllerRUp))
			{
				this.OnMouseAlternateReleased(true);
			}
		}
	}
}
