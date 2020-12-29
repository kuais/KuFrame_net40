
namespace Ku
{
    public class KuTask : ITask
    {
        public enum TaskStatus { Idle, Running, Finished, Failed, Cancel, Timeout };

        private static long _id;

        protected long _taskid;
        protected TaskStatus _taskStatus;
        protected long timeActivated;

        public long Taskid => _taskid;
        public int Timeout { get; set; }
        public TaskStatus Status => _taskStatus;
        public KuModel token { get; set; }

        protected KuTask()
        {
            if (_id == 0xFFFFFFFF) _id = 0;
            _taskid = _id++;
            Timeout = 5000;
        }

        protected virtual KuTask Init()
        {
            _taskStatus = TaskStatus.Idle;
            token.Clear();
            return this;
        }
    }
}
