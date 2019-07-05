using System;

namespace Ku
{
    public interface ILog
    {
        void AddLog(object text);
    }
    public interface IEventTrigger
    {
        void TriggerEvent(string name, params object[] args);
    }
    public interface ITask
    {
    }
    public interface ITaskHandler
    {
        void AddTask(ITask task);
        void RemoveTask(ITask task);
    }
    public interface IDataHandler
    {
        void Handle(KuBuffer buffer);
    }
    public interface IError
    {
        void OnError(Exception ex);
    }

    //public interface IOHandler
    //{
    //    byte[] Read();
    //    void Write(byte[] temp);
    //}
    
}
