// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.EntityFrameworkCore.TestUtilities.Xunit;
using Xunit;

// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToExpressionBodyWhenPossible
// ReSharper disable AccessToDisposedClosure
// ReSharper disable AccessToModifiedClosure
// ReSharper disable StringStartsWithIsCultureSpecific
// ReSharper disable StringEndsWithIsCultureSpecific
namespace Microsoft.EntityFrameworkCore.Query
{
    public abstract class AsyncSimpleQueryTestBase<TFixture> : AsyncQueryTestBase<TFixture>
        where TFixture : NorthwindQueryFixtureBase<NoopModelCustomizer>, new()
    {
        protected AsyncSimpleQueryTestBase(TFixture fixture)
            : base(fixture)
        {
        }

        protected NorthwindContext CreateContext() => Fixture.CreateContext();

        [ConditionalFact]
        public virtual async Task GroupBy_tracking_after_dispose()
        {
            List<IGrouping<string, Order>> groups;

            using (var context = CreateContext())
            {
                groups = await context.Orders.GroupBy(o => o.CustomerID).ToListAsync();
            }

            var _ = groups[0].First();
        }

        [ConditionalFact]
        public virtual async Task Query_backed_by_database_view()
        {
            using (var context = CreateContext())
            {
                var results = await context.Query<ProductQuery>().ToArrayAsync();

                Assert.Equal(69, results.Length);
            }
        }

        [ConditionalFact]
        public virtual async Task ToList_context_subquery_deadlock_issue()
        {
            using (var context = CreateContext())
            {
                var _ = await context.Customers
                    .Select(
                        c => new
                        {
                            c.CustomerID,
                            Posts = context.Orders.Where(o => o.CustomerID == c.CustomerID)
                                .Select(
                                    m => new
                                    {
                                        m.CustomerID
                                    })
                                .ToList()
                        })
                    .ToListAsync();
            }
        }

        [ConditionalFact]
        public virtual async Task ToArray_on_nav_subquery_in_projection()
        {
            using (var context = CreateContext())
            {
                var results
                    = await context.Customers.Select(
                            c => new
                            {
                                Orders = c.Orders.ToArray()
                            })
                        .ToListAsync();

                Assert.Equal(830, results.SelectMany(a => a.Orders).ToList().Count);
            }
        }

        [ConditionalFact]
        public virtual async Task ToArray_on_nav_subquery_in_projection_nested()
        {
            using (var context = CreateContext())
            {
                var results
                    = await context.Customers.Select(
                            c => new
                            {
                                Orders = c.Orders.Select(
                                        o => new
                                        {
                                            OrderDetails = o.OrderDetails.ToArray()
                                        })
                                    .ToArray()
                            })
                        .ToListAsync();

                Assert.Equal(2155, results.SelectMany(a => a.Orders.SelectMany(o => o.OrderDetails)).ToList().Count);
            }
        }

        [ConditionalFact]
        public virtual async Task ToList_on_nav_subquery_in_projection()
        {
            using (var context = CreateContext())
            {
                var results
                    = await context.Customers.Select(
                            c => new
                            {
                                Orders = c.Orders.ToList()
                            })
                        .ToListAsync();

                Assert.Equal(830, results.SelectMany(a => a.Orders).ToList().Count);
            }
        }

        [ConditionalFact]
        public virtual async Task ToList_on_nav_subquery_with_predicate_in_projection()
        {
            using (var context = CreateContext())
            {
                var results
                    = await context.Customers.Select(
                            c => new
                            {
                                Orders = c.Orders.Where(o => o.OrderID > 10).ToList()
                            })
                        .ToListAsync();

                Assert.Equal(830, results.SelectMany(a => a.Orders).ToList().Count);
            }
        }

        [ConditionalFact]
        public virtual async Task Average_on_nav_subquery_in_projection()
        {
            using (var context = CreateContext())
            {
                var results
                    = await context.Customers.Select(
                            c => new
                            {
                                Ave = c.Orders.Average(o => o.Freight)
                            })
                        .ToListAsync();

                Assert.Equal(91, results.ToList().Count);
            }
        }

        [ConditionalFact]
        public virtual async Task ToListAsync_can_be_canceled()
        {
            for (var i = 0; i < 10; i++)
            {
                // without fix, this usually throws within 2 or three iterations

                using (var context = CreateContext())
                {
                    var tokenSource = new CancellationTokenSource();
                    var query = context.Employees.AsNoTracking().ToListAsync(tokenSource.Token);
                    tokenSource.Cancel();
                    List<Employee> result = null;
                    Exception exception = null;
                    try
                    {
                        result = await query;
                    }
                    catch (Exception e)
                    {
                        exception = e;
                    }

                    if (exception != null)
                    {
                        Assert.Null(result);
                    }
                    else
                    {
                        Assert.Equal(9, result.Count);
                    }
                }
            }
        }

        [ConditionalFact]
        public virtual async Task Mixed_sync_async_query()
        {
            using (var context = CreateContext())
            {
                var results
                    = (await context.Customers
                        .Select(
                            c => new
                            {
                                c.CustomerID,
                                Orders = context.Orders.Where(o => o.Customer.CustomerID == c.CustomerID)
                            }).ToListAsync())
                    .Select(
                        x => new
                        {
                            Orders = x.Orders
                                .GroupJoin(
                                    new[] { "ALFKI" }, y => x.CustomerID, y => y, (h, id) => new
                                    {
                                        h.Customer
                                    })
                        })
                    .ToList();

                Assert.Equal(830, results.SelectMany(r => r.Orders).ToList().Count);
            }
        }

        [ConditionalFact]
        public virtual async Task LoadAsync_should_track_results()
        {
            using (var context = CreateContext())
            {
                await context.Customers.LoadAsync();

                Assert.Equal(91, context.ChangeTracker.Entries().Count());
            }
        }

        protected virtual Task Single_Predicate_Cancellation_test(CancellationToken cancellationToken)
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.SingleAsync(c => c.CustomerID == "ALFKI", cancellationToken));
        }

        [ConditionalFact]
        public virtual async Task Mixed_sync_async_in_query_cache()
        {
            using (var context = CreateContext())
            {
                Assert.Equal(91, context.Customers.AsNoTracking().ToList().Count);
                Assert.Equal(91, (await context.Customers.AsNoTracking().ToListAsync()).Count);
            }
        }

        [ConditionalFact]
        public virtual Task Distinct_Take_Count()
        {
            return AssertSingleResultAsync<Order, int>(
                os => os.Distinct().Take(5).CountAsync());
        }

        [ConditionalFact]
        public virtual Task Take_Distinct_Count()
        {
            return AssertSingleResultAsync<Order>(
                os => os.Take(5).Distinct().CountAsync());
        }

        [ConditionalFact]
        public virtual Task Any_simple()
        {
            return AssertSingleResultAsync<Customer>(
                cs => cs.AnyAsync());
        }

        [ConditionalFact]
        public virtual Task OrderBy_Take_Count()
        {
            return AssertSingleResultAsync<Order>(
                os => os.OrderBy(o => o.OrderID).Take(5).CountAsync());
        }

        [ConditionalFact]
        public virtual Task Take_OrderBy_Count()
        {
            return AssertSingleResultAsync<Order>(
                os => os.Take(5).OrderBy(o => o.OrderID).CountAsync());
        }

        [ConditionalFact]
        public virtual Task Any_predicate()
        {
            return AssertSingleResultAsync<Customer>(
                cs => cs.AnyAsync(c => c.ContactName.StartsWith("A")));
        }

        [ConditionalFact]
        public virtual Task All_top_level()
        {
            return AssertSingleResultAsync<Customer>(
                cs => cs.AllAsync(c => c.ContactName.StartsWith("A")));
        }

        [ConditionalFact]
        public virtual Task All_top_level_subquery()
        {
            // ReSharper disable once PossibleUnintendedReferenceComparison
            return AssertSingleResultAsync<Customer>(
                cs => cs.AllAsync(c1 => cs.Any(c2 => cs.Any(c3 => c1 == c3))));
        }

        [ConditionalFact]
        public virtual Task Take_with_single()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.OrderBy(c => c.CustomerID).Take(1).SingleAsync(),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task Take_with_single_select_many()
        {
            return AssertSingleResultAsync<Customer, Order>(
                (cs, os) =>
                    (from c in cs
                     from o in os
                     orderby c.CustomerID, o.OrderID
                     select new
                     {
                         c,
                         o
                     })
                    .Take(1)
                    .Cast<object>()
                    .SingleAsync(),
                entryCount: 2);
        }

        [ConditionalFact]
        public virtual Task First_client_predicate()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.OrderBy(c => c.CustomerID).FirstAsync(c => c.IsLondon),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task Where_primitive()
        {
            return AssertQueryScalarAsync<Employee>(
                es => es.Select(e => e.EmployeeID).Take(9).Where(i => i == 5));
        }

        [ConditionalFact]
        public virtual Task Select_constant_int()
        {
            return AssertQueryScalarAsync<Customer>(cs => cs.Select(c => 0));
        }

        [ConditionalFact]
        public virtual Task Select_local()
        {
            // ReSharper disable once ConvertToConstant.Local
            var x = 10;

            return AssertQueryScalarAsync<Customer>(cs => cs.Select(c => x));
        }

        [ConditionalFact]
        public virtual Task Select_scalar_primitive()
        {
            return AssertQueryScalarAsync<Employee>(
                es => es.Select(e => e.EmployeeID));
        }

        [ConditionalFact]
        public virtual Task Select_scalar_primitive_after_take()
        {
            return AssertQueryScalarAsync<Employee>(
                es => es.Take(9).Select(e => e.EmployeeID));
        }

        [ConditionalFact]
        public virtual Task OrderBy_scalar_primitive()
        {
            return AssertQueryScalarAsync<Employee>(
                es => es.Select(e => e.EmployeeID).OrderBy(i => i),
                assertOrder: true);
        }

        [ConditionalFact]
        public virtual Task SelectMany_primitive()
        {
            return AssertQueryScalarAsync<Employee>(
                es =>
                    from e1 in es
                    from i in es.Select(e2 => e2.EmployeeID)
                    select i);
        }

        [ConditionalFact]
        public virtual Task SelectMany_primitive_select_subquery()
        {
            return AssertQueryScalarAsync<Employee>(
                es =>
                    from e1 in es
                    from i in es.Select(e2 => e2.EmployeeID)
                    select es.Any());
        }

        [ConditionalFact]
        public virtual Task Join_Where_Count()
        {
            return AssertSingleResultAsync<Customer, Order>(
                (cs, os) =>
                    (from c in cs
                     join o in os on c.CustomerID equals o.CustomerID
                     where c.CustomerID == "ALFKI"
                     select c).CountAsync());
        }

        [ConditionalFact]
        public virtual Task Multiple_joins_Where_Order_Any()
        {
            return AssertSingleResultAsync<Customer, Order, OrderDetail>(
                (cs, os, ods) =>
                    cs.Join(
                            os, c => c.CustomerID, o => o.CustomerID, (cr, or) => new
                            {
                                cr,
                                or
                            })
                        .Join(
                            ods, e => e.or.OrderID, od => od.OrderID, (e, od) => new
                            {
                                e.cr,
                                e.or,
                                od
                            })
                        .Where(r => r.cr.City == "London").OrderBy(r => r.cr.CustomerID)
                        .AnyAsync());
        }

        [ConditionalFact]
        public virtual Task Join_OrderBy_Count()
        {
            return AssertSingleResultAsync<Customer, Order>(
                (cs, os) =>
                    (from c in cs
                     join o in os on c.CustomerID equals o.CustomerID
                     orderby c.CustomerID
                     select c).CountAsync());
        }

        private class Foo
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Bar { get; set; }
        }

        private const uint NonExistentID = uint.MaxValue;

        [ConditionalFact]
        public virtual Task Default_if_empty_top_level_projection()
        {
            return AssertQueryScalarAsync<Employee>(
                es =>
                    from e in es.Where(e => e.EmployeeID == NonExistentID).Select(e => e.EmployeeID).DefaultIfEmpty()
                    select e);
        }

        [ConditionalFact]
        public virtual Task SelectMany_Count()
        {
            return AssertSingleResultAsync<Customer, Order>(
                (cs, os) =>
                    (from c in cs
                     from o in os
                     select c.CustomerID).CountAsync());
        }

        [ConditionalFact]
        public virtual Task SelectMany_LongCount()
        {
            return AssertSingleResultAsync<Customer, Order>(
                (cs, os) =>
                    (from c in cs
                     from o in os
                     select c.CustomerID).LongCountAsync());
        }

        [ConditionalFact]
        public virtual Task SelectMany_OrderBy_ThenBy_Any()
        {
            return AssertSingleResultAsync<Customer, Order>(
                (cs, os) =>
                    (from c in cs
                     from o in os
                     orderby c.CustomerID, c.City
                     select c).AnyAsync());
        }

        // TODO: Composite keys, slow..

        //        [ConditionalFact]
        //        public virtual async Task Multiple_joins_with_join_conditions_in_where()
        //        {
        //            AssertQuery<Customer, Order, OrderDetail>((cs, os, ods) =>
        //                from c in cs
        //                from o in os.OrderBy(o1 => o1.OrderID).Take(10)
        //                from od in ods
        //                where o.CustomerID == c.CustomerID
        //                    && o.OrderID == od.OrderID
        //                where c.CustomerID == "ALFKI"
        //                select od.ProductID,
        //                assertOrder: true);
        //        }
        //        [ConditionalFact]
        //
        //        public virtual async Task TestMultipleJoinsWithMissingJoinCondition()
        //        {
        //            AssertQuery<Customer, Order, OrderDetail>((cs, os, ods) =>
        //                from c in cs
        //                from o in os
        //                from od in ods
        //                where o.CustomerID == c.CustomerID
        //                where c.CustomerID == "ALFKI"
        //                select od.ProductID
        //                );
        //        }

        [ConditionalFact]
        public virtual Task OrderBy_ThenBy_Any()
        {
            return AssertSingleResultAsync<Customer>(
                cs => cs.OrderBy(c => c.CustomerID).ThenBy(c => c.ContactName).AnyAsync());
        }

        [ConditionalFact]
        public virtual Task Sum_with_no_arg()
        {
            return AssertSingleResultAsync<Order>(os => os.Select(o => o.OrderID).SumAsync());
        }

        [ConditionalFact]
        public virtual Task Sum_with_binary_expression()
        {
            return AssertSingleResultAsync<Order>(os => os.Select(o => o.OrderID * 2).SumAsync());
        }

        [ConditionalFact]
        public virtual Task Sum_with_no_arg_empty()
        {
            return AssertSingleResultAsync<Order>(os => os.Where(o => o.OrderID == 42).Select(o => o.OrderID).SumAsync());
        }

        [ConditionalFact]
        public virtual Task Sum_with_arg()
        {
            return AssertSingleResultAsync<Order>(os => os.SumAsync(o => o.OrderID));
        }

        [ConditionalFact]
        public virtual Task Sum_with_arg_expression()
        {
            return AssertSingleResultAsync<Order>(os => os.SumAsync(o => o.OrderID + o.OrderID));
        }

        [ConditionalFact]
        public virtual Task Sum_with_coalesce()
        {
            return AssertSingleResultAsync<Product>(ps => ps.Where(p => p.ProductID < 40).SumAsync(p => p.UnitPrice ?? 0));
        }

        [ConditionalFact]
        public virtual Task Sum_over_subquery_is_client_eval()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.SumAsync(c => c.Orders.Sum(o => o.OrderID)));
        }

        [ConditionalFact]
        public virtual Task Average_with_no_arg()
        {
            return AssertSingleResultAsync<Order>(os => os.Select(o => o.OrderID).AverageAsync());
        }

        [ConditionalFact]
        public virtual Task Average_with_binary_expression()
        {
            return AssertSingleResultAsync<Order>(os => os.Select(o => o.OrderID * 2).AverageAsync());
        }

        [ConditionalFact]
        public virtual Task Average_with_arg()
        {
            return AssertSingleResultAsync<Order>(os => os.AverageAsync(o => o.OrderID));
        }

        [ConditionalFact]
        public virtual Task Average_with_arg_expression()
        {
            return AssertSingleResultAsync<Order>(os => os.AverageAsync(o => o.OrderID + o.OrderID));
        }

        [ConditionalFact]
        public virtual Task Average_with_coalesce()
        {
            return AssertSingleResultAsync<Product>(
                ps => ps.Where(p => p.ProductID < 40).AverageAsync(p => p.UnitPrice ?? 0),
                asserter: (e, a) => Assert.InRange((decimal)e - (decimal)a, -0.1m, 0.1m));
        }

        [ConditionalFact]
        public virtual Task Average_over_subquery_is_client_eval()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.AverageAsync(c => c.Orders.Sum(o => o.OrderID)));
        }

        [ConditionalFact]
        public virtual Task Min_with_no_arg()
        {
            return AssertSingleResultAsync<Order>(os => os.Select(o => o.OrderID).MinAsync());
        }

        [ConditionalFact]
        public virtual Task Min_with_arg()
        {
            return AssertSingleResultAsync<Order>(os => os.MinAsync(o => o.OrderID));
        }

        [ConditionalFact]
        public virtual Task Min_with_coalesce()
        {
            return AssertSingleResultAsync<Product>(ps => ps.Where(p => p.ProductID < 40).MinAsync(p => p.UnitPrice ?? 0));
        }

        [ConditionalFact]
        public virtual Task Min_over_subquery_is_client_eval()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.MinAsync(c => c.Orders.Sum(o => o.OrderID)));
        }

        [ConditionalFact]
        public virtual Task Max_with_no_arg()
        {
            return AssertSingleResultAsync<Order>(os => os.Select(o => o.OrderID).MaxAsync());
        }

        [ConditionalFact]
        public virtual Task Max_with_arg()
        {
            return AssertSingleResultAsync<Order>(os => os.MaxAsync(o => o.OrderID));
        }

        [ConditionalFact]
        public virtual Task Max_with_coalesce()
        {
            return AssertSingleResultAsync<Product>(ps => ps.Where(p => p.ProductID < 40).MaxAsync(p => p.UnitPrice ?? 0));
        }

        [ConditionalFact]
        public virtual Task Max_over_subquery_is_client_eval()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.MaxAsync(c => c.Orders.Sum(o => o.OrderID)));
        }

        [ConditionalFact]
        public virtual Task Count_with_no_predicate()
        {
            return AssertSingleResultAsync<Order>(os => os.CountAsync());
        }

        [ConditionalFact]
        public virtual Task Count_with_predicate()
        {
            return AssertSingleResultAsync<Order>(os => os.CountAsync(o => o.CustomerID == "ALFKI"));
        }

        [ConditionalFact]
        public virtual Task Count_with_order_by()
        {
            return AssertSingleResultAsync<Order>(os => os.OrderBy(o => o.CustomerID).CountAsync());
        }

        [ConditionalFact]
        public virtual Task Where_OrderBy_Count()
        {
            return AssertSingleResultAsync<Order>(os => os.Where(o => o.CustomerID == "ALFKI").OrderBy(o => o.OrderID).CountAsync());
        }

        [ConditionalFact]
        public virtual Task OrderBy_Where_Count()
        {
            return AssertSingleResultAsync<Order>(os => os.OrderBy(o => o.OrderID).Where(o => o.CustomerID == "ALFKI").CountAsync());
        }

        [ConditionalFact]
        public virtual Task OrderBy_Count_with_predicate()
        {
            return AssertSingleResultAsync<Order>(os => os.OrderBy(o => o.OrderID).CountAsync(o => o.CustomerID == "ALFKI"));
        }

        [ConditionalFact]
        public virtual Task OrderBy_Where_Count_with_predicate()
        {
            return AssertSingleResultAsync<Order>(os => os.OrderBy(o => o.OrderID).Where(o => o.OrderID > 10).CountAsync(o => o.CustomerID != "ALFKI"));
        }

        [ConditionalFact]
        public virtual Task Where_OrderBy_Count_client_eval()
        {
            return AssertSingleResultAsync<Order>(os => os.Where(o => ClientEvalPredicate(o)).OrderBy(o => ClientEvalSelectorStateless()).CountAsync());
        }

        [ConditionalFact]
        public virtual Task Where_OrderBy_Count_client_eval_mixed()
        {
            return AssertSingleResultAsync<Order>(os => os.Where(o => o.OrderID > 10).OrderBy(o => ClientEvalPredicate(o)).CountAsync());
        }

        [ConditionalFact]
        public virtual Task OrderBy_Where_Count_client_eval()
        {
            return AssertSingleResultAsync<Order>(os => os.OrderBy(o => ClientEvalSelectorStateless()).Where(o => ClientEvalPredicate(o)).CountAsync());
        }

        [ConditionalFact]
        public virtual Task OrderBy_Where_Count_client_eval_mixed()
        {
            return AssertSingleResultAsync<Order>(os => os.OrderBy(o => o.OrderID).Where(o => ClientEvalPredicate(o)).CountAsync());
        }

        [ConditionalFact]
        public virtual Task OrderBy_Count_with_predicate_client_eval()
        {
            return AssertSingleResultAsync<Order>(os => os.OrderBy(o => ClientEvalSelectorStateless()).CountAsync(o => ClientEvalPredicate(o)));
        }

        [ConditionalFact]
        public virtual Task OrderBy_Count_with_predicate_client_eval_mixed()
        {
            return AssertSingleResultAsync<Order>(os => os.OrderBy(o => o.OrderID).CountAsync(o => ClientEvalPredicateStateless()));
        }

        [ConditionalFact]
        public virtual Task OrderBy_Where_Count_with_predicate_client_eval()
        {
            return AssertSingleResultAsync<Order>(os => os.OrderBy(o => ClientEvalSelectorStateless()).Where(o => ClientEvalPredicateStateless()).CountAsync(o => ClientEvalPredicate(o)));
        }

        [ConditionalFact]
        public virtual Task OrderBy_Where_Count_with_predicate_client_eval_mixed()
        {
            return AssertSingleResultAsync<Order>(os => os.OrderBy(o => o.OrderID).Where(o => ClientEvalPredicate(o)).CountAsync(o => o.CustomerID != "ALFKI"));
        }

        public static bool ClientEvalPredicateStateless() => true;

        protected static bool ClientEvalPredicate(Order order) => order.OrderID > 10000;

        private static int ClientEvalSelectorStateless() => 42;

        protected internal uint ClientEvalSelector(Order order) => order.EmployeeID % 10 ?? 0;

        [ConditionalFact]
        public virtual Task Distinct_Count()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.Distinct().CountAsync());
        }

        [ConditionalFact]
        public virtual Task Select_Distinct_Count()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.Select(c => c.City).Distinct().CountAsync());
        }

        [ConditionalFact]
        public virtual Task Select_Select_Distinct_Count()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.Select(c => c.City).Select(c => c).Distinct().CountAsync());
        }

        [ConditionalFact]
        public virtual async Task Single_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () =>
                    await AssertSingleResultAsync<Customer, Customer>(
                        cs => cs.SingleAsync()));
        }

        [ConditionalFact]
        public virtual Task Single_Predicate()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.SingleAsync(c => c.CustomerID == "ALFKI"),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task Where_Single()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                // ReSharper disable once ReplaceWithSingleCallToSingle
                cs => cs.Where(c => c.CustomerID == "ALFKI").SingleAsync(),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual async Task SingleOrDefault_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () =>
                    await AssertSingleResultAsync<Customer, Customer>(
                        cs => cs.SingleOrDefaultAsync()));
        }

        [ConditionalFact]
        public virtual Task SingleOrDefault_Predicate()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.SingleOrDefaultAsync(c => c.CustomerID == "ALFKI"),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task Where_SingleOrDefault()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                // ReSharper disable once ReplaceWithSingleCallToSingleOrDefault
                cs => cs.Where(c => c.CustomerID == "ALFKI").SingleOrDefaultAsync(),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task FirstAsync()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.OrderBy(c => c.ContactName).FirstAsync(),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task First_Predicate()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.OrderBy(c => c.ContactName).FirstAsync(c => c.City == "London"),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task Where_First()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                // ReSharper disable once ReplaceWithSingleCallToFirst
                cs => cs.OrderBy(c => c.ContactName).Where(c => c.City == "London").FirstAsync(),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task FirstOrDefault()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.OrderBy(c => c.ContactName).FirstOrDefaultAsync(),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task FirstOrDefault_Predicate()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.OrderBy(c => c.ContactName).FirstOrDefaultAsync(c => c.City == "London"),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task Where_FirstOrDefault()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
                cs => cs.OrderBy(c => c.ContactName).Where(c => c.City == "London").FirstOrDefaultAsync(),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task Last()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.OrderBy(c => c.ContactName).LastAsync(),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task Last_when_no_order_by()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                // ReSharper disable once ReplaceWithSingleCallToLast
                cs => cs.Where(c => c.CustomerID == "ALFKI").LastAsync(),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task Last_Predicate()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.OrderBy(c => c.ContactName).LastAsync(c => c.City == "London"),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task Where_Last()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                // ReSharper disable once ReplaceWithSingleCallToLast
                cs => cs.OrderBy(c => c.ContactName).Where(c => c.City == "London").LastAsync(),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task LastOrDefault()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.OrderBy(c => c.ContactName).LastOrDefaultAsync(),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task LastOrDefault_Predicate()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                cs => cs.OrderBy(c => c.ContactName).LastOrDefaultAsync(c => c.City == "London"),
                entryCount: 1);
        }

        [ConditionalFact]
        public virtual Task Where_LastOrDefault()
        {
            return AssertSingleResultAsync<Customer, Customer>(
                // ReSharper disable once ReplaceWithSingleCallToLastOrDefault
                cs => cs.OrderBy(c => c.ContactName).Where(c => c.City == "London").LastOrDefaultAsync(),
                entryCount: 1);
        }

        protected static string LocalMethod1()
        {
            return "M";
        }

        protected static string LocalMethod2()
        {
            return "m";
        }

        [ConditionalFact]
        public virtual Task Contains_top_level()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.Select(c => c.CustomerID).ContainsAsync("ALFKI"));
        }

        [ConditionalFact(Skip = "#12138")]
        public virtual async Task Throws_on_concurrent_query_list()
        {
            using (var context = CreateContext())
            {
                using (var synchronizationEvent = new ManualResetEventSlim(false))
                {
                    using (var blockingSemaphore = new SemaphoreSlim(0))
                    {
                        var blockingTask = Task.Run(
                            () =>
                                context.Customers.Select(
                                    c => Process(c, synchronizationEvent, blockingSemaphore)).ToList());

                        var throwingTask = Task.Run(
                            async () =>
                            {
                                synchronizationEvent.Wait();

                                Assert.Equal(
                                    CoreStrings.ConcurrentMethodInvocation,
                                    (await Assert.ThrowsAsync<InvalidOperationException>(
                                        () => context.Customers.ToListAsync())).Message);
                            });

                        await throwingTask;

                        blockingSemaphore.Release(1);

                        await blockingTask;
                    }
                }
            }
        }

        [ConditionalFact(Skip = "#12138")]
        public virtual async Task Throws_on_concurrent_query_first()
        {
            using (var context = CreateContext())
            {
                using (var synchronizationEvent = new ManualResetEventSlim(false))
                {
                    using (var blockingSemaphore = new SemaphoreSlim(0))
                    {
                        var blockingTask = Task.Run(
                            () =>
                                context.Customers.Select(
                                    c => Process(c, synchronizationEvent, blockingSemaphore)).ToList());

                        var throwingTask = Task.Run(
                            async () =>
                            {
                                synchronizationEvent.Wait();
                                Assert.Equal(
                                    CoreStrings.ConcurrentMethodInvocation,
                                    (await Assert.ThrowsAsync<InvalidOperationException>(
                                        () => context.Customers.FirstAsync())).Message);
                            });

                        await throwingTask;

                        blockingSemaphore.Release(1);

                        await blockingTask;
                    }
                }
            }
        }

        private static Customer Process(Customer c, ManualResetEventSlim e, SemaphoreSlim s)
        {
            e.Set();
            s.Wait();
            s.Release(1);
            return c;
        }

        // Set Operations

        [ConditionalFact]
        public virtual async Task Concat_dbset()
        {
            using (var context = CreateContext())
            {
                var query = await context.Set<Customer>()
                    .Where(c => c.City == "México D.F.")
                    .Concat(
                        context.Set<Customer>())
                    .ToListAsync();

                Assert.Equal(96, query.Count);
            }
        }

        [ConditionalFact]
        public virtual async Task Concat_simple()
        {
            using (var context = CreateContext())
            {
                var query = await context.Set<Customer>()
                    .Where(c => c.City == "México D.F.")
                    .Concat(
                        context.Set<Customer>()
                            .Where(s => s.ContactTitle == "Owner"))
                    .ToListAsync();

                Assert.Equal(22, query.Count);
            }
        }

        [ConditionalFact]
        public virtual async Task Concat_non_entity()
        {
            using (var context = CreateContext())
            {
                var query = await context.Set<Customer>()
                    .Where(c => c.City == "México D.F.")
                    .Select(c => c.CustomerID)
                    .Concat(
                        context.Set<Customer>()
                            .Where(s => s.ContactTitle == "Owner")
                            .Select(c => c.CustomerID))
                    .ToListAsync();

                Assert.Equal(22, query.Count);
            }
        }

        [ConditionalFact]
        public virtual async Task Except_non_entity()
        {
            using (var context = CreateContext())
            {
                var query = await context.Set<Customer>()
                    .Where(s => s.ContactTitle == "Owner")
                    .Select(c => c.CustomerID)
                    .Except(
                        context.Set<Customer>()
                            .Where(c => c.City == "México D.F.")
                            .Select(c => c.CustomerID))
                    .ToListAsync();

                Assert.Equal(14, query.Count);
            }
        }

        [ConditionalFact]
        public virtual async Task Intersect_non_entity()
        {
            using (var context = CreateContext())
            {
                var query = await context.Set<Customer>()
                    .Where(c => c.City == "México D.F.")
                    .Select(c => c.CustomerID)
                    .Intersect(
                        context.Set<Customer>()
                            .Where(s => s.ContactTitle == "Owner")
                            .Select(c => c.CustomerID))
                    .ToListAsync();

                Assert.Equal(3, query.Count);
            }
        }

        [ConditionalFact]
        public virtual async Task Union_non_entity()
        {
            using (var context = CreateContext())
            {
                var query = await context.Set<Customer>()
                    .Where(s => s.ContactTitle == "Owner")
                    .Select(c => c.CustomerID)
                    .Union(
                        context.Set<Customer>()
                            .Where(c => c.City == "México D.F.")
                            .Select(c => c.CustomerID))
                    .ToListAsync();

                Assert.Equal(19, query.Count);
            }
        }

        [ConditionalFact]
        public virtual async Task Select_bitwise_or()
        {
            using (var context = CreateContext())
            {
                var query = await context.Customers.OrderBy(c => c.CustomerID).Select(
                    c => new
                    {
                        c.CustomerID,
                        Value = c.CustomerID == "ALFKI" | c.CustomerID == "ANATR"
                    }).ToListAsync();

                Assert.All(query.Take(2), t => Assert.True(t.Value));
                Assert.All(query.Skip(2), t => Assert.False(t.Value));
            }
        }

        [ConditionalFact]
        public virtual async Task Select_bitwise_or_multiple()
        {
            using (var context = CreateContext())
            {
                var query = await context.Customers.OrderBy(c => c.CustomerID)
                    .Select(
                        c => new
                        {
                            c.CustomerID,
                            Value = c.CustomerID == "ALFKI" | c.CustomerID == "ANATR" | c.CustomerID == "ANTON"
                        }).ToListAsync();

                Assert.All(query.Take(3), t => Assert.True(t.Value));
                Assert.All(query.Skip(3), t => Assert.False(t.Value));
            }
        }

        [ConditionalFact]
        public virtual async Task Select_bitwise_and()
        {
            using (var context = CreateContext())
            {
                var query = await context.Customers.OrderBy(c => c.CustomerID).Select(
                    c => new
                    {
                        c.CustomerID,
                        Value = c.CustomerID == "ALFKI" & c.CustomerID == "ANATR"
                    }).ToListAsync();

                Assert.All(query, t => Assert.False(t.Value));
            }
        }

        [ConditionalFact]
        public virtual async Task Select_bitwise_and_or()
        {
            using (var context = CreateContext())
            {
                var query = await context.Customers.OrderBy(c => c.CustomerID)
                    .Select(
                        c => new
                        {
                            c.CustomerID,
                            Value = c.CustomerID == "ALFKI" & c.CustomerID == "ANATR" | c.CustomerID == "ANTON"
                        }).ToListAsync();

                Assert.All(query.Where(c => c.CustomerID != "ANTON"), t => Assert.False(t.Value));
            }
        }

        [ConditionalFact]
        public virtual async Task Select_bitwise_or_with_logical_or()
        {
            using (var context = CreateContext())
            {
                var query = await context.Customers.OrderBy(c => c.CustomerID).Select(
                    c => new
                    {
                        c.CustomerID,
                        Value = c.CustomerID == "ALFKI" | c.CustomerID == "ANATR" || c.CustomerID == "ANTON"
                    }).ToListAsync();

                Assert.All(query.Take(3), t => Assert.True(t.Value));
                Assert.All(query.Skip(3), t => Assert.False(t.Value));
            }
        }

        [ConditionalFact]
        public virtual async Task Select_bitwise_and_with_logical_and()
        {
            using (var context = CreateContext())
            {
                var query = await context.Customers.OrderBy(c => c.CustomerID).Select(
                    c => new
                    {
                        c.CustomerID,
                        Value = c.CustomerID == "ALFKI" & c.CustomerID == "ANATR" && c.CustomerID == "ANTON"
                    }).ToListAsync();

                Assert.All(query, t => Assert.False(t.Value));
            }
        }

        [ConditionalFact]
        public virtual Task Skip_CountAsync()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.Skip(7).CountAsync());
        }

        [ConditionalFact]
        public virtual Task Skip_LongCountAsync()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.Skip(7).LongCountAsync());
        }

        [ConditionalFact]
        public virtual Task OrderBy_Skip_CountAsync()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.OrderBy(c => c.Country).Skip(7).CountAsync());
        }

        [ConditionalFact]
        public virtual Task OrderBy_Skip_LongCountAsync()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.OrderBy(c => c.Country).Skip(7).LongCountAsync());
        }

        [ConditionalFact]
        public virtual Task Cast_to_same_Type_CountAsync_works()
        {
            return AssertSingleResultAsync<Customer>(cs => cs.Cast<Customer>().CountAsync());
        }
    }
}
