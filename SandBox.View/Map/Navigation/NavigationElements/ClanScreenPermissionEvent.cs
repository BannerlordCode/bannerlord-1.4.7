using System;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace SandBox.View.Map.Navigation.NavigationElements
{
	// Token: 0x0200006B RID: 107
	public class ClanScreenPermissionEvent : EventBase
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x00024C16 File Offset: 0x00022E16
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x00024C1E File Offset: 0x00022E1E
		public Action<bool, TextObject> IsClanScreenAvailable { get; private set; }

		// Token: 0x0600049F RID: 1183 RVA: 0x00024C27 File Offset: 0x00022E27
		public ClanScreenPermissionEvent(Action<bool, TextObject> isClanScreenAvailable)
		{
			this.IsClanScreenAvailable = isClanScreenAvailable;
		}
	}
}
