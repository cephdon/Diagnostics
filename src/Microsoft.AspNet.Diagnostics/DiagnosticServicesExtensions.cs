// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Diagnostics;

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
            return services.AddWebEncoders();
        }
    }
}