using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000052 RID: 82
	public class Imgui
	{
		// Token: 0x06000877 RID: 2167 RVA: 0x00006AD7 File Offset: 0x00004CD7
		public static void BeginMainThreadScope()
		{
			EngineApplicationInterface.IImgui.BeginMainThreadScope();
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00006AE3 File Offset: 0x00004CE3
		public static void EndMainThreadScope()
		{
			EngineApplicationInterface.IImgui.EndMainThreadScope();
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00006AEF File Offset: 0x00004CEF
		public static void PushStyleColor(Imgui.ColorStyle style, ref Vec3 color)
		{
			EngineApplicationInterface.IImgui.PushStyleColor((int)style, ref color);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00006AFD File Offset: 0x00004CFD
		public static void PopStyleColor()
		{
			EngineApplicationInterface.IImgui.PopStyleColor();
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00006B09 File Offset: 0x00004D09
		public static void NewFrame()
		{
			EngineApplicationInterface.IImgui.NewFrame();
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00006B15 File Offset: 0x00004D15
		public static void Render()
		{
			EngineApplicationInterface.IImgui.Render();
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00006B21 File Offset: 0x00004D21
		public static void Begin(string text)
		{
			EngineApplicationInterface.IImgui.Begin(text);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00006B2E File Offset: 0x00004D2E
		public static void Begin(string text, ref bool is_open)
		{
			EngineApplicationInterface.IImgui.BeginWithCloseButton(text, ref is_open);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00006B3C File Offset: 0x00004D3C
		public static void End()
		{
			EngineApplicationInterface.IImgui.End();
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00006B48 File Offset: 0x00004D48
		public static void Text(string text)
		{
			EngineApplicationInterface.IImgui.Text(text);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00006B55 File Offset: 0x00004D55
		public static bool Checkbox(string text, ref bool is_checked)
		{
			return EngineApplicationInterface.IImgui.Checkbox(text, ref is_checked);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00006B63 File Offset: 0x00004D63
		public static bool TreeNode(string name)
		{
			return EngineApplicationInterface.IImgui.TreeNode(name);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00006B70 File Offset: 0x00004D70
		public static void TreePop()
		{
			EngineApplicationInterface.IImgui.TreePop();
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00006B7C File Offset: 0x00004D7C
		public static void Separator()
		{
			EngineApplicationInterface.IImgui.Separator();
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00006B88 File Offset: 0x00004D88
		public static bool Button(string text)
		{
			return EngineApplicationInterface.IImgui.Button(text);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00006B98 File Offset: 0x00004D98
		public static void PlotLines(string name, float[] values, int valuesCount, int valuesOffset, string overlayText, float minScale, float maxScale, float graphWidth, float graphHeight, int stride)
		{
			EngineApplicationInterface.IImgui.PlotLines(name, values, valuesCount, valuesOffset, overlayText, minScale, maxScale, graphWidth, graphHeight, stride);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00006BBF File Offset: 0x00004DBF
		public static void ProgressBar(float progress)
		{
			EngineApplicationInterface.IImgui.ProgressBar(progress);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00006BCC File Offset: 0x00004DCC
		public static void NewLine()
		{
			EngineApplicationInterface.IImgui.NewLine();
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00006BD8 File Offset: 0x00004DD8
		public static void SameLine(float posX = 0f, float spacingWidth = 0f)
		{
			EngineApplicationInterface.IImgui.SameLine(posX, spacingWidth);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00006BE6 File Offset: 0x00004DE6
		public static bool Combo(string label, ref int selectedIndex, string items)
		{
			return EngineApplicationInterface.IImgui.Combo(label, ref selectedIndex, items);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00006BF5 File Offset: 0x00004DF5
		public static bool ComboCustomSeperator(string label, ref int selectedIndex, string items, char seperator)
		{
			return EngineApplicationInterface.IImgui.ComboCustomSeperator(label, ref selectedIndex, items, seperator.ToString());
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00006C0B File Offset: 0x00004E0B
		public static bool InputInt(string label, ref int value)
		{
			return EngineApplicationInterface.IImgui.InputInt(label, ref value);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00006C19 File Offset: 0x00004E19
		public static bool SliderFloat(string label, ref float value, float min, float max)
		{
			return EngineApplicationInterface.IImgui.SliderFloat(label, ref value, min, max);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00006C29 File Offset: 0x00004E29
		public static void Columns(int count = 1, string id = "", bool border = true)
		{
			EngineApplicationInterface.IImgui.Columns(count, id, border);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00006C38 File Offset: 0x00004E38
		public static void NextColumn()
		{
			EngineApplicationInterface.IImgui.NextColumn();
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00006C44 File Offset: 0x00004E44
		public static bool RadioButton(string label, bool active)
		{
			return EngineApplicationInterface.IImgui.RadioButton(label, active);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00006C52 File Offset: 0x00004E52
		public static bool CollapsingHeader(string label)
		{
			return EngineApplicationInterface.IImgui.CollapsingHeader(label);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00006C5F File Offset: 0x00004E5F
		public static bool IsItemHovered()
		{
			return EngineApplicationInterface.IImgui.IsItemHovered();
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00006C6B File Offset: 0x00004E6B
		public static void SetTooltip(string label)
		{
			EngineApplicationInterface.IImgui.SetTooltip(label);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00006C78 File Offset: 0x00004E78
		public static bool SmallButton(string label)
		{
			return EngineApplicationInterface.IImgui.SmallButton(label);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00006C85 File Offset: 0x00004E85
		public static bool InputFloat(string label, ref float val, float step, float stepFast, int decimalPrecision = -1)
		{
			return EngineApplicationInterface.IImgui.InputFloat(label, ref val, step, stepFast, decimalPrecision);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00006C98 File Offset: 0x00004E98
		public static bool InputText(string label, ref string text)
		{
			bool flag = false;
			text = EngineApplicationInterface.IImgui.InputText(label, text, ref flag);
			return flag;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00006CBC File Offset: 0x00004EBC
		public static bool InputTextMultilineCopyPaste(string label, int textBoxHeight, ref string text)
		{
			bool flag = false;
			text = EngineApplicationInterface.IImgui.InputTextMultilineCopyPaste(label, text, textBoxHeight, ref flag);
			return flag;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00006CDE File Offset: 0x00004EDE
		public static bool InputFloat2(string label, ref float val0, ref float val1, int decimalPrecision = -1)
		{
			return EngineApplicationInterface.IImgui.InputFloat2(label, ref val0, ref val1, decimalPrecision);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00006CEE File Offset: 0x00004EEE
		public static bool InputFloat3(string label, ref float val0, ref float val1, ref float val2, int decimalPrecision = -1)
		{
			return EngineApplicationInterface.IImgui.InputFloat3(label, ref val0, ref val1, ref val2, decimalPrecision);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00006D00 File Offset: 0x00004F00
		public static bool InputFloat4(string label, ref float val0, ref float val1, ref float val2, ref float val3, int decimalPrecision = -1)
		{
			return EngineApplicationInterface.IImgui.InputFloat4(label, ref val0, ref val1, ref val2, ref val3, decimalPrecision);
		}

		// Token: 0x020000C1 RID: 193
		public enum ColorStyle
		{
			// Token: 0x040003BB RID: 955
			Text,
			// Token: 0x040003BC RID: 956
			TextDisabled,
			// Token: 0x040003BD RID: 957
			WindowBg,
			// Token: 0x040003BE RID: 958
			ChildWindowBg,
			// Token: 0x040003BF RID: 959
			PopupBg,
			// Token: 0x040003C0 RID: 960
			Border,
			// Token: 0x040003C1 RID: 961
			BorderShadow,
			// Token: 0x040003C2 RID: 962
			FrameBg,
			// Token: 0x040003C3 RID: 963
			FrameBgHovered,
			// Token: 0x040003C4 RID: 964
			FrameBgActive,
			// Token: 0x040003C5 RID: 965
			TitleBg,
			// Token: 0x040003C6 RID: 966
			TitleBgCollapsed,
			// Token: 0x040003C7 RID: 967
			TitleBgActive,
			// Token: 0x040003C8 RID: 968
			MenuBarBg,
			// Token: 0x040003C9 RID: 969
			ScrollbarBg,
			// Token: 0x040003CA RID: 970
			ScrollbarGrab,
			// Token: 0x040003CB RID: 971
			ScrollbarGrabHovered,
			// Token: 0x040003CC RID: 972
			ScrollbarGrabActive,
			// Token: 0x040003CD RID: 973
			ComboBg,
			// Token: 0x040003CE RID: 974
			CheckMark,
			// Token: 0x040003CF RID: 975
			SliderGrab,
			// Token: 0x040003D0 RID: 976
			SliderGrabActive,
			// Token: 0x040003D1 RID: 977
			Button,
			// Token: 0x040003D2 RID: 978
			ButtonHovered,
			// Token: 0x040003D3 RID: 979
			ButtonActive,
			// Token: 0x040003D4 RID: 980
			Header,
			// Token: 0x040003D5 RID: 981
			HeaderHovered,
			// Token: 0x040003D6 RID: 982
			HeaderActive,
			// Token: 0x040003D7 RID: 983
			Column,
			// Token: 0x040003D8 RID: 984
			ColumnHovered,
			// Token: 0x040003D9 RID: 985
			ColumnActive,
			// Token: 0x040003DA RID: 986
			ResizeGrip,
			// Token: 0x040003DB RID: 987
			ResizeGripHovered,
			// Token: 0x040003DC RID: 988
			ResizeGripActive,
			// Token: 0x040003DD RID: 989
			CloseButton,
			// Token: 0x040003DE RID: 990
			CloseButtonHovered,
			// Token: 0x040003DF RID: 991
			CloseButtonActive,
			// Token: 0x040003E0 RID: 992
			PlotLines,
			// Token: 0x040003E1 RID: 993
			PlotLinesHovered,
			// Token: 0x040003E2 RID: 994
			PlotHistogram,
			// Token: 0x040003E3 RID: 995
			PlotHistogramHovered,
			// Token: 0x040003E4 RID: 996
			TextSelectedBg,
			// Token: 0x040003E5 RID: 997
			ModalWindowDarkening
		}
	}
}
