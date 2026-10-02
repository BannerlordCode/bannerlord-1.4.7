using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000097 RID: 151
	public class MultiplayerScoreboardStripedBackgroundWidget : MultiplayerScoreboardStatsListPanel
	{
		// Token: 0x06000828 RID: 2088 RVA: 0x000177AB File Offset: 0x000159AB
		public MultiplayerScoreboardStripedBackgroundWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x000177B4 File Offset: 0x000159B4
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			if (base.ChildCount % 2 == 1)
			{
				child.Sprite = base.Context.SpriteData.GetSprite("BlankWhiteSquare_9");
				child.Color = Color.ConvertStringToColor("#000000FF");
				child.AlphaFactor = 0.2f;
			}
		}
	}
}
