using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Clan
{
	// Token: 0x020000B3 RID: 179
	public class MultiplayerLobbyClanMemberRankVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000954 RID: 2388 RVA: 0x0001A4A7 File Offset: 0x000186A7
		public MultiplayerLobbyClanMemberRankVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0001A4B8 File Offset: 0x000186B8
		private void UpdateTypeVisual()
		{
			if (this.Type == 0)
			{
				this.SetState("Member");
				return;
			}
			if (this.Type == 1)
			{
				this.SetState("Officer");
				return;
			}
			if (this.Type == 2)
			{
				this.SetState("Leader");
				return;
			}
			Debug.FailedAssert("This member type is not defined in widget", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Multiplayer\\Lobby\\Clan\\MultiplayerLobbyClanMemberRankVisualBrushWidget.cs", "UpdateTypeVisual", 28);
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x0001A519 File Offset: 0x00018719
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x0001A521 File Offset: 0x00018721
		[Editor(false)]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged(value, "Type");
					this.UpdateTypeVisual();
				}
			}
		}

		// Token: 0x04000436 RID: 1078
		private int _type = -1;
	}
}
