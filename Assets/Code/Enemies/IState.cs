public interface IState
{
    void TimeTick();

    void OnEnter();

    void OnExit();
}
