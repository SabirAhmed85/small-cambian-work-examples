create procedure usp_GetKpiSites
as
begin

select s.SiteID, s.SiteName, sr.KpiRegionID as SubRegionID, sr.KpiRegionName as SubRegionName, r.KpiRegionID as RegionID, r.KpiRegionName as RegionName
from tblSites s
inner join KpiRegions sr
on sr.KpiRegionID = s.KpiRegionID
inner join KpiRegions r
on r.KpiRegionID = sr.ParentID

end
