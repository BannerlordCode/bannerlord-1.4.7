using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000076 RID: 118
	internal class AdminPanelOptionGroup : IAdminPanelOptionGroup, IAdminPanelTickable
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000FEC6 File Offset: 0x0000E0C6
		string IAdminPanelOptionGroup.UniqueId
		{
			get
			{
				return this._uniqueId;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0000FECE File Offset: 0x0000E0CE
		TextObject IAdminPanelOptionGroup.Name
		{
			get
			{
				return this._nameTextObj;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0000FED6 File Offset: 0x0000E0D6
		MBReadOnlyList<IAdminPanelOption> IAdminPanelOptionGroup.Options
		{
			get
			{
				return this._options;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000FEDE File Offset: 0x0000E0DE
		MBReadOnlyList<IAdminPanelAction> IAdminPanelOptionGroup.Actions
		{
			get
			{
				return this._actions;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000FEE6 File Offset: 0x0000E0E6
		bool IAdminPanelOptionGroup.RequiresRestart
		{
			get
			{
				return this._requiresRestart;
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000FEEE File Offset: 0x0000E0EE
		public AdminPanelOptionGroup(string uniqueId, TextObject name, bool requiresRestart = false)
		{
			this._uniqueId = uniqueId;
			this._nameTextObj = name;
			this._requiresRestart = requiresRestart;
			this._options = new MBList<IAdminPanelOption>();
			this._actions = new MBList<IAdminPanelAction>();
			this._tickableOptions = new MBList<IAdminPanelTickable>();
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000FF2C File Offset: 0x0000E12C
		public void AddOption(IAdminPanelOption option)
		{
			this._options.Add(option);
			IAdminPanelTickable adminPanelTickable;
			if ((adminPanelTickable = option as IAdminPanelTickable) != null)
			{
				this._tickableOptions.Add(adminPanelTickable);
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000FF5B File Offset: 0x0000E15B
		public void AddAction(IAdminPanelAction action)
		{
			this._actions.Add(action);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000FF6C File Offset: 0x0000E16C
		void IAdminPanelTickable.OnTick(float dt)
		{
			for (int i = 0; i < this._tickableOptions.Count; i++)
			{
				this._tickableOptions[i].OnTick(dt);
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000FFA4 File Offset: 0x0000E1A4
		void IAdminPanelOptionGroup.OnFinalize()
		{
			for (int i = 0; i < this._options.Count; i++)
			{
				IAdminPanelOptionInternal adminPanelOptionInternal;
				if ((adminPanelOptionInternal = this._options[i] as IAdminPanelOptionInternal) != null)
				{
					adminPanelOptionInternal.OnFinalize();
				}
			}
			for (int j = 0; j < this._actions.Count; j++)
			{
				IAdminPanelActionInternal adminPanelActionInternal;
				if ((adminPanelActionInternal = this._actions[j] as IAdminPanelActionInternal) != null)
				{
					adminPanelActionInternal.OnFinalize();
				}
			}
		}

		// Token: 0x04000110 RID: 272
		private readonly bool _requiresRestart;

		// Token: 0x04000111 RID: 273
		private readonly string _uniqueId;

		// Token: 0x04000112 RID: 274
		private readonly TextObject _nameTextObj;

		// Token: 0x04000113 RID: 275
		private readonly MBList<IAdminPanelOption> _options;

		// Token: 0x04000114 RID: 276
		private readonly MBList<IAdminPanelAction> _actions;

		// Token: 0x04000115 RID: 277
		private readonly MBList<IAdminPanelTickable> _tickableOptions;
	}
}
