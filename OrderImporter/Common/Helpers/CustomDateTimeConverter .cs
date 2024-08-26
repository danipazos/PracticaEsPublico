using Newtonsoft.Json;
using OrderImporter.Common.Configuration;
using System.Globalization;

namespace OrderImporter.Common.Helpers
{
    public sealed class CustomDateTimeConverter(IAppConfig appConfig) : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return DateTime.MinValue;
            }

            string dateString = reader.Value.ToString();
            if (DateTime.TryParseExact(dateString, appConfig.OriginDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return DateTime.Parse(result.ToString(appConfig.ResultDateFormat)).Date;
            }

            return DateTime.MinValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(appConfig.ResultDateFormat));
        }
    }
}
