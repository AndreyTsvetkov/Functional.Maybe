using System;
using System.Collections.Concurrent;
using System.Linq;
using Newtonsoft.Json;

namespace Functional.Maybe.Json;

public class MaybeConverter : JsonConverter
{
	private static readonly Type OpenMaybeType = typeof(Maybe<>);

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
		GetConverter(value!.GetType()).WriteJson(writer, value, serializer);

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
		GetConverter(objectType).ReadJson(reader, serializer);

	public override bool CanConvert(Type objectType) =>
		objectType.IsGenericType
		&& objectType.GetGenericTypeDefinition() == OpenMaybeType;

	private TypeConverter GetConverter(Type closedMaybeType)
	{
		var underlyingType = closedMaybeType.GetGenericArguments()[0];


		if (!_converters.TryGetValue(underlyingType, out TypeConverter converter))
		{
			var converterType = typeof(MaybeConverter<>).MakeGenericType(underlyingType);
			_converters.TryAdd(underlyingType, new TypeConverter(converterType));
			converter = _converters[underlyingType];
		}

		return converter;
	}

	private readonly ConcurrentDictionary<Type, TypeConverter> _converters = new();

	private class TypeConverter
	{
		private readonly Action<JsonWriter, object, JsonSerializer> _writeJsonDelegate;
		private readonly Func<JsonReader, JsonSerializer, object> _readJsonDelegate;

		public TypeConverter(Type maybeConverterType)
		{
			var converter = Activator.CreateInstance(maybeConverterType);

			// ReSharper disable AssignNullToNotNullAttribute — мы знаем устройство типа MaybeConverter.
			_writeJsonDelegate = (Action<JsonWriter, object, JsonSerializer>)Delegate.CreateDelegate(
				typeof(Action<JsonWriter, object, JsonSerializer>),
				converter,
				maybeConverterType.GetMethod(nameof(MaybeConverter.WriteJson)));

			_readJsonDelegate = (Func<JsonReader, JsonSerializer, object>)Delegate.CreateDelegate(
				typeof(Func<JsonReader, JsonSerializer, object>),
				converter,
				maybeConverterType.GetMethod(nameof(MaybeConverter.ReadJson)));

			// ReSharper restore AssignNullToNotNullAttribute
		}

		public void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			_writeJsonDelegate(writer, value, serializer);
		}

		public object ReadJson(JsonReader reader, JsonSerializer serializer)
		{
			return _readJsonDelegate(reader, serializer);
		}
	}
}

internal class MaybeConverter<T>
{
	// ReSharper disable once StaticMemberInGenericType
	private static readonly Type[] SimpleTypes =
	[
		typeof(string), typeof(int), typeof(byte), typeof(short), typeof(decimal), typeof(float), typeof(double),
		typeof(Guid)
	];

	private static bool IsSimpleType(Type t) =>
		SimpleTypes.Contains(t) ||
		t.IsEnum;

	private static readonly Type UnderlyingType = typeof(T);

	// ReSharper disable once UnusedMember.Global — used via reflection
	public void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		var typed = (Maybe<T>)value;
		if (IsSimpleType(UnderlyingType))
		{
			if (typed.HasValue)
				writer.WriteValue(typed.Value);
			else
				writer.WriteNull();
		}
		else
		{
			if (typed.HasValue)
				serializer.Serialize(writer, typed.Value);
			else
				writer.WriteNull();
		}
	}

	// ReSharper disable once UnusedMember.Global — used via reflection
	public object ReadJson(JsonReader reader, JsonSerializer serializer) =>
		reader.TokenType == JsonToken.Null
			? Maybe<T>.Nothing
			: (
				IsSimpleType(UnderlyingType)
					? reader.Value is T value ? value : TryConvert(reader.Value)
					: serializer.Deserialize<T>(reader)
			).ToMaybe();

	private static T TryConvert(object value)
	{
		if (UnderlyingType == typeof(Guid) && value is string s)
			return (T)(object)Guid.Parse(s);

		if (UnderlyingType.IsEnum)
			return (UnderlyingType.GetEnumUnderlyingType() != value.GetType()).Then(
					// например, enum на базе int, а тут передано value — long,
					// тогда обработаем через GetName/Parse, чтобы не жоглировать всеми вариантами типов целых чисел
					() => Enum.GetName(UnderlyingType, value).ToMaybe()
				).Collapse()
				.Select(enumOptionName => (T)Enum.Parse(UnderlyingType, enumOptionName))
				.OrElse(() => (T)value);

		return (T)Convert.ChangeType(value, UnderlyingType);
	}
}