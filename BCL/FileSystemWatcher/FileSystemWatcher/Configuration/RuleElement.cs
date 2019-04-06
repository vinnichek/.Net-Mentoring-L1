using System.Configuration;

namespace FileSystemWatcher.Configuration
{
    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty("template", IsKey = true)]
        public string Template => (string)base["template"];

        [ConfigurationProperty("destinationDirectory")]
        public string DestinationDirectory => (string)base["destinationDirectory"];

        [ConfigurationProperty("isIndexNumberRequired")]
        public bool IsIndexNumberRequired => (bool)base["isIndexNumberRequired"];

        [ConfigurationProperty("isDateRequired")]
        public bool IsDateRequired => (bool)base["isDateRequired"];
    }
}
