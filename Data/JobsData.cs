using PollingWebApi.Models;
using System.Timers;

namespace PollingWebApi.Data
{
    public static class JobsData
    {
        // Dictionary to store timers for each job
        private static Dictionary<Guid, System.Timers.Timer> updateTimers = new Dictionary<Guid, System.Timers.Timer>();

        public static List<Job> JobsList = new List<Job>();


        public static void Update(Guid Id)
        {
            Job job = JobsList.First(x => x.Id == Id);
            job.percentageCompletion += 2;

            if (job.percentageCompletion >= 100)
            {
                // Job is completed, stop further updates
                StopUpdating(Id);
            }
        }        

        public static void StartUpdating(Guid Id)
        {
            if (!updateTimers.ContainsKey(Id))
            {
                var timer = new System.Timers.Timer();
                timer.Interval = 1000; // 3 seconds in milliseconds
                timer.Elapsed += (sender, e) => Update(Id);
                timer.AutoReset = true;
                timer.Start();

                updateTimers[Id] = timer;
            }
        }

        public static void StopUpdating(Guid Id)
        {
            if (updateTimers.ContainsKey(Id))
            {
                System.Timers.Timer timer = updateTimers[Id];
                timer.Stop();
                timer.Dispose();
                updateTimers.Remove(Id);
            }
        }
    }
}
