using System.Configuration;

namespace OrderImporter.Common.Configuration
{
    public class AppConfig : IAppConfig
    {
        public string OrdersApiUrl { get; private set; }
        public int PageSize { get; private set; }
        public int Retries { get; private set; }
        public int SecondsBetweenRetries { get; private set; }
        public string OriginDateFormat { get; private set; }
        public string ResultDateFormat { get; private set; }
        public int MaxConcurrentRequests { get; private set; }
        public string OrderCsvFileName { get; private set; }
        public string[] PropertiesToGroupBy { get; private set; }

        private AppConfig() { }

        public static AppConfig Load()
        {
            var config = new AppConfig
            {
                OrdersApiUrl = GetSetting("OrdersApiUrl"),
                PageSize = GetSettingAsInt("PageSize"),
                Retries = GetSettingAsInt("Retries"),
                SecondsBetweenRetries = GetSettingAsInt("SecondsBetweenRetries"),
                OriginDateFormat = GetSetting("OriginDateFormat"),
                ResultDateFormat = GetSetting("ResultDateFormat"),
                MaxConcurrentRequests = GetSettingAsInt("MaxConcurrentRequests"),
                OrderCsvFileName = GetSetting("OrderCsvFileName"),
                PropertiesToGroupBy = GetSetting("PropertiesToGroupBy").Split('|', StringSplitOptions.RemoveEmptyEntries)
            };

            return config;
        }

        private static string GetSetting(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                throw new ConfigurationErrorsException($"La clave '{key}' falta o no tiene valor en el fichero de configuración.");
            }
            return value;
        }

        private static int GetSettingAsInt(string key)
        {
            string value = GetSetting(key);
            if (!int.TryParse(value, out int result))
            {
                throw new ConfigurationErrorsException($"La clave '{key}' debe tener un valor numérico válido.");
            }
            return result;
        }
    }
}
