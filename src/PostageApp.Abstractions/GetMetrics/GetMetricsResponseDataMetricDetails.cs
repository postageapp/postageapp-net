namespace PostageApp.Abstractions
{
    public class GetMetricsResponseDataMetricDetails
    {
        public double CurrentPercent { get; set; }
        public double PreviousPercent { get; set; }
        public double DiffPercent { get; set; }
        public double CurrentValue { get; set; }
        public double PreviousValue { get; set; }
    }
}
