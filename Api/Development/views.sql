use  unicefedudb;
CREATE OR REPLACE VIEW FacilityView
AS
SELECT Facility.Id,
	FDC.InstanceId,
    MAX(CASE WHEN EDC.Id = 1 THEN FDC.Value END) AS FacilityCode,
    MAX(CASE WHEN EDC.Id = 2 THEN FDC.Value END) AS Name,
    MAX(CASE WHEN EDC.Id = 3 THEN FDC.Value END) AS TargetedPopulation,
    MAX(CASE WHEN EDC.Id = 4 THEN FDC.Value END) AS FacilityStatus,
    MAX(CASE WHEN EDC.Id = 5 THEN FDC.Value END) AS Latitude,
    MAX(CASE WHEN EDC.Id = 6 THEN FDC.Value END) AS Longitude,
    MAX(CASE WHEN EDC.Id = 7 THEN FDC.Value END) AS Donors,
    MAX(CASE WHEN EDC.Id = 8 THEN FDC.Value END) AS NonEducationPartner,
    MAX(CASE WHEN EDC.Id = 9 THEN FDC.Value END) AS ProgramPartnerId,
    MAX(CASE WHEN EDC.Id = 10 THEN FDC.Value END) AS ImplementationPartnerId,
    MAX(CASE WHEN EDC.Id = 11 THEN FDC.Value END) AS Remarks,
    MAX(CASE WHEN EDC.Id = 12 THEN FDC.Value END) AS UpazilaId,
    MAX(CASE WHEN EDC.Id = 13 THEN FDC.Value END) AS UnionId,
    MAX(CASE WHEN EDC.Id = 14 THEN FDC.Value end) AS CampId,
    MAX(CASE WHEN EDC.Id = 15 THEN FDC.Value END) AS ParaName,
    MAX(CASE WHEN EDC.Id = 16 THEN FDC.Value END) AS BlockId,
    MAX(CASE WHEN EDC.Id = 17 THEN FDC.Value END) AS TeacherId,
    MAX(CASE WHEN EDC.Id = 18 THEN FDC.Value END) AS FacilityType    
FROM Entity_Dynamic_Column EDC
INNER JOIN Facility_Dynamic_Cell FDC ON EDC.Id = FDC.EntityDynamicColumnId
LEFT JOIN Facility ON FDC.FacilityId = Facility.Id
WHERE EDC.IsFixed = 1
GROUP BY FDC.FacilityId, FDC.InstanceId;


Create OR REPLACE view unicefedudb.View_BeneficiaryFixedColumns as
SELECT bdc.InstanceId,bdc.BeneficiaryId as Id ,
Max(case when edc.Id=122 then bdc.Value end) as UnhcrId,
Max(case when edc.Id=123 then bdc.Value end) as Name,
Max(case when edc.Id=124 then bdc.Value end) as FatherName,
Max(case when edc.Id=125 then bdc.Value end) as MotherName,
Max(case when edc.Id=126 then bdc.Value end) as FCNId,
Max(case when edc.Id=127 then bdc.Value end) as DateOfBirth,
Max(case when edc.Id=128 then bdc.Value end) as Sex,
Max(case when edc.Id=129 then bdc.Value end) as Disabled,
Max(case when edc.Id=130 then bdc.Value end) as LevelOfStudy,
Max(case when edc.Id=131 then bdc.Value end) as EnrollmentDate,
Max(case when edc.Id=132 then bdc.Value end) as FacilityId,
Max(case when edc.Id=133 then bdc.Value end) as BeneficiaryCampId,
Max(case when edc.Id=134 then bdc.Value end) as BlockId,
Max(case when edc.Id=135 then bdc.Value end) as SubBlockId,
Max(case when edc.Id=136 then bdc.Value end) as Remarks 

FROM unicefedudb.Beneficiary_Dynamic_Cell bdc 
join unicefedudb.Entity_Dynamic_Column edc on bdc.EntityDynamicColumnId= edc.Id
where edc.EntityTypeId=1 and edc.IsFixed=true
group by bdc.InstanceId,bdc.BeneficiaryId
order by bdc.InstanceId,bdc.BeneficiaryId;



