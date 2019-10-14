# Hit UFO 2

![gif](./assets/6.gif)

## 规则

点击**START**开始游戏

[红, 黄, 绿, 蓝]颜色分别有[1, 2, 3, 4]分,速度和大小也会相应区分

在第$i$轮满$10*i$分会将分数清零并进入下一轮

(也可以手动点击**NEXT ROUND**)

点击**SWITCH**立即切换小球运动模式

点击**RESTART**重新开始游戏

## 编码

```bash
hit-ufo-2/Assets
├── Resources
│   └── Prefabs
└── Scripts
    ├── Controllers
    │   ├── ActionAdapter.cs
    │   ├── Action.cs
    │   ├── ActionManager.cs
    │   ├── Director.cs
    │   ├── FirstController.cs
    │   ├── IActionCallback.cs
    │   ├── ISceneController.cs
    │   ├── IUserAction.cs
    │   ├── LevelManager.cs
    │   ├── Shooter.cs
    │   ├── UFOActionDynamics.cs
    │   ├── UFOActionKinematics.cs
    │   └── UFOController.cs
    ├── Models
    │   ├── Singleton.cs
    │   └── UFOFactory.cs
    └── Views
        └── UserGUI.cs

```

### ActionAdapter类

负责将UFO不同的运动方式适配使得`ActionManager`可以接受

预设两种运行方式`Kinematics`和`Dynamics`,分别代表匀速直线运行和受垂直向下的重力的运动

```c#
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
```

通过点击按钮最终调用`SwitchActionMode`方法切换运动模式

```c#
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
```

通过`MakeAction`生成可接受的`action`格式,由主场景控制器发送给`actionManager`调度

```c#
/// FirstController 
private void runAction(GameObject ufo, IActionCallback callback)
{
    var action = actionAdapter.MakeAction(ufo, callback);
    startedActions.Add(action);
    actionManager.AddAction(action);
}

/// ActionAdapter
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
```



### LevelManager类

负责维护当前难度和得分

从工厂中获取UFO实例后经由`modifyUfoLevel()`对初始速度,位置,分数等信息进行随机化设置

```c#
    public void modifyUfoLevel(GameObject ufo) 
    {
        var random = new System.Random(Guid.NewGuid().GetHashCode());
        Func<int, int> rndn = (n) => { return random.Next(0, n); };
        Func<int, int> rndpn = (n) => { return random.Next(0, n*2) - n; };
        ufo.transform.position = new Vector3(rndn(maxAxisLen[0]), rndn(maxAxisLen[1]), rndn(maxAxisLen[2]));
        var ufoCtl = ufo.GetComponent<UFOController>();
        ufoCtl.speed = new Vector3(rndpn(10), rndpn(10), rndpn(10)) * (0.05f * (float)curLevel);
        int ufoKind = rndn(ufoFactory.ufoKinds);
        float scale = ufoFactory.ufoScales[ufoKind];
        ufo.transform.localScale = new Vector3(scale, scale, scale);
        ufo.GetComponent<Renderer>().material.color = ufoFactory.ufoColors[ufoKind];
        ufoCtl.score = ufoFactory.ufoScores[ufoKind];
    }
```

在点击得分后经由`CheckScore()`检验是否进入下一关

```c#
    public void CheckScore()
    {
        if (curScore > curLevel * 10) {
            LevelUp();
            sceneController.NextRound();
        }
    }
    public void LevelUp()
    {
        curScore = 0;
        curLevel = curLevel + 1;
    }
```



