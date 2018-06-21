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

        private async Task Execute(Func<bool, Task> executor, ExecutionMode mode)
        {
            if (mode == ExecutionMode.SyncOnly || mode == ExecutionMode.SyncAsync)
            {
                await executor(false);
            }

            if (mode == ExecutionMode.AsyncOnly || mode == ExecutionMode.SyncAsync)
            {
                await executor(true);
            }
        }

        private async Task<TResult> Execute<TResult>(Func<bool, Task<TResult>> executor, ExecutionMode mode)
        {
            TResult result = default;

            if (mode == ExecutionMode.SyncOnly || mode == ExecutionMode.SyncAsync)
            {
                result = await executor(false);
            }

            if (mode == ExecutionMode.AsyncOnly || mode == ExecutionMode.SyncAsync)
            {
                result = await executor(true);
            }

            return result;
        }

        #region AssertQuery

        // one argument

        //public override Task AssertQuery<TItem1>(
        //    Func<IQueryable<TItem1>, IQueryable<object>> query,
        //    Func<dynamic, object> elementSorter = null,
        //    Action<dynamic, dynamic> elementAsserter = null,
        //    bool assertOrder = false,
        //    int entryCount = 0,
        //    ExecutionMode mode = ExecutionMode.SyncAsync)
        //    where TItem1 : class
        //    => AssertQuery(query, query, elementSorter, elementAsserter, assertOrder, entryCount, mode);

        public override async Task AssertQuery<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
        {
            await Execute(
                async isAsync =>
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
                }, executionMode);
        }

        // two arguments

        //public virtual Task AssertQuery<TItem1, TItem2>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query,
        //    Func<dynamic, object> elementSorter = null,
        //    Action<dynamic, dynamic> elementAsserter = null,
        //    bool assertOrder = false,
        //    int entryCount = 0,
        //    ExecutionMode mode = ExecutionMode.SyncAsync)
        //    where TItem1 : class
        //    where TItem2 : class
        //    => AssertQuery(query, query, elementSorter, elementAsserter, assertOrder, entryCount, mode);

        public override async Task AssertQuery<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
        {
            await Execute(
                async isAsync =>
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
                }, executionMode);
        }

        // three arguments
        //public virtual Task AssertQuery<TItem1, TItem2, TItem3>(
        //    Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> query,
        //    Func<dynamic, object> elementSorter = null,
        //    Action<dynamic, dynamic> elementAsserter = null,
        //    bool assertOrder = false,
        //    int entryCount = 0,
        //    ExecutionMode mode = ExecutionMode.SyncAsync)
        //    where TItem1 : class
        //    where TItem2 : class
        //    where TItem3 : class
        //    => AssertQuery(query, query, elementSorter, elementAsserter, assertOrder, entryCount, mode);

        public override async Task AssertQuery<TItem1, TItem2, TItem3>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<TItem3>, IQueryable<object>> expectedQuery,
            Func<dynamic, object> elementSorter = null,
            Action<dynamic, dynamic> elementAsserter = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
        {
            await Execute(
                async isAsync =>
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
                }, executionMode);
        }

        #endregion


        #region Assert single result


        public override async Task AssertFirst<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<dynamic>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<dynamic>> expectedQuery,
            Expression<Func<dynamic, bool>> actualFirstPredicate,
            Expression<Func<dynamic, bool>> expectedFirstPredicate,
            Action<object, object> asserter = null,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
        {
            await Execute(
                async isAsync =>
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
                }, executionMode);
        }

        public override async Task AssertFirstOrDefault<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<dynamic>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<dynamic>> expectedQuery,
            Expression<Func<dynamic, bool>> actualFirstOrDefaultPredicate,
            Expression<Func<dynamic, bool>> expectedFirstOrDefaultPredicate,
            Action<object, object> asserter = null,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
        {
            await Execute(
                async isAsync =>
                {
                    using (var context = _contextCreator())
                    {
                        var actual = isAsync
                            ? actualFirstOrDefaultPredicate != null
                                ? await actualQuery(SetExtractor.Set<TItem1>(context)).FirstOrDefaultAsync(actualFirstOrDefaultPredicate)
                                : await actualQuery(SetExtractor.Set<TItem1>(context)).FirstOrDefaultAsync()
                            : actualFirstOrDefaultPredicate != null
                                ? actualQuery(SetExtractor.Set<TItem1>(context)).FirstOrDefault(actualFirstOrDefaultPredicate)
                                : actualQuery(SetExtractor.Set<TItem1>(context)).FirstOrDefault();

                        var expected = expectedFirstOrDefaultPredicate != null
                            ? expectedQuery(ExpectedData.Set<TItem1>()).FirstOrDefault(expectedFirstOrDefaultPredicate)
                            : expectedQuery(ExpectedData.Set<TItem1>()).FirstOrDefault();

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
                }, executionMode);
        }

        #endregion





















        #region AssertIncludeQuery

        public Task<List<object>> AssertIncludeQuery<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> query,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
            where TItem1 : class
            => AssertIncludeQuery(query, query, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount, executionMode);

        public override async Task<List<object>> AssertIncludeQuery<TItem1>(
            Func<IQueryable<TItem1>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<object>> expectedQuery,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
        {
            var result = await Execute(
                async isAsync =>
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
                }, executionMode);

            return result;
        }

        public Task<List<object>> AssertIncludeQuery<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> query,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
            where TItem1 : class
            where TItem2 : class
            => AssertIncludeQuery(query, query, expectedIncludes, elementSorter, clientProjections, assertOrder, entryCount, executionMode);

        public override async Task<List<object>> AssertIncludeQuery<TItem1, TItem2>(
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> actualQuery,
            Func<IQueryable<TItem1>, IQueryable<TItem2>, IQueryable<object>> expectedQuery,
            List<IExpectedInclude> expectedIncludes,
            Func<dynamic, object> elementSorter = null,
            List<Func<dynamic, object>> clientProjections = null,
            bool assertOrder = false,
            int entryCount = 0,
            ExecutionMode executionMode = ExecutionMode.SyncAsync)
        {
            var result = await Execute(
                async isAsync =>
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
                }, executionMode);

            return result;
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
