using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000062 RID: 98
	public class PopupSceneSwitchCameraSequence : PopupSceneSequence
	{
		// Token: 0x060003B1 RID: 945 RVA: 0x0001B890 File Offset: 0x00019A90
		protected override void OnInit()
		{
			this._switchEntity = base.GameEntity.Scene.GetFirstEntityWithName(this.EntityName);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0001B8BC File Offset: 0x00019ABC
		public override void OnInitialState()
		{
			if (this._switchEntity != null)
			{
				GameEntity gameEntity = base.GameEntity.Scene.FindEntityWithTag("customcamera");
				if (gameEntity != null)
				{
					gameEntity.RemoveTag("customcamera");
				}
				this._switchEntity.AddTag("customcamera");
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001B90F File Offset: 0x00019B0F
		public override void OnPositiveState()
		{
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001B911 File Offset: 0x00019B11
		public override void OnNegativeState()
		{
		}

		// Token: 0x04000217 RID: 535
		public string EntityName = "";

		// Token: 0x04000218 RID: 536
		private GameEntity _switchEntity;
	}
}
