namespace UnicefEducationMIS.Data.Logging
{
    public interface IDbContextService
    {
        UnicefEduDbContext GetContext();
    }
}
