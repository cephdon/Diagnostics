// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.Builder
{
    public static class RuntimeInfoExtensions
    {
        public static IApplicationBuilder UseRuntimeInfoPage(this IApplicationBuilder builder)
        {
            return UseRuntimeInfoPage(builder, new RuntimeInfoPageOptions());
        }

        public static IApplicationBuilder UseRuntimeInfoPage(this IApplicationBuilder builder, string path)
        {
            return UseRuntimeInfoPage(builder, new RuntimeInfoPageOptions() { Path = new PathString(path) });
        }

        public static IApplicationBuilder UseRuntimeInfoPage(
            [NotNull] this IApplicationBuilder builder,
            [NotNull] RuntimeInfoPageOptions options)
        {
            var libraryManager = builder.ApplicationServices.GetService(typeof(ILibraryManager)) as ILibraryManager;
            var htmlEncoder = builder.ApplicationServices.GetHtmlEncoder();
            return builder.UseMiddleware<RuntimeInfoMiddleware>(options, libraryManager, htmlEncoder);
        }
    }
}