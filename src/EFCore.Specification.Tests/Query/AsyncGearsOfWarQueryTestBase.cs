// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestModels.GearsOfWarModel;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.EntityFrameworkCore.TestUtilities.Xunit;
using Xunit;

// ReSharper disable InconsistentNaming
namespace Microsoft.EntityFrameworkCore.Query
{
    public abstract class AsyncGearsOfWarQueryTestBase<TFixture> : AsyncQueryTestBase<TFixture>
        where TFixture : GearsOfWarQueryFixtureBase, new()
    {
        protected AsyncGearsOfWarQueryTestBase(TFixture fixture)
            : base(fixture)
        {
        }

        protected GearsOfWarContext CreateContext() => Fixture.CreateContext();

        [ConditionalFact]
        public virtual Task Include_multiple_one_to_one_and_one_to_many()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<CogTag>(t => t.Gear, "Gear"),
                new ExpectedInclude<Gear>(g => g.Weapons, "Weapons", "Gear"),
                new ExpectedInclude<Officer>(o => o.Weapons, "Weapons", "Gear")
            };

            return AssertIncludeQueryAsync<CogTag>(
                ts => ts.Include(t => t.Gear.Weapons),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_multiple_one_to_one_and_one_to_many_self_reference()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Weapon>(w => w.Owner, "Owner"),
                new ExpectedInclude<Gear>(g => g.Weapons, "Weapons", "Owner"),
                new ExpectedInclude<Officer>(o => o.Weapons, "Weapons", "Owner")
            };

            return AssertIncludeQueryAsync<Weapon>(
                ws => ws.Include(w => w.Owner.Weapons),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_multiple_one_to_one_optional_and_one_to_one_required()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<CogTag>(t => t.Gear, "Gear"),
                new ExpectedInclude<Gear>(g => g.Squad, "Squad", "Gear"),
                new ExpectedInclude<Officer>(o => o.Squad, "Squad", "Gear")
            };

            return AssertIncludeQueryAsync<CogTag>(
                ts => ts.Include(t => t.Gear.Squad),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_multiple_one_to_one_and_one_to_one_and_one_to_many()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<CogTag>(t => t.Gear, "Gear"),
                new ExpectedInclude<Gear>(g => g.Squad, "Squad", "Gear"),
                new ExpectedInclude<Officer>(o => o.Squad, "Squad", "Gear"),
                new ExpectedInclude<Squad>(s => s.Members, "Members", "Gear.Squad")
            };

            return AssertIncludeQueryAsync<CogTag>(
                ts => ts.Include(t => t.Gear.Squad.Members),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_multiple_circular()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Gear>(g => g.CityOfBirth, "CityOfBirth"),
                new ExpectedInclude<Officer>(o => o.CityOfBirth, "CityOfBirth"),
                new ExpectedInclude<City>(c => c.StationedGears, "StationedGears", "CityOfBirth")
            };

            return AssertIncludeQueryAsync<Gear>(
                gs => gs.Include(g => g.CityOfBirth.StationedGears),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_multiple_circular_with_filter()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Gear>(g => g.CityOfBirth, "CityOfBirth"),
                new ExpectedInclude<Officer>(o => o.CityOfBirth, "CityOfBirth"),
                new ExpectedInclude<City>(c => c.StationedGears, "StationedGears", "CityOfBirth")
            };

            return AssertIncludeQueryAsync<Gear>(
                gs => gs.Include(g => g.CityOfBirth.StationedGears).Where(g => g.Nickname == "Marcus"),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_using_alternate_key()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Gear>(g => g.Weapons, "Weapons"),
                new ExpectedInclude<Officer>(o => o.Weapons, "Weapons")
            };

