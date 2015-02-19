create procedure usp_GetKpiSitesForUser
	@UserID int, @WeekEndingDate datetime
as
begin

declare @UserRoleID int
set @UserRoleID = (select RoleID from tblCambianSystemUsers where UserID = @UserID)

select s.SiteID, s.SiteName, sr.KpiRegionID as SubRegionID, sr.KpiRegionName as SubRegionName, r.KpiRegionID as RegionID, r.KpiRegionName as RegionName, 
	data.SubmittedDateTime, data.SignedOffBy, data.Capacity, data.Occupancy, data.OccPercent, data.OccChange,
	data.ShiftVariance, data.AgencyVariance, data.ManagerVariance, data.TeamLeaderVariance, data.RCWVariance,
	data.TotalOffShift, data.TotalOnShift,
	case when data.TotalOnShift > 0 then 100 * data.TotalOffShift / (data.TotalOnShift) else null end as OffShiftPercent,
	cast (
		case
			when @UserID in (s.SiteManagerID, sr.RegionManagerID, r.RegionManagerID) or @UserRoleID = 1 then 1
			else 0
		end
		as bit) as UserCanEdit
from tblSites s
inner join KpiRegions sr
on sr.KpiRegionID = s.KpiRegionID
inner join KpiRegions r
on r.KpiRegionID = sr.ParentID
--Select the required scores and pivot them so that rows become columns in the final output
-- The pivot table returns one row per site, which is then joined back to the main query
left join (
	select sso.SubmittedDateTime, su.Firstname + ' ' + su.Lastname as SignedOffBy, sc.*
	from KpiSiteSignoffs sso
	inner join tblCambianSystemUsers su
	on su.UserID = sso.SubmittedByUserID
	inner join
	(
		select SiteID, [1] as Capacity, [205] as Occupancy, 100 * [224] as OccPercent, [206]-[205] as OccChange,
			[121] as ShiftVariance, [125] as AgencyVariance, [122] as ManagerVariance, [123] as TeamLeaderVariance, [124] as RCWVariance,
			[135] + [139] + [143] + [147] + [151] + [155] as TotalOffShift, [126] as BudgetHours, [116] as TotalOnShift
		from
		(
			select SiteID, KpiMeasureID, Value 
			from KpiScores 
			where Year = Year(@WeekEndingDate)
			and Month = Month(@WeekEndingDate)
			and Day = Day(@WeekEndingDate)
		) src
		pivot
		(
			max(Value)
			for KpiMeasureID in ([1], [205], [224], [206], [121], [125], [122], [123], [124], [135], [139], [143], [147], [151], [155], [126], [116])
		) pvt
	) sc
	on sc.SiteID = sso.SiteID
	and sso.Year = Year(@WeekEndingDate)
	and sso.Month = Month(@WeekEndingDate)
	and sso.Day = Day(@WeekEndingDate)
	and sso.SignoffLevel = 0
	and sso.KpiCollectionID = 0
	and sso.SignoffStatus = 2
) data
on data.SiteID = s.SiteID
-- Display all the sites from any subregions where the user is a Home Manager in any site
where s.KpiRegionID in (select KpiRegionID from tblSites where SiteManagerID = @UserID)
-- Or all the sites from the area (parent region) where the user is Region Manager in any region
or sr.ParentID in (select ParentID from KpiRegions where RegionManagerID = @UserID)
-- Or all the sites from all areas where the user is Ops Director in any area
or r.ParentID in (select ParentID from KpiRegions where RegionManagerID = @UserID)
-- OR show every site when user has System Administrator role (RoleID = 1)
or (select RoleID from tblCambianSystemUsers where UserID = @UserID) = 1

end
