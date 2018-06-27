// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class AsyncGearsOfWarQuerySqlServerTest : AsyncGearsOfWarQueryTestBase<GearsOfWarQuerySqlServerFixture>
    {
        public AsyncGearsOfWarQuerySqlServerTest(GearsOfWarQuerySqlServerFixture fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            Fixture.TestSqlLoggerFactory.Clear();
            //Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        public override async Task Include_multiple_one_to_one_and_one_to_many()
        {
            await base.Include_multiple_one_to_one_and_one_to_many();

            AssertSql(
                @"SELECT [t].[Id], [t].[GearNickName], [t].[GearSquadId], [t].[Note], [t0].[Nickname], [t0].[SquadId], [t0].[AssignedCityName], [t0].[CityOrBirthName], [t0].[Discriminator], [t0].[FullName], [t0].[HasSoulPatch], [t0].[LeaderNickname], [t0].[LeaderSquadId], [t0].[Rank]
FROM [Tags] AS [t]
LEFT JOIN (
    SELECT [t.Gear].*
    FROM [Gears] AS [t.Gear]
    WHERE [t.Gear].[Discriminator] IN (N'Officer', N'Gear')
) AS [t0] ON ([t].[GearNickName] = [t0].[Nickname]) AND ([t].[GearSquadId] = [t0].[SquadId])
ORDER BY [t0].[FullName]",
                //
                @"SELECT [t.Gear.Weapons].[Id], [t.Gear.Weapons].[AmmunitionType], [t.Gear.Weapons].[IsAutomatic], [t.Gear.Weapons].[Name], [t.Gear.Weapons].[OwnerFullName], [t.Gear.Weapons].[SynergyWithId]
FROM [Weapons] AS [t.Gear.Weapons]
INNER JOIN (
    SELECT DISTINCT [t2].[FullName]
    FROM [Tags] AS [t1]
    LEFT JOIN (
        SELECT [t.Gear0].*
        FROM [Gears] AS [t.Gear0]
        WHERE [t.Gear0].[Discriminator] IN (N'Officer', N'Gear')
    ) AS [t2] ON ([t1].[GearNickName] = [t2].[Nickname]) AND ([t1].[GearSquadId] = [t2].[SquadId])
) AS [t3] ON [t.Gear.Weapons].[OwnerFullName] = [t3].[FullName]
ORDER BY [t3].[FullName]");
        }

        public override async Task Include_multiple_one_to_one_and_one_to_many_self_reference()
        {
            await base.Include_multiple_one_to_one_and_one_to_many_self_reference();

            AssertSql(
                @"SELECT [w].[Id], [w].[AmmunitionType], [w].[IsAutomatic], [w].[Name], [w].[OwnerFullName], [w].[SynergyWithId], [t].[Nickname], [t].[SquadId], [t].[AssignedCityName], [t].[CityOrBirthName], [t].[Discriminator], [t].[FullName], [t].[HasSoulPatch], [t].[LeaderNickname], [t].[LeaderSquadId], [t].[Rank]
FROM [Weapons] AS [w]
LEFT JOIN (
    SELECT [w.Owner].*
    FROM [Gears] AS [w.Owner]
    WHERE [w.Owner].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON [w].[OwnerFullName] = [t].[FullName]
ORDER BY [t].[FullName]",
                //
                @"SELECT [w.Owner.Weapons].[Id], [w.Owner.Weapons].[AmmunitionType], [w.Owner.Weapons].[IsAutomatic], [w.Owner.Weapons].[Name], [w.Owner.Weapons].[OwnerFullName], [w.Owner.Weapons].[SynergyWithId]
FROM [Weapons] AS [w.Owner.Weapons]
INNER JOIN (
    SELECT DISTINCT [t0].[FullName]
    FROM [Weapons] AS [w0]
    LEFT JOIN (
        SELECT [w.Owner0].*
        FROM [Gears] AS [w.Owner0]
        WHERE [w.Owner0].[Discriminator] IN (N'Officer', N'Gear')
    ) AS [t0] ON [w0].[OwnerFullName] = [t0].[FullName]
) AS [t1] ON [w.Owner.Weapons].[OwnerFullName] = [t1].[FullName]
ORDER BY [t1].[FullName]");
        }

        public override async Task Include_multiple_one_to_one_optional_and_one_to_one_required()
        {
            await base.Include_multiple_one_to_one_optional_and_one_to_one_required();

            AssertSql(
                @"SELECT [t].[Id], [t].[GearNickName], [t].[GearSquadId], [t].[Note], [t0].[Nickname], [t0].[SquadId], [t0].[AssignedCityName], [t0].[CityOrBirthName], [t0].[Discriminator], [t0].[FullName], [t0].[HasSoulPatch], [t0].[LeaderNickname], [t0].[LeaderSquadId], [t0].[Rank], [t.Gear.Squad].[Id], [t.Gear.Squad].[InternalNumber], [t.Gear.Squad].[Name]
FROM [Tags] AS [t]
LEFT JOIN (
    SELECT [t.Gear].*
    FROM [Gears] AS [t.Gear]
    WHERE [t.Gear].[Discriminator] IN (N'Officer', N'Gear')
) AS [t0] ON ([t].[GearNickName] = [t0].[Nickname]) AND ([t].[GearSquadId] = [t0].[SquadId])
LEFT JOIN [Squads] AS [t.Gear.Squad] ON [t0].[SquadId] = [t.Gear.Squad].[Id]");
        }

        public override async Task Include_multiple_one_to_one_and_one_to_one_and_one_to_many()
        {
            await base.Include_multiple_one_to_one_and_one_to_one_and_one_to_many();

            AssertSql(
                @"SELECT [t].[Id], [t].[GearNickName], [t].[GearSquadId], [t].[Note], [t0].[Nickname], [t0].[SquadId], [t0].[AssignedCityName], [t0].[CityOrBirthName], [t0].[Discriminator], [t0].[FullName], [t0].[HasSoulPatch], [t0].[LeaderNickname], [t0].[LeaderSquadId], [t0].[Rank], [t.Gear.Squad].[Id], [t.Gear.Squad].[InternalNumber], [t.Gear.Squad].[Name]
FROM [Tags] AS [t]
LEFT JOIN (
    SELECT [t.Gear].*
    FROM [Gears] AS [t.Gear]
    WHERE [t.Gear].[Discriminator] IN (N'Officer', N'Gear')
) AS [t0] ON ([t].[GearNickName] = [t0].[Nickname]) AND ([t].[GearSquadId] = [t0].[SquadId])
LEFT JOIN [Squads] AS [t.Gear.Squad] ON [t0].[SquadId] = [t.Gear.Squad].[Id]
ORDER BY [t.Gear.Squad].[Id]",
                //
                @"SELECT [t.Gear.Squad.Members].[Nickname], [t.Gear.Squad.Members].[SquadId], [t.Gear.Squad.Members].[AssignedCityName], [t.Gear.Squad.Members].[CityOrBirthName], [t.Gear.Squad.Members].[Discriminator], [t.Gear.Squad.Members].[FullName], [t.Gear.Squad.Members].[HasSoulPatch], [t.Gear.Squad.Members].[LeaderNickname], [t.Gear.Squad.Members].[LeaderSquadId], [t.Gear.Squad.Members].[Rank]
FROM [Gears] AS [t.Gear.Squad.Members]
INNER JOIN (
    SELECT DISTINCT [t.Gear.Squad0].[Id]
    FROM [Tags] AS [t1]
    LEFT JOIN (
        SELECT [t.Gear0].*
        FROM [Gears] AS [t.Gear0]
        WHERE [t.Gear0].[Discriminator] IN (N'Officer', N'Gear')
    ) AS [t2] ON ([t1].[GearNickName] = [t2].[Nickname]) AND ([t1].[GearSquadId] = [t2].[SquadId])
    LEFT JOIN [Squads] AS [t.Gear.Squad0] ON [t2].[SquadId] = [t.Gear.Squad0].[Id]
) AS [t3] ON [t.Gear.Squad.Members].[SquadId] = [t3].[Id]
WHERE [t.Gear.Squad.Members].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t3].[Id]");
        }

        public override async Task Include_multiple_circular()
        {
            await base.Include_multiple_circular();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank], [g.CityOfBirth].[Name], [g.CityOfBirth].[Location]
FROM [Gears] AS [g]
INNER JOIN [Cities] AS [g.CityOfBirth] ON [g].[CityOrBirthName] = [g.CityOfBirth].[Name]
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [g.CityOfBirth].[Name]",
                //
                @"SELECT [g.CityOfBirth.StationedGears].[Nickname], [g.CityOfBirth.StationedGears].[SquadId], [g.CityOfBirth.StationedGears].[AssignedCityName], [g.CityOfBirth.StationedGears].[CityOrBirthName], [g.CityOfBirth.StationedGears].[Discriminator], [g.CityOfBirth.StationedGears].[FullName], [g.CityOfBirth.StationedGears].[HasSoulPatch], [g.CityOfBirth.StationedGears].[LeaderNickname], [g.CityOfBirth.StationedGears].[LeaderSquadId], [g.CityOfBirth.StationedGears].[Rank]
FROM [Gears] AS [g.CityOfBirth.StationedGears]
INNER JOIN (
    SELECT DISTINCT [g.CityOfBirth0].[Name]
    FROM [Gears] AS [g0]
    INNER JOIN [Cities] AS [g.CityOfBirth0] ON [g0].[CityOrBirthName] = [g.CityOfBirth0].[Name]
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON [g.CityOfBirth.StationedGears].[AssignedCityName] = [t].[Name]
WHERE [g.CityOfBirth.StationedGears].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[Name]");
        }

        public override async Task Include_multiple_circular_with_filter()
        {
            await base.Include_multiple_circular_with_filter();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank], [g.CityOfBirth].[Name], [g.CityOfBirth].[Location]
FROM [Gears] AS [g]
INNER JOIN [Cities] AS [g.CityOfBirth] ON [g].[CityOrBirthName] = [g.CityOfBirth].[Name]
WHERE [g].[Discriminator] IN (N'Officer', N'Gear') AND ([g].[Nickname] = N'Marcus')
ORDER BY [g.CityOfBirth].[Name]",
                //
                @"SELECT [g.CityOfBirth.StationedGears].[Nickname], [g.CityOfBirth.StationedGears].[SquadId], [g.CityOfBirth.StationedGears].[AssignedCityName], [g.CityOfBirth.StationedGears].[CityOrBirthName], [g.CityOfBirth.StationedGears].[Discriminator], [g.CityOfBirth.StationedGears].[FullName], [g.CityOfBirth.StationedGears].[HasSoulPatch], [g.CityOfBirth.StationedGears].[LeaderNickname], [g.CityOfBirth.StationedGears].[LeaderSquadId], [g.CityOfBirth.StationedGears].[Rank]
FROM [Gears] AS [g.CityOfBirth.StationedGears]
INNER JOIN (
    SELECT DISTINCT [g.CityOfBirth0].[Name]
    FROM [Gears] AS [g0]
    INNER JOIN [Cities] AS [g.CityOfBirth0] ON [g0].[CityOrBirthName] = [g.CityOfBirth0].[Name]
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear') AND ([g0].[Nickname] = N'Marcus')
) AS [t] ON [g.CityOfBirth.StationedGears].[AssignedCityName] = [t].[Name]
WHERE [g.CityOfBirth.StationedGears].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[Name]");
        }

        public override async Task Include_using_alternate_key()
        {
            await base.Include_using_alternate_key();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank]
FROM [Gears] AS [g]
WHERE [g].[Discriminator] IN (N'Officer', N'Gear') AND ([g].[Nickname] = N'Marcus')
ORDER BY [g].[FullName]",
                //
                @"SELECT [g.Weapons].[Id], [g.Weapons].[AmmunitionType], [g.Weapons].[IsAutomatic], [g.Weapons].[Name], [g.Weapons].[OwnerFullName], [g.Weapons].[SynergyWithId]
FROM [Weapons] AS [g.Weapons]
INNER JOIN (
    SELECT [g0].[FullName]
    FROM [Gears] AS [g0]
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear') AND ([g0].[Nickname] = N'Marcus')
) AS [t] ON [g.Weapons].[OwnerFullName] = [t].[FullName]
ORDER BY [t].[FullName]");
        }

        public override async Task Include_multiple_include_then_include()
        {
            await base.Include_multiple_include_then_include();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank], [g.CityOfBirth].[Name], [g.CityOfBirth].[Location], [g.AssignedCity].[Name], [g.AssignedCity].[Location]
FROM [Gears] AS [g]
INNER JOIN [Cities] AS [g.CityOfBirth] ON [g].[CityOrBirthName] = [g.CityOfBirth].[Name]
LEFT JOIN [Cities] AS [g.AssignedCity] ON [g].[AssignedCityName] = [g.AssignedCity].[Name]
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [g].[Nickname], [g.AssignedCity].[Name], [g.CityOfBirth].[Name]",
                //
                @"SELECT [g.AssignedCity.BornGears].[Nickname], [g.AssignedCity.BornGears].[SquadId], [g.AssignedCity.BornGears].[AssignedCityName], [g.AssignedCity.BornGears].[CityOrBirthName], [g.AssignedCity.BornGears].[Discriminator], [g.AssignedCity.BornGears].[FullName], [g.AssignedCity.BornGears].[HasSoulPatch], [g.AssignedCity.BornGears].[LeaderNickname], [g.AssignedCity.BornGears].[LeaderSquadId], [g.AssignedCity.BornGears].[Rank], [g.Tag].[Id], [g.Tag].[GearNickName], [g.Tag].[GearSquadId], [g.Tag].[Note]
FROM [Gears] AS [g.AssignedCity.BornGears]
LEFT JOIN [Tags] AS [g.Tag] ON ([g.AssignedCity.BornGears].[Nickname] = [g.Tag].[GearNickName]) AND ([g.AssignedCity.BornGears].[SquadId] = [g.Tag].[GearSquadId])
INNER JOIN (
    SELECT DISTINCT [g.AssignedCity0].[Name], [g0].[Nickname]
    FROM [Gears] AS [g0]
    INNER JOIN [Cities] AS [g.CityOfBirth0] ON [g0].[CityOrBirthName] = [g.CityOfBirth0].[Name]
    LEFT JOIN [Cities] AS [g.AssignedCity0] ON [g0].[AssignedCityName] = [g.AssignedCity0].[Name]
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON [g.AssignedCity.BornGears].[CityOrBirthName] = [t].[Name]
WHERE [g.AssignedCity.BornGears].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[Nickname], [t].[Name]",
                //
                @"SELECT [g.AssignedCity.StationedGears].[Nickname], [g.AssignedCity.StationedGears].[SquadId], [g.AssignedCity.StationedGears].[AssignedCityName], [g.AssignedCity.StationedGears].[CityOrBirthName], [g.AssignedCity.StationedGears].[Discriminator], [g.AssignedCity.StationedGears].[FullName], [g.AssignedCity.StationedGears].[HasSoulPatch], [g.AssignedCity.StationedGears].[LeaderNickname], [g.AssignedCity.StationedGears].[LeaderSquadId], [g.AssignedCity.StationedGears].[Rank], [g.Tag0].[Id], [g.Tag0].[GearNickName], [g.Tag0].[GearSquadId], [g.Tag0].[Note]
FROM [Gears] AS [g.AssignedCity.StationedGears]
LEFT JOIN [Tags] AS [g.Tag0] ON ([g.AssignedCity.StationedGears].[Nickname] = [g.Tag0].[GearNickName]) AND ([g.AssignedCity.StationedGears].[SquadId] = [g.Tag0].[GearSquadId])
INNER JOIN (
    SELECT DISTINCT [g.AssignedCity1].[Name], [g1].[Nickname]
    FROM [Gears] AS [g1]
    INNER JOIN [Cities] AS [g.CityOfBirth1] ON [g1].[CityOrBirthName] = [g.CityOfBirth1].[Name]
    LEFT JOIN [Cities] AS [g.AssignedCity1] ON [g1].[AssignedCityName] = [g.AssignedCity1].[Name]
    WHERE [g1].[Discriminator] IN (N'Officer', N'Gear')
) AS [t0] ON [g.AssignedCity.StationedGears].[AssignedCityName] = [t0].[Name]
WHERE [g.AssignedCity.StationedGears].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t0].[Nickname], [t0].[Name]",
                //
                @"SELECT [g.CityOfBirth.BornGears].[Nickname], [g.CityOfBirth.BornGears].[SquadId], [g.CityOfBirth.BornGears].[AssignedCityName], [g.CityOfBirth.BornGears].[CityOrBirthName], [g.CityOfBirth.BornGears].[Discriminator], [g.CityOfBirth.BornGears].[FullName], [g.CityOfBirth.BornGears].[HasSoulPatch], [g.CityOfBirth.BornGears].[LeaderNickname], [g.CityOfBirth.BornGears].[LeaderSquadId], [g.CityOfBirth.BornGears].[Rank], [g.Tag1].[Id], [g.Tag1].[GearNickName], [g.Tag1].[GearSquadId], [g.Tag1].[Note]
FROM [Gears] AS [g.CityOfBirth.BornGears]
LEFT JOIN [Tags] AS [g.Tag1] ON ([g.CityOfBirth.BornGears].[Nickname] = [g.Tag1].[GearNickName]) AND ([g.CityOfBirth.BornGears].[SquadId] = [g.Tag1].[GearSquadId])
INNER JOIN (
    SELECT DISTINCT [g.CityOfBirth2].[Name], [g2].[Nickname], [g.AssignedCity2].[Name] AS [Name0]
    FROM [Gears] AS [g2]
    INNER JOIN [Cities] AS [g.CityOfBirth2] ON [g2].[CityOrBirthName] = [g.CityOfBirth2].[Name]
    LEFT JOIN [Cities] AS [g.AssignedCity2] ON [g2].[AssignedCityName] = [g.AssignedCity2].[Name]
    WHERE [g2].[Discriminator] IN (N'Officer', N'Gear')
) AS [t1] ON [g.CityOfBirth.BornGears].[CityOrBirthName] = [t1].[Name]
WHERE [g.CityOfBirth.BornGears].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t1].[Nickname], [t1].[Name0], [t1].[Name]",
                //
                @"SELECT [g.CityOfBirth.StationedGears].[Nickname], [g.CityOfBirth.StationedGears].[SquadId], [g.CityOfBirth.StationedGears].[AssignedCityName], [g.CityOfBirth.StationedGears].[CityOrBirthName], [g.CityOfBirth.StationedGears].[Discriminator], [g.CityOfBirth.StationedGears].[FullName], [g.CityOfBirth.StationedGears].[HasSoulPatch], [g.CityOfBirth.StationedGears].[LeaderNickname], [g.CityOfBirth.StationedGears].[LeaderSquadId], [g.CityOfBirth.StationedGears].[Rank], [g.Tag2].[Id], [g.Tag2].[GearNickName], [g.Tag2].[GearSquadId], [g.Tag2].[Note]
FROM [Gears] AS [g.CityOfBirth.StationedGears]
LEFT JOIN [Tags] AS [g.Tag2] ON ([g.CityOfBirth.StationedGears].[Nickname] = [g.Tag2].[GearNickName]) AND ([g.CityOfBirth.StationedGears].[SquadId] = [g.Tag2].[GearSquadId])
INNER JOIN (
    SELECT DISTINCT [g.CityOfBirth3].[Name], [g3].[Nickname], [g.AssignedCity3].[Name] AS [Name0]
    FROM [Gears] AS [g3]
    INNER JOIN [Cities] AS [g.CityOfBirth3] ON [g3].[CityOrBirthName] = [g.CityOfBirth3].[Name]
    LEFT JOIN [Cities] AS [g.AssignedCity3] ON [g3].[AssignedCityName] = [g.AssignedCity3].[Name]
    WHERE [g3].[Discriminator] IN (N'Officer', N'Gear')
) AS [t2] ON [g.CityOfBirth.StationedGears].[AssignedCityName] = [t2].[Name]
WHERE [g.CityOfBirth.StationedGears].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t2].[Nickname], [t2].[Name0], [t2].[Name]");
        }

        public override async Task Include_navigation_on_derived_type()
        {
            await base.Include_navigation_on_derived_type();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank]
FROM [Gears] AS [g]
WHERE [g].[Discriminator] = N'Officer'
ORDER BY [g].[Nickname], [g].[SquadId]",
                //
                @"SELECT [o.Reports].[Nickname], [o.Reports].[SquadId], [o.Reports].[AssignedCityName], [o.Reports].[CityOrBirthName], [o.Reports].[Discriminator], [o.Reports].[FullName], [o.Reports].[HasSoulPatch], [o.Reports].[LeaderNickname], [o.Reports].[LeaderSquadId], [o.Reports].[Rank]
FROM [Gears] AS [o.Reports]
INNER JOIN (
    SELECT [g0].[Nickname], [g0].[SquadId]
    FROM [Gears] AS [g0]
    WHERE [g0].[Discriminator] = N'Officer'
) AS [t] ON ([o.Reports].[LeaderNickname] = [t].[Nickname]) AND ([o.Reports].[LeaderSquadId] = [t].[SquadId])
WHERE [o.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[Nickname], [t].[SquadId]");
        }

        public override async Task Select_Where_Navigation_Included()
        {
            await base.Select_Where_Navigation_Included();

            AssertSql(
                @"SELECT [o].[Id], [o].[GearNickName], [o].[GearSquadId], [o].[Note], [t].[Nickname], [t].[SquadId], [t].[AssignedCityName], [t].[CityOrBirthName], [t].[Discriminator], [t].[FullName], [t].[HasSoulPatch], [t].[LeaderNickname], [t].[LeaderSquadId], [t].[Rank]
FROM [Tags] AS [o]
LEFT JOIN (
    SELECT [o.Gear].*
    FROM [Gears] AS [o.Gear]
    WHERE [o.Gear].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON ([o].[GearNickName] = [t].[Nickname]) AND ([o].[GearSquadId] = [t].[SquadId])
WHERE [t].[Nickname] = N'Marcus'");
        }

        public override async Task Include_with_join_reference1()
        {
            await base.Include_with_join_reference1();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank], [g.CityOfBirth].[Name], [g.CityOfBirth].[Location]
FROM [Gears] AS [g]
INNER JOIN [Cities] AS [g.CityOfBirth] ON [g].[CityOrBirthName] = [g.CityOfBirth].[Name]
INNER JOIN [Tags] AS [t] ON ([g].[SquadId] = [t].[GearSquadId]) AND ([g].[Nickname] = [t].[GearNickName])
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')");
        }

        public override async Task Include_with_join_reference2()
        {
            await base.Include_with_join_reference2();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank], [g.CityOfBirth].[Name], [g.CityOfBirth].[Location]
FROM [Tags] AS [t]
INNER JOIN [Gears] AS [g] ON ([t].[GearSquadId] = [g].[SquadId]) AND ([t].[GearNickName] = [g].[Nickname])
INNER JOIN [Cities] AS [g.CityOfBirth] ON [g].[CityOrBirthName] = [g.CityOfBirth].[Name]
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')");
        }

        public override async Task Include_with_join_collection1()
        {
            await base.Include_with_join_collection1();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank]
FROM [Gears] AS [g]
INNER JOIN [Tags] AS [t] ON ([g].[SquadId] = [t].[GearSquadId]) AND ([g].[Nickname] = [t].[GearNickName])
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [g].[FullName]",
                //
                @"SELECT [g.Weapons].[Id], [g.Weapons].[AmmunitionType], [g.Weapons].[IsAutomatic], [g.Weapons].[Name], [g.Weapons].[OwnerFullName], [g.Weapons].[SynergyWithId]
FROM [Weapons] AS [g.Weapons]
INNER JOIN (
    SELECT DISTINCT [g0].[FullName]
    FROM [Gears] AS [g0]
    INNER JOIN [Tags] AS [t0] ON ([g0].[SquadId] = [t0].[GearSquadId]) AND ([g0].[Nickname] = [t0].[GearNickName])
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t1] ON [g.Weapons].[OwnerFullName] = [t1].[FullName]
ORDER BY [t1].[FullName]");
        }

        public override async Task Include_with_join_collection2()
        {
            await base.Include_with_join_collection2();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank]
