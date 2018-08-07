namespace PostageApp.Abstractions
{
    public class GetMetricsResponseDataMetrics
    {
        public GetMetricsResponseDataMetric Hour { get; set; }
        public GetMetricsResponseDataMetric Date { get; set; }
        public GetMetricsResponseDataMetric Week { get; set; }
        public GetMetricsResponseDataMetric Month { get; set; }
    }
}
