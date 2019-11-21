using System;
using System.Threading;

namespace Ku
{
    public abstract class KuThread
    {
        public Action onStart;
        public Action onStop;
        public Action action;
        public int interval;
        public abstract void Start();
        /// <summary>
        /// 在新线程执行操作
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <param name="interval">延迟多少毫秒执行</param>
        public virtual void Start(Action action, int interval = 0)
        {
            this.action = action;
            this.interval = interval;
            this.Start();
        }
    }
    public class RunThread : KuThread
    {
        public RunThread()
        {
            interval = 0;
        }

        public override void Start()
        {
            new Thread(() =>
            {
                onStart?.Invoke();
                Thread.Sleep(interval);
                action?.Invoke();
                onStop?.Invoke();
            }).Start();
        }
    }

    public class LoopThread : KuThread
    {
        public bool Running { get; private set; } = false;
        public LoopThread()
        {
            interval = 1000;
        }
        public override void Start()
        {
            Running = true;
            new Thread(() =>
            {
                onStart?.Invoke();
                while (Running)
                {
                    Thread.Sleep(interval);
                    action?.Invoke();
                }
                onStop?.Invoke();
            }).Start();
        }
        public void Stop()
        {
            Running = false;
        }
    }
}