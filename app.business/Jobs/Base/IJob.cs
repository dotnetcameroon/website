namespace app.business.Jobs.Base;

public interface IJob
{
    IEnumerable<JobDefinition> GetJobDefinitions();
}