// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.EntityFrameworkCore.TestModels.GearsOfWarModel;
using Microsoft.EntityFrameworkCore.TestUtilities.Xunit;

namespace Microsoft.EntityFrameworkCore.Query
{
    public abstract class GearsOfWarQueryTestBase2<TFixture> : QueryTestBase<TFixture>
        where TFixture : GearsOfWarQueryFixtureBase, new()
    {
        protected GearsOfWarQueryTestBase2(TFixture fixture)
            : base(fixture)
        {
        }

        //[ConditionalFact]
        //public virtual void Bitwise_projects_values_in_select()
        //{
        //    AssertFirst<Gear>(
        //        gs => gs
        //            .Where(g => (g.Rank & MilitaryRank.Corporal) == MilitaryRank.Corporal)
        //            .Select(
        //                b => new
        //                {
        //                    BitwiseTrue = (b.Rank & MilitaryRank.Corporal) == MilitaryRank.Corporal,
        //                    BitwiseFalse = (b.Rank & MilitaryRank.Corporal) == MilitaryRank.Sergeant,
        //                    BitwiseValue = b.Rank & MilitaryRank.Corporal
        //                }));
        //}
    }
}
