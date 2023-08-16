namespace Core.Domain.ViewOnly
{
    public class SugarCrmOppertunityList
    {
        public int next_offset { get; set; }

        public List<SugarCrmOppertunity> records { get; set; }
    }
}
