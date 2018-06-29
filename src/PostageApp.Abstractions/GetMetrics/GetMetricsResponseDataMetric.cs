namespace PostageApp.Abstractions
{
    public class GetMetricsResponseDataMetric
    {
        public GetMetricsResponseDataMetricDetails Delivered { get; set; }
        public GetMetricsResponseDataMetricDetails Opened { get; set; }
        public GetMetricsResponseDataMetricDetails Failed { get; set; }
        public GetMetricsResponseDataMetricDetails Rejected { get; set; }
        public GetMetricsResponseDataMetricDetails Created { get; set; }
        public GetMetricsResponseDataMetricDetails Queued { get; set; }
    }
}
