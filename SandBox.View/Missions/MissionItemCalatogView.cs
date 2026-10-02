using System;
using SandBox.Missions.MissionLogics;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x0200001D RID: 29
	public class MissionItemCalatogView : MissionView
	{
		// Token: 0x060000CC RID: 204 RVA: 0x00009C60 File Offset: 0x00007E60
		public override void AfterStart()
		{
			base.AfterStart();
			this._itemCatalogController = base.Mission.GetMissionBehavior<ItemCatalogController>();
			this._itemCatalogController.BeforeCatalogTick += this.OnBeforeCatalogTick;
			this._itemCatalogController.AfterCatalogTick += this.OnAfterCatalogTick;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00009CB2 File Offset: 0x00007EB2
		private void OnBeforeCatalogTick(int currentItemIndex)
		{
			Utilities.TakeScreenshot("ItemCatalog/" + this._itemCatalogController.AllItems[currentItemIndex - 1].Name + ".bmp");
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00009CE0 File Offset: 0x00007EE0
		private void OnAfterCatalogTick()
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			Vec3 lookDirection = base.Mission.MainAgent.LookDirection;
			matrixFrame.origin = base.Mission.MainAgent.Position + lookDirection * 2f + new Vec3(0f, 0f, 1.273f, -1f);
			matrixFrame.rotation.u = lookDirection;
			matrixFrame.rotation.s = new Vec3(1f, 0f, 0f, -1f);
			matrixFrame.rotation.f = new Vec3(0f, 0f, 1f, -1f);
			matrixFrame.rotation.Orthonormalize();
			base.Mission.SetCameraFrame(ref matrixFrame, 1f);
			Camera camera = Camera.CreateCamera();
			camera.Frame = matrixFrame;
			base.MissionScreen.CustomCamera = camera;
		}

		// Token: 0x04000079 RID: 121
		private ItemCatalogController _itemCatalogController;
	}
}
