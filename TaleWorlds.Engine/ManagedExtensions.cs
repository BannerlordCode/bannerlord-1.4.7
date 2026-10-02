using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000058 RID: 88
	public static class ManagedExtensions
	{
		// Token: 0x060008C1 RID: 2241 RVA: 0x00007098 File Offset: 0x00005298
		private static void OnEditorVariableChanged(DotNetObject managedObject, uint classNameHash, uint fieldNameHash)
		{
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0000709C File Offset: 0x0000529C
		[EngineCallback(null, false)]
		internal static void SetObjectFieldString(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, string value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x000070D4 File Offset: 0x000052D4
		[EngineCallback(null, false)]
		internal static void SetObjectFieldDouble(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, double value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00007120 File Offset: 0x00005320
		[EngineCallback(null, false)]
		internal static void SetObjectFieldFloat(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, float value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0000716C File Offset: 0x0000536C
		[EngineCallback(null, false)]
		internal static void SetObjectFieldBool(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, bool value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000071B8 File Offset: 0x000053B8
		[EngineCallback(null, false)]
		internal static void SetObjectFieldInt(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, int value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00007204 File Offset: 0x00005404
		[EngineCallback(null, false)]
		internal static void SetObjectFieldVec3(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, Vec3 value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00007250 File Offset: 0x00005450
		[EngineCallback(null, false)]
		internal static void SetObjectFieldEntity(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, (value != UIntPtr.Zero) ? new GameEntity(value) : null);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x000072AC File Offset: 0x000054AC
		[EngineCallback(null, false)]
		internal static void SetObjectFieldTexture(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, (value != UIntPtr.Zero) ? new Texture(value) : null);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00007308 File Offset: 0x00005508
		[EngineCallback(null, false)]
		internal static void SetObjectFieldMesh(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, (value != UIntPtr.Zero) ? new MetaMesh(value) : null);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00007364 File Offset: 0x00005564
		[EngineCallback(null, false)]
		internal static void SetObjectFieldMaterial(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, UIntPtr value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, (value != UIntPtr.Zero) ? new Material(value) : null);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x000073C0 File Offset: 0x000055C0
		[EngineCallback(null, false)]
		internal static void SetObjectFieldColor(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, Vec3 value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, new Color
			{
				Red = value.x,
				Green = value.y,
				Blue = value.z,
				Alpha = value.w
			});
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00007448 File Offset: 0x00005648
		[EngineCallback(null, false)]
		internal static void SetObjectFieldMatrixFrame(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, MatrixFrame value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			fieldOfClass.SetValue(managedObject, value);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00007494 File Offset: 0x00005694
		[EngineCallback(null, false)]
		internal static void SetObjectFieldEnum(DotNetObject managedObject, uint classNameHash, uint fieldNameHash, string value, int callFieldChangeEventAsInteger)
		{
			bool flag = callFieldChangeEventAsInteger != 0;
			string name = managedObject.GetType().Name;
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return;
			}
			object obj = Enum.Parse(fieldOfClass.FieldType, value);
			fieldOfClass.SetValue(managedObject, obj);
			if (flag)
			{
				ManagedExtensions.OnEditorVariableChanged(managedObject, classNameHash, fieldNameHash);
			}
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x000074E8 File Offset: 0x000056E8
		[EngineCallback(null, false)]
		internal static void GetObjectField(DotNetObject managedObject, uint classNameHash, ref ScriptComponentFieldHolder scriptComponentFieldHolder, uint fieldNameHash, RglScriptFieldType type)
		{
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			switch (type)
			{
			case RglScriptFieldType.RglSftString:
				scriptComponentFieldHolder.s = (string)fieldOfClass.GetValue(managedObject);
				return;
			case RglScriptFieldType.RglSftDouble:
				scriptComponentFieldHolder.d = (double)Convert.ChangeType(fieldOfClass.GetValue(managedObject), typeof(double));
				return;
			case RglScriptFieldType.RglSftFloat:
				scriptComponentFieldHolder.f = (float)Convert.ChangeType(fieldOfClass.GetValue(managedObject), typeof(float));
				return;
			case RglScriptFieldType.RglSftBool:
				scriptComponentFieldHolder.b = (((bool)fieldOfClass.GetValue(managedObject)) ? 1 : 0);
				return;
			case RglScriptFieldType.RglSftInt:
				scriptComponentFieldHolder.i = (int)Convert.ChangeType(fieldOfClass.GetValue(managedObject), typeof(int));
				return;
			case RglScriptFieldType.RglSftVec3:
			{
				Vec3 vec = (Vec3)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.v3 = new Vec3(vec, vec.w);
				return;
			}
			case RglScriptFieldType.RglSftEntity:
			{
				GameEntity gameEntity = (GameEntity)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.entityPointer = ((gameEntity != null) ? ((UIntPtr)Convert.ChangeType(gameEntity.Pointer, typeof(UIntPtr))) : ((UIntPtr)0UL));
				return;
			}
			case RglScriptFieldType.RglSftTexture:
			{
				Texture texture = (Texture)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.texturePointer = ((texture != null) ? ((UIntPtr)Convert.ChangeType(texture.Pointer, typeof(UIntPtr))) : ((UIntPtr)0UL));
				return;
			}
			case RglScriptFieldType.RglSftMesh:
			{
				MetaMesh metaMesh = (MetaMesh)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.meshPointer = ((metaMesh != null) ? ((UIntPtr)Convert.ChangeType(metaMesh.Pointer, typeof(UIntPtr))) : ((UIntPtr)0UL));
				return;
			}
			case RglScriptFieldType.RglSftEnum:
				scriptComponentFieldHolder.enumValue = fieldOfClass.GetValue(managedObject).ToString();
				return;
			case RglScriptFieldType.RglSftMaterial:
			{
				Material material = (Material)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.materialPointer = ((material != null) ? ((UIntPtr)Convert.ChangeType(material.Pointer, typeof(UIntPtr))) : ((UIntPtr)0UL));
				return;
			}
			case RglScriptFieldType.RglSftButton:
				break;
			case RglScriptFieldType.RglSftColor:
			{
				Color color = (Color)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.color.x = color.Red;
				scriptComponentFieldHolder.color.y = color.Green;
				scriptComponentFieldHolder.color.z = color.Blue;
				scriptComponentFieldHolder.color.w = color.Alpha;
				break;
			}
			case RglScriptFieldType.RglSftMatrixFrame:
			{
				MatrixFrame matrixFrame = (MatrixFrame)fieldOfClass.GetValue(managedObject);
				scriptComponentFieldHolder.matrixFrame = matrixFrame;
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00007794 File Offset: 0x00005994
		[EngineCallback(null, false)]
		internal static void CopyObjectFieldsFrom(DotNetObject dst, DotNetObject src, string className, int callFieldChangeEventAsInteger)
		{
			foreach (KeyValuePair<uint, FieldInfo> keyValuePair in Managed.GetEditableFieldsOfClass(Managed.GetStringHashCode(className)))
			{
				FieldInfo value = keyValuePair.Value;
				value.SetValue(dst, value.GetValue(src));
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x000077FC File Offset: 0x000059FC
		[EngineCallback(null, false)]
		internal static DotNetObject CreateScriptComponentInstance(string className, UIntPtr entityPtr, ManagedScriptComponent managedScriptComponent)
		{
			ScriptComponentBehavior scriptComponentBehavior = null;
			Func<ScriptComponentBehavior> func = (Func<ScriptComponentBehavior>)Managed.GetConstructorDelegateOfClass(className);
			if (func != null)
			{
				scriptComponentBehavior = func();
				if (scriptComponentBehavior != null)
				{
					scriptComponentBehavior.Construct(entityPtr, managedScriptComponent);
				}
			}
			else
			{
				ConstructorInfo constructorOfClass = Managed.GetConstructorOfClass(className);
				if (constructorOfClass != null)
				{
					scriptComponentBehavior = constructorOfClass.Invoke(new object[0]) as ScriptComponentBehavior;
					if (scriptComponentBehavior != null)
					{
						scriptComponentBehavior.Construct(entityPtr, managedScriptComponent);
					}
				}
				else
				{
					MBDebug.ShowWarning("CreateScriptComponentInstance failed: " + className);
				}
			}
			return scriptComponentBehavior;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00007870 File Offset: 0x00005A70
		[EngineCallback(null, false)]
		internal static string GetScriptComponentClassNames()
		{
			List<Type> list = new List<Type>();
			foreach (Type type in Managed.ModuleTypes.Values)
			{
				if (!type.IsAbstract && typeof(ScriptComponentBehavior).IsAssignableFrom(type))
				{
					list.Add(type);
				}
			}
			string text = "";
			for (int i = 0; i < list.Count; i++)
			{
				Type type2 = list[i];
				string text2 = type2.Name;
				string text3 = "!";
				object[] customAttributesSafe = type2.GetCustomAttributesSafe(typeof(ScriptComponentParams), true);
				if (customAttributesSafe.Length != 0)
				{
					ScriptComponentParams scriptComponentParams = (ScriptComponentParams)customAttributesSafe[0];
					if (scriptComponentParams.NameOverride.Length > 0)
					{
						text2 = scriptComponentParams.NameOverride;
					}
					if (scriptComponentParams.Tag.Length > 0)
					{
						text3 = scriptComponentParams.Tag;
					}
				}
				text += text2;
				text += "-";
				text += type2.BaseType.Name;
				text += "-";
				text += text3;
				if (i + 1 != list.Count)
				{
					text += " ";
				}
			}
			return text;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x000079CC File Offset: 0x00005BCC
		[EngineCallback(null, false)]
		internal static bool GetEditorVisibilityOfField(uint classNameHash, uint fieldNamehash)
		{
			object[] customAttributesSafe = Managed.GetFieldOfClass(classNameHash, fieldNamehash).GetCustomAttributesSafe(typeof(EditorVisibleScriptComponentVariable), true);
			return customAttributesSafe.Length == 0 || (customAttributesSafe[0] as EditorVisibleScriptComponentVariable).Visible;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00007A04 File Offset: 0x00005C04
		[EngineCallback(null, false)]
		internal static RglScriptFieldType GetTypeOfField(uint classNameHash, uint fieldNameHash)
		{
			FieldInfo fieldOfClass = Managed.GetFieldOfClass(classNameHash, fieldNameHash);
			if (fieldOfClass == null)
			{
				return RglScriptFieldType.RglSftInvalid;
			}
			Type fieldType = fieldOfClass.FieldType;
			if (fieldOfClass.FieldType == typeof(string))
			{
				return RglScriptFieldType.RglSftString;
			}
			if (fieldOfClass.FieldType == typeof(double))
			{
				return RglScriptFieldType.RglSftDouble;
			}
			if (fieldOfClass.FieldType.IsEnum)
			{
				return RglScriptFieldType.RglSftEnum;
			}
			if (fieldOfClass.FieldType == typeof(float))
			{
				return RglScriptFieldType.RglSftFloat;
			}
			if (fieldOfClass.FieldType == typeof(bool))
			{
				return RglScriptFieldType.RglSftBool;
			}
			if (fieldType == typeof(byte) || fieldType == typeof(sbyte) || fieldType == typeof(short) || fieldType == typeof(ushort) || fieldType == typeof(int) || fieldType == typeof(uint) || fieldType == typeof(long) || fieldType == typeof(ulong))
			{
				return RglScriptFieldType.RglSftInt;
			}
			if (fieldOfClass.FieldType == typeof(Vec3))
			{
				return RglScriptFieldType.RglSftVec3;
			}
			if (fieldOfClass.FieldType == typeof(GameEntity))
			{
				return RglScriptFieldType.RglSftEntity;
			}
			if (fieldOfClass.FieldType == typeof(Texture))
			{
				return RglScriptFieldType.RglSftTexture;
			}
			if (fieldOfClass.FieldType == typeof(MetaMesh))
			{
				return RglScriptFieldType.RglSftMesh;
			}
			if (fieldOfClass.FieldType == typeof(Material))
			{
				return RglScriptFieldType.RglSftMaterial;
			}
			if (fieldOfClass.FieldType == typeof(SimpleButton))
			{
				return RglScriptFieldType.RglSftButton;
			}
			if (fieldOfClass.FieldType == typeof(MatrixFrame))
			{
				return RglScriptFieldType.RglSftMatrixFrame;
			}
			if (fieldOfClass.FieldType == typeof(Color))
			{
				return RglScriptFieldType.RglSftColor;
			}
			return RglScriptFieldType.RglSftInvalid;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00007BFE File Offset: 0x00005DFE
		[EngineCallback(null, false)]
		internal static void ForceGarbageCollect()
		{
			Utilities.FlushManagedObjectsMemory();
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00007C08 File Offset: 0x00005E08
		[EngineCallback(null, false)]
		internal static void CollectCommandLineFunctions()
		{
			foreach (string text in CommandLineFunctionality.CollectCommandLineFunctions())
			{
				Utilities.AddCommandLineFunction(text);
			}
		}
	}
}
