using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension.Standalone.Native;
using TaleWorlds.TwoDimension.Standalone.Native.OpenGL;
using TaleWorlds.TwoDimension.Standalone.Native.Windows;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x02000005 RID: 5
	public class GraphicsContext
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002060 File Offset: 0x00000260
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002068 File Offset: 0x00000268
		internal WindowsForm Control { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002071 File Offset: 0x00000271
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002078 File Offset: 0x00000278
		public static GraphicsContext Active { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002080 File Offset: 0x00000280
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002088 File Offset: 0x00000288
		internal Dictionary<string, OpenGLTexture> LoadedTextures { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002091 File Offset: 0x00000291
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002099 File Offset: 0x00000299
		public MatrixFrame ProjectionMatrix
		{
			get
			{
				return this._projectionMatrix;
			}
			set
			{
				this._projectionMatrix = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000020A2 File Offset: 0x000002A2
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000020AA File Offset: 0x000002AA
		public MatrixFrame ViewMatrix
		{
			get
			{
				return this._viewMatrix;
			}
			set
			{
				this._viewMatrix = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000020B3 File Offset: 0x000002B3
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000020BB File Offset: 0x000002BB
		public MatrixFrame ModelMatrix
		{
			get
			{
				return this._modelMatrix;
			}
			set
			{
				this._modelMatrix = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000020C4 File Offset: 0x000002C4
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000020CC File Offset: 0x000002CC
		internal bool IsShuttingDown { get; private set; }

		// Token: 0x06000014 RID: 20 RVA: 0x000020D8 File Offset: 0x000002D8
		public GraphicsContext()
		{
			this.LoadedTextures = new Dictionary<string, OpenGLTexture>();
			this._loadedShaders = new Dictionary<string, Shader>();
			this._stopwatch = new Stopwatch();
			this.MaxTimeToRenderOneFrame = 16;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002164 File Offset: 0x00000364
		public void CreateContext(ResourceDepot resourceDepot)
		{
			this._resourceDepot = resourceDepot;
			this._handleDeviceContext = User32.GetDC(this.Control.Handle);
			if (this._handleDeviceContext == IntPtr.Zero)
			{
				Debug.Print("Can't get device context", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			if (!Opengl32.wglMakeCurrent(this._handleDeviceContext, IntPtr.Zero))
			{
				Debug.Print("Can't reset context", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			PixelFormatDescriptor pixelFormatDescriptor = default(PixelFormatDescriptor);
			Marshal.SizeOf(typeof(PixelFormatDescriptor));
			pixelFormatDescriptor.nSize = (ushort)Marshal.SizeOf(typeof(PixelFormatDescriptor));
			pixelFormatDescriptor.nVersion = 1;
			pixelFormatDescriptor.dwFlags = 37U;
			pixelFormatDescriptor.iPixelType = 0;
			pixelFormatDescriptor.cColorBits = 32;
			pixelFormatDescriptor.cRedBits = 0;
			pixelFormatDescriptor.cRedShift = 0;
			pixelFormatDescriptor.cGreenBits = 0;
			pixelFormatDescriptor.cGreenShift = 0;
			pixelFormatDescriptor.cBlueBits = 0;
			pixelFormatDescriptor.cBlueShift = 0;
			pixelFormatDescriptor.cAlphaBits = 8;
			pixelFormatDescriptor.cAlphaShift = 0;
			pixelFormatDescriptor.cAccumBits = 0;
			pixelFormatDescriptor.cAccumRedBits = 0;
			pixelFormatDescriptor.cAccumGreenBits = 0;
			pixelFormatDescriptor.cAccumBlueBits = 0;
			pixelFormatDescriptor.cAccumAlphaBits = 0;
			pixelFormatDescriptor.cDepthBits = 24;
			pixelFormatDescriptor.cStencilBits = 8;
			pixelFormatDescriptor.cAuxBuffers = 0;
			pixelFormatDescriptor.iLayerType = 0;
			pixelFormatDescriptor.bReserved = 0;
			pixelFormatDescriptor.dwLayerMask = 0U;
			pixelFormatDescriptor.dwVisibleMask = 0U;
			pixelFormatDescriptor.dwDamageMask = 0U;
			int num = Gdi32.ChoosePixelFormat(this._handleDeviceContext, ref pixelFormatDescriptor);
			if (!Gdi32.SetPixelFormat(this._handleDeviceContext, num, ref pixelFormatDescriptor))
			{
				Debug.Print("can't set pixel format", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			this._handleRenderContext = Opengl32.wglCreateContext(this._handleDeviceContext);
			if (this._handleRenderContext == IntPtr.Zero)
			{
				StandaloneApplicationUtility.TerminateWithMessageBox("Graphics driver error", "Could not create default OpenGL context.");
			}
			this.SetActive();
			string @string = Opengl32.GetString(7938U);
			string string2 = Opengl32.GetString(7936U);
			string string3 = Opengl32.GetString(7937U);
			Watchdog.LogProperty("crash_tags.txt", "Runtime", "DefaultContextVersionOpenGL", @string);
			Watchdog.LogProperty("crash_tags.txt", "Runtime", "DefaultContextVendorOpenGL", string2);
			Watchdog.LogProperty("crash_tags.txt", "Runtime", "DefaultContextRendererOpenGL", string3);
			IntPtr handleRenderContext = this._handleRenderContext;
			this._handleRenderContext = IntPtr.Zero;
			GraphicsContext.Active = null;
			Opengl32ARB.LoadContextExtension(this._handleDeviceContext);
			int[] array = new int[10];
			int num2 = 0;
			array[num2++] = 8337;
			array[num2++] = 3;
			array[num2++] = 8338;
			array[num2++] = 3;
			array[num2++] = 37158;
			array[num2++] = 1;
			array[num2++] = 0;
			this._handleRenderContext = Opengl32ARB.wglCreateContextAttribs(this._handleDeviceContext, IntPtr.Zero, array);
			if (this._handleRenderContext == IntPtr.Zero)
			{
				StandaloneApplicationUtility.TerminateWithMessageBox("Graphics driver error", "Could not create OpenGL context.");
			}
			this.SetActive();
			string string4 = Opengl32.GetString(7938U);
			string string5 = Opengl32.GetString(7936U);
			string string6 = Opengl32.GetString(7937U);
			Watchdog.LogProperty("crash_tags.txt", "Runtime", "ContextVersionOpenGL", string4);
			Watchdog.LogProperty("crash_tags.txt", "Runtime", "ContextVendorOpenGL", string5);
			Watchdog.LogProperty("crash_tags.txt", "Runtime", "ContextRendererOpenGL", string6);
			Opengl32ARB.LoadExtensions(this._handleDeviceContext);
			Opengl32.wglDeleteContext(handleRenderContext);
			Opengl32.ShadeModel(ShadingModel.Smooth);
			Opengl32.ClearColor(0f, 0f, 0f, 0f);
			Opengl32.ClearDepth(1.0);
			Opengl32.Disable(Target.DepthTest);
			Opengl32.Hint(3152U, 4354U);
			this.ProjectionMatrix = MatrixFrame.Identity.Filled();
			this.ViewMatrix = MatrixFrame.Identity.Filled();
			this.ModelMatrix = MatrixFrame.Identity.Filled();
			this._simpleVAO = VertexArrayObject.Create();
			this._textureVAO = VertexArrayObject.CreateWithUVBuffer();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002584 File Offset: 0x00000784
		public void SetActive()
		{
			if (GraphicsContext.Active != this)
			{
				if (Opengl32.wglMakeCurrent(this._handleDeviceContext, this._handleRenderContext))
				{
					GraphicsContext.Active = this;
					return;
				}
				Debug.Print("Can't activate context", 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000025C0 File Offset: 0x000007C0
		public void BeginFrame(int width, int height)
		{
			this._anyInvalidMatricesThisFrame = false;
			this._glContextRecoveredThisFrame = false;
			this._stopwatch.Start();
			if (this.IsShuttingDown)
			{
				return;
			}
			if (this._forceContextReactivation)
			{
				Debug.Print("[LAUNCHER]: Display or DPI change detected, reactivating GL context.", 0, Debug.DebugColor.White, 17592186044416UL);
				this._forceContextReactivation = false;
				GraphicsContext.Active = null;
				this.SetActive();
			}
			IntPtr intPtr = Opengl32.wglGetCurrentContext();
			if (intPtr == IntPtr.Zero || intPtr != this._handleRenderContext)
			{
				GraphicsContext.Active = null;
				this.SetActive();
				intPtr = Opengl32.wglGetCurrentContext();
				if (intPtr == IntPtr.Zero)
				{
					if (!this.IsShuttingDown)
					{
						this._anyInvalidMatricesThisFrame = true;
						this._lastFailureReason = "GLContextRecoveryFailed";
					}
					return;
				}
				this._glContextRecoveredThisFrame = true;
			}
			this.Resize(width, height);
			Opengl32.Clear(AttribueMask.ColorBufferBit);
			Opengl32.ClearDepth(1.0);
			Opengl32.Disable(Target.DepthTest);
			Opengl32.Disable(Target.SCISSOR_TEST);
			Opengl32.Disable(Target.STENCIL_TEST);
			Opengl32.Disable(Target.Blend);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026CC File Offset: 0x000008CC
		public void SwapBuffers()
		{
			int num = (int)this._stopwatch.ElapsedMilliseconds;
			int num2 = 0;
			if (this.MaxTimeToRenderOneFrame > num)
			{
				num2 = this.MaxTimeToRenderOneFrame - num;
			}
			if (num2 > 0)
			{
				Thread.Sleep(num2);
			}
			if (!Gdi32.SwapBuffers(this._handleDeviceContext) && !this.IsShuttingDown)
			{
				this._forceContextReactivation = true;
				this._lastFailureReason = "SwapBuffersFailed";
			}
			this._stopwatch.Restart();
			if (this.IsShuttingDown)
			{
				return;
			}
			if (this._glContextRecoveredThisFrame && !this._anyInvalidMatricesThisFrame)
			{
				this._failedRenderFrames = 0;
			}
			else if (this._anyInvalidMatricesThisFrame)
			{
				this._failedRenderFrames++;
			}
			else
			{
				this._failedRenderFrames = 0;
			}
			if (this._failedRenderFrames >= 180)
			{
				Watchdog.LogProperty("crash_tags.txt", "Runtime", "RenderFatal", 180 + " consecutive failed render frames");
				Watchdog.LogProperty("crash_tags.txt", "Runtime", "RenderFatalLastError", this._lastFailureReason);
				if (!this.IsShuttingDown)
				{
					Debug.ShowMessageBox("Launcher render error", "ERROR", 4U);
				}
				Environment.Exit(1);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000027E4 File Offset: 0x000009E4
		public bool IsActive
		{
			get
			{
				return GraphicsContext.Active == this;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000027EE File Offset: 0x000009EE
		public void RequestContextReactivation()
		{
			this._forceContextReactivation = true;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000027F8 File Offset: 0x000009F8
		public void DestroyContext()
		{
			this.IsShuttingDown = true;
			Opengl32.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
			Opengl32.wglDeleteContext(this._handleRenderContext);
			this._handleRenderContext = IntPtr.Zero;
			User32.ReleaseDC(this.Control.Handle, this._handleDeviceContext);
			this._handleDeviceContext = IntPtr.Zero;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002858 File Offset: 0x00000A58
		public void SetScissor(ScissorTestInfo scissorTestInfo)
		{
			Opengl32.GetInteger(Target.VIEWPORT, this._scissorParameters);
			SimpleRectangle simpleRectangle = scissorTestInfo.GetSimpleRectangle();
			Opengl32.Scissor((int)simpleRectangle.X, this._scissorParameters[3] - (int)simpleRectangle.Height - (int)simpleRectangle.Y, (int)simpleRectangle.Width, (int)simpleRectangle.Height);
			Opengl32.Enable(Target.SCISSOR_TEST);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000028BC File Offset: 0x00000ABC
		public void ResetScissor()
		{
			Opengl32.Disable(Target.SCISSOR_TEST);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000028C8 File Offset: 0x00000AC8
		public Shader GetOrLoadShader(string shaderName)
		{
			if (!this._loadedShaders.ContainsKey(shaderName))
			{
				try
				{
					string filePath = this._resourceDepot.GetFilePath(shaderName + ".vert");
					string filePath2 = this._resourceDepot.GetFilePath(shaderName + ".frag");
					string text = File.ReadAllText(filePath);
					string text2 = File.ReadAllText(filePath2);
					Shader shader = Shader.CreateShader(this, text, text2);
					this._loadedShaders.Add(shaderName, shader);
					return shader;
				}
				catch (Exception)
				{
					this._loadedShaders.Add(shaderName, null);
					return null;
				}
			}
			return this._loadedShaders[shaderName];
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000296C File Offset: 0x00000B6C
		public void DrawImage(SimpleMaterial material, in ImageDrawObject drawObject)
		{
			Shader shader = this.PrepareRender(material, in drawObject.Rectangle);
			if (shader == null)
			{
				return;
			}
			this.DrawImageAux(shader, material, in drawObject);
			VertexArrayObject.UnBind();
			shader.StopUsing();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000029A0 File Offset: 0x00000BA0
		public void DrawText(TextMaterial material, in TextDrawObject drawObject)
		{
			Shader shader = this.PrepareRender(material, in drawObject.Rectangle);
			if (shader == null)
			{
				return;
			}
			this.DrawTextAux(shader, material, in drawObject);
			VertexArrayObject.UnBind();
			shader.StopUsing();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000029D4 File Offset: 0x00000BD4
		public void DrawPolygon(PrimitivePolygonMaterial material, in ImageDrawObject drawObject)
		{
			Shader shader = this.PrepareRender(material, in drawObject.Rectangle);
			if (shader == null)
			{
				return;
			}
			this.DrawPolygonAux(shader, material, in drawObject);
			VertexArrayObject.UnBind();
			shader.StopUsing();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002A08 File Offset: 0x00000C08
		private Shader PrepareRender(Material material, in Rectangle2D rect)
		{
			Shader orLoadShader = this.GetOrLoadShader(material.GetType().Name);
			if (orLoadShader == null)
			{
				return null;
			}
			if (this._screenWidth <= 0 || this._screenHeight <= 0)
			{
				return null;
			}
			Rectangle2D rectangle2D = rect;
			MatrixFrame cachedVisualMatrixFrame = rectangle2D.GetCachedVisualMatrixFrame();
			if (cachedVisualMatrixFrame.AreAllComponentsValid() && !cachedVisualMatrixFrame.IsZero)
			{
				this.ModelMatrix = cachedVisualMatrixFrame;
			}
			else
			{
				this.ModelMatrix = GraphicsContext.ValidateModelMatrix(cachedVisualMatrixFrame);
			}
			MatrixFrame matrixFrame = GraphicsContext.ValidateModelMatrix(this._modelMatrix);
			MatrixFrame matrixFrame2 = GraphicsContext.ValidateViewMatrix(in this._viewMatrix);
			MatrixFrame matrixFrame3 = GraphicsContext.ValidateProjectionMatrix(in this._projectionMatrix);
			orLoadShader.Use();
			if (Opengl32.GetError() != 0U)
			{
				orLoadShader.StopUsing();
				return null;
			}
			Matrix4x4 matrix4x = matrixFrame.ToMatrix4x4() * matrixFrame2.ToMatrix4x4() * matrixFrame3.ToMatrix4x4();
			orLoadShader.SetMatrix("MVP", in matrix4x);
			return orLoadShader;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002AE0 File Offset: 0x00000CE0
		private static MatrixFrame ValidateModelMatrix(MatrixFrame modelMatrix)
		{
			if (!modelMatrix.origin.IsValidXYZW)
			{
				modelMatrix.origin = new Vec3(0f, 0f, 0f, 0f);
			}
			if (!modelMatrix.rotation.s.IsValidXYZW)
			{
				modelMatrix.rotation.s = new Vec3(100f, 0f, 0f, 0f);
			}
			if (!modelMatrix.rotation.f.IsValidXYZW)
			{
				modelMatrix.rotation.f = new Vec3(0f, 100f, 0f, 0f);
			}
			if (!modelMatrix.rotation.u.IsValidXYZW)
			{
				modelMatrix.rotation.u = new Vec3(0f, 0f, 1f, 0f);
			}
			modelMatrix.Fill();
			return modelMatrix;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002BCB File Offset: 0x00000DCB
		private static MatrixFrame ValidateViewMatrix(in MatrixFrame viewMatrix)
		{
			if (viewMatrix.AreAllComponentsValid())
			{
				return viewMatrix;
			}
			return MatrixFrame.CreateLookAt(in Vec3.Up, in Vec3.Zero, in Vec3.Forward);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002BF5 File Offset: 0x00000DF5
		private static MatrixFrame ValidateProjectionMatrix(in MatrixFrame projectionMatrix)
		{
			if (projectionMatrix.AreAllComponentsValid())
			{
				return projectionMatrix;
			}
			return MatrixExtensions.CreateOrthographicOffCenter(0f, 900f, 600f, 0f, 0f, 1f);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002C30 File Offset: 0x00000E30
		private void DrawImageAux(Shader shader, SimpleMaterial material, in ImageDrawObject drawObject)
		{
			if (material.Texture != null)
			{
				OpenGLTexture openGLTexture = material.Texture.PlatformTexture as OpenGLTexture;
				shader.SetTexture("Texture", openGLTexture);
			}
			shader.SetBoolean("OverlayEnabled", material.OverlayEnabled);
			if (material.OverlayEnabled)
			{
				OpenGLTexture openGLTexture2 = material.OverlayTexture.PlatformTexture as OpenGLTexture;
				shader.SetVector2("StartCoord", material.StartCoordinate);
				shader.SetVector2("Size", material.Size);
				shader.SetTexture("OverlayTexture", openGLTexture2);
				shader.SetVector2("OverlayOffset", new Vector2(material.OverlayXOffset, material.OverlayYOffset));
			}
			float num = MathF.Clamp(material.HueFactor / 360f, -0.5f, 0.5f);
			float num2 = MathF.Clamp(material.SaturationFactor / 360f, -0.5f, 0.5f);
			float num3 = MathF.Clamp(material.ValueFactor / 360f, -0.5f, 0.5f);
			shader.SetColor("InputColor", material.Color);
			shader.SetFloat("ColorFactor", material.ColorFactor);
			shader.SetFloat("AlphaFactor", material.AlphaFactor);
			shader.SetFloat("HueFactor", num);
			shader.SetFloat("SaturationFactor", num2);
			shader.SetFloat("ValueFactor", num3);
			this._textureVAO.Bind();
			if (material.CircularMaskingEnabled)
			{
				shader.SetBoolean("CircularMaskingEnabled", true);
				shader.SetVector2("MaskingCenter", material.CircularMaskingCenter);
				shader.SetFloat("MaskingRadius", material.CircularMaskingRadius);
				shader.SetFloat("MaskingSmoothingRadius", material.CircularMaskingSmoothingRadius);
			}
			else
			{
				shader.SetBoolean("CircularMaskingEnabled", false);
			}
			Vector2 vector = new Vector2(drawObject.Uvs.x, drawObject.Uvs.y);
			Vector2 vector2 = new Vector2(drawObject.Uvs.z, drawObject.Uvs.w);
			float[] array = new float[] { 0f, 0f, 0f, 1f, 1f, 1f, 1f, 0f };
			uint[] array2 = new uint[] { 0U, 1U, 2U, 0U, 2U, 3U };
			float[] array3 = new float[] { vector.X, vector.Y, vector.X, vector2.Y, vector2.X, vector2.Y, vector2.X, vector.Y };
			this._textureVAO.LoadVertexData(array);
			this._textureVAO.LoadUVData(array3);
			this._textureVAO.LoadIndexData(array2);
			this.DrawElements(array2, material.Blending);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002ED0 File Offset: 0x000010D0
		private void DrawTextAux(Shader shader, TextMaterial textMaterial, in TextDrawObject drawObject)
		{
			if (textMaterial.Texture != null)
			{
				OpenGLTexture openGLTexture = textMaterial.Texture.PlatformTexture as OpenGLTexture;
				shader.SetTexture("Texture", openGLTexture);
			}
			shader.SetColor("InputColor", textMaterial.Color);
			shader.SetColor("GlowColor", textMaterial.GlowColor);
			shader.SetColor("OutlineColor", textMaterial.OutlineColor);
			shader.SetFloat("OutlineAmount", textMaterial.OutlineAmount);
			shader.SetFloat("ScaleFactor", 1.5f / textMaterial.ScaleFactor);
			shader.SetFloat("SmoothingConstant", textMaterial.SmoothingConstant);
			shader.SetFloat("GlowRadius", textMaterial.GlowRadius);
			shader.SetFloat("Blur", textMaterial.Blur);
			shader.SetFloat("ShadowOffset", textMaterial.ShadowOffset);
			shader.SetFloat("ShadowAngle", textMaterial.ShadowAngle);
			shader.SetFloat("ColorFactor", textMaterial.ColorFactor);
			shader.SetFloat("AlphaFactor", textMaterial.AlphaFactor);
			this._textureVAO.Bind();
			this._textureVAO.LoadVertexData(drawObject.Text_Vertices);
			this._textureVAO.LoadUVData(drawObject.Text_TextureCoordinates);
			this._textureVAO.LoadIndexData(drawObject.Text_Indices);
			this.DrawElements(drawObject.Text_Indices, textMaterial.Blending);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003024 File Offset: 0x00001224
		private void DrawPolygonAux(Shader shader, PrimitivePolygonMaterial material, in ImageDrawObject drawObject)
		{
			Color color = material.Color;
			shader.SetColor("Color", color);
			new Vector2(drawObject.Uvs.x, drawObject.Uvs.y);
			new Vector2(drawObject.Uvs.z, drawObject.Uvs.w);
			float[] array = new float[] { 0f, 0f, 0f, 1f, 1f, 1f, 1f, 0f };
			uint[] array2 = new uint[] { 0U, 1U, 2U, 0U, 2U, 3U };
			this._simpleVAO.Bind();
			this._textureVAO.LoadVertexData(array);
			this.DrawElements(array2, material.Blending);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000030C4 File Offset: 0x000012C4
		private void DrawElements(uint[] indices, bool blending)
		{
			this.SetBlending(blending);
			using (new AutoPinner(indices))
			{
				Opengl32.DrawElements(BeginMode.Triangles, indices.Length, DataType.UnsignedInt, null);
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000310C File Offset: 0x0000130C
		internal void Resize(int width, int height)
		{
			if (!this.IsActive)
			{
				this.SetActive();
			}
			this._screenWidth = width;
			this._screenHeight = height;
			Opengl32.Viewport(0, 0, width, height);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003133 File Offset: 0x00001333
		public void LoadTextureUsing(OpenGLTexture texture, ResourceDepot resourceDepot, string name)
		{
			if (!this.LoadedTextures.ContainsKey(name))
			{
				texture.LoadFromFile(resourceDepot, name);
				this.LoadedTextures.Add(name, texture);
				return;
			}
			texture.CopyFrom(this.LoadedTextures[name]);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000316C File Offset: 0x0000136C
		public OpenGLTexture LoadTexture(ResourceDepot resourceDepot, string name)
		{
			OpenGLTexture openGLTexture;
			if (this.LoadedTextures.ContainsKey(name))
			{
				openGLTexture = this.LoadedTextures[name];
			}
			else
			{
				openGLTexture = OpenGLTexture.FromFile(resourceDepot, name);
				this.LoadedTextures.Add(name, openGLTexture);
			}
			return openGLTexture;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000031B0 File Offset: 0x000013B0
		public OpenGLTexture GetTexture(string textureName)
		{
			OpenGLTexture openGLTexture = null;
			if (this.LoadedTextures.ContainsKey(textureName))
			{
				openGLTexture = this.LoadedTextures[textureName];
			}
			return openGLTexture;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000031DB File Offset: 0x000013DB
		public void SetBlending(bool enable)
		{
			this._blendingMode = enable;
			if (this._blendingMode)
			{
				Opengl32.Enable(Target.Blend);
				Opengl32ARB.BlendFuncSeparate(BlendingSourceFactor.SourceAlpha, BlendingDestinationFactor.OneMinusSourceAlpha, BlendingSourceFactor.One, BlendingDestinationFactor.One);
				return;
			}
			Opengl32.Disable(Target.Blend);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003217 File Offset: 0x00001417
		public void SetVertexArrayClientState(bool enable)
		{
			if (this._vertexArrayClientState != enable)
			{
				this._vertexArrayClientState = enable;
				if (this._vertexArrayClientState)
				{
					Opengl32.EnableClientState(32884U);
					return;
				}
				Opengl32.DisableClientState(32884U);
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003246 File Offset: 0x00001446
		public void SetTextureCoordArrayClientState(bool enable)
		{
			if (this._textureCoordArrayClientState != enable)
			{
				this._textureCoordArrayClientState = enable;
				if (this._textureCoordArrayClientState)
				{
					Opengl32.EnableClientState(32888U);
					return;
				}
				Opengl32.DisableClientState(32888U);
			}
		}

		// Token: 0x04000001 RID: 1
		public const int MaxFrameRate = 60;

		// Token: 0x04000002 RID: 2
		public readonly int MaxTimeToRenderOneFrame;

		// Token: 0x04000004 RID: 4
		private IntPtr _handleDeviceContext;

		// Token: 0x04000005 RID: 5
		private IntPtr _handleRenderContext;

		// Token: 0x04000008 RID: 8
		private int[] _scissorParameters = new int[4];

		// Token: 0x04000009 RID: 9
		private MatrixFrame _modelMatrix = MatrixFrame.Identity.Filled();

		// Token: 0x0400000A RID: 10
		private MatrixFrame _viewMatrix = MatrixFrame.Identity.Filled();

		// Token: 0x0400000B RID: 11
		private MatrixFrame _projectionMatrix = MatrixFrame.Identity.Filled();

		// Token: 0x0400000C RID: 12
		private Stopwatch _stopwatch;

		// Token: 0x0400000D RID: 13
		private Dictionary<string, Shader> _loadedShaders;

		// Token: 0x0400000E RID: 14
		private VertexArrayObject _simpleVAO;

		// Token: 0x0400000F RID: 15
		private VertexArrayObject _textureVAO;

		// Token: 0x04000010 RID: 16
		private int _screenWidth;

		// Token: 0x04000011 RID: 17
		private int _screenHeight;

		// Token: 0x04000012 RID: 18
		private const int FailedRenderFramesFatalThreshold = 180;

		// Token: 0x04000013 RID: 19
		private int _failedRenderFrames;

		// Token: 0x04000014 RID: 20
		private bool _anyInvalidMatricesThisFrame;

		// Token: 0x04000015 RID: 21
		private bool _forceContextReactivation;

		// Token: 0x04000016 RID: 22
		private bool _glContextRecoveredThisFrame;

		// Token: 0x04000017 RID: 23
		private string _lastFailureReason = "";

		// Token: 0x04000019 RID: 25
		private ResourceDepot _resourceDepot;

		// Token: 0x0400001A RID: 26
		private bool _blendingMode;

		// Token: 0x0400001B RID: 27
		private bool _vertexArrayClientState;

		// Token: 0x0400001C RID: 28
		private bool _textureCoordArrayClientState;
	}
}
