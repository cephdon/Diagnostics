// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Diagnostics;
using Microsoft.Framework.ConfigurationModel;

namespace Microsoft.Framework.DependencyInjection
{
    public static class DiagnosticServicesExtensions
    {
        /// <summary>
        /// Adds diagnostic middleware services.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDiagnostics([NotNull] this IServiceCollection services)
        {
            return services.AddDiagnostics(configuration: null);
        }

        /// <summary>
        /// Adds diagnostic middleware services.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddDiagnostics([NotNull] this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddWebEncoders();
        }
    }
}