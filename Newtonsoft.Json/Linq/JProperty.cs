using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BB RID: 187
	[NullableContext(1)]
	[Nullable(0)]
	public class JProperty : JContainer
	{
		// Token: 0x06000A54 RID: 2644 RVA: 0x00029670 File Offset: 0x00027870
		public override Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			Task task = writer.WritePropertyNameAsync(this._name, cancellationToken);
			if (task.IsCompletedSucessfully())
			{
				return this.WriteValueAsync(writer, cancellationToken, converters);
			}
			return this.WriteToAsync(task, writer, cancellationToken, converters);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x000296A8 File Offset: 0x000278A8
		private async Task WriteToAsync(Task task, JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			await task.ConfigureAwait(false);
			await this.WriteValueAsync(writer, cancellationToken, converters).ConfigureAwait(false);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0002970C File Offset: 0x0002790C
		private Task WriteValueAsync(JsonWriter writer, CancellationToken cancellationToken, JsonConverter[] converters)
		{
			JToken value = this.Value;
			if (value == null)
			{
				return writer.WriteNullAsync(cancellationToken);
			}
			return value.WriteToAsync(writer, cancellationToken, converters);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00029734 File Offset: 0x00027934
		public new static Task<JProperty> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JProperty.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00029740 File Offset: 0x00027940
		public new static async Task<JProperty> LoadAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (reader.TokenType == JsonToken.None)
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = reader.ReadAsync(cancellationToken).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
				}
			}
			await reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty p = new JProperty((string)reader.Value);
			p.SetLineInfo(reader as IJsonLineInfo, settings);
			await p.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			return p;
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x00029793 File Offset: 0x00027993
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0002979B File Offset: 0x0002799B
		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return this._name;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x000297A3 File Offset: 0x000279A3
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x000297B0 File Offset: 0x000279B0
		public new JToken Value
		{
			[DebuggerStepThrough]
			get
			{
				return this._content._token;
			}
			set
			{
				base.CheckReentrancy();
				JToken jtoken = value ?? JValue.CreateNull();
				if (this._content._token == null)
				{
					this.InsertItem(0, jtoken, false);
					return;
				}
				this.SetItem(0, jtoken);
			}
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x000297EE File Offset: 0x000279EE
		public JProperty(JProperty other)
			: base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0002980E File Offset: 0x00027A0E
		internal override JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.Value;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00029820 File Offset: 0x00027A20
		[NullableContext(2)]
		internal override void SetItem(int index, JToken item)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (JContainer.IsTokenUnchanged(this.Value, item))
			{
				return;
			}
			JObject jobject = (JObject)base.Parent;
			if (jobject != null)
			{
				jobject.InternalPropertyChanging(this);
			}
			base.SetItem(0, item);
			JObject jobject2 = (JObject)base.Parent;
			if (jobject2 == null)
			{
				return;
			}
			jobject2.InternalPropertyChanged(this);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0002987A File Offset: 0x00027A7A
		[NullableContext(2)]
		internal override bool RemoveItem(JToken item)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0002989A File Offset: 0x00027A9A
		internal override void RemoveItemAt(int index)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x000298BA File Offset: 0x00027ABA
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._content.IndexOf(item);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x000298CD File Offset: 0x00027ACD
		[NullableContext(2)]
		internal override bool InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return false;
			}
			if (this.Value != null)
			{
				throw new JsonException("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
			}
			return base.InsertItem(0, item, false);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0002990D File Offset: 0x00027B0D
		[NullableContext(2)]
		internal override bool ContainsItem(JToken item)
		{
			return this.Value == item;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00029918 File Offset: 0x00027B18
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			JProperty jproperty = content as JProperty;
			JToken jtoken = ((jproperty != null) ? jproperty.Value : null);
			if (jtoken != null && jtoken.Type != JTokenType.Null)
			{
				this.Value = jtoken;
			}
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0002994C File Offset: 0x00027B4C
		internal override void ClearItems()
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0002996C File Offset: 0x00027B6C
		internal override bool DeepEquals(JToken node)
		{
			JProperty jproperty = node as JProperty;
			return jproperty != null && this._name == jproperty.Name && base.ContentsEqual(jproperty);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0002999F File Offset: 0x00027B9F
		internal override JToken CloneToken()
		{
			return new JProperty(this);
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x000299A7 File Offset: 0x00027BA7
		public override JTokenType Type
		{
			[DebuggerStepThrough]
			get
			{
				return JTokenType.Property;
			}
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x000299AA File Offset: 0x00027BAA
		internal JProperty(string name)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000299CF File Offset: 0x00027BCF
		public JProperty(string name, params object[] content)
			: this(name, content)
		{
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x000299DC File Offset: 0x00027BDC
		public JProperty(string name, [Nullable(2)] object content)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
			this.Value = (base.IsMultiContent(content) ? new JArray(content) : JContainer.CreateFromContent(content));
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00029A2C File Offset: 0x00027C2C
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WritePropertyName(this._name);
			JToken value = this.Value;
			if (value != null)
			{
				value.WriteTo(writer, converters);
				return;
			}
			writer.WriteNull();
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00029A5E File Offset: 0x00027C5E
		internal override int GetDeepHashCode()
		{
			int hashCode = this._name.GetHashCode();
			JToken value = this.Value;
			return hashCode ^ ((value != null) ? value.GetDeepHashCode() : 0);
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00029A7E File Offset: 0x00027C7E
		public new static JProperty Load(JsonReader reader)
		{
			return JProperty.Load(reader, null);
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00029A88 File Offset: 0x00027C88
		public new static JProperty Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty jproperty = new JProperty((string)reader.Value);
			jproperty.SetLineInfo(reader as IJsonLineInfo, settings);
			jproperty.ReadTokenFrom(reader, settings);
			return jproperty;
		}

		// Token: 0x04000367 RID: 871
		private readonly JProperty.JPropertyList _content = new JProperty.JPropertyList();

		// Token: 0x04000368 RID: 872
		private readonly string _name;

		// Token: 0x020001C4 RID: 452
		[Nullable(0)]
		private class JPropertyList : IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
		{
			// Token: 0x06000F83 RID: 3971 RVA: 0x000440EE File Offset: 0x000422EE
			public IEnumerator<JToken> GetEnumerator()
			{
				if (this._token != null)
				{
					yield return this._token;
				}
				yield break;
			}

			// Token: 0x06000F84 RID: 3972 RVA: 0x000440FD File Offset: 0x000422FD
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000F85 RID: 3973 RVA: 0x00044105 File Offset: 0x00042305
			public void Add(JToken item)
			{
				this._token = item;
			}

			// Token: 0x06000F86 RID: 3974 RVA: 0x0004410E File Offset: 0x0004230E
			public void Clear()
			{
				this._token = null;
			}

			// Token: 0x06000F87 RID: 3975 RVA: 0x00044117 File Offset: 0x00042317
			public bool Contains(JToken item)
			{
				return this._token == item;
			}

			// Token: 0x06000F88 RID: 3976 RVA: 0x00044122 File Offset: 0x00042322
			public void CopyTo(JToken[] array, int arrayIndex)
			{
				if (this._token != null)
				{
					array[arrayIndex] = this._token;
				}
			}

			// Token: 0x06000F89 RID: 3977 RVA: 0x00044135 File Offset: 0x00042335
			public bool Remove(JToken item)
			{
				if (this._token == item)
				{
					this._token = null;
					return true;
				}
				return false;
			}

			// Token: 0x1700029E RID: 670
			// (get) Token: 0x06000F8A RID: 3978 RVA: 0x0004414A File Offset: 0x0004234A
			public int Count
			{
				get
				{
					if (this._token == null)
					{
						return 0;
					}
					return 1;
				}
			}

			// Token: 0x1700029F RID: 671
			// (get) Token: 0x06000F8B RID: 3979 RVA: 0x00044157 File Offset: 0x00042357
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000F8C RID: 3980 RVA: 0x0004415A File Offset: 0x0004235A
			public int IndexOf(JToken item)
			{
				if (this._token != item)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x06000F8D RID: 3981 RVA: 0x00044168 File Offset: 0x00042368
			public void Insert(int index, JToken item)
			{
				if (index == 0)
				{
					this._token = item;
				}
			}

			// Token: 0x06000F8E RID: 3982 RVA: 0x00044174 File Offset: 0x00042374
			public void RemoveAt(int index)
			{
				if (index == 0)
				{
					this._token = null;
				}
			}

			// Token: 0x170002A0 RID: 672
			public JToken this[int index]
			{
				get
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException();
					}
					return this._token;
				}
				set
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException();
					}
					this._token = value;
				}
			}

			// Token: 0x040007A3 RID: 1955
			[Nullable(2)]
			internal JToken _token;
		}
	}
}
