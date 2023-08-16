using Newtonsoft.Json;

namespace Core.Domain.ViewOnly
{
    public class SugarCrmOppertunity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Account_name { get; set; }
        public string DisplayName => Name + " (" + Account_name + ")";
    }

    public class Name
    {
        [JsonProperty("$contains")]
        public string starts { get; set; }
    }

    public class Root
    {
        public Name name { get; set; }
    }
}
