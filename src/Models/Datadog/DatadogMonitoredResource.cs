// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace AzureMcp.Models.Datadog
{
    public class DatadogMonitoredResource
    {
        /// <summary>
        /// The Azure resource ID.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Flag indicating if the resource is sending metrics to Datadog.
        /// </summary>
        public bool? SendingMetrics { get; set; }

        /// <summary>
        /// Reason why the resource is (or is not) sending metrics to Datadog.
        /// </summary>
        public required string ReasonForMetricsStatus { get; set; }

        /// <summary>
        /// Flag indicating if the resource is sending logs to Datadog.
        /// </summary>
        public bool? SendingLogs { get; set; }

        /// <summary>
        /// Reason why the resource is (or is not) sending logs to Datadog.
        /// </summary>
        public required string ReasonForLogsStatus { get; set; }
    }
}
