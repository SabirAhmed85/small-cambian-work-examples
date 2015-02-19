CREATE PROCEDURE [dbo].[usp_PivotReportForKPIspreadsheet]
	(@UserID nvarchar(max), @RegionID nvarchar(2), @Day nvarchar(2), @Month nvarchar(2), @Year nvarchar(4))
AS
BEGIN
	DECLARE @pivot_query NVARCHAR(max)
	DECLARE @measures_cols NVARCHAR(max)
	DECLARE @measures_cols2 NVARCHAR(max);
	
	SET @measures_cols = N'';
	SELECT @measures_cols += N', piv.[' + KpiMeasureName + ']' FROM (SELECT KpiMeasureName FROM KpiMeasures WHERE KpiFunctionID <> 7) AS x; 
	SET @measures_cols2 = REPLACE(@measures_cols, ', piv.[', ', [');
	SET @measures_cols2 = SUBSTRING(@measures_cols2, 3, LEN(@measures_cols2));
	
	SET @pivot_query = N'SELECT KpiRegionName AS Region, SiteName As Site' + @measures_cols + N'
FROM
(
	select r.KpiRegionName, si.SiteName, m.KpiMeasureName, s.Value
	from tblSites si 
		inner join KpiRegions r on si.KpiRegionID = r.KpiRegionID
		left join KpiScores s on si.SiteID = s.SiteID
		left join KpiMeasures m on m.KpiMeasureID = s.KpiMeasureID
		inner join tblCambianSystemUsersAccessSites ua on ua.SiteID = si.SiteID
	where (m.KpiFunctionID is null or m.KpiFunctionID <> 7)
		and r.ParentID = ' + @RegionID + '
		and (s.Day is null or s.Day =  ' + @Day + ') and (s.Month is null or s.Month =  ' + @Month + 
		') and (s.Year is null or s.Year =  ' + @Year + ')	--Filtered by week
		and ua.UserID = ' + @UserID + ' and ua.Access = ''1''	
) AS j
PIVOT
(
	SUM(Value) FOR KpiMeasureName IN (' + @measures_cols2 + ')
) AS piv
ORDER BY KpiRegionName
';

	EXEC sp_executesql @pivot_query;
END