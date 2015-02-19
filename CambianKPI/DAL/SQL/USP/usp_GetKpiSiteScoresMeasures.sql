
create procedure [dbo].[usp_GetKpiSiteScoresMeasures]
	@SiteID int, @WeekEndingDate datetime
as
begin

declare @PreviousWeek datetime
set @PreviousWeek = DATEADD(DAY, -7, @WeekEndingDate)
insert KpiScores(KpiMeasureID, SiteID, Year, Month, Day, Value, CreatedByID, CreatedDate, ModifiedByID, ModifiedDate)
select s.KpiMeasureID, s.SiteID, YEAR(@WeekEndingDate) as Year, Month(@WeekEndingDate) as Month, Day(@WeekEndingDate) as Day, s.Value,
	s.CreatedByID, s.CreatedDate, s.ModifiedByID, s.ModifiedDate
from KpiMeasures m
inner join KpiScores s
	on m.KpiMeasureID = s.KpiMeasureID
	and s.SiteID = @SiteID
	and s.Year = YEAR(@PreviousWeek)
	and s.Month = Month(@PreviousWeek)
	and s.Day = Day(@PreviousWeek)
where m.CarryOver = 1
and s.KpiMeasureID not in (
	select s.KpiMeasureID
	from KpiMeasures m
	inner join KpiScores s
		on m.KpiMeasureID = s.KpiMeasureID
		and s.SiteID = @SiteID
		and s.Year = YEAR(@WeekEndingDate)
		and s.Month = Month(@WeekEndingDate)
		and s.Day = Day(@WeekEndingDate)
	where m.CarryOver = 1
)

insert KpiScores (KpiMeasureID, SiteID, Year, Month, Day, Value, CreatedByID, CreatedDate, ModifiedByID, ModifiedDate)
select sq2.KpiMeasureID, sq2.SiteID, sq2.Year, sq2.Month, sq2.Day, sq2.Value, sq2.CreatedByID, sq2.CreatedDate, sq2.ModifiedByID, sq2.ModifiedDate from 
	(select sq.KpiMeasureID, sq.SiteID, sq.Year, sq.Month, sq.Day, sq.Value, sq.CreatedByID, sq.CreatedDate, sq.ModifiedByID, sq.ModifiedDate, ks.KpiMeasureID as KpiMeasureIDFromRight from 
		(select m.KpiMeasureID, @SiteID as SiteID, YEAR(@WeekEndingDate) as Year, MONTH(@WeekEndingDate) as Month, DAY(@WeekEndingDate) as Day, sum(s.Value) as Value, MAX(s.CreatedByID) as CreatedByID, MAX(s.CreatedDate) as CreatedDate, MAX(s.ModifiedByID) as ModifiedByID, MAX(s.ModifiedDate) as ModifiedDate from KpiScores s
			Join KpiMeasures m on s.KpiMeasureID = m.Input1MeasureID
			WHERE	m.KpiFunctionID = 16 and
					s.Year = Year(@WeekEndingDate) and
					s.Month = Month(@WeekEndingDate) and
					s.Day <= Day(@WeekEndingDate) and
					s.SiteID = @SiteID and
					s.KpiMeasureID = m.Input1MeasureID
		group by m.KpiMeasureID) sq 
	LEFT join KpiScores ks on 
		ks.KpiMeasureID = sq.KpiMeasureID and 
		ks.SiteID = @SiteID and 
		ks.Year = YEAR(@WeekEndingDate) and
		ks.Month = MONTH(@WeekEndingDate) and
		ks.Day = DAY(@WeekEndingDate) ) sq2
WHERE sq2.KpiMeasureIDFromRight is null

insert KpiScores (KpiMeasureID, SiteID, Year, Month, Day, Value, CreatedByID, CreatedDate, ModifiedByID, ModifiedDate)
select sq2.KpiMeasureID, sq2.SiteID, sq2.Year, sq2.Month, sq2.Day, sq2.Value, sq2.CreatedByID, sq2.CreatedDate, sq2.ModifiedByID, sq2.ModifiedDate from 
	(select sq.KpiMeasureID, sq.SiteID, sq.Year, sq.Month, sq.Day, sq.Value, sq.CreatedByID, sq.CreatedDate, sq.ModifiedByID, sq.ModifiedDate, ks.KpiMeasureID as KpiMeasureIDFromRight from 
		(select m.KpiMeasureID, @SiteID as SiteID, YEAR(@WeekEndingDate) as Year, MONTH(@WeekEndingDate) as Month, DAY(@WeekEndingDate) as Day, sum(s.Value) as Value, MAX(s.CreatedByID) as CreatedByID, MAX(s.CreatedDate) as CreatedDate, MAX(s.ModifiedByID) as ModifiedByID, MAX(s.ModifiedDate) as ModifiedDate from KpiScores s
			Join KpiMeasures m on s.KpiMeasureID = m.Input1MeasureID
			WHERE	m.KpiFunctionID = 20 and
					s.Year = Year(@WeekEndingDate) and
					( (s.Month < Month(@WeekEndingDate)) or (s.Month = Month(@WeekEndingDate) and s.Day <= Day(@WeekEndingDate) )) and
					s.SiteID = @SiteID and
					s.KpiMeasureID = m.Input1MeasureID
		group by m.KpiMeasureID) sq 
	LEFT join KpiScores ks on 
		ks.KpiMeasureID = sq.KpiMeasureID and 
		ks.SiteID = @SiteID and 
		ks.Year = YEAR(@WeekEndingDate) and
		ks.Month = MONTH(@WeekEndingDate) and
		ks.Day = DAY(@WeekEndingDate) ) sq2
