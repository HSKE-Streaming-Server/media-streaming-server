using System.Threading.Tasks;
using Quartz;

namespace API.Utility
{
    public class DeleteFilesJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}