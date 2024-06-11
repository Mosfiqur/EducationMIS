namespace UnicefEducationMIS.Core.QueryModel
{
    public class BeneficiaryByFacilityIdQueryModel:BaseQueryModel
    {
        public long FacilityId { get; set; }
        public long InstanceId { get; set; }

    }
}
