
public enum SSActionEventType:int { Started, Completed }
public interface ISSActionCallback
{
    void SSActionEvent(SSAction action);
}
