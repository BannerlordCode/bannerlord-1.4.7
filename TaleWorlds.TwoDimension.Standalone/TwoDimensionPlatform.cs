using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x0200000F RID: 15
	public class TwoDimensionPlatform : ITwoDimensionPlatform, ITwoDimensionResourceContext
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00005070 File Offset: 0x00003270
		float ITwoDimensionPlatform.Width
		{
			get
			{
				return (float)this._form.Width;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000507E File Offset: 0x0000327E
		float ITwoDimensionPlatform.Height
		{
			get
			{
				return (float)this._form.Height;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000508C File Offset: 0x0000328C
		float ITwoDimensionPlatform.ReferenceWidth
		{
			get
			{
				return 1154f;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00005093 File Offset: 0x00003293
		float ITwoDimensionPlatform.ReferenceHeight
		{
			get
			{
				return 701f;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000BA RID: 186 RVA: 0x0000509A File Offset: 0x0000329A
		float ITwoDimensionPlatform.ApplicationTime
		{
			get
			{
				return (float)Environment.TickCount;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000050A2 File Offset: 0x000032A2
		public TwoDimensionPlatform(GraphicsForm form, bool isAssetsUnderDefaultFolders)
		{
			this._form = form;
			this._isAssetsUnderDefaultFolders = isAssetsUnderDefaultFolders;
			this._graphicsContext = this._form.GraphicsContext;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000050C9 File Offset: 0x000032C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITwoDimensionPlatform.DrawImage(SimpleMaterial material, in ImageDrawObject drawObject2D, int layer)
		{
			this._graphicsContext.DrawImage(material, in drawObject2D);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000050D8 File Offset: 0x000032D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITwoDimensionPlatform.DrawText(TextMaterial material, in TextDrawObject drawObject2D, int layer)
		{
			this._graphicsContext.DrawText(material, in drawObject2D);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000050E7 File Offset: 0x000032E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITwoDimensionPlatform.OnFrameBegin()
		{
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000050E9 File Offset: 0x000032E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITwoDimensionPlatform.OnFrameEnd()
		{
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000050EB File Offset: 0x000032EB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void ITwoDimensionPlatform.Clear()
		{
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000050F0 File Offset: 0x000032F0
		Texture ITwoDimensionResourceContext.LoadTexture(ResourceDepot resourceDepot, string name)
		{
			OpenGLTexture openGLTexture = new OpenGLTexture();
			string text = name;
			if (!this._isAssetsUnderDefaultFolders)
			{
				string[] array = name.Split(new char[] { '\\' });
				text = array[array.Length - 1];
			}
			openGLTexture.LoadFromFile(resourceDepot, text);
			return new Texture(openGLTexture);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005133 File Offset: 0x00003333
		void ITwoDimensionPlatform.PlaySound(string soundName)
		{
			Debug.Print("Playing sound: " + soundName, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005151 File Offset: 0x00003351
		void ITwoDimensionPlatform.SetScissor(ScissorTestInfo scissorTestInfo)
		{
			this._graphicsContext.SetScissor(scissorTestInfo);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000515F File Offset: 0x0000335F
		void ITwoDimensionPlatform.ResetScissors()
		{
			this._graphicsContext.ResetScissor();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000516C File Offset: 0x0000336C
		void ITwoDimensionPlatform.CreateSoundEvent(string soundName)
		{
			Debug.Print("Created sound event: " + soundName, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000518A File Offset: 0x0000338A
		void ITwoDimensionPlatform.StopAndRemoveSoundEvent(string soundName)
		{
			Debug.Print("Stopped sound event: " + soundName, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000051A8 File Offset: 0x000033A8
		void ITwoDimensionPlatform.PlaySoundEvent(string soundName)
		{
			Debug.Print("Played sound event: " + soundName, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000051C6 File Offset: 0x000033C6
		void ITwoDimensionPlatform.OpenOnScreenKeyboard(string initialText, string descriptionText, int maxLength, int keyboardTypeEnum)
		{
			Debug.Print("Opened on-screen keyboard", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000051DE File Offset: 0x000033DE
		void ITwoDimensionPlatform.BeginDebugPanel(string panelTitle)
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000051E0 File Offset: 0x000033E0
		void ITwoDimensionPlatform.EndDebugPanel()
		{
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000051E2 File Offset: 0x000033E2
		void ITwoDimensionPlatform.DrawDebugText(string text)
		{
			Debug.Print(text, 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000051F6 File Offset: 0x000033F6
		bool ITwoDimensionPlatform.IsDebugModeEnabled()
		{
			return false;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000051F9 File Offset: 0x000033F9
		bool ITwoDimensionPlatform.DrawDebugTreeNode(string text)
		{
			return false;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000051FC File Offset: 0x000033FC
		void ITwoDimensionPlatform.DrawCheckbox(string label, ref bool isChecked)
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000051FE File Offset: 0x000033FE
		bool ITwoDimensionPlatform.IsDebugItemHovered()
		{
			return false;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005201 File Offset: 0x00003401
		void ITwoDimensionPlatform.PopDebugTreeNode()
		{
		}

		// Token: 0x04000044 RID: 68
		private GraphicsContext _graphicsContext;

		// Token: 0x04000045 RID: 69
		private GraphicsForm _form;

		// Token: 0x04000046 RID: 70
		private bool _isAssetsUnderDefaultFolders;
	}
}
