using System;
using System.Collections.Generic;
using TaleWorlds.InputSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200022E RID: 558
	public class GenericPanelGameKeyCategory : GameKeyContext
	{
		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060020C6 RID: 8390 RVA: 0x00073771 File Offset: 0x00071971
		// (set) Token: 0x060020C7 RID: 8391 RVA: 0x00073778 File Offset: 0x00071978
		public static GenericPanelGameKeyCategory Current { get; private set; }

		// Token: 0x060020C8 RID: 8392 RVA: 0x00073780 File Offset: 0x00071980
		public GenericPanelGameKeyCategory(string categoryId = "GenericPanelGameKeyCategory")
			: base(categoryId, 116, GameKeyContext.GameKeyContextType.Default)
		{
			GenericPanelGameKeyCategory.Current = this;
			this.RegisterHotKeys();
			this.RegisterGameKeys();
			this.RegisterGameAxisKeys();
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000737A4 File Offset: 0x000719A4
		private void RegisterHotKeys()
		{
			List<Key> list = new List<Key>
			{
				new Key(InputKey.Escape),
				new Key(InputKey.ControllerRRight)
			};
			List<Key> list2 = new List<Key>
			{
				new Key(InputKey.Enter),
				new Key(InputKey.NumpadEnter),
				new Key(InputKey.ControllerRLeft)
			};
			List<Key> list3 = new List<Key>
			{
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list4 = new List<Key>
			{
				new Key(InputKey.Escape),
				new Key(InputKey.ControllerROption)
			};
			List<Key> list5 = new List<Key>
			{
				new Key(InputKey.Q),
				new Key(InputKey.ControllerLBumper)
			};
			List<Key> list6 = new List<Key>
			{
				new Key(InputKey.E),
				new Key(InputKey.ControllerRBumper)
			};
			List<Key> list7 = new List<Key>
			{
				new Key(InputKey.D),
				new Key(InputKey.ControllerRTrigger)
			};
			List<Key> list8 = new List<Key>
			{
				new Key(InputKey.A),
				new Key(InputKey.ControllerLTrigger)
			};
			List<Key> list9 = new List<Key>
			{
				new Key(InputKey.R),
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list10 = new List<Key>
			{
				new Key(InputKey.ControllerROption)
			};
			List<Key> list11 = new List<Key>
			{
				new Key(InputKey.Delete),
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list12 = new List<Key>
			{
				new Key(InputKey.Escape),
				new Key(InputKey.ControllerRUp)
			};
			List<Key> list13 = new List<Key>
			{
				new Key(InputKey.Enter),
				new Key(InputKey.NumpadEnter),
				new Key(InputKey.ControllerRDown)
			};
			base.RegisterHotKey(new HotKey("Exit", "GenericPanelGameKeyCategory", list, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Confirm", "GenericPanelGameKeyCategory", list2, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Reset", "GenericPanelGameKeyCategory", list3, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("ToggleEscapeMenu", "GenericPanelGameKeyCategory", list4, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("SwitchToPreviousTab", "GenericPanelGameKeyCategory", list5, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("SwitchToNextTab", "GenericPanelGameKeyCategory", list6, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("GiveAll", "GenericPanelGameKeyCategory", list7, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("TakeAll", "GenericPanelGameKeyCategory", list8, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Randomize", "GenericPanelGameKeyCategory", list9, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Start", "GenericPanelGameKeyCategory", list10, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Delete", "GenericPanelGameKeyCategory", list11, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("SelectProfile", "GenericPanelGameKeyCategory", list12, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
			base.RegisterHotKey(new HotKey("Play", "GenericPanelGameKeyCategory", list13, HotKey.Modifiers.None, HotKey.Modifiers.None), true);
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x00073AD5 File Offset: 0x00071CD5
		private void RegisterGameKeys()
		{
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00073AD7 File Offset: 0x00071CD7
		private void RegisterGameAxisKeys()
		{
		}

		// Token: 0x04000C1F RID: 3103
		public const string CategoryId = "GenericPanelGameKeyCategory";

		// Token: 0x04000C20 RID: 3104
		public const string Exit = "Exit";

		// Token: 0x04000C21 RID: 3105
		public const string Confirm = "Confirm";

		// Token: 0x04000C22 RID: 3106
		public const string ResetChanges = "Reset";

		// Token: 0x04000C23 RID: 3107
		public const string ToggleEscapeMenu = "ToggleEscapeMenu";

		// Token: 0x04000C24 RID: 3108
		public const string SwitchToPreviousTab = "SwitchToPreviousTab";

		// Token: 0x04000C25 RID: 3109
		public const string SwitchToNextTab = "SwitchToNextTab";

		// Token: 0x04000C26 RID: 3110
		public const string GiveAll = "GiveAll";

		// Token: 0x04000C27 RID: 3111
		public const string TakeAll = "TakeAll";

		// Token: 0x04000C28 RID: 3112
		public const string Randomize = "Randomize";

		// Token: 0x04000C29 RID: 3113
		public const string Start = "Start";

		// Token: 0x04000C2A RID: 3114
		public const string Delete = "Delete";

		// Token: 0x04000C2B RID: 3115
		public const string SelectProfile = "SelectProfile";

		// Token: 0x04000C2C RID: 3116
		public const string Play = "Play";
	}
}
