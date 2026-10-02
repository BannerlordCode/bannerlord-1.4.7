using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001BB RID: 443
	[ScriptingInterfaceBase]
	internal interface IMBDebugExtensions
	{
		// Token: 0x06001900 RID: 6400
		[EngineMethod("render_debug_circle_on_terrain", false, null, false)]
		void RenderDebugCircleOnTerrain(UIntPtr scenePointer, ref MatrixFrame frame, float radius, uint color, bool depthCheck, bool isDotted);

		// Token: 0x06001901 RID: 6401
		[EngineMethod("render_debug_arc_on_terrain", false, null, false)]
		void RenderDebugArcOnTerrain(UIntPtr scenePointer, ref MatrixFrame frame, float radius, float beginAngle, float endAngle, uint color, bool depthCheck, bool isDotted);

		// Token: 0x06001902 RID: 6402
		[EngineMethod("render_debug_line_on_terrain", false, null, false)]
		void RenderDebugLineOnTerrain(UIntPtr scenePointer, Vec3 position, Vec3 direction, uint color, bool depthCheck, float time, bool isDotted, float pointDensity);

		// Token: 0x06001903 RID: 6403
		[EngineMethod("override_native_parameter", false, null, false)]
		void OverrideNativeParameter(string paramName, float value);

		// Token: 0x06001904 RID: 6404
		[EngineMethod("reload_native_parameters", false, null, false)]
		void ReloadNativeParameters();
	}
}
