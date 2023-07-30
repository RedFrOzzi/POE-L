namespace Database
{
    public class BuffTriggerOnFullStacks : Buff
    {
        public int StacksAmount = 0;
        public int StacksToTrigger = 5;

        public virtual void OnFullStacks() { }
    }

}
