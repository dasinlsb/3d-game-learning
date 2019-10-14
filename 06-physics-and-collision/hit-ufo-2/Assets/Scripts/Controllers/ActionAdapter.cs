using UnityEngine;
public class ActionAdapter {
    private ActionMode  curActionMode;
    private enum ActionMode
    {
        Kinematics,
        Dynamics,
    }
    
    public ActionAdapter()
    {
        curActionMode = ActionMode.Kinematics;
    }

    public void SwitchActionMode() {
        switch (curActionMode)
        {
            case ActionMode.Kinematics:
                curActionMode = ActionMode.Dynamics;
                break;
            case ActionMode.Dynamics:
                curActionMode = ActionMode.Kinematics;
                break;
            default:
                break;
        }
    }
    
    public Action MakeAction(GameObject ufo, IActionCallback callback)
    {
        var ctl = ufo.GetComponent<UFOController>() as UFOController;
        switch(curActionMode)
        {
            case ActionMode.Kinematics: {
                var action = UFOActionKinematics.GetAction(ctl.speed) as Action;
                action.gameObject = ufo;
                action.transform = ufo.transform;
                action.callback = callback;
                return action;
            }
            case ActionMode.Dynamics: {
                var action = UFOActionDynamics.GetAction(ctl.speed) as Action;
                action.gameObject = ufo;
                action.transform = ufo.transform;
                action.callback = callback;
                return action;
            }
            default:
                break;
        }

        return null;        
    }

}