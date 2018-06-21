// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.TestUtilities
{
    public abstract class QueryAsserterBase2
    {
        public virtual ISetExtractor SetExtractor { get; set; }
        public virtual IExpectedData ExpectedData { get; set; }

        #region AssertQuery

        public abstract Task AssertQuery<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
            where TItem1 : class;

        public abstract Task AssertQuery<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
            where TItem1 : class
            where TItem2 : class;

        public abstract Task AssertQuery<TItem1, TItem2, TItem3>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
            where TItem1 : class
            where TItem2 : class
            where TItem3 : class;

        #endregion

        #region Assert single result

        public abstract Task AssertFirst<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<dynamic>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<dynamic>> expectedQuery,
            Expression<Func<dynamic, bool>> actualFirstPredicate,
            Expression<Func<dynamic, bool>> expectedFirstPredicate,
            Action<object, object> asserter = null,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
            where TItem1 : class;

        public abstract Task AssertFirstOrDefault<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<dynamic>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<dynamic>> expectedQuery,
            Expression<Func<dynamic, bool>> actualFirstOrDefaultPredicate,
            Expression<Func<dynamic, bool>> expectedFirstOrDefaultPredicate,
            Action<object, object> asserter = null,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
            where TItem1 : class;

        #endregion














        #region AssertIncludeQuery

        public abstract Task<List<object>> AssertIncludeQuery<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
            where TItem1 : class;

        public abstract Task<List<object>> AssertIncludeQuery<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
            where TItem1 : class
            where TItem2 : class;

        #endregion
    }

    public enum ExecutionMode
    {
        SyncAsync,
        SyncOnly,
        AsyncOnly,
    };
}
