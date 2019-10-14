
public enum ActionEventType:int { Started, Completed }
public interface IActionCallback
{
    void ActionEvent(Action action);
}
