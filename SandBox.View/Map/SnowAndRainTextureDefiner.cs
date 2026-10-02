using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;

namespace SandBox.View.Map
{
	// Token: 0x0200005F RID: 95
	public class SnowAndRainTextureDefiner : ScriptComponentBehavior
	{
		// Token: 0x060003C0 RID: 960 RVA: 0x0001DEB8 File Offset: 0x0001C0B8
		protected override void OnInit()
		{
			this.SetDataToScene();
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0001DEC0 File Offset: 0x0001C0C0
		protected override void OnTerrainReload(int step)
		{
			if (step == 1)
			{
				this.SetDataToScene();
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001DECC File Offset: 0x0001C0CC
		protected override void OnEditorInit()
		{
			if (base.GameEntity.Scene.ContainsTerrain)
			{
				base.GameEntity.Scene.SetDynamicSnowTexture(this.SnowAndRainTexture);
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001DF08 File Offset: 0x0001C108
		protected override void OnEditorVariableChanged(string variableName)
		{
			if (variableName == "SnowAndRainTexture" && base.GameEntity.Scene.ContainsTerrain)
			{
				base.GameEntity.Scene.SetDynamicSnowTexture(this.SnowAndRainTexture);
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0001DF50 File Offset: 0x0001C150
		private void SetDataToScene()
		{
			if (this.SnowAndRainTexture != null)
			{
				((MapScene)Campaign.Current.MapSceneWrapper).SetSnowAndRainDataWithDimension(this.SnowAndRainTexture, this.WeatherNodeGridWidthAndHeight);
			}
		}

		// Token: 0x040001E8 RID: 488
		[EditorVisibleScriptComponentVariable(true)]
		public Texture SnowAndRainTexture;

		// Token: 0x040001E9 RID: 489
		[EditorVisibleScriptComponentVariable(true)]
		public int WeatherNodeGridWidthAndHeight;
	}
}
