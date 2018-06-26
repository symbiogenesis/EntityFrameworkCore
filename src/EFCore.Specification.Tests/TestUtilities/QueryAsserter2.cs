// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.EntityFrameworkCore.TestUtilities
{
    public class QueryAsserter2<TContext> : QueryAsserterBase2
        where TContext : DbContext
    {
        private readonly Func<TContext> _contextCreator;
        private readonly Dictionary<Type, Func<dynamic, object>> _entitySorters;
        private readonly Dictionary<Type, Action<dynamic, dynamic>> _entityAsserters;
        private readonly IncludeQueryResultAsserter _includeResultAsserter;

        public QueryAsserter2(
            Func<TContext> contextCreator,
            IExpectedData expectedData,
            Dictionary<Type, Func<dynamic, object>> entitySorters,
            Dictionary<Type, Action<dynamic, dynamic>> entityAsserters)
        {
            _contextCreator = contextCreator;
            ExpectedData = expectedData;

            _entitySorters = entitySorters ?? new Dictionary<Type, Func<dynamic, object>>();
            _entityAsserters = entityAsserters ?? new Dictionary<Type, Action<dynamic, dynamic>>();

            SetExtractor = new DefaultSetExtractor();
            _includeResultAsserter = new IncludeQueryResultAsserter(_entitySorters, _entityAsserters);
        }

        #region AssertQuery

        public override async Task AssertQuery<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).ToArrayAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).ToArray();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).ToArray();

                var firstNonNullableElement = expected.FirstOrDefault(e => e != null);
                if (firstNonNullableElement != null)
                {
                    if (!assertOrder
                        && elementSorter == null)
                    {
                        _entitySorters.TryGetValue(firstNonNullableElement.GetType(), out elementSorter);
                    }

                    if (elementAsserter == null)
                    {
                        _entityAsserters.TryGetValue(firstNonNullableElement.GetType(), out elementAsserter);
                    }
                }

                TestHelpers.AssertResults(
                    expected,
                    actual,
                    elementSorter,
                    elementAsserter,
                    assertOrder);

                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());
            }
        }

        public override async Task AssertQuery<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context)).ToArrayAsync()
                    : actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context)).ToArray();

                var expected = expectedQuery(
                    ExpectedData.Set<TItem1>(),
                    ExpectedData.Set<TItem2>()).ToArray();

                var firstNonNullableElement = expected.FirstOrDefault(e => e != null);
                if (firstNonNullableElement != null)
                {
                    if (!assertOrder
                        && elementSorter == null)
                    {
                        _entitySorters.TryGetValue(firstNonNullableElement.GetType(), out elementSorter);
                    }

                    if (elementAsserter == null)
                    {
                        _entityAsserters.TryGetValue(firstNonNullableElement.GetType(), out elementAsserter);
                    }
                }

                TestHelpers.AssertResults(
                    expected,
                    actual,
                    elementSorter,
                    elementAsserter,
                    assertOrder);

                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());
            }
        }

        public override async Task AssertQuery<TItem1, TItem2, TItem3>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context),
                        SetExtractor.Set<TItem3>(context)).ToArrayAsync()
                    : actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context),
                        SetExtractor.Set<TItem3>(context)).ToArray();

                var expected = expectedQuery(
                    ExpectedData.Set<TItem1>(),
                    ExpectedData.Set<TItem2>(),
                    ExpectedData.Set<TItem3>()).ToArray();

                var firstNonNullableElement = expected.FirstOrDefault(e => e != null);
                if (firstNonNullableElement != null)
                {
                    if (!assertOrder
                        && elementSorter == null)
                    {
                        _entitySorters.TryGetValue(firstNonNullableElement.GetType(), out elementSorter);
                    }

                    if (elementAsserter == null)
                    {
                        _entityAsserters.TryGetValue(firstNonNullableElement.GetType(), out elementAsserter);
                    }
                }

                TestHelpers.AssertResults(
                    expected,
                    actual,
                    elementSorter,
                    elementAsserter,
                    assertOrder);

                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());
            }
        }

        #endregion

        #region AssertQueryScalar

        // one argument

        public virtual Task AssertQueryScalar<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<int>> query,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            => AssertQueryScalar(query, query, assertOrder, isAsync);

        public virtual Task AssertQueryScalar<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<int>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int>> expectedQuery,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            => AssertQueryScalar<TItem1, int>(actualQuery, expectedQuery, assertOrder, isAsync);

        public virtual Task AssertQueryScalar<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<long>> query,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            => AssertQueryScalar(query, query, assertOrder, isAsync);

        public virtual Task AssertQueryScalar<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<short>> query,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            => AssertQueryScalar(query, query, assertOrder, isAsync);

        public virtual Task AssertQueryScalarAsync<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<bool>> query,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            => AssertQueryScalar(query, query, assertOrder, isAsync);

        public virtual Task AssertQueryScalarAsync<TItem1, TResult>(
            Func<IQueryable<TItem1>, IQueryable<TResult>> query,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            where TResult : struct
            => AssertQueryScalar(query, query, assertOrder, isAsync);

        public override async Task AssertQueryScalar<TItem1, TResult>(
            Func<IQueryable<TItem1>, IQueryable<TResult>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TResult>> expectedQuery,
            bool assertOrder = false,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).ToArrayAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).ToArray();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).ToArray();

                TestHelpers.AssertResults(
                    expected,
                    actual,
                    e => e,
                    Assert.Equal,
                    assertOrder);
            }
        }

        // two arguments

        public virtual Task AssertQueryScalar<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int>> query,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            where TItem2 : class
            => AssertQueryScalar(query, query, assertOrder, isAsync);

        public virtual Task AssertQueryScalar<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int>> expectedQuery,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            where TItem2 : class
            => AssertQueryScalar<TItem1, TItem2, int>(actualQuery, expectedQuery, assertOrder, isAsync);

        public override async Task AssertQueryScalar<TItem1, TItem2, TResult>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult>> expectedQuery,
            bool assertOrder = false,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context)).ToArrayAsync()
                    : actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context)).ToArray();

                var expected = expectedQuery(
                    ExpectedData.Set<TItem1>(),
                    ExpectedData.Set<TItem2>()).ToArray();

                TestHelpers.AssertResults(
                    expected,
                    actual,
                    e => e,
                    Assert.Equal,
                    assertOrder);
            }
        }

        // three arguments

        public virtual Task AssertQueryScalar<TItem1, TItem2, TItem3>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<int>> query,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            where TItem2 : class
            where TItem3 : class
            => AssertQueryScalar(query, query, assertOrder, isAsync);

        public override async Task AssertQueryScalar<TItem1, TItem2, TItem3, TResult>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<TResult>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<TResult>> expectedQuery,
            bool assertOrder = false,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context),
                        SetExtractor.Set<TItem3>(context)).ToArrayAsync()
                    : actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context),
                        SetExtractor.Set<TItem3>(context)).ToArray();

                var expected = expectedQuery(
                    ExpectedData.Set<TItem1>(),
                    ExpectedData.Set<TItem2>(),
                    ExpectedData.Set<TItem3>()).ToArray();

                TestHelpers.AssertResults(
                    expected,
                    actual,
                    e => e,
                    Assert.Equal,
                    assertOrder);
            }
        }







































        #endregion

        #region AssertQueryNullableScalar

        // one argument

        public virtual Task AssertQueryScalar<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<int?>> query,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            => AssertQueryScalar(query, query, assertOrder, isAsync);

        public virtual Task AssertQueryScalar<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<int?>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int?>> expectedQuery,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            => AssertQueryScalar<TItem1, int>(actualQuery, expectedQuery, assertOrder, isAsync);

        public override async Task AssertQueryScalar<TItem1, TResult>(
            Func<IQueryable<TItem1>, IQueryable<TResult?>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TResult?>> expectedQuery,
            bool assertOrder = false,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).ToArrayAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).ToArray();
                var expected = expectedQuery(ExpectedData.Set<TItem1>()).ToArray();
                TestHelpers.AssertResultsNullable(
                    expected,
                    actual,
                    e => e,
                    Assert.Equal,
                    assertOrder);
            }
        }

        // two arguments

        public virtual Task AssertQueryScalar<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int?>> query,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            where TItem2 : class
            => AssertQueryScalar(query, query, assertOrder, isAsync);

        public virtual Task AssertQueryScalar<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int?>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<int?>> expectedQuery,
            bool assertOrder = false,
            bool isAsync = false)
            where TItem1 : class
            where TItem2 : class
            => AssertQueryScalar<TItem1, TItem2, int>(actualQuery, expectedQuery, assertOrder, isAsync);

        public override async Task AssertQueryScalar<TItem1, TItem2, TResult>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult?>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TResult?>> expectedQuery,
            bool assertOrder = false,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context)).ToArrayAsync()
                    : actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context)).ToArray();

                var expected = expectedQuery(
                    ExpectedData.Set<TItem1>(),
                    ExpectedData.Set<TItem2>()).ToArray();
                TestHelpers.AssertResultsNullable(
                    expected,
                    actual,
                    e => e,
                    Assert.Equal,
                    assertOrder);
            }
        }

        #endregion

        #region AssertFirst

        public override async Task AssertFirst<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).FirstAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).First();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).First();

                if (asserter == null
                    && expected != null)
                {
                    _entityAsserters.TryGetValue(expected.GetType(), out asserter);
                }

                if (asserter != null)
                {
                    asserter(expected, actual);
                }
                else
                {
                    Assert.Equal(expected, actual);
                }

                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());
            }
        }

        public override async Task AssertFirst<TItem1, TResult>(
            Func<IQueryable<TItem1>, IQueryable<TResult>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TResult>> expectedQuery,
            Expression<Func<TResult, bool>> actualFirstPredicate,
            Expression<Func<TResult, bool>> expectedFirstPredicate,
            Action<object, object> asserter = null,
            int entryCount = 0,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? actualFirstPredicate != null
                        ? await actualQuery(SetExtractor.Set<TItem1>(context)).FirstAsync(actualFirstPredicate)
                        : await actualQuery(SetExtractor.Set<TItem1>(context)).FirstAsync()
                    : actualFirstPredicate != null
                        ? actualQuery(SetExtractor.Set<TItem1>(context)).First(actualFirstPredicate)
                        : actualQuery(SetExtractor.Set<TItem1>(context)).First();

                var expected = expectedFirstPredicate != null
                    ? expectedQuery(ExpectedData.Set<TItem1>()).First(expectedFirstPredicate)
                    : expectedQuery(ExpectedData.Set<TItem1>()).First();

                if (asserter == null
                    && expected != null)
                {
                    _entityAsserters.TryGetValue(expected.GetType(), out asserter);
                }

                if (asserter != null)
                {
                    asserter(expected, actual);
                }
                else
                {
                    Assert.Equal(expected, actual);
                }

                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());
            }
        }

        #endregion

        #region AssertCount

        public override async Task AssertCount<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).CountAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).Count();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).Count();

                Assert.Equal(expected, actual);
                Assert.Equal(0, context.ChangeTracker.Entries().Count());
            }
        }

        public override async Task AssertCount<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context), SetExtractor.Set<TItem2>(context)).CountAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context), SetExtractor.Set<TItem2>(context)).Count();

                var expected = expectedQuery(ExpectedData.Set<TItem1>(), ExpectedData.Set<TItem2>()).Count();

                Assert.Equal(expected, actual);
            }
        }

        #endregion

        #region AssertLongCount

        public override async Task AssertLongCount<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).LongCountAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).LongCount();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).LongCount();

                Assert.Equal(expected, actual);
                Assert.Equal(0, context.ChangeTracker.Entries().Count());
            }
        }

        #endregion

        #region AssertMin

        public override async Task AssertMin<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).MinAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).Min();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).Min();

                if (asserter == null
                    && expected != null)
                {
                    _entityAsserters.TryGetValue(expected.GetType(), out asserter);
                }

                if (asserter != null)
                {
                    asserter(expected, actual);
                }
                else
                {
                    Assert.Equal(expected, actual);
                }

                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());
            }
        }

        public override async Task AssertMin<TItem1, TResult>(
            Func<IQueryable<TItem1>, IQueryable<TResult>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TResult>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).MinAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).Min();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).Min();

                if (asserter == null
                    && expected != null)
                {
                    _entityAsserters.TryGetValue(expected.GetType(), out asserter);
                }

                if (asserter != null)
                {
                    asserter(expected, actual);
                }
                else
                {
                    Assert.Equal(expected, actual);
                }

                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());
            }
        }

        #endregion

        #region AssertMax

        public override async Task AssertMax<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).MaxAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).Max();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).Max();

                if (asserter == null
                    && expected != null)
                {
                    _entityAsserters.TryGetValue(expected.GetType(), out asserter);
                }

                if (asserter != null)
                {
                    asserter(expected, actual);
                }
                else
                {
                    Assert.Equal(expected, actual);
                }

                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());
            }
        }

        public override async Task AssertMax<TItem1, TResult>(
            Func<IQueryable<TItem1>, IQueryable<TResult>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TResult>> expectedQuery,
            Action<object, object> asserter = null,
            int entryCount = 0,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).MaxAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).Max();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).Max();

                if (asserter == null
                    && expected != null)
                {
                    _entityAsserters.TryGetValue(expected.GetType(), out asserter);
                }

                if (asserter != null)
                {
                    asserter(expected, actual);
                }
                else
                {
                    Assert.Equal(expected, actual);
                }

                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());
            }
        }

        #endregion

        #region AssertSum

        public override async Task AssertSum<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<int>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int>> expectedQuery,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).SumAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).Sum();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).Sum();

                Assert.Equal(expected, actual);
                Assert.Equal(0, context.ChangeTracker.Entries().Count());
            }
        }

        public override async Task AssertSum<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<int?>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int?>> expectedQuery,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).SumAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).Sum();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).Sum();

                Assert.Equal(expected, actual);
                Assert.Equal(0, context.ChangeTracker.Entries().Count());
            }
        }

        public override async Task AssertSumWithSelector<TItem1, TResult>(
            Func<IQueryable<TItem1>, IQueryable<TResult>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TResult>> expectedQuery,
            Expression<Func<TResult, int>> actualSelector,
            Expression<Func<TResult, int>> expectedSelector,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).SumAsync(actualSelector)
                    : actualQuery(SetExtractor.Set<TItem1>(context)).Sum(actualSelector);

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).Sum(expectedSelector);

                Assert.Equal(expected, actual);
                Assert.Equal(0, context.ChangeTracker.Entries().Count());
            }
        }

        #endregion

        #region AssertAverage

        public override async Task AssertAverage<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<int>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<int>> expectedQuery,
            Action<object, object> asserter = null,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(SetExtractor.Set<TItem1>(context)).AverageAsync()
                    : actualQuery(SetExtractor.Set<TItem1>(context)).Average();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).Average();

                if (asserter == null)
                {
                    _entityAsserters.TryGetValue(expected.GetType(), out asserter);
                }

                if (asserter != null)
                {
                    asserter(expected, actual);
                }
                else
                {
                    Assert.Equal(expected, actual);
                }

                Assert.Equal(0, context.ChangeTracker.Entries().Count());
            }
        }

        #endregion

        //public override async Task AssertFirstOrDefault<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<dynamic>> actualQuery,
        //    Func<IQueryable<TItem1>, IQueryable<dynamic>> expectedQuery,
        //    Expression<Func<dynamic, bool>> actualFirstOrDefaultPredicate,
        //    Expression<Func<dynamic, bool>> expectedFirstOrDefaultPredicate,
        //    Action<object, object> asserter = null,
        //    int entryCount = 0,
        //    bool isAsync = false)
        //{
        //    using (var context = _contextCreator())
        //    {
        //        var actual = isAsync
        //            ? actualFirstOrDefaultPredicate != null
        //                ? await actualQuery(SetExtractor.Set<TItem1>(context)).FirstOrDefaultAsync(actualFirstOrDefaultPredicate)
        //                : await actualQuery(SetExtractor.Set<TItem1>(context)).FirstOrDefaultAsync()
        //            : actualFirstOrDefaultPredicate != null
        //                ? actualQuery(SetExtractor.Set<TItem1>(context)).FirstOrDefault(actualFirstOrDefaultPredicate)
        //                : actualQuery(SetExtractor.Set<TItem1>(context)).FirstOrDefault();

        //        var expected = expectedFirstOrDefaultPredicate != null
        //            ? expectedQuery(ExpectedData.Set<TItem1>()).FirstOrDefault(expectedFirstOrDefaultPredicate)
        //            : expectedQuery(ExpectedData.Set<TItem1>()).FirstOrDefault();

        //        if (asserter == null
        //            && expected != null)
        //        {
        //            _entityAsserters.TryGetValue(expected.GetType(), out asserter);
        //        }

        //        if (asserter != null)
        //        {
        //            asserter(expected, actual);
        //        }
        //        else
        //        {
        //            Assert.Equal(expected, actual);
        //        }

        //        Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());
        //    }
        //}






















        #region AssertIncludeQuery

        public Task<List<object>> AssertIncludeQuery<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0,
            bool isAsync = false)
            where TItem1 : class
            => AssertIncludeQuery(query, query, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount, isAsync);

        public override async Task<List<object>> AssertIncludeQuery<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(
                        SetExtractor.Set<TItem1>(context)).ToListAsync()
                    : actualQuery(
                        SetExtractor.Set<TItem1>(context)).ToList();

                var expected = expectedQuery(ExpectedData.Set<TItem1>()).ToList();

                if (!assertOrder)
                {
                    if (elementSorter == null)
                    {
                        var firstNonNullableElement = expected.FirstOrDefault(e => e != null);
                        if (firstNonNullableElement != null)
                        {
                            _entitySorters.TryGetValue(firstNonNullableElement.GetType(), out elementSorter);
                        }
                    }

                    if (elementSorter != null)
                    {
                        actual = actual.OrderBy(elementSorter).ToList();
                        expected = expected.OrderBy(elementSorter).ToList();
                    }
                }

                if (clientProjections != null)
                {
                    foreach (var clientProjection in clientProjections)
                    {
                        var projectedActual = actual.Select(clientProjection).ToList();
                        var projectedExpected = expected.Select(clientProjection).ToList();

                        _includeResultAsserter.AssertResult(projectedExpected, projectedActual, expectedIncludes);
                    }
                }
                else
                {
                    _includeResultAsserter.AssertResult(expected, actual, expectedIncludes);
                }

                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());

                return actual;
            }
        }

        public Task<List<object>> AssertIncludeQuery<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0,
            bool isAsync = false)
            where TItem1 : class
            where TItem2 : class
            => AssertIncludeQuery(query, query, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount, isAsync);

        public override async Task<List<object>> AssertIncludeQuery<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0,
            bool isAsync = false)
        {
            using (var context = _contextCreator())
            {
                var actual = isAsync
                    ? await actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context)).ToListAsync()
                    : actualQuery(
                        SetExtractor.Set<TItem1>(context),
                        SetExtractor.Set<TItem2>(context)).ToList();

                var expected = expectedQuery(
                    ExpectedData.Set<TItem1>(),
                    ExpectedData.Set<TItem2>()).ToList();

                if (!assertOrder)
                {
                    if (elementSorter == null)
                    {
                        var firstNonNullableElement = expected.FirstOrDefault(e => e != null);
                        if (firstNonNullableElement != null)
                        {
                            _entitySorters.TryGetValue(firstNonNullableElement.GetType(), out elementSorter);
                        }
                    }

                    if (elementSorter != null)
                    {
                        actual = actual.OrderBy(elementSorter).ToList();
                        expected = expected.OrderBy(elementSorter).ToList();
                    }
                }

                if (clientProjections != null)
                {
                    foreach (var clientProjection in clientProjections)
                    {
                        var projectedActual = actual.Select(clientProjection).ToList();
                        var projectedExpected = expected.Select(clientProjection).ToList();

                        _includeResultAsserter.AssertResult(projectedExpected, projectedActual, expectedIncludes);
                    }
                }
                else
                {
                    _includeResultAsserter.AssertResult(expected, actual, expectedIncludes);
                }

                Assert.Equal(entryCount, context.ChangeTracker.Entries().Count());

                return actual;
            }
        }

        #endregion

        private class DefaultSetExtractor : ISetExtractor
        {
            public override IQueryable<TEntity> Set<TEntity>(DbContext context)
            {
                var entityOrQueryType = context.Model.FindEntityType(typeof(TEntity));

                return entityOrQueryType.IsQueryType
                    ? (IQueryable<TEntity>)context.Query<TEntity>()
                    : context.Set<TEntity>();
            }
        }
    }
}