WHERE sq2.KpiMeasureIDFromRight is null

insert KpiScores (KpiMeasureID, SiteID, Year, Month, Day, Value, CreatedByID, CreatedDate, ModifiedByID, ModifiedDate)
select 206 as KpiMeasureID, @SiteID as SiteID, YEAR(@WeekEndingDate) as Year, MONTH(@WeekEndingDate) as Month, DAY(@WeekEndingDate) as Day, sum(Value) as Value, CreatedByID, CreatedDate, ModifiedByID, ModifiedDate from KpiScores m WHERE 
	Year = Year(@PreviousWeek) and
	Month = Month(@PreviousWeek) and
	Day = Day(@PreviousWeek) and
	SiteID = @SiteID and 
	KpiMeasureID = 205
	and 206 not in (select KpiMeasureID from KpiScores where 
		SiteID = @SiteID and 
		Year = Year(@PreviousWeek) and 
		Month = Month(@WeekEndingDate) and 
		Day = Day(@WeekEndingDate)
	)
group by CreatedByID, CreatedDate, ModifiedByID, ModifiedDate;


SELECT				 m.KpiMeasureName, m.KpiMeasureID, m.KpiFunctionID, s.Value, g.KpiGroupName, 
						COALESCE (s.SiteID, @SiteID) AS SiteID, m.Guidance, m.CentralEntry, g.KpiGroupID, m.TargetValue, m.TargetIsMaximum, s.Comment as Comment,
						ma.KpiMeasureID as MonthlyMeasureID, sa.Value as MonthlyValue, m.minThreshold as minThreshold, m.maxThreshold as MaxThreshold, sq3.KpiMeasureID as relatedKpiMeasureIDToHighlight, sq3.validationCount
FROM				KpiMeasures m
INNER JOIN			KpiFunctions f		ON m.KpiFunctionID = f.KpiFunctionID
										AND f.IsAccumulation = 0
INNER JOIN          KpiGroups g			ON m.KpiGroupID = g.KpiGroupID 
LEFT OUTER JOIN		KpiScores s			ON m.KpiMeasureID = s.KpiMeasureID
										-- part of the JOIN specification (not a WHERE clause) to filter by SiteID
										AND s.SiteID = @SiteID
										-- also use the JOIN to filter on the correct date
										AND s.Year = Year(@WeekEndingDate)
										AND s.Month = Month(@WeekEndingDate)
										AND s.Day = Day(@WeekEndingDate)
										-- find the monthly accumulation measures that match each weekly measure
LEFT OUTER JOIN		KpiMeasures ma		ON m.KpiMeasureID = ma.Input1MeasureID
										AND ma.KpiFunctionID = 7
										-- retrieve any scores for those monthly accumulations
LEFT OUTER JOIN		KpiScores sa		ON sa.KpiMeasureID = ma.KpiMeasureID
										AND sa.Year = Year(@WeekEndingDate)
										AND sa.Month = Month(@WeekEndingDate)
										AND sa.Day = Day(@WeekEndingDate)
LEFT OUTER JOIN		(select * from 
						(select t.relatedKpiMeasureID, sq.KpiMeasureID, sq.validationCount from KpiThresholdMeasures t
						LEFT OUTER JOIN 
							(select s.KpiMeasureID, m.validationCount from KpiScores s
							LEFT OUTER JOIN KpiMeasures m 
							on s.KpiMeasureID = m.KpiMeasureID
							where (s.Value < m.minThreshold or s.Value > m.MaxThreshold)
							AND (s.Year = Year(@WeekEndingDate)
							AND s.Month = Month(@WeekEndingDate)
							AND s.Day = Day(@WeekEndingDate)
							AND s.SiteID = @SiteID)) sq 
						on t.KpiMeasureID = sq.KpiMeasureID) sq2
					where sq2.KpiMeasureID > 0 ) sq3 on m.KpiMeasureID = sq3.relatedKpiMeasureID

end