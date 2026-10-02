using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000085 RID: 133
	public class SpectatorCameraView : MissionView
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x00025782 File Offset: 0x00023982
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			base.MissionScreen.SceneLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("MultiplayerHotkeyCategory"));
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000257AC File Offset: 0x000239AC
		public override void AfterStart()
		{
			for (int i = 0; i < 10; i++)
			{
				this._spectateCamerFrames.Add(MatrixFrame.Identity);
			}
			for (int j = 0; j < 10; j++)
			{
				string text = "spectate_cam_" + j.ToString();
				List<GameEntity> list = Mission.Current.Scene.FindEntitiesWithTag(text).ToList<GameEntity>();
				if (list.Count > 0)
				{
					this._spectateCamerFrames[j] = list[0].GetGlobalFrame();
				}
			}
		}

		// Token: 0x040002D8 RID: 728
		private List<MatrixFrame> _spectateCamerFrames = new List<MatrixFrame>();
	}
}
