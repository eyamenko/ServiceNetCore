namespace ServiceNetCore
{
    public abstract class Worker
    {
        public virtual void Start() { }
        public virtual void Stop() { }
    }
}