using System;
using System.Collections.Generic;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.GauntletUI.Mission;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000017 RID: 23
	[OverrideView(typeof(MissionMultiplayerPracticeEscapeMenu))]
	public class MissionGauntletMultiplayerPracticeEscapeMenu : MissionGauntletEscapeMenuBase
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00006BE0 File Offset: 0x00004DE0
		public MissionGauntletMultiplayerPracticeEscapeMenu()
			: base("MultiplayerEscapeMenu")
		{
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006BED File Offset: 0x00004DED
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this.DataSource = new MPEscapeMenuVM(null, null);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006C02 File Offset: 0x00004E02
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this.DataSource.Tick(dt);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006C18 File Offset: 0x00004E18
		protected override List<EscapeMenuItemVM> GetEscapeMenuItems()
		{
			List<EscapeMenuItemVM> list = new List<EscapeMenuItemVM>();
			list.Add(new EscapeMenuItemVM(new TextObject("{=e139gKZc}Return to the Game", null), delegate(object o)
			{
				base.OnEscapeMenuToggled(false);
			}, null, () => new Tuple<bool, TextObject>(false, null), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=EXqcmGy4}Return to Lobby", null), delegate(object o)
			{
				base.OnEscapeMenuToggled(false);
				base.Mission.EndMission();
			}, null, () => new Tuple<bool, TextObject>(false, null), false));
			return list;
		}
	}
}
