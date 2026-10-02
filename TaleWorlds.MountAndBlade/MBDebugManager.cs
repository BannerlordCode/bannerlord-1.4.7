using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C4 RID: 452
	public class MBDebugManager : IDebugManager
	{
		// Token: 0x06001B25 RID: 6949 RVA: 0x0005F554 File Offset: 0x0005D754
		void IDebugManager.SetCrashReportCustomString(string customString)
		{
			Utilities.SetCrashReportCustomString(customString);
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0005F55C File Offset: 0x0005D75C
		void IDebugManager.SetCrashReportCustomStack(string customStack)
		{
			Utilities.SetCrashReportCustomStack(customStack);
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0005F564 File Offset: 0x0005D764
		void IDebugManager.ShowWarning(string message)
		{
			MBDebug.ShowWarning(message);
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x0005F56C File Offset: 0x0005D76C
		void IDebugManager.ShowError(string message)
		{
			MBDebug.ShowError(message);
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x0005F574 File Offset: 0x0005D774
		void IDebugManager.ShowMessageBox(string lpText, string lpCaption, uint uType)
		{
			MBDebug.ShowMessageBox(lpText, lpCaption, uType);
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x0005F57E File Offset: 0x0005D77E
		void IDebugManager.Assert(bool condition, string message, string callerFile, string callerMethod, int callerLine)
		{
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0005F580 File Offset: 0x0005D780
		void IDebugManager.SilentAssert(bool condition, string message, bool getDump, string callerFile, string callerMethod, int callerLine)
		{
			MBDebug.SilentAssert(condition, message, getDump, callerFile, callerMethod, callerLine);
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x0005F590 File Offset: 0x0005D790
		void IDebugManager.Print(string message, int logLevel, Debug.DebugColor color, ulong debugFilter)
		{
			MBDebug.Print(message, logLevel, color, debugFilter);
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x0005F59C File Offset: 0x0005D79C
		void IDebugManager.PrintError(string error, string stackTrace, ulong debugFilter)
		{
			MBDebug.Print(error, 0, Debug.DebugColor.White, debugFilter);
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0005F5A8 File Offset: 0x0005D7A8
		void IDebugManager.PrintWarning(string warning, ulong debugFilter)
		{
			MBDebug.Print(warning, 0, Debug.DebugColor.White, debugFilter);
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x0005F5B4 File Offset: 0x0005D7B4
		void IDebugManager.DisplayDebugMessage(string message)
		{
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0005F5B6 File Offset: 0x0005D7B6
		void IDebugManager.WatchVariable(string name, object value)
		{
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0005F5B8 File Offset: 0x0005D7B8
		void IDebugManager.WriteDebugLineOnScreen(string message)
		{
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x0005F5BA File Offset: 0x0005D7BA
		void IDebugManager.RenderDebugLine(Vec3 position, Vec3 direction, uint color, bool depthCheck, float time)
		{
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x0005F5BC File Offset: 0x0005D7BC
		void IDebugManager.RenderDebugSphere(Vec3 position, float radius, uint color, bool depthCheck, float time)
		{
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x0005F5BE File Offset: 0x0005D7BE
		void IDebugManager.RenderDebugFrame(MatrixFrame frame, float lineLength, float time)
		{
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x0005F5C0 File Offset: 0x0005D7C0
		void IDebugManager.RenderDebugText(float screenX, float screenY, string text, uint color, float time)
		{
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x0005F5C2 File Offset: 0x0005D7C2
		void IDebugManager.RenderDebugText3D(Vec3 position, string text, uint color, int screenPosOffsetX, int screenPosOffsetY, float time)
		{
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x0005F5C4 File Offset: 0x0005D7C4
		void IDebugManager.RenderDebugRectWithColor(float left, float bottom, float right, float top, uint color)
		{
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x0005F5C6 File Offset: 0x0005D7C6
		Vec3 IDebugManager.GetDebugVector()
		{
			return MBDebug.DebugVector;
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x0005F5CD File Offset: 0x0005D7CD
		void IDebugManager.SetDebugVector(Vec3 value)
		{
			MBDebug.DebugVector = value;
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x0005F5D5 File Offset: 0x0005D7D5
		void IDebugManager.SetTestModeEnabled(bool testModeEnabled)
		{
			MBDebug.TestModeEnabled = testModeEnabled;
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x0005F5DD File Offset: 0x0005D7DD
		void IDebugManager.AbortGame()
		{
			MBDebug.AbortGame(5);
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x0005F5E5 File Offset: 0x0005D7E5
		void IDebugManager.DoDelayedexit(int returnCode)
		{
			Utilities.DoDelayedexit(returnCode);
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x0005F5ED File Offset: 0x0005D7ED
		void IDebugManager.ReportMemoryBookmark(string message)
		{
		}
	}
}
