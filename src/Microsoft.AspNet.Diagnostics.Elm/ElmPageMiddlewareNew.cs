﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Diagnostics.Elm.Views;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using System.Linq;

namespace Microsoft.AspNet.Diagnostics.Elm
{
    /// <summary>
    /// Enables viewing logs captured by the <see cref="ElmCaptureMiddleware"/>.
    /// </summary>
    public class ElmPageMiddlewareNew
    {
        private readonly RequestDelegate _next;
        private readonly ElmOptions _options;
        private readonly ElmStoreNew _store;

        public ElmPageMiddlewareNew(RequestDelegate next, IOptions<ElmOptions> options, ElmStoreNew store)
        {
            _next = next;
            _options = options.Options;
            _store = store;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments(_options.Path))
            {
                await _next(context);
                return;
            }

            var t = await ParseParams(context);
            var options = t.Item1;
            var redirect = t.Item2;
            if (redirect)
            {
                return;
            }
            if (context.Request.Path == _options.Path)
            {
                RenderMainLogPage(options, context);
            }
            else
            {
                RenderDetailsPage(options, context);
            }
        }

        private async void RenderMainLogPage(ViewOptions options, HttpContext context)
        {
            var model = new LogPageModelNew()
            {
                Activities = _store.GetActivities(),
                Options = options,
                Path = _options.Path
            };
            var logPage = new LogPage(model);

            await logPage.ExecuteAsync(context);
        }

        private async void RenderDetailsPage(ViewOptions options, HttpContext context)
        {
            var parts = context.Request.Path.Value.Split('/');
            var id = Guid.Empty;
            if (!Guid.TryParse(parts[parts.Length - 1], out id))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid Id");
                return;
            }
            var model = new DetailsPageModel()
            {
                Activity = _store.GetActivities().Where(a => a.Id == id).FirstOrDefault(),
                Options = options
            };
            var detailsPage = new DetailsPage(model);
            await detailsPage.ExecuteAsync(context);
        }

        private async Task<Tuple<ViewOptions, bool>> ParseParams(HttpContext context)
        {
            var options = new ViewOptions()
            {
                MinLevel = LogLevel.Verbose,
                NamePrefix = string.Empty
            };
            var isRedirect = false;

            IFormCollection form = null;
            if (context.Request.HasFormContentType)
            {
                form = await context.Request.ReadFormAsync();
            }

            if (form != null && form.ContainsKey("clear"))
            {
                _store.Clear();
                context.Response.Redirect(context.Request.PathBase.Add(_options.Path).ToString());
                isRedirect = true;
            }
            else
            {
                if (context.Request.Query.ContainsKey("level"))
                {
                    var minLevel = options.MinLevel;
                    if (Enum.TryParse<LogLevel>(context.Request.Query["level"], out minLevel))
                    {
                        options.MinLevel = minLevel;
                    }
                }
                if (context.Request.Query.ContainsKey("name"))
                {
                    var namePrefix = context.Request.Query.GetValues("name")[0];
                    options.NamePrefix = namePrefix;
                }
            }
            return Tuple.Create(options, isRedirect);
        }
    }
}