namespace Story.Models.States
{
    public interface IState
    {
        public int State { get; }
        public void OnEnter();
        public void OnExit();
    }
}