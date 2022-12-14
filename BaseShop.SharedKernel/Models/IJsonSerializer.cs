using System;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace BaseShop.SharedKernel.Models;

  public interface IJsonSerializer
  {
    string Serialize(object obj);
    object Deserialize(string value, Type type);
    T Deserialize<T>(string value) where T : class;
  }

public class NotImplementedJsonSerializer : IJsonSerializer
{
  public string Serialize(object obj)
  {
    throw new NotSupportedException(string.Format("{0} does not support serializing object.", GetType().FullName));
  }
  public object Deserialize(string value, Type type)
  {
    throw new NotSupportedException(string.Format("{0} does not support deserializing object.", GetType().FullName));
  }
  public T Deserialize<T>(string value) where T : class
  {
    throw new NotSupportedException(string.Format("{0} does not support deserializing object.", GetType().FullName));
  }
}



/// </summary>
public static class ConfigurationExtensions
{
  /// <summary>Use Json.Net as the json serializer.
  /// </summary>
  /// <returns></returns>
  public static System.Configuration UseJsonNet(this System.Configuration configuration)
  {
    configuration.SetDefault<IJsonSerializer, NewtonsoftJsonSerializer>(new NewtonsoftJsonSerializer());
    return configuration;
  }
}


/// <summary>Json.Net implementationof IJsonSerializer.
/// </summary>
public class NewtonsoftJsonSerializer : IJsonSerializer
{
  public JsonSerializerSettings Settings { get; private set; }

  public NewtonsoftJsonSerializer()
  {
    Settings = new JsonSerializerSettings
    {
      Converters = new List<JsonConverter> { new IsoDateTimeConverter() },
      ContractResolver = new CustomContractResolver(),
      ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
    };
  }

  /// <summary>Serialize an object to json string.
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public string Serialize(object obj)
  {
    return obj == null ? null : JsonConvert.SerializeObject(obj, Settings);
  }
  /// <summary>Deserialize a json string to an object.
  /// </summary>
  /// <param name="value"></param>
  /// <param name="type"></param>
  /// <returns></returns>
  public object Deserialize(string value, Type type)
  {
    return JsonConvert.DeserializeObject(value, type, Settings);
  }
  /// <summary>Deserialize a json string to a strong type object.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public T Deserialize<T>(string value) where T : class
  {
    return JsonConvert.DeserializeObject<T>(JObject.Parse(value).ToString(), Settings);
  }

  class CustomContractResolver : DefaultContractResolver
  {
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
      var jsonProperty = base.CreateProperty(member, memberSerialization);
      if (jsonProperty.Writable) return jsonProperty;
      var property = member as PropertyInfo;
      if (property == null) return jsonProperty;
      var hasPrivateSetter = property.GetSetMethod(true) != null;
      jsonProperty.Writable = hasPrivateSetter;

      return jsonProperty;
    }
  }
}




    /// <summary>Json.Net implementationof IJsonSerializer.
    /// </summary>
    public class NewtonsoftJsonSerializer : IJsonSerializer
{
  public JsonSerializerSettings Settings { get; private set; }

  public NewtonsoftJsonSerializer()
  {
    Settings = new JsonSerializerSettings
    {
      Converters = new List<JsonConverter> { new IsoDateTimeConverter() },
      ContractResolver = new CustomContractResolver(),
      ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
    };
  }

  /// <summary>Serialize an object to json string.
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public string Serialize(object obj)
  {
    return obj == null ? null : JsonConvert.SerializeObject(obj, Settings);
  }
  /// <summary>Deserialize a json string to an object.
  /// </summary>
  /// <param name="value"></param>
  /// <param name="type"></param>
  /// <returns></returns>
  public object Deserialize(string value, Type type)
  {
    return JsonConvert.DeserializeObject(value, type, Settings);
  }
  /// <summary>Deserialize a json string to a strong type object.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value"></param>
  /// <returns></returns>
  public T Deserialize<T>(string value) where T : class
  {
    return JsonConvert.DeserializeObject<T>(JObject.Parse(value).ToString(), Settings);
  }

  class CustomContractResolver : DefaultContractResolver
  {
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
      var jsonProperty = base.CreateProperty(member, memberSerialization);
      if (jsonProperty.Writable) return jsonProperty;
      var property = member as PropertyInfo;
      if (property == null) return jsonProperty;
      var hasPrivateSetter = property.GetSetMethod(true) != null;
      jsonProperty.Writable = hasPrivateSetter;

      return jsonProperty;
    }
  }
}
}
