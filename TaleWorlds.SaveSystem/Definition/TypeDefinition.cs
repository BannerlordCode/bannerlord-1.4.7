using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Load;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.SaveSystem.Definition
{
	// Token: 0x0200006C RID: 108
	public class TypeDefinition : TypeDefinitionBase
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00010A6D File Offset: 0x0000EC6D
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x00010A75 File Offset: 0x0000EC75
		public List<MemberDefinition> MemberDefinitions { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00010A7E File Offset: 0x0000EC7E
		public IEnumerable<MethodInfo> InitializationCallbacks
		{
			get
			{
				return this._initializationCallbacks;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00010A86 File Offset: 0x0000EC86
		public IEnumerable<MethodInfo> LateInitializationCallbacks
		{
			get
			{
				return this._lateInitializationCallbacks;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00010A8E File Offset: 0x0000EC8E
		public IEnumerable<string> Errors
		{
			get
			{
				return this._errors.AsReadOnly();
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00010A9B File Offset: 0x0000EC9B
		public bool IsClassDefinition
		{
			get
			{
				return this._isClass;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00010AA3 File Offset: 0x0000ECA3
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x00010AAB File Offset: 0x0000ECAB
		public List<CustomField> CustomFields { get; private set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x00010AB4 File Offset: 0x0000ECB4
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x00010ABC File Offset: 0x0000ECBC
		public CollectObjectsDelegate CollectObjectsMethod { get; private set; }

		// Token: 0x060003AA RID: 938 RVA: 0x00010AC8 File Offset: 0x0000ECC8
		public TypeDefinition(Type type, SaveId saveId, IObjectResolver objectResolver)
			: base(type, saveId)
		{
			this._isClass = base.Type.IsClass;
			this._errors = new List<string>();
			this._properties = new Dictionary<MemberTypeId, PropertyDefinition>();
			this._fields = new Dictionary<MemberTypeId, FieldDefinition>();
			this.MemberDefinitions = new List<MemberDefinition>();
			this.CustomFields = new List<CustomField>();
			this._initializationCallbacks = new List<MethodInfo>();
			this._lateInitializationCallbacks = new List<MethodInfo>();
			this._objectResolver = objectResolver;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00010B42 File Offset: 0x0000ED42
		public TypeDefinition(Type type, int saveId, IObjectResolver objectResolver)
			: this(type, new TypeSaveId(saveId), objectResolver)
		{
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00010B52 File Offset: 0x0000ED52
		public bool CheckIfRequiresAdvancedResolving(object originalObject)
		{
			return this._objectResolver != null && this._objectResolver.CheckIfRequiresAdvancedResolving(originalObject);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00010B6A File Offset: 0x0000ED6A
		public object ResolveObject(object originalObject)
		{
			if (this._objectResolver != null)
			{
				return this._objectResolver.ResolveObject(originalObject);
			}
			return originalObject;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00010B82 File Offset: 0x0000ED82
		public object AdvancedResolveObject(object originalObject, MetaData metaData, ObjectLoadData objectLoadData)
		{
			if (this._objectResolver != null)
			{
				return this._objectResolver.AdvancedResolveObject(originalObject, metaData, objectLoadData);
			}
			return originalObject;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00010B9C File Offset: 0x0000ED9C
		public void CollectInitializationCallbacks()
		{
			Type type = base.Type;
			while (type != typeof(object))
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (methodInfo.DeclaringType == type)
					{
						if (methodInfo.GetCustomAttributesSafe(typeof(LoadInitializationCallback)).ToArray<Attribute>().Length != 0 && !this._initializationCallbacks.Contains(methodInfo))
						{
							this._initializationCallbacks.Insert(0, methodInfo);
						}
						if (methodInfo.GetCustomAttributesSafe(typeof(LateLoadInitializationCallback)).ToArray<Attribute>().Length != 0 && !this._lateInitializationCallbacks.Contains(methodInfo))
						{
							this._lateInitializationCallbacks.Insert(0, methodInfo);
						}
					}
				}
				type = type.BaseType;
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00010C64 File Offset: 0x0000EE64
		public void CollectProperties()
		{
			foreach (PropertyInfo propertyInfo in base.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				Attribute[] array = propertyInfo.GetCustomAttributesSafe(typeof(SaveablePropertyAttribute)).ToArray<Attribute>();
				if (array.Length != 0)
				{
					SaveablePropertyAttribute saveablePropertyAttribute = (SaveablePropertyAttribute)array[0];
					byte classLevel = TypeDefinitionBase.GetClassLevel(propertyInfo.DeclaringType);
					MemberTypeId memberTypeId = new MemberTypeId(classLevel, saveablePropertyAttribute.LocalSaveId);
					PropertyDefinition propertyDefinition = new PropertyDefinition(propertyInfo, memberTypeId);
					if (this._properties.ContainsKey(memberTypeId))
					{
						this._errors.Add(string.Concat(new object[]
						{
							"SaveId ",
							memberTypeId,
							" of property ",
							propertyDefinition.PropertyInfo.Name,
							" is already defined in type ",
							base.Type.FullName
						}));
					}
					else
					{
						this._properties.Add(memberTypeId, propertyDefinition);
						this.MemberDefinitions.Add(propertyDefinition);
					}
				}
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00010D67 File Offset: 0x0000EF67
		private static IEnumerable<FieldInfo> GetFieldsOfType(Type type)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!fieldInfo.IsPrivate)
				{
					yield return fieldInfo;
				}
			}
			FieldInfo[] array = null;
			Type typeToCheck = type;
			while (typeToCheck != typeof(object))
			{
				FieldInfo[] fields2 = typeToCheck.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
				foreach (FieldInfo fieldInfo2 in fields2)
				{
					if (fieldInfo2.IsPrivate)
					{
						yield return fieldInfo2;
					}
				}
				array = null;
				typeToCheck = typeToCheck.BaseType;
			}
			yield break;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00010D78 File Offset: 0x0000EF78
		public void CollectFields()
		{
			foreach (FieldInfo fieldInfo in TypeDefinition.GetFieldsOfType(base.Type).ToArray<FieldInfo>())
			{
				Attribute[] array2 = fieldInfo.GetCustomAttributesSafe(typeof(SaveableFieldAttribute)).ToArray<Attribute>();
				if (array2.Length != 0)
				{
					SaveableFieldAttribute saveableFieldAttribute = (SaveableFieldAttribute)array2[0];
					byte classLevel = TypeDefinitionBase.GetClassLevel(fieldInfo.DeclaringType);
					MemberTypeId memberTypeId = new MemberTypeId(classLevel, saveableFieldAttribute.LocalSaveId);
					FieldDefinition fieldDefinition = new FieldDefinition(fieldInfo, memberTypeId);
					if (this._fields.ContainsKey(memberTypeId))
					{
						this._errors.Add(string.Concat(new object[]
						{
							"SaveId ",
							memberTypeId,
							" of field ",
							fieldDefinition.FieldInfo,
							" is already defined in type ",
							base.Type.FullName
						}));
					}
					else
					{
						this._fields.Add(memberTypeId, fieldDefinition);
						this.MemberDefinitions.Add(fieldDefinition);
					}
				}
			}
			foreach (CustomField customField in this.CustomFields)
			{
				string name = customField.Name;
				short saveId = customField.SaveId;
				FieldInfo field = base.Type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				byte classLevel2 = TypeDefinitionBase.GetClassLevel(field.DeclaringType);
				MemberTypeId memberTypeId2 = new MemberTypeId(classLevel2, saveId);
				FieldDefinition fieldDefinition2 = new FieldDefinition(field, memberTypeId2);
				if (this._fields.ContainsKey(memberTypeId2))
				{
					this._errors.Add(string.Concat(new object[]
					{
						"SaveId ",
						memberTypeId2,
						" of field ",
						fieldDefinition2.FieldInfo,
						" is already defined in type ",
						base.Type.FullName
					}));
				}
				else
				{
					this._fields.Add(memberTypeId2, fieldDefinition2);
					this.MemberDefinitions.Add(fieldDefinition2);
				}
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00010F7C File Offset: 0x0000F17C
		public void AddCustomField(string fieldName, short saveId)
		{
			this.CustomFields.Add(new CustomField(fieldName, saveId));
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00010F90 File Offset: 0x0000F190
		public PropertyDefinition GetPropertyDefinitionWithId(MemberTypeId id)
		{
			PropertyDefinition propertyDefinition;
			this._properties.TryGetValue(id, out propertyDefinition);
			return propertyDefinition;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00010FB0 File Offset: 0x0000F1B0
		public FieldDefinition GetFieldDefinitionWithId(MemberTypeId id)
		{
			FieldDefinition fieldDefinition;
			this._fields.TryGetValue(id, out fieldDefinition);
			return fieldDefinition;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00010FCD File Offset: 0x0000F1CD
		public Dictionary<MemberTypeId, PropertyDefinition>.ValueCollection PropertyDefinitions
		{
			get
			{
				return this._properties.Values;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00010FDA File Offset: 0x0000F1DA
		public Dictionary<MemberTypeId, FieldDefinition>.ValueCollection FieldDefinitions
		{
			get
			{
				return this._fields.Values;
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00010FE7 File Offset: 0x0000F1E7
		public void InitializeForAutoGeneration(CollectObjectsDelegate collectObjectsDelegate)
		{
			this.CollectObjectsMethod = collectObjectsDelegate;
		}

		// Token: 0x04000115 RID: 277
		private Dictionary<MemberTypeId, PropertyDefinition> _properties;

		// Token: 0x04000116 RID: 278
		private Dictionary<MemberTypeId, FieldDefinition> _fields;

		// Token: 0x04000118 RID: 280
		private List<string> _errors;

		// Token: 0x04000119 RID: 281
		private List<MethodInfo> _initializationCallbacks;

		// Token: 0x0400011A RID: 282
		private List<MethodInfo> _lateInitializationCallbacks;

		// Token: 0x0400011B RID: 283
		private bool _isClass;

		// Token: 0x0400011C RID: 284
		private readonly IObjectResolver _objectResolver;
	}
}
