namespace app.Jobs.Base;

public interface IJob
{
    IEnumerable<JobDefinition> GetJobDefinitions();
}