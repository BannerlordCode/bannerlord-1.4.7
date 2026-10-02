using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000074 RID: 116
	internal class AdminPanelAction : IAdminPanelActionInternal, IAdminPanelAction
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0000FAAB File Offset: 0x0000DCAB
		public string UniqueId
		{
			get
			{
				return this._uniqueId;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000FAB3 File Offset: 0x0000DCB3
		public string Name
		{
			get
			{
				TextObject nameTextObj = this._nameTextObj;
				return ((nameTextObj != null) ? nameTextObj.ToString() : null) ?? string.Empty;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000373 RID: 883 RVA: 0x0000FAD0 File Offset: 0x0000DCD0
		public string Description
		{
			get
			{
				TextObject descriptionTextObj = this._descriptionTextObj;
				return ((descriptionTextObj != null) ? descriptionTextObj.ToString() : null) ?? string.Empty;
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000FAED File Offset: 0x0000DCED
		public AdminPanelAction(string uniqueId)
		{
			this._uniqueId = uniqueId;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000FAFC File Offset: 0x0000DCFC
		public virtual void OnFinalize()
		{
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000FAFE File Offset: 0x0000DCFE
		void IAdminPanelAction.OnActionExecuted()
		{
			Action onActionExecuted = this._onActionExecuted;
			if (onActionExecuted == null)
			{
				return;
			}
			onActionExecuted();
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000FB10 File Offset: 0x0000DD10
		public virtual bool GetIsAvailable()
		{
			return true;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000FB13 File Offset: 0x0000DD13
		public virtual bool GetIsDisabled(out string reason)
		{
			reason = string.Empty;
			return false;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000FB1D File Offset: 0x0000DD1D
		public AdminPanelAction BuildName(TextObject name)
		{
			this._nameTextObj = name;
			return this;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000FB27 File Offset: 0x0000DD27
		public AdminPanelAction BuildDescription(TextObject description)
		{
			this._descriptionTextObj = description;
			return this;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000FB31 File Offset: 0x0000DD31
		public AdminPanelAction BuildOnActionExecutedCallback(Action onActionExecuted)
		{
			this._onActionExecuted = onActionExecuted;
			return this;
		}

		// Token: 0x040000FF RID: 255
		private readonly string _uniqueId;

		// Token: 0x04000100 RID: 256
		private TextObject _nameTextObj;

		// Token: 0x04000101 RID: 257
		private TextObject _descriptionTextObj;

		// Token: 0x04000102 RID: 258
		private Action _onActionExecuted;
	}
}
