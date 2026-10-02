using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x0200002B RID: 43
	public class DiamondDebugManager : IDebugManager
	{
		// Token: 0x06000143 RID: 323 RVA: 0x00005C9D File Offset: 0x00003E9D
		public DiamondDebugManager(ParameterContainer parameters)
		{
			this._parameters = parameters;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005CAC File Offset: 0x00003EAC
		public DiamondDebugManager()
		{
			this._parameters = null;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005CBB File Offset: 0x00003EBB
		void IDebugManager.SetCrashReportCustomString(string customString)
		{
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005CBD File Offset: 0x00003EBD
		void IDebugManager.SetCrashReportCustomStack(string customStack)
		{
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005CBF File Offset: 0x00003EBF
		void IDebugManager.ShowMessageBox(string lpText, string lpCaption, uint uType)
		{
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005CC1 File Offset: 0x00003EC1
		void IDebugManager.ShowError(string message)
		{
			this.PrintMessage(message, DiamondDebugManager.DiamondDebugCategory.Error);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005CCB File Offset: 0x00003ECB
		void IDebugManager.ShowWarning(string message)
		{
			this.PrintMessage(message, DiamondDebugManager.DiamondDebugCategory.Warning);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005CD5 File Offset: 0x00003ED5
		void IDebugManager.Assert(bool condition, string message, string callerFile, string callerMethod, int callerLine)
		{
			if (!condition)
			{
				throw new Exception(string.Format("Assertion failed: {0} in {1}, line:{2}", message, callerFile, callerLine));
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005CF3 File Offset: 0x00003EF3
		void IDebugManager.SilentAssert(bool condition, string message, bool getDump, string callerFile, string callerMethod, int callerLine)
		{
			if (!condition)
			{
				this.PrintMessage(string.Format("Assertion failed: {0} in {1}, line:{2}", message, callerMethod, callerLine), DiamondDebugManager.DiamondDebugCategory.Warning);
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005D13 File Offset: 0x00003F13
		void IDebugManager.Print(string message, int logLevel, Debug.DebugColor color, ulong debugFilter)
		{
			this.PrintMessage(message, DiamondDebugManager.DiamondDebugCategory.General);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005D1D File Offset: 0x00003F1D
		void IDebugManager.PrintError(string error, string stackTrace, ulong debugFilter)
		{
			this.PrintMessage(error + stackTrace, DiamondDebugManager.DiamondDebugCategory.Error);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005D2D File Offset: 0x00003F2D
		void IDebugManager.PrintWarning(string warning, ulong debugFilter)
		{
			this.PrintMessage(warning, DiamondDebugManager.DiamondDebugCategory.Warning);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005D37 File Offset: 0x00003F37
		void IDebugManager.DisplayDebugMessage(string message)
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005D39 File Offset: 0x00003F39
		void IDebugManager.WatchVariable(string name, object value)
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005D3B File Offset: 0x00003F3B
		void IDebugManager.WriteDebugLineOnScreen(string message)
		{
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005D3D File Offset: 0x00003F3D
		void IDebugManager.RenderDebugLine(Vec3 position, Vec3 direction, uint color, bool depthCheck, float time)
		{
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005D3F File Offset: 0x00003F3F
		void IDebugManager.RenderDebugSphere(Vec3 position, float radius, uint color, bool depthCheck, float time)
		{
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005D41 File Offset: 0x00003F41
		void IDebugManager.RenderDebugFrame(MatrixFrame frame, float lineLength, float time)
		{
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005D43 File Offset: 0x00003F43
		void IDebugManager.RenderDebugText(float screenX, float screenY, string text, uint color, float time)
		{
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005D45 File Offset: 0x00003F45
		void IDebugManager.RenderDebugText3D(Vec3 position, string text, uint color, int screenPosOffsetX, int screenPosOffsetY, float time)
		{
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005D47 File Offset: 0x00003F47
		void IDebugManager.RenderDebugRectWithColor(float left, float bottom, float right, float top, uint color)
		{
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005D49 File Offset: 0x00003F49
		Vec3 IDebugManager.GetDebugVector()
		{
			return Vec3.Zero;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005D50 File Offset: 0x00003F50
		void IDebugManager.SetDebugVector(Vec3 value)
		{
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005D52 File Offset: 0x00003F52
		void IDebugManager.SetTestModeEnabled(bool testModeEnabled)
		{
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005D54 File Offset: 0x00003F54
		void IDebugManager.AbortGame()
		{
			Environment.Exit(-5);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005D5D File Offset: 0x00003F5D
		void IDebugManager.DoDelayedexit(int returnCode)
		{
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005D60 File Offset: 0x00003F60
		public int GetLogLevel()
		{
			int num;
			if (this._parameters != null && this._parameters.TryGetParameterAsInt("LogLevel", out num))
			{
				return num;
			}
			return 1;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00005D8C File Offset: 0x00003F8C
		protected void PrintMessage(string message, DiamondDebugManager.DiamondDebugCategory debugCategory)
		{
			if (this.GetLogLevel() <= (int)debugCategory)
			{
				Console.Out.Flush();
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = DiamondDebugManager._colors[debugCategory];
				Console.Write(message);
				Console.ResetColor();
				Console.WriteLine();
				Console.Out.Flush();
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005DDC File Offset: 0x00003FDC
		void IDebugManager.ReportMemoryBookmark(string message)
		{
		}

		// Token: 0x040000A2 RID: 162
		private static Dictionary<DiamondDebugManager.DiamondDebugCategory, ConsoleColor> _colors = new Dictionary<DiamondDebugManager.DiamondDebugCategory, ConsoleColor>
		{
			{
				DiamondDebugManager.DiamondDebugCategory.General,
				ConsoleColor.Green
			},
			{
				DiamondDebugManager.DiamondDebugCategory.Warning,
				ConsoleColor.Yellow
			},
			{
				DiamondDebugManager.DiamondDebugCategory.Error,
				ConsoleColor.Red
			}
		};

		// Token: 0x040000A3 RID: 163
		private ParameterContainer _parameters;

		// Token: 0x020000D1 RID: 209
		public enum DiamondDebugCategory
		{
			// Token: 0x040002A0 RID: 672
			General,
			// Token: 0x040002A1 RID: 673
			Warning,
			// Token: 0x040002A2 RID: 674
			Error
		}
	}
}