            return AssertIncludeQueryAsync<Gear>(
                gs => gs.Include(g => g.Weapons).Where(g => g.Nickname == "Marcus"),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_multiple_include_then_include()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Gear>(g => g.AssignedCity, "AssignedCity"),
                new ExpectedInclude<Officer>(o => o.AssignedCity, "AssignedCity"),
                new ExpectedInclude<City>(c => c.BornGears, "BornGears", "AssignedCity"),
                new ExpectedInclude<Gear>(g => g.Tag, "Tag", "AssignedCity.BornGears"),
                new ExpectedInclude<Officer>(o => o.Tag, "Tag", "AssignedCity.BornGears"),
                new ExpectedInclude<City>(c => c.StationedGears, "StationedGears", "AssignedCity"),
                new ExpectedInclude<Gear>(g => g.Tag, "Tag", "AssignedCity.StationedGears"),
                new ExpectedInclude<Officer>(o => o.Tag, "Tag", "AssignedCity.StationedGears"),
                new ExpectedInclude<Gear>(g => g.CityOfBirth, "CityOfBirth"),
                new ExpectedInclude<Officer>(o => o.CityOfBirth, "CityOfBirth"),
                new ExpectedInclude<City>(c => c.BornGears, "BornGears", "CityOfBirth"),
                new ExpectedInclude<Gear>(g => g.Tag, "Tag", "CityOfBirth.BornGears"),
                new ExpectedInclude<Officer>(o => o.Tag, "Tag", "CityOfBirth.BornGears"),
                new ExpectedInclude<City>(c => c.StationedGears, "StationedGears", "CityOfBirth"),
                new ExpectedInclude<Gear>(g => g.Tag, "Tag", "CityOfBirth.StationedGears"),
                new ExpectedInclude<Officer>(o => o.Tag, "Tag", "CityOfBirth.StationedGears")
            };

