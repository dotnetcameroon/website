using System.Linq.Expressions;

namespace app.business.Jobs.Base;
public class JobDefinition
{
    public Expression<Action> MethodCall { get; }
    public string Cron { get; }
    public string Name { get; }

    private JobDefinition(string jobName, string cron, Expression<Action> methodCall)
    {
        Cron = cron;
        MethodCall = methodCall;
        Name = jobName;
    }

    public static JobDefinition Create(string jobName, string cron, Expression<Action> methodCall)
    {
        return new JobDefinition(jobName, cron, methodCall);
    }
}
