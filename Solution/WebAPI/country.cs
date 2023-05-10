namespace WebAPI
{
    public class Country
    {
        public string? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int RegionId { get; set; }
        public Region? Region { get; set; }
    }
}