            return AssertIncludeQueryAsync<Gear>(
                gs => gs.Include(g => g.AssignedCity.BornGears).ThenInclude(g => g.Tag)
                    .Include(g => g.AssignedCity.StationedGears).ThenInclude(g => g.Tag)
                    .Include(g => g.CityOfBirth.BornGears).ThenInclude(g => g.Tag)
                    .Include(g => g.CityOfBirth.StationedGears).ThenInclude(g => g.Tag)
                    .OrderBy(g => g.Nickname),
                expectedIncludes,
                assertOrder: true);
        }

        [ConditionalFact]
        public virtual Task Include_navigation_on_derived_type()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Officer>(o => o.Reports, "Reports")
            };

            return AssertIncludeQueryAsync<Gear>(
                gs => gs.OfType<Officer>().Include(o => o.Reports),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task String_based_Include_navigation_on_derived_type()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Officer>(o => o.Reports, "Reports")
            };

            return AssertIncludeQueryAsync<Gear>(
                gs => gs.OfType<Officer>().Include("Reports"),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Select_Where_Navigation_Included()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<CogTag>(t => t.Gear, "Gear")
            };

            return AssertIncludeQueryAsync<CogTag>(
                ts => from t in ts.Include(o => o.Gear)
                      where t.Gear.Nickname == "Marcus"
                      select t,
                ts => from t in ts
                      where Maybe(t.Gear, () => t.Gear.Nickname) == "Marcus"
                      select t,
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_with_join_reference1()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Gear>(g => g.CityOfBirth, "CityOfBirth"),
                new ExpectedInclude<Officer>(o => o.CityOfBirth, "CityOfBirth")
            };

            return AssertIncludeQueryAsync<Gear, CogTag>(
                (gs, ts) =>
                    gs.Join(
                        ts,
                        g => new
                        {
                            SquadId = (int?)g.SquadId,
                            g.Nickname
                        },
                        t => new
                        {
                            SquadId = t.GearSquadId,
                            Nickname = t.GearNickName
                        },
                        (g, t) => g).Include(g => g.CityOfBirth),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_with_join_reference2()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Gear>(g => g.CityOfBirth, "CityOfBirth"),
                new ExpectedInclude<Officer>(o => o.CityOfBirth, "CityOfBirth")
            };

            return AssertIncludeQueryAsync<CogTag, Gear>(
                (ts, gs) =>
                    ts.Join(
                        gs,
                        t => new
                        {
                            SquadId = t.GearSquadId,
                            Nickname = t.GearNickName
                        },
                        g => new
                        {
                            SquadId = (int?)g.SquadId,
                            g.Nickname
                        },
                        (t, g) => g).Include(g => g.CityOfBirth),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_with_join_collection1()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Gear>(g => g.Weapons, "Weapons"),
                new ExpectedInclude<Officer>(o => o.Weapons, "Weapons")
            };

            return AssertIncludeQueryAsync<Gear, CogTag>(
                (gs, ts) =>
                    gs.Join(
                        ts,
                        g => new
                        {
                            SquadId = (int?)g.SquadId,
                            g.Nickname
                        },
                        t => new
                        {
                            SquadId = t.GearSquadId,
                            Nickname = t.GearNickName
                        },
                        (g, t) => g).Include(g => g.Weapons),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_with_join_collection2()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Gear>(g => g.Weapons, "Weapons"),
                new ExpectedInclude<Officer>(o => o.Weapons, "Weapons")
            };

            return AssertIncludeQueryAsync<CogTag, Gear>(
                (ts, gs) =>
                    ts.Join(
                        gs,
                        t => new
                        {
                            SquadId = t.GearSquadId,
                            Nickname = t.GearNickName
                        },
                        g => new
                        {
                            SquadId = (int?)g.SquadId,
                            g.Nickname
                        },
                        (t, g) => g).Include(g => g.Weapons),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_reference_on_derived_type_using_string()
        {
            return AssertIncludeQueryAsync<LocustLeader>(
                lls => lls.Include("DefeatedBy"),
                new List<IExpectedInclude>
                {
                    new ExpectedInclude<LocustCommander>(lc => lc.DefeatedBy, "DefeatedBy")
                });
        }

        [ConditionalFact]
        public virtual Task Include_reference_on_derived_type_using_string_nested1()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<LocustCommander>(lc => lc.DefeatedBy, "DefeatedBy"),
                new ExpectedInclude<Gear>(g => g.Squad, "Squad", "DefeatedBy")
            };

            return AssertIncludeQueryAsync<LocustLeader>(
                lls => lls.Include("DefeatedBy.Squad"),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_reference_on_derived_type_using_string_nested2()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<LocustCommander>(lc => lc.DefeatedBy, "DefeatedBy"),
                new ExpectedInclude<Officer>(o => o.Reports, "Reports", "DefeatedBy"),
                new ExpectedInclude<Gear>(g => g.CityOfBirth, "CityOfBirth", "DefeatedBy.Reports")
            };

            return AssertIncludeQueryAsync<LocustLeader>(
                lls => lls.Include("DefeatedBy.Reports.CityOfBirth"),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_reference_on_derived_type_using_lambda()
        {
            return AssertIncludeQueryAsync<LocustLeader>(
                lls => lls.Include(ll => ((LocustCommander)ll).DefeatedBy),
                new List<IExpectedInclude>
                {
                    new ExpectedInclude<LocustCommander>(lc => lc.DefeatedBy, "DefeatedBy")
                });
        }

        [ConditionalFact]
        public virtual Task Include_reference_on_derived_type_using_lambda_with_soft_cast()
        {
            return AssertIncludeQueryAsync<LocustLeader>(
                lls => lls.Include(ll => (ll as LocustCommander).DefeatedBy),
                new List<IExpectedInclude>
                {
                    new ExpectedInclude<LocustCommander>(lc => lc.DefeatedBy, "DefeatedBy")
                });
        }

        [ConditionalFact]
        public virtual Task Include_reference_on_derived_type_using_lambda_with_tracking()
        {
            return AssertIncludeQueryAsync<LocustLeader>(
                lls => lls.AsTracking().Include(ll => ((LocustCommander)ll).DefeatedBy),
                new List<IExpectedInclude>
                {
                    new ExpectedInclude<LocustCommander>(lc => lc.DefeatedBy, "DefeatedBy")
                },
                entryCount: 7);
        }

        [ConditionalFact]
        public virtual Task Include_collection_on_derived_type_using_string()
        {
            return AssertIncludeQueryAsync<Gear>(
                gs => gs.Include("Reports"),
                new List<IExpectedInclude>
                {
                    new ExpectedInclude<Officer>(o => o.Reports, "Reports")
                });
        }

        [ConditionalFact]
        public virtual Task Include_collection_on_derived_type_using_lambda()
        {
            return AssertIncludeQueryAsync<Gear>(
                gs => gs.Include(g => ((Officer)g).Reports),
                new List<IExpectedInclude>
                {
                    new ExpectedInclude<Officer>(o => o.Reports, "Reports")
                });
        }

        [ConditionalFact]
        public virtual Task Include_collection_on_derived_type_using_lambda_with_soft_cast()
        {
            return AssertIncludeQueryAsync<Gear>(
                gs => gs.Include(g => (g as Officer).Reports),
                new List<IExpectedInclude>
                {
                    new ExpectedInclude<Officer>(o => o.Reports, "Reports")
                });
        }

        [ConditionalFact]
        public virtual Task Include_base_navigation_on_derived_entity()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Officer>(e => e.Tag, "Tag"),
                new ExpectedInclude<Officer>(e => e.Weapons, "Weapons")
            };

            return AssertIncludeQueryAsync<Gear>(
                gs => gs.Include(g => ((Officer)g).Tag).Include(g => ((Officer)g).Weapons),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task ThenInclude_collection_on_derived_after_base_reference()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<CogTag>(e => e.Gear, "Gear"),
                new ExpectedInclude<Officer>(e => e.Weapons, "Weapons", "Gear")
            };

            return AssertIncludeQueryAsync<CogTag>(
                ts => ts.Include(t => t.Gear).ThenInclude(g => (g as Officer).Weapons),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task ThenInclude_collection_on_derived_after_derived_reference()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<LocustHorde>(e => e.Commander, "Commander"),
                new ExpectedInclude<LocustCommander>(e => e.DefeatedBy, "DefeatedBy", "Commander"),
                new ExpectedInclude<Officer>(e => e.Reports, "Reports", "Commander.DefeatedBy")
            };

            return AssertIncludeQueryAsync<Faction>(
                fs => fs.Include(f => (f as LocustHorde).Commander).ThenInclude(c => (c.DefeatedBy as Officer).Reports),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task ThenInclude_collection_on_derived_after_derived_collection()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Officer>(e => e.Reports, "Reports"),
                new ExpectedInclude<Officer>(e => e.Reports, "Reports", "Reports")
            };

            return AssertIncludeQueryAsync<Gear>(
                gs => gs.Include(g => ((Officer)g).Reports).ThenInclude(g => ((Officer)g).Reports),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task ThenInclude_reference_on_derived_after_derived_collection()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<LocustHorde>(e => e.Leaders, "Leaders"),
                new ExpectedInclude<LocustCommander>(e => e.DefeatedBy, "DefeatedBy", "Leaders")
            };

            return AssertIncludeQueryAsync<Faction>(
                fs => fs.Include(f => ((LocustHorde)f).Leaders).ThenInclude(l => ((LocustCommander)l).DefeatedBy),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Multiple_derived_included_on_one_method()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<LocustHorde>(e => e.Commander, "Commander"),
                new ExpectedInclude<LocustCommander>(e => e.DefeatedBy, "DefeatedBy", "Commander"),
                new ExpectedInclude<Officer>(e => e.Reports, "Reports", "Commander.DefeatedBy")
            };

            return AssertIncludeQueryAsync<Faction>(
                fs => fs.Include(f => (((LocustHorde)f).Commander.DefeatedBy as Officer).Reports),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_on_derived_multi_level()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Officer>(e => e.Reports, "Reports"),
                new ExpectedInclude<Gear>(e => e.Squad, "Squad", "Reports"),
                new ExpectedInclude<Squad>(e => e.Missions, "Missions", "Reports.Squad")
            };

            return AssertIncludeQueryAsync<Gear>(
                gs => gs.Include(g => ((Officer)g).Reports).ThenInclude(g => g.Squad.Missions),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual Task Include_with_concat()
        {
            var expectedIncludes = new List<IExpectedInclude>
            {
                new ExpectedInclude<Gear>(g => g.Squad, "Squad"),
                new ExpectedInclude<Officer>(o => o.Squad, "Squad")
            };

            return AssertIncludeQueryAsync<Gear>(
                gs => gs.Include(g => g.Squad).Concat(gs),
                expectedIncludes);
        }

        [ConditionalFact]
        public virtual async Task Include_with_group_by_on_entity_qsre()
        {
            using (var ctx = CreateContext())
            {
                var query = ctx.Squads.Include(s => s.Members).GroupBy(s => s);
                var results = await query.ToListAsync();

                foreach (var result in results)
                {
                    foreach (var grouping in result)
                    {
                        Assert.True(grouping.Members.Count > 0);
                    }
                }
            }
        }

        [ConditionalFact]
        public virtual async Task Include_with_group_by_on_entity_qsre_with_composite_key()
        {
            using (var ctx = CreateContext())
            {
                var query = ctx.Gears.Include(g => g.Weapons).GroupBy(g => g);
                var results = await query.ToListAsync();

                foreach (var result in results)
                {
                    foreach (var grouping in result)
                    {
                        Assert.True(grouping.Weapons.Count > 0);
                    }
                }
            }
        }

        [ConditionalFact]
        public virtual async Task Include_with_group_by_on_entity_navigation()
        {
            using (var ctx = CreateContext())
            {
                var query = ctx.Factions.OfType<LocustHorde>().Include(lh => lh.Leaders).GroupBy(lh => lh.Commander.DefeatedBy);
                var results = await query.ToListAsync();

                foreach (var result in results)
                {
                    foreach (var grouping in result)
                    {
                        Assert.True(grouping.Leaders.Count > 0);
                    }
                }
            }
        }

        [ConditionalFact]
        public virtual Task Include_with_order_by_constant()
        {
            return AssertIncludeQueryAsync<Squad>(
                ss => ss.Include(s => s.Members).OrderBy(s => 42),
                expectedQuery: ss => ss,
                new List<IExpectedInclude>
                {
                    new ExpectedInclude<Squad>(s => s.Members, "Members")
                });
        }

        [ConditionalFact]
        public virtual async Task Include_groupby_constant()
        {
            using (var ctx = CreateContext())
            {
                var query = ctx.Squads.Include(s => s.Members).GroupBy(s => 1);
                var result = await query.ToListAsync();

                Assert.Equal(1, result.Count);
                var bucket = result[0].ToList();
                Assert.Equal(2, bucket.Count);
                Assert.NotNull(bucket[0].Members);
                Assert.NotNull(bucket[1].Members);
            }
        }

        [ConditionalFact]
        public virtual Task Include_collection_with_complex_OrderBy()
        {
            return AssertIncludeQueryAsync<Gear>(
                os => os.OfType<Officer>()
                    .Include(o => o.Reports)
                    .OrderBy(o => o.Weapons.Count),
                new List<IExpectedInclude>
                {
                    new ExpectedInclude<Officer>(o => o.Reports, "Reports")
                });
        }

        [ConditionalFact]
        public virtual Task Include_collection_with_complex_OrderBy2()
        {
            return AssertIncludeQueryAsync<Gear>(
                os => os.OfType<Officer>()
                    .Include(o => o.Reports)
                    .OrderBy(o => o.Weapons.OrderBy(w => w.Id).FirstOrDefault().IsAutomatic),
                new List<IExpectedInclude>
                {
                    new ExpectedInclude<Officer>(o => o.Reports, "Reports")
                });
        }

        [ConditionalFact]
        public virtual Task Include_collection_with_complex_OrderBy3()
        {
            return AssertIncludeQueryAsync<Gear>(
                os => os.OfType<Officer>()
                    .Include(o => o.Reports)
                    .OrderBy(o => o.Weapons.OrderBy(w => w.Id).Select(w => w.IsAutomatic).FirstOrDefault()),
                new List<IExpectedInclude>
                {
                    new ExpectedInclude<Officer>(o => o.Reports, "Reports")
                });
        }

        [ConditionalFact]
        public virtual async Task Cast_to_derived_type_causes_client_eval()
        {
            using (var context = CreateContext())
            {
                await Assert.ThrowsAsync<InvalidCastException>(
                    () => context.Gears.Cast<Officer>().ToListAsync());
            }
        }

        [ConditionalFact]
        public virtual Task Select_subquery_int_with_inside_cast_and_coalesce()
        {
            return AssertQueryScalarAsync<Gear>(
                gs => gs.Select(g => g.Weapons.OrderBy(w => w.Id).Select(w => (int?)w.Id).FirstOrDefault() ?? 42));
        }

        [ConditionalFact]
        public virtual Task Select_subquery_int_with_outside_cast_and_coalesce()
        {
            return AssertQueryScalarAsync<Gear>(
                gs => gs.Select(g => (int?)g.Weapons.OrderBy(w => w.Id).Select(w => w.Id).FirstOrDefault() ?? 42));
        }

        [ConditionalFact]
        public virtual Task Select_subquery_int_with_pushdown_and_coalesce()
        {
            return AssertQueryScalarAsync<Gear>(
                gs => gs.Select(g => (int?)g.Weapons.OrderBy(w => w.Id).FirstOrDefault().Id ?? 42));
        }
    }
}
