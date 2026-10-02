using System;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.Engine
{
	// Token: 0x02000046 RID: 70
	public class ScreenManagerEngineConnection : IScreenManagerEngineConnection
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00004223 File Offset: 0x00002423
		float IScreenManagerEngineConnection.RealScreenResolutionWidth
		{
			get
			{
				return Screen.RealScreenResolutionWidth;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x0000422A File Offset: 0x0000242A
		float IScreenManagerEngineConnection.RealScreenResolutionHeight
		{
			get
			{
				return Screen.RealScreenResolutionHeight;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00004231 File Offset: 0x00002431
		float IScreenManagerEngineConnection.AspectRatio
		{
			get
			{
				return Screen.AspectRatio;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x00004238 File Offset: 0x00002438
		Vec2 IScreenManagerEngineConnection.DesktopResolution
		{
			get
			{
				return Screen.DesktopResolution;
			}
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0000423F File Offset: 0x0000243F
		void IScreenManagerEngineConnection.ActivateMouseCursor(CursorType mouseId)
		{
			MouseManager.ActivateMouseCursor(mouseId);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00004247 File Offset: 0x00002447
		void IScreenManagerEngineConnection.SetMouseVisible(bool value)
		{
			EngineApplicationInterface.IScreen.SetMouseVisible(value);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00004254 File Offset: 0x00002454
		bool IScreenManagerEngineConnection.GetMouseVisible()
		{
			return EngineApplicationInterface.IScreen.GetMouseVisible();
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00004260 File Offset: 0x00002460
		bool IScreenManagerEngineConnection.GetIsEnterButtonRDown()
		{
			return EngineApplicationInterface.IScreen.IsEnterButtonCross();
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0000426C File Offset: 0x0000246C
		void IScreenManagerEngineConnection.BeginDebugPanel(string panelTitle)
		{
			Imgui.BeginMainThreadScope();
			Imgui.Begin(panelTitle);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00004279 File Offset: 0x00002479
		void IScreenManagerEngineConnection.EndDebugPanel()
		{
			Imgui.End();
			Imgui.EndMainThreadScope();
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00004285 File Offset: 0x00002485
		void IScreenManagerEngineConnection.DrawDebugText(string text)
		{
			Imgui.Text(text);
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0000428D File Offset: 0x0000248D
		bool IScreenManagerEngineConnection.DrawDebugTreeNode(string text)
		{
			return Imgui.TreeNode(text);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00004295 File Offset: 0x00002495
		void IScreenManagerEngineConnection.PopDebugTreeNode()
		{
			Imgui.TreePop();
		}
	}
}
