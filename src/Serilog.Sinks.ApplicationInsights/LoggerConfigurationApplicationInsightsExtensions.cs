﻿// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.ApplicationInsights() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationApplicationInsightsExtensions
    {
        /// <summary>
        /// Adds a Serilog sink that writes <see cref="LogEvent">log events</see> to Microsoft Application Insights as <see cref="EventTelemetry"/>.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="instrumentationKey">Required Application Insights instrumentation key.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="logEventDataToTelemetryForwarder">The <see cref="LogEvent" /> data to AI <see cref="ITelemetry" /> forwarder
        /// provides control over what data of each <see cref="LogEvent" /> is sent to Application Insights, particularly the Message itself but also Properties.
        /// If none is provided, all properties are sent to AI (albeit flattened).</param>
        /// <returns>
        /// Logger configuration, allowing configuration to continue.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">loggerConfiguration</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">instrumentationKey;Cannot be empty or null.</exception>
        public static LoggerConfiguration ApplicationInsightsEvents(
            this LoggerSinkConfiguration loggerConfiguration,
            string instrumentationKey,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            Action<LogEvent, IFormatProvider, ITelemetry, ISupportProperties> logEventDataToTelemetryForwarder = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

            return loggerConfiguration.Sink(
                new ApplicationInsightsEventsSink(CreateTelemetryClientFromInstrumentationkey(instrumentationKey), formatProvider, logEventDataToTelemetryForwarder),
                restrictedToMinimumLevel);
        }

        /// <summary>
        /// Adds a Serilog sink that writes <see cref="LogEvent">log events</see> to Microsoft Application Insights as <see cref="EventTelemetry"/>.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="configuration">Required Application Insights configuration settings.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="logEventDataToTelemetryForwarder">The <see cref="LogEvent" /> data to AI <see cref="ITelemetry" /> forwarder
        /// provides control over what data of each <see cref="LogEvent" /> is sent to Application Insights, particularly the Message itself but also Properties.
        /// If none is provided, all properties are sent to AI (albeit flattened).</param>
        /// <returns>
        /// Logger configuration, allowing configuration to continue.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// loggerConfiguration
        /// or
        /// configuration
        /// </exception>
        public static LoggerConfiguration ApplicationInsightsEvents(
            this LoggerSinkConfiguration loggerConfiguration,
            TelemetryConfiguration configuration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            Action<LogEvent, IFormatProvider, ITelemetry, ISupportProperties> logEventDataToTelemetryForwarder = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");
            if (configuration == null) throw new ArgumentNullException("configuration");

            return loggerConfiguration.Sink(
                new ApplicationInsightsEventsSink(CreateTelemetryClientFromConfiguration(configuration), formatProvider, logEventDataToTelemetryForwarder),
                restrictedToMinimumLevel);
        }

        /// <summary>
        /// Adds a Serilog sink that writes <see cref="LogEvent">log events</see> to Microsoft Application Insights as <see cref="EventTelemetry"/>.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <param name="restrictedToMinimumLevel">The restricted to minimum level.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="logEventDataToTelemetryForwarder">The <see cref="LogEvent" /> data to AI <see cref="ITelemetry" /> forwarder
        /// provides control over what data of each <see cref="LogEvent" /> is sent to Application Insights, particularly the Message itself but also Properties.
        /// If none is provided, all properties are sent to AI (albeit flattened).</param>
        /// <returns>
        /// Logger configuration, allowing configuration to continue.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// loggerConfiguration
        /// or
        /// configuration
        /// </exception>
        public static LoggerConfiguration ApplicationInsightsEvents(
            this LoggerSinkConfiguration loggerConfiguration,
            TelemetryClient telemetryClient,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            Action<LogEvent, IFormatProvider, ITelemetry, ISupportProperties> logEventDataToTelemetryForwarder = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");
            if (telemetryClient == null) throw new ArgumentNullException("telemetryClient");

            return loggerConfiguration.Sink(
                new ApplicationInsightsEventsSink(telemetryClient, formatProvider, logEventDataToTelemetryForwarder),
                restrictedToMinimumLevel);
        }

        /// <summary>
        /// Adds a Serilog sink that writes <see cref="LogEvent">log events</see> to Microsoft Application Insights as <see cref="TraceTelemetry"/>.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="instrumentationKey">Required Application Insights instrumentation key.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="logEventDataToTelemetryForwarder">The <see cref="LogEvent" /> data to AI <see cref="ITelemetry" /> forwarder
        /// provides control over what data of each <see cref="LogEvent" /> is sent to Application Insights, particularly the Message itself but also Properties.
        /// If none is provided, all properties are sent to AI (albeit flattened).</param>
        /// <returns>
        /// Logger configuration, allowing configuration to continue.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">loggerConfiguration</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">instrumentationKey;Cannot be empty or null.</exception>
        public static LoggerConfiguration ApplicationInsightsTraces(
            this LoggerSinkConfiguration loggerConfiguration,
            string instrumentationKey,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            Action<LogEvent, IFormatProvider, ITelemetry, ISupportProperties> logEventDataToTelemetryForwarder = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

            return loggerConfiguration.Sink(
                new ApplicationInsightsTracesSink(CreateTelemetryClientFromInstrumentationkey(instrumentationKey), formatProvider, logEventDataToTelemetryForwarder),
                restrictedToMinimumLevel);
        }

        /// <summary>
        /// Adds a Serilog sink that writes <see cref="LogEvent">log events</see> to Microsoft Application Insights as <see cref="TraceTelemetry"/>.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="configuration">Required Application Insights configuration settings.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="logEventDataToTelemetryForwarder">The <see cref="LogEvent" /> data to AI <see cref="ITelemetry" /> forwarder
        /// provides control over what data of each <see cref="LogEvent" /> is sent to Application Insights, particularly the Message itself but also Properties.
        /// If none is provided, all properties are sent to AI (albeit flattened).</param>
        /// <returns>
        /// Logger configuration, allowing configuration to continue.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// loggerConfiguration
        /// or
        /// configuration
        /// </exception>
        public static LoggerConfiguration ApplicationInsightsTraces(
            this LoggerSinkConfiguration loggerConfiguration,
            TelemetryConfiguration configuration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            Action<LogEvent, IFormatProvider, ITelemetry, ISupportProperties> logEventDataToTelemetryForwarder = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");
            if (configuration == null) throw new ArgumentNullException("configuration");

            return loggerConfiguration.Sink(
                new ApplicationInsightsTracesSink(CreateTelemetryClientFromConfiguration(configuration), formatProvider, logEventDataToTelemetryForwarder),
                restrictedToMinimumLevel);
        }

        /// <summary>
        /// Adds a Serilog sink that writes <see cref="LogEvent">log events</see> to Microsoft Application Insights as <see cref="TraceTelemetry"/>.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <param name="restrictedToMinimumLevel">The restricted to minimum level.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="logEventDataToTelemetryForwarder">The <see cref="LogEvent" /> data to AI <see cref="ITelemetry" /> forwarder
        /// provides control over what data of each <see cref="LogEvent" /> is sent to Application Insights, particularly the Message itself but also Properties.
        /// If none is provided, all properties are sent to AI (albeit flattened).</param>
        /// <returns>
        /// Logger configuration, allowing configuration to continue.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// loggerConfiguration
        /// or
        /// configuration
        /// </exception>
        public static LoggerConfiguration ApplicationInsightsTraces(
            this LoggerSinkConfiguration loggerConfiguration,
            TelemetryClient telemetryClient,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            Action<LogEvent, IFormatProvider, ITelemetry, ISupportProperties> logEventDataToTelemetryForwarder = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");
            if (telemetryClient == null) throw new ArgumentNullException("telemetryClient");

            return loggerConfiguration.Sink(
                new ApplicationInsightsTracesSink(telemetryClient, formatProvider, logEventDataToTelemetryForwarder),
                restrictedToMinimumLevel);
        }
        
        /// <summary>
        /// Adds a Serilog sink that writes <see cref="LogEvent">log events</see> to Microsoft Application Insights as <see cref="EventTelemetry"/>.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="instrumentationKey">Required Application Insights instrumentation key.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>
        /// Logger configuration, allowing configuration to continue.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">loggerConfiguration</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">instrumentationKey;Cannot be empty or null.</exception>
        [Obsolete("This unspecific AI Telemetry Sink will be removed once Serilog Core reaches v2.0, please use either .ApplicationInsightsEvents(..) or .ApplicationInsightsTraces(..)")]
        public static LoggerConfiguration ApplicationInsights(
            this LoggerSinkConfiguration loggerConfiguration,
            string instrumentationKey,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

            return loggerConfiguration.ApplicationInsightsEvents(instrumentationKey, restrictedToMinimumLevel, formatProvider);
        }

        /// <summary>
        /// Adds a Serilog sink that writes <see cref="LogEvent">log events</see> to Microsoft Application Insights as <see cref="EventTelemetry"/>.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="configuration">Required Application Insights configuration settings.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>
        /// Logger configuration, allowing configuration to continue.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// loggerConfiguration
        /// or
        /// configuration
        /// </exception>
        [Obsolete("This unspecific AI Telemetry Sink will be removed once Serilog Core reaches v2.0, please use either .ApplicationInsightsEvents(..) or .ApplicationInsightsTraces(..)")]
        public static LoggerConfiguration ApplicationInsights(
            this LoggerSinkConfiguration loggerConfiguration,
            TelemetryConfiguration configuration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

            return loggerConfiguration.ApplicationInsightsEvents(configuration, restrictedToMinimumLevel, formatProvider);
        }

        /// <summary>
        /// Adds a Serilog sink that writes <see cref="LogEvent">log events</see> to Microsoft Application Insights as <see cref="EventTelemetry"/>.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="telemetryClient">The telemetry client.</param>
        /// <param name="restrictedToMinimumLevel">The restricted to minimum level.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// loggerConfiguration
        /// or
        /// configuration
        /// </exception>
        [Obsolete("This unspecific AI Telemetry Sink will be removed once Serilog Core reaches v2.0, please use either .ApplicationInsightsEvents(..) or .ApplicationInsightsTraces(..)")]
        public static LoggerConfiguration ApplicationInsights(
            this LoggerSinkConfiguration loggerConfiguration,
            TelemetryClient telemetryClient,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

            return loggerConfiguration.ApplicationInsightsEvents(telemetryClient, restrictedToMinimumLevel, formatProvider);
        }

        /// <summary>
        /// Creates the telemetry client from a provided <paramref name="instrumentationKey"/>.
        /// </summary>
        /// <param name="instrumentationKey">The instrumentation key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">instrumentationKey;Cannot be empty or null.</exception>
        private static TelemetryClient CreateTelemetryClientFromInstrumentationkey(string instrumentationKey = "")
        {
            var telemetryClient = new TelemetryClient();

            if (string.IsNullOrWhiteSpace(instrumentationKey) == false)
            {
                telemetryClient.InstrumentationKey = instrumentationKey;
            }

            return telemetryClient;
        }

        /// <summary>
        /// Creates the telemetry client from the provided <paramref name="telemetryConfiguration"/>.
        /// </summary>
        /// <param name="telemetryConfiguration">The telemetry configuration.</param>
        /// <returns>A new <see cref="TelemetryClient"/> if a <paramref name="telemetryConfiguration"/> was provided, otherwise the <see cref="TelemetryConfiguration.Active"/>.</returns>
        private static TelemetryClient CreateTelemetryClientFromConfiguration(TelemetryConfiguration telemetryConfiguration = null)
        {
            return new TelemetryClient(telemetryConfiguration ?? TelemetryConfiguration.Active);
        }
    }
}
