namespace PostageApp.Abstractions
{
    // DRY
    public class GetProjectInfoResponseDataAccountTransmissions
    {
        public long Today { get; set; }
        public long ThisMonth { get; set; }
        public long Overall { get; set; }
    }
}
