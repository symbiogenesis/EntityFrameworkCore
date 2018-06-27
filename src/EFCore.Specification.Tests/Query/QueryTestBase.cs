// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Query
{
    public abstract class QueryTestBase<TFixture> : IClassFixture<TFixture>
        where TFixture : class, IQueryFixtureBase, new()
    {
        protected QueryTestBase(TFixture fixture) => Fixture = fixture;

        protected TFixture Fixture { get; }

        #region AssertAny

        protected virtual Task AssertAny<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query)
            where TItem1 : class
            => AssertAny(isAsync, query, query);

        protected virtual Task AssertAny<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertAny(actualQuery, expectedQuery, isAsync);

        protected virtual Task AssertAny<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query)
            where TItem1 : class
            where TItem2 : class
            => AssertAny(isAsync, query, query);

        protected virtual Task AssertAny<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery)
            where TItem1 : class
            where TItem2 : class
            => Fixture.QueryAsserter2.AssertAny(actualQuery, expectedQuery, isAsync);

        protected virtual Task AssertAny<TItem1, TItem2, TItem3>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> query)
            where TItem1 : class
            where TItem2 : class
            where TItem3 : class
            => AssertAny(isAsync, query, query);

        protected virtual Task AssertAny<TItem1, TItem2, TItem3>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> expectedQuery)
            where TItem1 : class
            where TItem2 : class
            where TItem3 : class
            => Fixture.QueryAsserter2.AssertAny(actualQuery, expectedQuery, isAsync);

        #endregion

        #region AssertAnyWithPredicate

        protected virtual Task AssertAnyWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> query,
            Expression<Func<TPredicate, bool>> predicate)
            where TItem1 : class
            => AssertAnyWithPredicate(isAsync, query, query, predicate, predicate);

        protected virtual Task AssertAnyWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> expectedQuery,
            Expression<Func<TPredicate, bool>> actualPredicate,
            Expression<Func<TPredicate, bool>> expectedPredicate)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertAnyWithPredicate(actualQuery, expectedQuery, actualPredicate, expectedPredicate, isAsync);

        #endregion

        #region AssertAll

        protected virtual Task AssertAll<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> query,
            Expression<Func<TPredicate, bool>> predicate)
            where TItem1 : class
            => AssertAll(isAsync, query, query, predicate, predicate);

        protected virtual Task AssertAll<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> expectedQuery,
            Expression<Func<TPredicate, bool>> actualPredicate,
            Expression<Func<TPredicate, bool>> expectedPredicate)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertAll(actualQuery, expectedQuery, actualPredicate, expectedPredicate, isAsync);

        #endregion

        #region AssertFirst

        protected virtual Task AssertFirst<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertFirst(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertFirst<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertFirst(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertFirstWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> query,
            Expression<Func<TPredicate, bool>> predicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertFirstWithPredicate(isAsync, query, query, predicate, predicate, asserter, entryCount);

        protected virtual Task AssertFirstWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> expectedQuery,
            Expression<Func<TPredicate, bool>> actualPredicate,
            Expression<Func<TPredicate, bool>> expectedPredicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertFirstWithPredicate(actualQuery, expectedQuery, actualPredicate, expectedPredicate, asserter, entryCount, isAsync);

        #endregion

        #region AssertFirstOrDefault

        protected virtual Task AssertFirstOrDefault<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertFirstOrDefault(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertFirstOrDefault<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertFirstOrDefault(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertFirstOrDefault<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            => AssertFirstOrDefault(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertFirstOrDefault<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            => Fixture.QueryAsserter2.AssertFirstOrDefault(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertFirstOrDefault<TItem1, TItem2, TItem3>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            where TItem3 : class
            => AssertFirstOrDefault(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertFirstOrDefault<TItem1, TItem2, TItem3>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            where TItem3 : class
            => Fixture.QueryAsserter2.AssertFirstOrDefault(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        #endregion

        #region AssertFirstOrDefaultWithPredicate

        protected virtual Task AssertFirstOrDefaultWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> query,
            Expression<Func<TPredicate, bool>> predicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertFirstOrDefaultWithPredicate(isAsync, query, query, predicate, predicate, asserter, entryCount);

        protected virtual Task AssertFirstOrDefaultWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> expectedQuery,
            Expression<Func<TPredicate, bool>> actualPredicate,
            Expression<Func<TPredicate, bool>> expectedPredicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertFirstOrDefaultWithPredicate(actualQuery, expectedQuery, actualPredicate, expectedPredicate, asserter, entryCount, isAsync);

        #endregion

        #region AssertSingle

        protected virtual Task AssertSingle<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertSingle(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertSingle<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSingle(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertSingle<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            => AssertSingle(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertSingle<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            => Fixture.QueryAsserter2.AssertSingle(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        #endregion

        #region AssertSingleWithPredicate

        protected virtual Task AssertSingleWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> query,
            Expression<Func<TPredicate, bool>> predicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertSingleWithPredicate(isAsync, query, query, predicate, predicate, asserter, entryCount);

        protected virtual Task AssertSingleWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> expectedQuery,
            Expression<Func<TPredicate, bool>> actualPredicate,
            Expression<Func<TPredicate, bool>> expectedPredicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSingleWithPredicate(actualQuery, expectedQuery, actualPredicate, expectedPredicate, asserter, entryCount, isAsync);

        #endregion

        #region AssertSingleOrDefault

        protected virtual Task AssertSingleOrDefault<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertSingleOrDefault(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertSingleOrDefault<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSingleOrDefault(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertSingleOrDefaultWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> query,
            Expression<Func<TPredicate, bool>> predicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertSingleOrDefaultWithPredicate(isAsync, query, query, predicate, predicate, asserter, entryCount);

        protected virtual Task AssertSingleOrDefaultWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> expectedQuery,
            Expression<Func<TPredicate, bool>> actualPredicate,
            Expression<Func<TPredicate, bool>> expectedPredicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSingleOrDefaultWithPredicate(actualQuery, expectedQuery, actualPredicate, expectedPredicate, asserter, entryCount, isAsync);

        #endregion

        #region AssertLast

        protected virtual Task AssertLast<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertLast(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertLast<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertLast(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertLastWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> query,
            Expression<Func<TPredicate, bool>> predicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertLastWithPredicate(isAsync, query, query, predicate, predicate, asserter, entryCount);

        protected virtual Task AssertLastWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> expectedQuery,
            Expression<Func<TPredicate, bool>> actualPredicate,
            Expression<Func<TPredicate, bool>> expectedPredicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertLastWithPredicate(actualQuery, expectedQuery, actualPredicate, expectedPredicate, asserter, entryCount, isAsync);

        #endregion

        #region AssertLastOrDefault

        protected virtual Task AssertLastOrDefault<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertLastOrDefault(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertLastOrDefault<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertLastOrDefault(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertLastOrDefaultWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> query,
            Expression<Func<TPredicate, bool>> predicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertLastOrDefaultWithPredicate(isAsync, query, query, predicate, predicate, asserter, entryCount);

        protected virtual Task AssertLastOrDefaultWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> expectedQuery,
            Expression<Func<TPredicate, bool>> actualPredicate,
            Expression<Func<TPredicate, bool>> expectedPredicate,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertLastOrDefaultWithPredicate(actualQuery, expectedQuery, actualPredicate, expectedPredicate, asserter, entryCount, isAsync);

        #endregion

        #region AssertCount

        protected virtual Task AssertCount<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query)
            where TItem1 : class
            => AssertCount(isAsync, query, query);

        protected virtual Task AssertCount<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertCount(actualQuery, expectedQuery, isAsync);

        protected virtual Task AssertCount<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<bool>> query)
            where TItem1 : class
            => AssertCount(isAsync, query, query);

        protected virtual Task AssertCount<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<bool>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<bool>> expectedQuery)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertCount(actualQuery, expectedQuery, isAsync);

        protected virtual Task AssertCountWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> query,
            Expression<Func<TPredicate, bool>> predicate)
            where TItem1 : class
            => AssertCountWithPredicate(isAsync, query, query, predicate, predicate);

        protected virtual Task AssertCountWithPredicate<TItem1, TPredicate>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TPredicate>> expectedQuery,
            Expression<Func<TPredicate, bool>> actualPredicate,
            Expression<Func<TPredicate, bool>> expectedPredicate)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertCountWithPredicate(actualQuery, expectedQuery, actualPredicate, expectedPredicate, isAsync);

        protected virtual Task AssertCount<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query)
            where TItem1 : class
            where TItem2 : class
            => AssertCount(isAsync, query, query);

        protected virtual Task AssertCount<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery)
            where TItem1 : class
            where TItem2 : class
            => Fixture.QueryAsserter2.AssertCount(actualQuery, expectedQuery, isAsync);

        #endregion

        #region AssertLongCount

        protected virtual Task AssertLongCount<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query)
            where TItem1 : class
            => AssertLongCount(isAsync, query, query);

        protected virtual Task AssertLongCount<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertLongCount(actualQuery, expectedQuery, isAsync);

        protected virtual Task AssertLongCount<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query)
            where TItem1 : class
            where TItem2 : class
            => AssertLongCount(isAsync, query, query);

        protected virtual Task AssertLongCount<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery)
            where TItem1 : class
            where TItem2 : class
            => Fixture.QueryAsserter2.AssertLongCount(actualQuery, expectedQuery, isAsync);

        #endregion

        #region AssertMin

        protected virtual Task AssertMin<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMin(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertMin<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMin(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertMin<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMin(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertMin<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMin(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertMin<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<long>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMin(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertMin<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<long>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<long>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMin(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertMinWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, int>> selector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMinWithSelector(isAsync, query, query, selector, selector, asserter, entryCount);

        protected virtual Task AssertMinWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, int>> actualSelector,
            Expression<Func<TSelector, int>> expectedSelector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMinWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, entryCount, isAsync);

        protected virtual Task AssertMinWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, int?>> selector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMinWithSelector(isAsync, query, query, selector, selector, asserter, entryCount);

        protected virtual Task AssertMinWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, int?>> actualSelector,
            Expression<Func<TSelector, int?>> expectedSelector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMinWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, entryCount, isAsync);

        protected virtual Task AssertMinWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, decimal>> selector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMinWithSelector(isAsync, query, query, selector, selector, asserter, entryCount);

        protected virtual Task AssertMinWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, decimal>> actualSelector,
            Expression<Func<TSelector, decimal>> expectedSelector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMinWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, entryCount, isAsync);

        #endregion

        #region AssertMax

        protected virtual Task AssertMax<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMax(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertMax<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMax(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertMax<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMax(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertMax<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMax(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertMax<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<long>> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMax(isAsync, query, query, asserter, entryCount);

        protected virtual Task AssertMax<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<long>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<long>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMax(actualQuery, expectedQuery, asserter, entryCount, isAsync);

        protected virtual Task AssertMaxWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, int>> selector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMaxWithSelector(isAsync, query, query, selector, selector, asserter, entryCount);

        protected virtual Task AssertMaxWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, int>> actualSelector,
            Expression<Func<TSelector, int>> expectedSelector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMaxWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, entryCount, isAsync);

        protected virtual Task AssertMaxWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, int?>> selector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMaxWithSelector(isAsync, query, query, selector, selector, asserter, entryCount);

        protected virtual Task AssertMaxWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, int?>> actualSelector,
            Expression<Func<TSelector, int?>> expectedSelector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMaxWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, entryCount, isAsync);

        protected virtual Task AssertMaxWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, decimal>> selector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertMaxWithSelector(isAsync, query, query, selector, selector, asserter, entryCount);

        protected virtual Task AssertMaxWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, decimal>> actualSelector,
            Expression<Func<TSelector, decimal>> expectedSelector,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertMaxWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, entryCount, isAsync);

        #endregion

        #region AssertSum

        protected virtual Task AssertSum<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int>> query,
            Action<object, object> asserter = null)
            where TItem1 : class
            => AssertSum(isAsync, query, query, asserter);

        protected virtual Task AssertSum<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int>> expectedQuery,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSum(actualQuery, expectedQuery, asserter, isAsync);

        protected virtual Task AssertSum<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int?>> query,
            Action<object, object> asserter = null)
            where TItem1 : class
            => AssertSum(isAsync, query, query, asserter);

        protected virtual Task AssertSum<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int?>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int?>> expectedQuery,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSum(actualQuery, expectedQuery, asserter, isAsync);

        #endregion

        #region AssertSumWithSelector

        protected virtual Task AssertSumWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, int>> selector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSumWithSelector(query, query, selector, selector, asserter, isAsync);

        protected virtual Task AssertSumWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, int>> actualSelector,
            Expression<Func<TSelector, int>> expectedSelector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSumWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, isAsync);

        protected virtual Task AssertSumWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, int?>> selector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSumWithSelector(query, query, selector, selector, asserter, isAsync);

        protected virtual Task AssertSumWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, int?>> actualSelector,
            Expression<Func<TSelector, int?>> expectedSelector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSumWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, isAsync);

        protected virtual Task AssertSumWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, decimal>> selector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSumWithSelector(query, query, selector, selector, asserter, isAsync);

        protected virtual Task AssertSumWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, decimal>> actualSelector,
            Expression<Func<TSelector, decimal>> expectedSelector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSumWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, isAsync);

        protected virtual Task AssertSumWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, float>> selector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSumWithSelector(query, query, selector, selector, asserter, isAsync);

        protected virtual Task AssertSumWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, float>> actualSelector,
            Expression<Func<TSelector, float>> expectedSelector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertSumWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, isAsync);

        protected virtual Task AssertSumWithSelector<TItem1, TItem2, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, int>> selector,
            Action<object, object> asserter = null)
            where TItem1 : class
            where TItem2 : class
            => Fixture.QueryAsserter2.AssertSumWithSelector(query, query, selector, selector, asserter, isAsync);

        protected virtual Task AssertSumWithSelector<TItem1, TItem2, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, int>> actualSelector,
            Expression<Func<TSelector, int>> expectedSelector,
            Action<object, object> asserter = null)
            where TItem1 : class
            where TItem2 : class
            => Fixture.QueryAsserter2.AssertSumWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, isAsync);

        #endregion

        #region AssertAverage

        protected virtual Task AssertAverage<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int>> query,
            Action<object, object> asserter = null)
            where TItem1 : class
            => AssertAverage(isAsync, query, query);

        protected virtual Task AssertAverage<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int>> expectedQuery,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertAverage(actualQuery, expectedQuery, asserter, isAsync);

        protected virtual Task AssertAverage<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<long>> query,
            Action<object, object> asserter = null)
            where TItem1 : class
            => AssertAverage(isAsync, query, query);

        protected virtual Task AssertAverage<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<long>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<long>> expectedQuery,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertAverage(actualQuery, expectedQuery, asserter, isAsync);

        protected virtual Task AssertAverageWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, int>> selector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => AssertAverageWithSelector(isAsync, query, query, selector, selector, asserter);

        protected virtual Task AssertAverageWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, int>> actualSelector,
            Expression<Func<TSelector, int>> expectedSelector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertAverageWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, isAsync);

        protected virtual Task AssertAverageWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, int?>> selector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => AssertAverageWithSelector(isAsync, query, query, selector, selector, asserter);

        protected virtual Task AssertAverageWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, int?>> actualSelector,
            Expression<Func<TSelector, int?>> expectedSelector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertAverageWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, isAsync);

        protected virtual Task AssertAverageWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, decimal>> selector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => AssertAverageWithSelector(isAsync, query, query, selector, selector, asserter);

        protected virtual Task AssertAverageWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, decimal>> actualSelector,
            Expression<Func<TSelector, decimal>> expectedSelector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertAverageWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, isAsync);

        protected virtual Task AssertAverageWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> query,
            Expression<Func<TSelector, float>> selector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => AssertAverageWithSelector(isAsync, query, query, selector, selector, asserter);

        protected virtual Task AssertAverageWithSelector<TItem1, TSelector>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TSelector>> expectedQuery,
            Expression<Func<TSelector, float>> actualSelector,
            Expression<Func<TSelector, float>> expectedSelector,
            Action<object, object> asserter = null)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertAverageWithSelector(actualQuery, expectedQuery, actualSelector, expectedSelector, asserter, isAsync);

        #endregion





















        #region AssertQuery

        public Task AssertQuery<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertQuery(query, query, elementSorter, elementAsserter, assertOrder, entryCount, isAsync);

        public Task AssertQuery<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertQuery(actualQuery, expectedQuery, elementSorter, elementAsserter, assertOrder, entryCount, isAsync);

        public Task AssertQuery<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            => Fixture.QueryAsserter2.AssertQuery(query, query, elementSorter, elementAsserter, assertOrder, entryCount, isAsync);

        public Task AssertQuery<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            => Fixture.QueryAsserter2.AssertQuery(actualQuery, expectedQuery, elementSorter, elementAsserter, assertOrder, entryCount, isAsync);

        public Task AssertQuery<TItem1, TItem2, TItem3>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> query,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            where TItem3 : class
            => AssertQuery(isAsync, query, query, elementSorter, elementAsserter, assertOrder, entryCount);

        public Task AssertQuery<TItem1, TItem2, TItem3>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            where TItem3 : class
            => Fixture.QueryAsserter2.AssertQuery(actualQuery, expectedQuery, elementSorter, elementAsserter, assertOrder, entryCount, isAsync);

        #endregion

        #region AssertQueryScalar

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<double>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar<TItem1, int>(isAsync, actualQuery, expectedQuery, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<uint>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<uint>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<uint>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar<TItem1, uint>(isAsync, actualQuery, expectedQuery, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<long>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<short>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<bool>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<bool>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<bool>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar<TItem1, bool>(isAsync, actualQuery, expectedQuery, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<DateTimeOffset>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TimeSpan>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1, TResult>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TResult>> query,
            bool assertOrder = false)
            where TItem1 : class
            where TResult : struct
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1, TResult>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TResult>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TResult>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            where TResult : struct
            => Fixture.QueryAsserter2.AssertQueryScalar(actualQuery, expectedQuery, assertOrder, isAsync);

        public Task AssertQueryScalar<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int>> query,
            bool assertOrder = false)
            where TItem1 : class
            where TItem2 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            where TItem2 : class
            => AssertQueryScalar<TItem1, TItem2, int>(isAsync, actualQuery, expectedQuery, assertOrder);

        public Task AssertQueryScalar<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<bool>> query,
            bool assertOrder = false)
            where TItem1 : class
            where TItem2 : class
            => AssertQueryScalar<TItem1, TItem2, bool>(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<bool>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<bool>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            where TItem2 : class
            => AssertQueryScalar<TItem1, TItem2, bool>(isAsync, actualQuery, expectedQuery, assertOrder);

        public Task AssertQueryScalar<TItem1, TItem2, TResult>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            where TItem2 : class
            where TResult : struct
            => Fixture.QueryAsserter2.AssertQueryScalar(actualQuery, expectedQuery, assertOrder, isAsync);

        public Task AssertQueryScalar<TItem1, TItem2, TItem3>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<int>> query,
            bool assertOrder = false)
            where TItem1 : class
            where TItem2 : class
            where TItem3 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1, TItem2, TItem3, TResult>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<TResult>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<TResult>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            where TItem2 : class
            where TItem3 : class
            where TResult : struct
            => Fixture.QueryAsserter2.AssertQueryScalar(actualQuery, expectedQuery, assertOrder, isAsync);

        #endregion

        #region AssertQueryScalar - nullable

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int?>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<int?>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int?>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar<TItem1, int>(isAsync, actualQuery, expectedQuery, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<bool?>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TimeSpan?>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<DateTime?>> query,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<bool?>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<bool?>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            => AssertQueryScalar<TItem1, bool>(isAsync, actualQuery, expectedQuery, assertOrder);

        public Task AssertQueryScalar<TItem1, TResult>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TResult?>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TResult?>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            where TResult : struct
            => Fixture.QueryAsserter2.AssertQueryScalar(actualQuery, expectedQuery, assertOrder, isAsync);

        public Task AssertQueryScalar<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int?>> query,
            bool assertOrder = false)
            where TItem1 : class
            where TItem2 : class
            => AssertQueryScalar(isAsync, query, query, assertOrder);

        public Task AssertQueryScalar<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int?>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int?>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            where TItem2 : class
            => AssertQueryScalar<TItem1, TItem2, int>(isAsync, actualQuery, expectedQuery, assertOrder);

        public Task AssertQueryScalar<TItem1, TItem2, TResult>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult?>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult?>> expectedQuery,
            bool assertOrder = false)
            where TItem1 : class
            where TItem2 : class
            where TResult : struct
            => Fixture.QueryAsserter2.AssertQueryScalar(actualQuery, expectedQuery, assertOrder, isAsync);

        #endregion

        #region AssertIncludeQuery

        public Task<List<object>> AssertIncludeQuery<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0)
            where TItem1 : class
            => AssertIncludeQuery(isAsync, query, query, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount);

        public Task<List<object>> AssertIncludeQuery<TItem1>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter2.AssertIncludeQuery(actualQuery, expectedQuery, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount, isAsync);

        public Task<List<object>> AssertIncludeQuery<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            => AssertIncludeQuery(isAsync, query, query, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount);

        public Task<List<object>> AssertIncludeQuery<TItem1, TItem2>(
            bool isAsync,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0)
            where TItem1 : class
            where TItem2 : class
            => Fixture.QueryAsserter2.AssertIncludeQuery(actualQuery, expectedQuery, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount, isAsync);

        #endregion































        #region AssertSingleResult

        protected void AssertSingleResult<TItem1>(
            Func<IQueryable<TItem1>, object> query,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => AssertSingleResult(query, query, asserter, entryCount);

        protected void AssertSingleResult<TItem1>(
            Func<IQueryable<TItem1>, object> actualQuery,
            Func<IQueryable<TItem1>, object> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0)
            where TItem1 : class
            => Fixture.QueryAsserter.AssertSingleResult(actualQuery, expectedQuery, asserter, entryCount);

        //public void AssertSingleResult<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, object> query,
        //    Action<object, object> asserter = null,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => AssertSingleResult(query, query, asserter, entryCount);

        //public void AssertSingleResult<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, object> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, object> expectedQuery,
        //    Action<object, object> asserter = null,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => Fixture.QueryAsserter.AssertSingleResult(actualQuery, expectedQuery, asserter, entryCount);

        //public void AssertSingleResult<TItem1, TItem2, TItem3>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, object> query,
        //    Action<object, object> asserter = null,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    where TItem2 : class
        //    where TItem3 : class
        //    => AssertSingleResult(query, query, asserter, entryCount);

        //public void AssertSingleResult<TItem1, TItem2, TItem3>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, object> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, object> expectedQuery,
        //    Action<object, object> asserter = null,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    where TItem2 : class
        //    where TItem3 : class
        //    => Fixture.QueryAsserter.AssertSingleResult(actualQuery, expectedQuery, asserter, entryCount);

        #endregion

        #region AssertQuery

        //public Task AssertQueryAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<object>> query,
        //    Func<dynamic, object> elementSorter = null,
        //    Action<dynamic, dynamic> elementAsserter = null,
        //    bool assertOrder = false,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    => Fixture.QueryAsserter.AssertQueryAsync(query, query, elementSorter, elementAsserter, assertOrder, entryCount);

        //public Task AssertQueryAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
        //    Func<dynamic, object> elementSorter = null,
        //    Action<dynamic, dynamic> elementAsserter = null,
        //    bool assertOrder = false,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    => Fixture.QueryAsserter.AssertQueryAsync(actualQuery, expectedQuery, elementSorter, elementAsserter, assertOrder, entryCount);

        //public Task AssertQueryAsync<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query,
        //    Func<dynamic, object> elementSorter = null,
        //    Action<dynamic, dynamic> elementAsserter = null,
        //    bool assertOrder = false,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => Fixture.QueryAsserter.AssertQueryAsync(query, query, elementSorter, elementAsserter, assertOrder, entryCount);

        //public Task AssertQueryAsync<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
        //    Func<dynamic, object> elementSorter = null,
        //    Action<dynamic, dynamic> elementAsserter = null,
        //    bool assertOrder = false,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => Fixture.QueryAsserter.AssertQueryAsync(actualQuery, expectedQuery, elementSorter, elementAsserter, assertOrder, entryCount);

        //public Task AssertQueryAsync<TItem1, TItem2, TItem3>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> query,
        //    Func<dynamic, object> elementSorter = null,
        //    Action<dynamic, dynamic> elementAsserter = null,
        //    bool assertOrder = false,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    where TItem2 : class
        //    where TItem3 : class
        //    => AssertQueryAsync(query, query, elementSorter, elementAsserter, assertOrder, entryCount);

        //public Task AssertQueryAsync<TItem1, TItem2, TItem3>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> expectedQuery,
        //    Func<dynamic, object> elementSorter = null,
        //    Action<dynamic, dynamic> elementAsserter = null,
        //    bool assertOrder = false,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    where TItem2 : class
        //    where TItem3 : class
        //    => Fixture.QueryAsserter.AssertQueryAsync(actualQuery, expectedQuery, elementSorter, elementAsserter, assertOrder, entryCount);

        #endregion

        #region AssertQueryScalar

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<int>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<double>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<int>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<int>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync<TItem1, int>(actualQuery, expectedQuery, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<uint>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<uint>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<uint>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync<TItem1, uint>(actualQuery, expectedQuery, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<long>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<short>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<bool>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<bool>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<bool>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync<TItem1, bool>(actualQuery, expectedQuery, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<DateTimeOffset>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<TimeSpan>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TResult>(
        //    Func<IQueryable<TItem1>, IQueryable<TResult>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TResult : struct
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TResult>(
        //    Func<IQueryable<TItem1>, IQueryable<TResult>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TResult>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TResult : struct
        //    => Fixture.QueryAsserter.AssertQueryScalarAsync(actualQuery, expectedQuery, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => AssertQueryScalarAsync<TItem1, TItem2, int>(actualQuery, expectedQuery, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<bool>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => AssertQueryScalarAsync<TItem1, TItem2, bool>(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<bool>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<bool>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => AssertQueryScalarAsync<TItem1, TItem2, bool>(actualQuery, expectedQuery, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TItem2, TResult>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TItem2 : class
        //    where TResult : struct
        //    => Fixture.QueryAsserter.AssertQueryScalarAsync(actualQuery, expectedQuery, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TItem2, TItem3>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<int>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TItem2 : class
        //    where TItem3 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TItem2, TItem3, TResult>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<TResult>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<TResult>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TItem2 : class
        //    where TItem3 : class
        //    where TResult : struct
        //    => Fixture.QueryAsserter.AssertQueryScalarAsync(actualQuery, expectedQuery, assertOrder);

        #endregion

        #region AssertQueryScalar - nullable

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<int?>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<int?>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<int?>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync<TItem1, int>(actualQuery, expectedQuery, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<bool?>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<TimeSpan?>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<DateTime?>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<bool?>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<bool?>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    => AssertQueryScalarAsync<TItem1, bool>(actualQuery, expectedQuery, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TResult>(
        //    Func<IQueryable<TItem1>, IQueryable<TResult?>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TResult?>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TResult : struct
        //    => Fixture.QueryAsserter.AssertQueryScalarAsync(actualQuery, expectedQuery, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int?>> query,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => AssertQueryScalarAsync(query, query, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int?>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int?>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => AssertQueryScalarAsync<TItem1, TItem2, int>(actualQuery, expectedQuery, assertOrder);

        //public Task AssertQueryScalarAsync<TItem1, TItem2, TResult>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult?>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult?>> expectedQuery,
        //    bool assertOrder = false)
        //    where TItem1 : class
        //    where TItem2 : class
        //    where TResult : struct
        //    => Fixture.QueryAsserter.AssertQueryScalarAsync(actualQuery, expectedQuery, assertOrder);

        #endregion

        #region AssertIncludeQuery

        //public Task<List<object>> AssertIncludeQueryAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<object>> query,
        //    List<IExpectedInclude> expectedIncludes,
        //    Func<dynamic, object> elementSorter = null,
        //    List<Func<dynamic, object>> clientProjections = null,
        //    bool assertOrder = false,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    => AssertIncludeQueryAsync(query, query, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount);

        //public Task<List<object>> AssertIncludeQueryAsync<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
        //    List<IExpectedInclude> expectedIncludes,
        //    Func<dynamic, object> elementSorter = null,
        //    List<Func<dynamic, object>> clientProjections = null,
        //    bool assertOrder = false,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    => Fixture.QueryAsserter.AssertIncludeQueryAsync(actualQuery, expectedQuery, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount);

        //public Task<List<object>> AssertIncludeQueryAsync<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query,
        //    List<IExpectedInclude> expectedIncludes,
        //    Func<dynamic, object> elementSorter = null,
        //    List<Func<dynamic, object>> clientProjections = null,
        //    bool assertOrder = false,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => AssertIncludeQueryAsync(query, query, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount);

        //public Task<List<object>> AssertIncludeQueryAsync<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
        //    List<IExpectedInclude> expectedIncludes,
        //    Func<dynamic, object> elementSorter = null,
        //    List<Func<dynamic, object>> clientProjections = null,
        //    bool assertOrder = false,
        //    int entryCount = 0)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => Fixture.QueryAsserter.AssertIncludeQueryAsync(actualQuery, expectedQuery, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount);

        #endregion

        #region Helpers - Sorters

        public static Func<dynamic, dynamic> GroupingSorter<TKey, TElement>()
            => e => ((IGrouping<TKey, TElement>)e).Key + " " + CollectionSorter<TElement>()(e);

        public static Func<dynamic, dynamic> CollectionSorter<TElement>()
            => e => ((IEnumerable<TElement>)e).Count();

        #endregion

        #region Helpers - Asserters

        public static Action<dynamic, dynamic> GroupingAsserter<TKey, TElement>(Func<TElement, object> elementSorter = null, Action<TElement, TElement> elementAsserter = null)
        {
            return (e, a) =>
            {
                Assert.Equal(((IGrouping<TKey, TElement>)e).Key, ((IGrouping<TKey, TElement>)a).Key);
                CollectionAsserter(elementSorter, elementAsserter)(e, a);
            };
        }

        public static Action<dynamic, dynamic> CollectionAsserter<TElement>(Func<TElement, object> elementSorter = null, Action<TElement, TElement> elementAsserter = null)
        {
            return (e, a) =>
            {
                var actual = elementSorter != null
                    ? ((IEnumerable<TElement>)a).OrderBy(elementSorter).ToList()
                    : ((IEnumerable<TElement>)a).ToList();

                var expected = elementSorter != null
                    ? ((IEnumerable<TElement>)e).OrderBy(elementSorter).ToList()
                    : ((IEnumerable<TElement>)e).ToList();

                Assert.Equal(expected.Count, actual.Count);
                elementAsserter = elementAsserter ?? Assert.Equal;

                for (var i = 0; i < expected.Count; i++)
                {
                    elementAsserter(expected[i], actual[i]);
                }
            };
        }

        #endregion

        #region Helpers - Maybe

        public static TResult Maybe<TResult>(object caller, Func<TResult> expression)
            where TResult : class
        {
            return caller == null ? null : expression();
        }

        public static TResult? MaybeScalar<TResult>(object caller, Func<TResult?> expression)
            where TResult : struct
        {
            return caller == null ? null : expression();
        }

        #endregion
    }
}
