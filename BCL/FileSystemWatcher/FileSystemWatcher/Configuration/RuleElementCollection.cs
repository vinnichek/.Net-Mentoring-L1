using System.Configuration;

namespace FileSystemWatcher.Configuration
{
    public class RuleElementCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("defaultDirectory")]
        public string DefaultDirectory => (string)this["defaultDirectory"];

        protected override ConfigurationElement CreateNewElement() => new RuleElement();

        protected override object GetElementKey(ConfigurationElement element) =>
            ((RuleElement)element).Template;
    }
}
