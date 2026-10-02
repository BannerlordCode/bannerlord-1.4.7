using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000024 RID: 36
	public static class GauntletExtensions
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x0000E97C File Offset: 0x0000CB7C
		public static void SetGlobalAlphaRecursively(this Widget widget, float alphaFactor)
		{
			widget.SetAlpha(alphaFactor);
			List<Widget> children = widget.Children;
			for (int i = 0; i < children.Count; i++)
			{
				children[i].SetGlobalAlphaRecursively(alphaFactor);
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000E9B8 File Offset: 0x0000CBB8
		public static void SetAlpha(this Widget widget, float alphaFactor)
		{
			BrushWidget brushWidget;
			if ((brushWidget = widget as BrushWidget) != null)
			{
				brushWidget.Brush.GlobalAlphaFactor = alphaFactor;
			}
			TextureWidget textureWidget;
			if ((textureWidget = widget as TextureWidget) != null)
			{
				textureWidget.Brush.GlobalAlphaFactor = alphaFactor;
			}
			widget.AlphaFactor = alphaFactor;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000E9F8 File Offset: 0x0000CBF8
		public static void RegisterBrushStatesOfWidget(this Widget widget)
		{
			BrushWidget brushWidget;
			if ((brushWidget = widget as BrushWidget) != null)
			{
				foreach (Style style in brushWidget.ReadOnlyBrush.Styles)
				{
					if (!widget.ContainsState(style.Name))
					{
						widget.AddState(style.Name);
					}
				}
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000EA70 File Offset: 0x0000CC70
		public static string GetFullIDPath(this Widget widget)
		{
			StringBuilder stringBuilder = new StringBuilder(string.IsNullOrEmpty(widget.Id) ? widget.GetType().Name : widget.Id);
			for (Widget widget2 = widget.ParentWidget; widget2 != null; widget2 = widget2.ParentWidget)
			{
				stringBuilder.Insert(0, (string.IsNullOrEmpty(widget2.Id) ? widget2.GetType().Name : widget2.Id) + "\\");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000EAF0 File Offset: 0x0000CCF0
		public static void ApplyActionForThisAndAllChildren(this Widget widget, Action<Widget> action)
		{
			action(widget);
			List<Widget> children = widget.Children;
			for (int i = 0; i < children.Count; i++)
			{
				children[i].ApplyActionForThisAndAllChildren(action);
			}
		}
	}
}
