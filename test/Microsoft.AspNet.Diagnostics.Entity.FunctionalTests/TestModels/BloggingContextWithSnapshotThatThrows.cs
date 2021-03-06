// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using System;

namespace Microsoft.AspNet.Diagnostics.Entity.Tests
{
    public class BloggingContextWithSnapshotThatThrows : BloggingContext
    {
        public BloggingContextWithSnapshotThatThrows(IServiceProvider provider, DbContextOptions options)
            : base(provider, options)
        { }

        [ContextType(typeof(BloggingContextWithSnapshotThatThrows))]
        public class BloggingContextWithSnapshotThatThrowsModelSnapshot : ModelSnapshot
        {
            public override IModel Model
            {
                get
                {
                    throw new Exception("Welcome to the invalid snapshot!");
                }
            }
        }

        [ContextType(typeof(BloggingContextWithSnapshotThatThrows))]
        public class MigrationOne : Migration
        {
            public override string Id
            {
                get { return "111111111111111_MigrationOne"; }
            }

            public override string ProductVersion
            {
                get { return CurrentProductVersion; }
            }

            public override IModel Target
            {
                get { return new BloggingContextWithSnapshotThatThrowsModelSnapshot().Model; }
            }

            public override void Up(MigrationBuilder migrationBuilder)
            {
                throw new Exception("Welcome to the invalid migration!");
            }

            public override void Down(MigrationBuilder migrationBuilder)
            { }
        }
    }
}