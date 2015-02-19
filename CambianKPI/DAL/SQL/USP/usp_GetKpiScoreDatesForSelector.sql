create procedure usp_GetKpiScoreDatesForSelector
	@SiteID int = null
as

begin

-- ensure that Monday is counted as first day of week to simplify calculations
set DATEFIRST 1

select distinct convert(char(10), WeekEndingDate, 103) as Text, convert(char(10), WeekEndingDate, 120) as Value from
(
	-- get the date of the most recent Sunday (assuming SET DATEFIRST 1 has been applied)
	select cast(dateadd(day, -datepart(weekday, getdate()), getdate()) as date) as WeekEndingDate
	union all
	-- get all the dates of scores for the current site (or all sites)
	select distinct cast(cast (10000 * Year + 100 * Month + Day as char) as date)
	from KpiScores
	where SiteID = @SiteID or @SiteID is null
) d
order by convert(char(10), WeekEndingDate, 120) desc

end