FROM [Tags] AS [t]
INNER JOIN [Gears] AS [g] ON ([t].[GearSquadId] = [g].[SquadId]) AND ([t].[GearNickName] = [g].[Nickname])
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [g].[FullName]",
                //
                @"SELECT [g.Weapons].[Id], [g.Weapons].[AmmunitionType], [g.Weapons].[IsAutomatic], [g.Weapons].[Name], [g.Weapons].[OwnerFullName], [g.Weapons].[SynergyWithId]
FROM [Weapons] AS [g.Weapons]
INNER JOIN (
    SELECT DISTINCT [g0].[FullName]
    FROM [Tags] AS [t0]
    INNER JOIN [Gears] AS [g0] ON ([t0].[GearSquadId] = [g0].[SquadId]) AND ([t0].[GearNickName] = [g0].[Nickname])
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t1] ON [g.Weapons].[OwnerFullName] = [t1].[FullName]
ORDER BY [t1].[FullName]");
        }

        public override async Task Include_reference_on_derived_type_using_string()
        {
            await base.Include_reference_on_derived_type_using_string();

            AssertSql(
                @"SELECT [l].[Name], [l].[Discriminator], [l].[LocustHordeId], [l].[ThreatLevel], [l].[DefeatedByNickname], [l].[DefeatedBySquadId], [l].[HighCommandId], [t].[Nickname], [t].[SquadId], [t].[AssignedCityName], [t].[CityOrBirthName], [t].[Discriminator], [t].[FullName], [t].[HasSoulPatch], [t].[LeaderNickname], [t].[LeaderSquadId], [t].[Rank]
FROM [LocustLeaders] AS [l]
LEFT JOIN (
    SELECT [l.DefeatedBy].*
    FROM [Gears] AS [l.DefeatedBy]
    WHERE [l.DefeatedBy].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON (([l].[Discriminator] = N'LocustCommander') AND ([l].[DefeatedByNickname] = [t].[Nickname])) AND (([l].[Discriminator] = N'LocustCommander') AND ([l].[DefeatedBySquadId] = [t].[SquadId]))
WHERE [l].[Discriminator] IN (N'LocustCommander', N'LocustLeader')");
        }

        public override async Task Include_reference_on_derived_type_using_string_nested1()
        {
            await base.Include_reference_on_derived_type_using_string_nested1();

            AssertSql(
                @"SELECT [l].[Name], [l].[Discriminator], [l].[LocustHordeId], [l].[ThreatLevel], [l].[DefeatedByNickname], [l].[DefeatedBySquadId], [l].[HighCommandId], [t].[Nickname], [t].[SquadId], [t].[AssignedCityName], [t].[CityOrBirthName], [t].[Discriminator], [t].[FullName], [t].[HasSoulPatch], [t].[LeaderNickname], [t].[LeaderSquadId], [t].[Rank], [l.DefeatedBy.Squad].[Id], [l.DefeatedBy.Squad].[InternalNumber], [l.DefeatedBy.Squad].[Name]
FROM [LocustLeaders] AS [l]
LEFT JOIN (
    SELECT [l.DefeatedBy].*
    FROM [Gears] AS [l.DefeatedBy]
    WHERE [l.DefeatedBy].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON (([l].[Discriminator] = N'LocustCommander') AND ([l].[DefeatedByNickname] = [t].[Nickname])) AND (([l].[Discriminator] = N'LocustCommander') AND ([l].[DefeatedBySquadId] = [t].[SquadId]))
LEFT JOIN [Squads] AS [l.DefeatedBy.Squad] ON [t].[SquadId] = [l.DefeatedBy.Squad].[Id]
WHERE [l].[Discriminator] IN (N'LocustCommander', N'LocustLeader')");
        }

        public override async Task Include_reference_on_derived_type_using_string_nested2()
        {
            await base.Include_reference_on_derived_type_using_string_nested2();

            AssertSql(
                @"SELECT [l].[Name], [l].[Discriminator], [l].[LocustHordeId], [l].[ThreatLevel], [l].[DefeatedByNickname], [l].[DefeatedBySquadId], [l].[HighCommandId], [t].[Nickname], [t].[SquadId], [t].[AssignedCityName], [t].[CityOrBirthName], [t].[Discriminator], [t].[FullName], [t].[HasSoulPatch], [t].[LeaderNickname], [t].[LeaderSquadId], [t].[Rank]
FROM [LocustLeaders] AS [l]
LEFT JOIN (
    SELECT [l.DefeatedBy].*
    FROM [Gears] AS [l.DefeatedBy]
    WHERE [l.DefeatedBy].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON (([l].[Discriminator] = N'LocustCommander') AND ([l].[DefeatedByNickname] = [t].[Nickname])) AND (([l].[Discriminator] = N'LocustCommander') AND ([l].[DefeatedBySquadId] = [t].[SquadId]))
WHERE [l].[Discriminator] IN (N'LocustCommander', N'LocustLeader')
ORDER BY [t].[Nickname], [t].[SquadId]",
                //
                @"SELECT [l.DefeatedBy.Reports].[Nickname], [l.DefeatedBy.Reports].[SquadId], [l.DefeatedBy.Reports].[AssignedCityName], [l.DefeatedBy.Reports].[CityOrBirthName], [l.DefeatedBy.Reports].[Discriminator], [l.DefeatedBy.Reports].[FullName], [l.DefeatedBy.Reports].[HasSoulPatch], [l.DefeatedBy.Reports].[LeaderNickname], [l.DefeatedBy.Reports].[LeaderSquadId], [l.DefeatedBy.Reports].[Rank], [g.CityOfBirth].[Name], [g.CityOfBirth].[Location]
FROM [Gears] AS [l.DefeatedBy.Reports]
INNER JOIN [Cities] AS [g.CityOfBirth] ON [l.DefeatedBy.Reports].[CityOrBirthName] = [g.CityOfBirth].[Name]
INNER JOIN (
    SELECT DISTINCT [t0].[Nickname], [t0].[SquadId]
    FROM [LocustLeaders] AS [l0]
    LEFT JOIN (
        SELECT [l.DefeatedBy0].*
        FROM [Gears] AS [l.DefeatedBy0]
        WHERE [l.DefeatedBy0].[Discriminator] IN (N'Officer', N'Gear')
    ) AS [t0] ON (([l0].[Discriminator] = N'LocustCommander') AND ([l0].[DefeatedByNickname] = [t0].[Nickname])) AND (([l0].[Discriminator] = N'LocustCommander') AND ([l0].[DefeatedBySquadId] = [t0].[SquadId]))
    WHERE [l0].[Discriminator] IN (N'LocustCommander', N'LocustLeader')
) AS [t1] ON ([l.DefeatedBy.Reports].[LeaderNickname] = [t1].[Nickname]) AND ([l.DefeatedBy.Reports].[LeaderSquadId] = [t1].[SquadId])
WHERE [l.DefeatedBy.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t1].[Nickname], [t1].[SquadId]");
        }

        public override async Task Include_reference_on_derived_type_using_lambda()
        {
            await base.Include_reference_on_derived_type_using_lambda();

            AssertSql(
                @"SELECT [ll].[Name], [ll].[Discriminator], [ll].[LocustHordeId], [ll].[ThreatLevel], [ll].[DefeatedByNickname], [ll].[DefeatedBySquadId], [ll].[HighCommandId], [t].[Nickname], [t].[SquadId], [t].[AssignedCityName], [t].[CityOrBirthName], [t].[Discriminator], [t].[FullName], [t].[HasSoulPatch], [t].[LeaderNickname], [t].[LeaderSquadId], [t].[Rank]
FROM [LocustLeaders] AS [ll]
LEFT JOIN (
    SELECT [ll.DefeatedBy].*
    FROM [Gears] AS [ll.DefeatedBy]
    WHERE [ll.DefeatedBy].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON (([ll].[Discriminator] = N'LocustCommander') AND ([ll].[DefeatedByNickname] = [t].[Nickname])) AND (([ll].[Discriminator] = N'LocustCommander') AND ([ll].[DefeatedBySquadId] = [t].[SquadId]))
WHERE [ll].[Discriminator] IN (N'LocustCommander', N'LocustLeader')");
        }

        public override async Task Include_reference_on_derived_type_using_lambda_with_soft_cast()
        {
            await base.Include_reference_on_derived_type_using_lambda_with_soft_cast();

            AssertSql(
                @"SELECT [ll].[Name], [ll].[Discriminator], [ll].[LocustHordeId], [ll].[ThreatLevel], [ll].[DefeatedByNickname], [ll].[DefeatedBySquadId], [ll].[HighCommandId], [t].[Nickname], [t].[SquadId], [t].[AssignedCityName], [t].[CityOrBirthName], [t].[Discriminator], [t].[FullName], [t].[HasSoulPatch], [t].[LeaderNickname], [t].[LeaderSquadId], [t].[Rank]
FROM [LocustLeaders] AS [ll]
LEFT JOIN (
    SELECT [ll.DefeatedBy].*
    FROM [Gears] AS [ll.DefeatedBy]
    WHERE [ll.DefeatedBy].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON (([ll].[Discriminator] = N'LocustCommander') AND ([ll].[DefeatedByNickname] = [t].[Nickname])) AND (([ll].[Discriminator] = N'LocustCommander') AND ([ll].[DefeatedBySquadId] = [t].[SquadId]))
WHERE [ll].[Discriminator] IN (N'LocustCommander', N'LocustLeader')");
        }

        public override async Task Include_reference_on_derived_type_using_lambda_with_tracking()
        {
            await base.Include_reference_on_derived_type_using_lambda_with_tracking();

            AssertSql(
                @"SELECT [l].[Name], [l].[Discriminator], [l].[LocustHordeId], [l].[ThreatLevel], [l].[DefeatedByNickname], [l].[DefeatedBySquadId], [l].[HighCommandId], [t].[Nickname], [t].[SquadId], [t].[AssignedCityName], [t].[CityOrBirthName], [t].[Discriminator], [t].[FullName], [t].[HasSoulPatch], [t].[LeaderNickname], [t].[LeaderSquadId], [t].[Rank]
FROM [LocustLeaders] AS [l]
LEFT JOIN (
    SELECT [l.DefeatedBy].*
    FROM [Gears] AS [l.DefeatedBy]
    WHERE [l.DefeatedBy].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON (([l].[Discriminator] = N'LocustCommander') AND ([l].[DefeatedByNickname] = [t].[Nickname])) AND (([l].[Discriminator] = N'LocustCommander') AND ([l].[DefeatedBySquadId] = [t].[SquadId]))
WHERE [l].[Discriminator] IN (N'LocustCommander', N'LocustLeader')");
        }

        public override async Task Include_collection_on_derived_type_using_string()
        {
            await base.Include_collection_on_derived_type_using_string();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank]
FROM [Gears] AS [g]
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [g].[Nickname], [g].[SquadId]",
                //
                @"SELECT [o.Reports].[Nickname], [o.Reports].[SquadId], [o.Reports].[AssignedCityName], [o.Reports].[CityOrBirthName], [o.Reports].[Discriminator], [o.Reports].[FullName], [o.Reports].[HasSoulPatch], [o.Reports].[LeaderNickname], [o.Reports].[LeaderSquadId], [o.Reports].[Rank]
FROM [Gears] AS [o.Reports]
INNER JOIN (
    SELECT [g0].[Nickname], [g0].[SquadId]
    FROM [Gears] AS [g0]
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON ([o.Reports].[LeaderNickname] = [t].[Nickname]) AND ([o.Reports].[LeaderSquadId] = [t].[SquadId])
WHERE [o.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[Nickname], [t].[SquadId]");
        }

        public override async Task Include_collection_on_derived_type_using_lambda()
        {
            await base.Include_collection_on_derived_type_using_lambda();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank]
FROM [Gears] AS [g]
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [g].[Nickname], [g].[SquadId]",
                //
                @"SELECT [g.Reports].[Nickname], [g.Reports].[SquadId], [g.Reports].[AssignedCityName], [g.Reports].[CityOrBirthName], [g.Reports].[Discriminator], [g.Reports].[FullName], [g.Reports].[HasSoulPatch], [g.Reports].[LeaderNickname], [g.Reports].[LeaderSquadId], [g.Reports].[Rank]
FROM [Gears] AS [g.Reports]
INNER JOIN (
    SELECT [g0].[Nickname], [g0].[SquadId]
    FROM [Gears] AS [g0]
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON ([g.Reports].[LeaderNickname] = [t].[Nickname]) AND ([g.Reports].[LeaderSquadId] = [t].[SquadId])
WHERE [g.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[Nickname], [t].[SquadId]");
        }

        public override async Task Include_collection_on_derived_type_using_lambda_with_soft_cast()
        {
            await base.Include_collection_on_derived_type_using_lambda_with_soft_cast();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank]
FROM [Gears] AS [g]
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [g].[Nickname], [g].[SquadId]",
                //
                @"SELECT [g.Reports].[Nickname], [g.Reports].[SquadId], [g.Reports].[AssignedCityName], [g.Reports].[CityOrBirthName], [g.Reports].[Discriminator], [g.Reports].[FullName], [g.Reports].[HasSoulPatch], [g.Reports].[LeaderNickname], [g.Reports].[LeaderSquadId], [g.Reports].[Rank]
FROM [Gears] AS [g.Reports]
INNER JOIN (
    SELECT [g0].[Nickname], [g0].[SquadId]
    FROM [Gears] AS [g0]
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON ([g.Reports].[LeaderNickname] = [t].[Nickname]) AND ([g.Reports].[LeaderSquadId] = [t].[SquadId])
WHERE [g.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[Nickname], [t].[SquadId]");
        }

        public override async Task Include_base_navigation_on_derived_entity()
        {
            await base.Include_base_navigation_on_derived_entity();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank], [g.Tag].[Id], [g.Tag].[GearNickName], [g.Tag].[GearSquadId], [g.Tag].[Note]
FROM [Gears] AS [g]
LEFT JOIN [Tags] AS [g.Tag] ON ([g].[Nickname] = [g.Tag].[GearNickName]) AND ([g].[SquadId] = [g.Tag].[GearSquadId])
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [g].[FullName]",
                //
                @"SELECT [g.Weapons].[Id], [g.Weapons].[AmmunitionType], [g.Weapons].[IsAutomatic], [g.Weapons].[Name], [g.Weapons].[OwnerFullName], [g.Weapons].[SynergyWithId]
FROM [Weapons] AS [g.Weapons]
INNER JOIN (
    SELECT DISTINCT [g0].[FullName]
    FROM [Gears] AS [g0]
    LEFT JOIN [Tags] AS [g.Tag0] ON ([g0].[Nickname] = [g.Tag0].[GearNickName]) AND ([g0].[SquadId] = [g.Tag0].[GearSquadId])
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON [g.Weapons].[OwnerFullName] = [t].[FullName]
ORDER BY [t].[FullName]");
        }

        public override async Task ThenInclude_collection_on_derived_after_base_reference()
        {
            await base.ThenInclude_collection_on_derived_after_base_reference();

            AssertSql(
                @"SELECT [t].[Id], [t].[GearNickName], [t].[GearSquadId], [t].[Note], [t0].[Nickname], [t0].[SquadId], [t0].[AssignedCityName], [t0].[CityOrBirthName], [t0].[Discriminator], [t0].[FullName], [t0].[HasSoulPatch], [t0].[LeaderNickname], [t0].[LeaderSquadId], [t0].[Rank]
FROM [Tags] AS [t]
LEFT JOIN (
    SELECT [t.Gear].*
    FROM [Gears] AS [t.Gear]
    WHERE [t.Gear].[Discriminator] IN (N'Officer', N'Gear')
) AS [t0] ON ([t].[GearNickName] = [t0].[Nickname]) AND ([t].[GearSquadId] = [t0].[SquadId])
ORDER BY [t0].[FullName]",
                //
                @"SELECT [t.Gear.Weapons].[Id], [t.Gear.Weapons].[AmmunitionType], [t.Gear.Weapons].[IsAutomatic], [t.Gear.Weapons].[Name], [t.Gear.Weapons].[OwnerFullName], [t.Gear.Weapons].[SynergyWithId]
FROM [Weapons] AS [t.Gear.Weapons]
INNER JOIN (
    SELECT DISTINCT [t2].[FullName]
    FROM [Tags] AS [t1]
    LEFT JOIN (
        SELECT [t.Gear0].*
        FROM [Gears] AS [t.Gear0]
        WHERE [t.Gear0].[Discriminator] IN (N'Officer', N'Gear')
    ) AS [t2] ON ([t1].[GearNickName] = [t2].[Nickname]) AND ([t1].[GearSquadId] = [t2].[SquadId])
) AS [t3] ON [t.Gear.Weapons].[OwnerFullName] = [t3].[FullName]
ORDER BY [t3].[FullName]");
        }

        public override async Task ThenInclude_collection_on_derived_after_derived_reference()
        {
            await base.ThenInclude_collection_on_derived_after_derived_reference();

            AssertSql(
                @"SELECT [f].[Id], [f].[CapitalName], [f].[Discriminator], [f].[Name], [f].[CommanderName], [f].[Eradicated], [t].[Name], [t].[Discriminator], [t].[LocustHordeId], [t].[ThreatLevel], [t].[DefeatedByNickname], [t].[DefeatedBySquadId], [t].[HighCommandId], [t0].[Nickname], [t0].[SquadId], [t0].[AssignedCityName], [t0].[CityOrBirthName], [t0].[Discriminator], [t0].[FullName], [t0].[HasSoulPatch], [t0].[LeaderNickname], [t0].[LeaderSquadId], [t0].[Rank]
FROM [Factions] AS [f]
LEFT JOIN (
    SELECT [f.Commander].*
    FROM [LocustLeaders] AS [f.Commander]
    WHERE [f.Commander].[Discriminator] = N'LocustCommander'
) AS [t] ON ([f].[Discriminator] = N'LocustHorde') AND ([f].[CommanderName] = [t].[Name])
LEFT JOIN (
    SELECT [f.Commander.DefeatedBy].*
    FROM [Gears] AS [f.Commander.DefeatedBy]
    WHERE [f.Commander.DefeatedBy].[Discriminator] IN (N'Officer', N'Gear')
) AS [t0] ON ([t].[DefeatedByNickname] = [t0].[Nickname]) AND ([t].[DefeatedBySquadId] = [t0].[SquadId])
WHERE [f].[Discriminator] = N'LocustHorde'
ORDER BY [t0].[Nickname], [t0].[SquadId]",
                //
                @"SELECT [f.Commander.DefeatedBy.Reports].[Nickname], [f.Commander.DefeatedBy.Reports].[SquadId], [f.Commander.DefeatedBy.Reports].[AssignedCityName], [f.Commander.DefeatedBy.Reports].[CityOrBirthName], [f.Commander.DefeatedBy.Reports].[Discriminator], [f.Commander.DefeatedBy.Reports].[FullName], [f.Commander.DefeatedBy.Reports].[HasSoulPatch], [f.Commander.DefeatedBy.Reports].[LeaderNickname], [f.Commander.DefeatedBy.Reports].[LeaderSquadId], [f.Commander.DefeatedBy.Reports].[Rank]
FROM [Gears] AS [f.Commander.DefeatedBy.Reports]
INNER JOIN (
    SELECT DISTINCT [t2].[Nickname], [t2].[SquadId]
    FROM [Factions] AS [f0]
    LEFT JOIN (
        SELECT [f.Commander0].*
        FROM [LocustLeaders] AS [f.Commander0]
        WHERE [f.Commander0].[Discriminator] = N'LocustCommander'
    ) AS [t1] ON ([f0].[Discriminator] = N'LocustHorde') AND ([f0].[CommanderName] = [t1].[Name])
    LEFT JOIN (
        SELECT [f.Commander.DefeatedBy0].*
        FROM [Gears] AS [f.Commander.DefeatedBy0]
        WHERE [f.Commander.DefeatedBy0].[Discriminator] IN (N'Officer', N'Gear')
    ) AS [t2] ON ([t1].[DefeatedByNickname] = [t2].[Nickname]) AND ([t1].[DefeatedBySquadId] = [t2].[SquadId])
    WHERE [f0].[Discriminator] = N'LocustHorde'
) AS [t3] ON ([f.Commander.DefeatedBy.Reports].[LeaderNickname] = [t3].[Nickname]) AND ([f.Commander.DefeatedBy.Reports].[LeaderSquadId] = [t3].[SquadId])
WHERE [f.Commander.DefeatedBy.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t3].[Nickname], [t3].[SquadId]");
        }

        public override async Task ThenInclude_collection_on_derived_after_derived_collection()
        {
            await base.ThenInclude_collection_on_derived_after_derived_collection();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank]
FROM [Gears] AS [g]
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [g].[Nickname], [g].[SquadId]",
                //
                @"SELECT [g.Reports].[Nickname], [g.Reports].[SquadId], [g.Reports].[AssignedCityName], [g.Reports].[CityOrBirthName], [g.Reports].[Discriminator], [g.Reports].[FullName], [g.Reports].[HasSoulPatch], [g.Reports].[LeaderNickname], [g.Reports].[LeaderSquadId], [g.Reports].[Rank]
FROM [Gears] AS [g.Reports]
INNER JOIN (
    SELECT [g0].[Nickname], [g0].[SquadId]
    FROM [Gears] AS [g0]
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON ([g.Reports].[LeaderNickname] = [t].[Nickname]) AND ([g.Reports].[LeaderSquadId] = [t].[SquadId])
WHERE [g.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[Nickname], [t].[SquadId], [g.Reports].[Nickname], [g.Reports].[SquadId]",
                //
                @"SELECT [g.Reports.Reports].[Nickname], [g.Reports.Reports].[SquadId], [g.Reports.Reports].[AssignedCityName], [g.Reports.Reports].[CityOrBirthName], [g.Reports.Reports].[Discriminator], [g.Reports.Reports].[FullName], [g.Reports.Reports].[HasSoulPatch], [g.Reports.Reports].[LeaderNickname], [g.Reports.Reports].[LeaderSquadId], [g.Reports.Reports].[Rank]
FROM [Gears] AS [g.Reports.Reports]
INNER JOIN (
    SELECT DISTINCT [g.Reports0].[Nickname], [g.Reports0].[SquadId], [t0].[Nickname] AS [Nickname0], [t0].[SquadId] AS [SquadId0]
    FROM [Gears] AS [g.Reports0]
    INNER JOIN (
        SELECT [g1].[Nickname], [g1].[SquadId]
        FROM [Gears] AS [g1]
        WHERE [g1].[Discriminator] IN (N'Officer', N'Gear')
    ) AS [t0] ON ([g.Reports0].[LeaderNickname] = [t0].[Nickname]) AND ([g.Reports0].[LeaderSquadId] = [t0].[SquadId])
    WHERE [g.Reports0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t1] ON ([g.Reports.Reports].[LeaderNickname] = [t1].[Nickname]) AND ([g.Reports.Reports].[LeaderSquadId] = [t1].[SquadId])
WHERE [g.Reports.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t1].[Nickname0], [t1].[SquadId0], [t1].[Nickname], [t1].[SquadId]");
        }

        public override async Task ThenInclude_reference_on_derived_after_derived_collection()
        {
            await base.ThenInclude_reference_on_derived_after_derived_collection();

            AssertSql(
                @"SELECT [f].[Id], [f].[CapitalName], [f].[Discriminator], [f].[Name], [f].[CommanderName], [f].[Eradicated]
FROM [Factions] AS [f]
WHERE [f].[Discriminator] = N'LocustHorde'
ORDER BY [f].[Id]",
                //
                @"SELECT [f.Leaders].[Name], [f.Leaders].[Discriminator], [f.Leaders].[LocustHordeId], [f.Leaders].[ThreatLevel], [f.Leaders].[DefeatedByNickname], [f.Leaders].[DefeatedBySquadId], [f.Leaders].[HighCommandId], [t].[Nickname], [t].[SquadId], [t].[AssignedCityName], [t].[CityOrBirthName], [t].[Discriminator], [t].[FullName], [t].[HasSoulPatch], [t].[LeaderNickname], [t].[LeaderSquadId], [t].[Rank]
FROM [LocustLeaders] AS [f.Leaders]
LEFT JOIN (
    SELECT [l.DefeatedBy].*
    FROM [Gears] AS [l.DefeatedBy]
    WHERE [l.DefeatedBy].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON (([f.Leaders].[Discriminator] = N'LocustCommander') AND ([f.Leaders].[DefeatedByNickname] = [t].[Nickname])) AND (([f.Leaders].[Discriminator] = N'LocustCommander') AND ([f.Leaders].[DefeatedBySquadId] = [t].[SquadId]))
INNER JOIN (
    SELECT [f0].[Id]
    FROM [Factions] AS [f0]
    WHERE [f0].[Discriminator] = N'LocustHorde'
) AS [t0] ON [f.Leaders].[LocustHordeId] = [t0].[Id]
WHERE [f.Leaders].[Discriminator] IN (N'LocustCommander', N'LocustLeader')
ORDER BY [t0].[Id]");
        }

        public override async Task Multiple_derived_included_on_one_method()
        {
            await base.Multiple_derived_included_on_one_method();

            AssertSql(
                @"SELECT [f].[Id], [f].[CapitalName], [f].[Discriminator], [f].[Name], [f].[CommanderName], [f].[Eradicated], [t].[Name], [t].[Discriminator], [t].[LocustHordeId], [t].[ThreatLevel], [t].[DefeatedByNickname], [t].[DefeatedBySquadId], [t].[HighCommandId], [t0].[Nickname], [t0].[SquadId], [t0].[AssignedCityName], [t0].[CityOrBirthName], [t0].[Discriminator], [t0].[FullName], [t0].[HasSoulPatch], [t0].[LeaderNickname], [t0].[LeaderSquadId], [t0].[Rank]
FROM [Factions] AS [f]
LEFT JOIN (
    SELECT [f.Commander].*
    FROM [LocustLeaders] AS [f.Commander]
    WHERE [f.Commander].[Discriminator] = N'LocustCommander'
) AS [t] ON ([f].[Discriminator] = N'LocustHorde') AND ([f].[CommanderName] = [t].[Name])
LEFT JOIN (
    SELECT [f.Commander.DefeatedBy].*
    FROM [Gears] AS [f.Commander.DefeatedBy]
    WHERE [f.Commander.DefeatedBy].[Discriminator] IN (N'Officer', N'Gear')
) AS [t0] ON ([t].[DefeatedByNickname] = [t0].[Nickname]) AND ([t].[DefeatedBySquadId] = [t0].[SquadId])
WHERE [f].[Discriminator] = N'LocustHorde'
ORDER BY [t0].[Nickname], [t0].[SquadId]",
                //
                @"SELECT [f.Commander.DefeatedBy.Reports].[Nickname], [f.Commander.DefeatedBy.Reports].[SquadId], [f.Commander.DefeatedBy.Reports].[AssignedCityName], [f.Commander.DefeatedBy.Reports].[CityOrBirthName], [f.Commander.DefeatedBy.Reports].[Discriminator], [f.Commander.DefeatedBy.Reports].[FullName], [f.Commander.DefeatedBy.Reports].[HasSoulPatch], [f.Commander.DefeatedBy.Reports].[LeaderNickname], [f.Commander.DefeatedBy.Reports].[LeaderSquadId], [f.Commander.DefeatedBy.Reports].[Rank]
FROM [Gears] AS [f.Commander.DefeatedBy.Reports]
INNER JOIN (
    SELECT DISTINCT [t2].[Nickname], [t2].[SquadId]
    FROM [Factions] AS [f0]
    LEFT JOIN (
        SELECT [f.Commander0].*
        FROM [LocustLeaders] AS [f.Commander0]
        WHERE [f.Commander0].[Discriminator] = N'LocustCommander'
    ) AS [t1] ON ([f0].[Discriminator] = N'LocustHorde') AND ([f0].[CommanderName] = [t1].[Name])
    LEFT JOIN (
        SELECT [f.Commander.DefeatedBy0].*
        FROM [Gears] AS [f.Commander.DefeatedBy0]
        WHERE [f.Commander.DefeatedBy0].[Discriminator] IN (N'Officer', N'Gear')
    ) AS [t2] ON ([t1].[DefeatedByNickname] = [t2].[Nickname]) AND ([t1].[DefeatedBySquadId] = [t2].[SquadId])
    WHERE [f0].[Discriminator] = N'LocustHorde'
) AS [t3] ON ([f.Commander.DefeatedBy.Reports].[LeaderNickname] = [t3].[Nickname]) AND ([f.Commander.DefeatedBy.Reports].[LeaderSquadId] = [t3].[SquadId])
WHERE [f.Commander.DefeatedBy.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t3].[Nickname], [t3].[SquadId]");
        }

        public override async Task Include_on_derived_multi_level()
        {
            await base.Include_on_derived_multi_level();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank]
FROM [Gears] AS [g]
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [g].[Nickname], [g].[SquadId]",
                //
                @"SELECT [g.Reports].[Nickname], [g.Reports].[SquadId], [g.Reports].[AssignedCityName], [g.Reports].[CityOrBirthName], [g.Reports].[Discriminator], [g.Reports].[FullName], [g.Reports].[HasSoulPatch], [g.Reports].[LeaderNickname], [g.Reports].[LeaderSquadId], [g.Reports].[Rank], [g.Squad].[Id], [g.Squad].[InternalNumber], [g.Squad].[Name]
FROM [Gears] AS [g.Reports]
INNER JOIN [Squads] AS [g.Squad] ON [g.Reports].[SquadId] = [g.Squad].[Id]
INNER JOIN (
    SELECT [g0].[Nickname], [g0].[SquadId]
    FROM [Gears] AS [g0]
    WHERE [g0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t] ON ([g.Reports].[LeaderNickname] = [t].[Nickname]) AND ([g.Reports].[LeaderSquadId] = [t].[SquadId])
WHERE [g.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[Nickname], [t].[SquadId], [g.Squad].[Id]",
                //
                @"SELECT [g.Squad.Missions].[SquadId], [g.Squad.Missions].[MissionId]
FROM [SquadMissions] AS [g.Squad.Missions]
INNER JOIN (
    SELECT DISTINCT [g.Squad0].[Id], [t0].[Nickname], [t0].[SquadId]
    FROM [Gears] AS [g.Reports0]
    INNER JOIN [Squads] AS [g.Squad0] ON [g.Reports0].[SquadId] = [g.Squad0].[Id]
    INNER JOIN (
        SELECT [g1].[Nickname], [g1].[SquadId]
        FROM [Gears] AS [g1]
        WHERE [g1].[Discriminator] IN (N'Officer', N'Gear')
    ) AS [t0] ON ([g.Reports0].[LeaderNickname] = [t0].[Nickname]) AND ([g.Reports0].[LeaderSquadId] = [t0].[SquadId])
    WHERE [g.Reports0].[Discriminator] IN (N'Officer', N'Gear')
) AS [t1] ON [g.Squad.Missions].[SquadId] = [t1].[Id]
ORDER BY [t1].[Nickname], [t1].[SquadId], [t1].[Id]");
        }

        public override async Task Include_with_concat()
        {
            await base.Include_with_concat();

            AssertSql(
                @"SELECT [g].[Nickname], [g].[SquadId], [g].[AssignedCityName], [g].[CityOrBirthName], [g].[Discriminator], [g].[FullName], [g].[HasSoulPatch], [g].[LeaderNickname], [g].[LeaderSquadId], [g].[Rank], [g.Squad].[Id], [g.Squad].[InternalNumber], [g.Squad].[Name]
FROM [Gears] AS [g]
INNER JOIN [Squads] AS [g.Squad] ON [g].[SquadId] = [g.Squad].[Id]
WHERE [g].[Discriminator] IN (N'Officer', N'Gear')",
                //
                @"SELECT [g0].[Nickname], [g0].[SquadId], [g0].[AssignedCityName], [g0].[CityOrBirthName], [g0].[Discriminator], [g0].[FullName], [g0].[HasSoulPatch], [g0].[LeaderNickname], [g0].[LeaderSquadId], [g0].[Rank]
FROM [Gears] AS [g0]
WHERE [g0].[Discriminator] IN (N'Officer', N'Gear')");
        }

        public override async Task Include_collection_with_complex_OrderBy()
        {
            await base.Include_collection_with_complex_OrderBy();

            AssertSql(
                @"SELECT [o].[Nickname], [o].[SquadId], [o].[AssignedCityName], [o].[CityOrBirthName], [o].[Discriminator], [o].[FullName], [o].[HasSoulPatch], [o].[LeaderNickname], [o].[LeaderSquadId], [o].[Rank]
FROM [Gears] AS [o]
WHERE [o].[Discriminator] = N'Officer'
ORDER BY (
    SELECT COUNT(*)
    FROM [Weapons] AS [w]
    WHERE [o].[FullName] = [w].[OwnerFullName]
), [o].[Nickname], [o].[SquadId]",
                //
                @"SELECT [o.Reports].[Nickname], [o.Reports].[SquadId], [o.Reports].[AssignedCityName], [o.Reports].[CityOrBirthName], [o.Reports].[Discriminator], [o.Reports].[FullName], [o.Reports].[HasSoulPatch], [o.Reports].[LeaderNickname], [o.Reports].[LeaderSquadId], [o.Reports].[Rank]
FROM [Gears] AS [o.Reports]
INNER JOIN (
    SELECT [o0].[Nickname], [o0].[SquadId], (
        SELECT COUNT(*)
        FROM [Weapons] AS [w0]
        WHERE [o0].[FullName] = [w0].[OwnerFullName]
    ) AS [c]
    FROM [Gears] AS [o0]
    WHERE [o0].[Discriminator] = N'Officer'
) AS [t] ON ([o.Reports].[LeaderNickname] = [t].[Nickname]) AND ([o.Reports].[LeaderSquadId] = [t].[SquadId])
WHERE [o.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[c], [t].[Nickname], [t].[SquadId]");
        }

        public override async Task Include_collection_with_complex_OrderBy2()
        {
            await base.Include_collection_with_complex_OrderBy2();

            AssertSql(
                @"SELECT [o].[Nickname], [o].[SquadId], [o].[AssignedCityName], [o].[CityOrBirthName], [o].[Discriminator], [o].[FullName], [o].[HasSoulPatch], [o].[LeaderNickname], [o].[LeaderSquadId], [o].[Rank]
FROM [Gears] AS [o]
WHERE [o].[Discriminator] = N'Officer'
ORDER BY (
    SELECT TOP(1) [w].[IsAutomatic]
    FROM [Weapons] AS [w]
    WHERE [o].[FullName] = [w].[OwnerFullName]
    ORDER BY [w].[Id]
), [o].[Nickname], [o].[SquadId]",
                //
                @"SELECT [o.Reports].[Nickname], [o.Reports].[SquadId], [o.Reports].[AssignedCityName], [o.Reports].[CityOrBirthName], [o.Reports].[Discriminator], [o.Reports].[FullName], [o.Reports].[HasSoulPatch], [o.Reports].[LeaderNickname], [o.Reports].[LeaderSquadId], [o.Reports].[Rank]
FROM [Gears] AS [o.Reports]
INNER JOIN (
    SELECT [o0].[Nickname], [o0].[SquadId], (
        SELECT TOP(1) [w0].[IsAutomatic]
        FROM [Weapons] AS [w0]
        WHERE [o0].[FullName] = [w0].[OwnerFullName]
        ORDER BY [w0].[Id]
    ) AS [c]
    FROM [Gears] AS [o0]
    WHERE [o0].[Discriminator] = N'Officer'
) AS [t] ON ([o.Reports].[LeaderNickname] = [t].[Nickname]) AND ([o.Reports].[LeaderSquadId] = [t].[SquadId])
WHERE [o.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[c], [t].[Nickname], [t].[SquadId]");
        }

        public override async Task Include_collection_with_complex_OrderBy3()
        {
            await base.Include_collection_with_complex_OrderBy3();

            AssertSql(
                @"SELECT [o].[Nickname], [o].[SquadId], [o].[AssignedCityName], [o].[CityOrBirthName], [o].[Discriminator], [o].[FullName], [o].[HasSoulPatch], [o].[LeaderNickname], [o].[LeaderSquadId], [o].[Rank]
FROM [Gears] AS [o]
WHERE [o].[Discriminator] = N'Officer'
ORDER BY COALESCE((
    SELECT TOP(1) [w].[IsAutomatic]
    FROM [Weapons] AS [w]
    WHERE [o].[FullName] = [w].[OwnerFullName]
    ORDER BY [w].[Id]
), 0), [o].[Nickname], [o].[SquadId]",
                //
                @"SELECT [o.Reports].[Nickname], [o.Reports].[SquadId], [o.Reports].[AssignedCityName], [o.Reports].[CityOrBirthName], [o.Reports].[Discriminator], [o.Reports].[FullName], [o.Reports].[HasSoulPatch], [o.Reports].[LeaderNickname], [o.Reports].[LeaderSquadId], [o.Reports].[Rank]
FROM [Gears] AS [o.Reports]
INNER JOIN (
    SELECT [o0].[Nickname], [o0].[SquadId], CAST(COALESCE((
        SELECT TOP(1) [w0].[IsAutomatic]
        FROM [Weapons] AS [w0]
        WHERE [o0].[FullName] = [w0].[OwnerFullName]
        ORDER BY [w0].[Id]
    ), 0) AS bit) AS [c]
    FROM [Gears] AS [o0]
    WHERE [o0].[Discriminator] = N'Officer'
) AS [t] ON ([o.Reports].[LeaderNickname] = [t].[Nickname]) AND ([o.Reports].[LeaderSquadId] = [t].[SquadId])
WHERE [o.Reports].[Discriminator] IN (N'Officer', N'Gear')
ORDER BY [t].[c], [t].[Nickname], [t].[SquadId]");
        }

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);
    }
}
