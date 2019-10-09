# Hit UFO

![img](assets/hw5.gif)

> 由于在虚拟机中录制,鼠标位置和实际有出入

## 规则

点击**START**开始游戏

[红, 黄, 绿, 蓝]颜色分别有[1, 2, 3, 4]分,速度和大小也会相应区分

在第$i$轮满$10*i$分会将分数清零并进入下一轮

(也可以手动点击**NEXT ROUND**)

点击**RESTART**重新开始游戏

## 编码

```bash
hit-ufo/Assets
├── Resources
│   └── Prefabs
└── Scripts
    ├── Controllers
    │   ├── FirstController.cs
    │   ├── ISceneController.cs
    │   ├── IUserAction.cs
    │   ├── SSDirector.cs
    │   └── UFOController.cs
    ├── Models
    │   ├── Singleton.cs
    │   └── UFOFactory.cs
    └── Views
        └── UserGUI.cs

```

### UFOFactory类

```c#
Queue<GameObject> ufoPool = new Queue<GameObject>();

public void Withdraw(GameObject ufo);

public GameObject Launch();
```

用队列结构缓存离开可视范围的UFO

`Withdraw()`负责将离开可视范围的UFO缓存到队列中

`Launch()`负责将队列中(若有)或新生成的UFO返回给主场记

### UFOController类

```c#

public class UFOController : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    public int score;
    public Color color;
    private void Update() 
    {
        this.transform.position += speed * Time.deltaTime * direction;
    }
}
```

负责记录单个UFO的速度,方向,分数,颜色等信息,以及更新其位置保持匀速直线运动

### FirstController类

```c#

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    void Update()
      {
      	// 通过修改游戏状态的方式实现一些加锁功能
        if (gameState != GameState.Playing)
        {
          return;
        }
        
        // 将离开可视范围的UFO删除
        List<GameObject> invisibleUfos = new List<GameObject>();
        foreach(var obj in visibleUfos) 
        {
          if (!isInView(obj.transform.position))
          {
            invisibleUfos.Add(obj);
          }
        }
        foreach (var ufo in invisibleUfos) 
        {
          ufoFactory.Withdraw(ufo);
          visibleUfos.Remove(ufo);
        }
        
        // 检测鼠标点击是否击中物体
        if (Input.GetButtonDown("Fire1")) 
        {
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          RaycastHit hit;
          if (Physics.Raycast(ray, out hit))
          {        
            GameObject ufo = hit.transform.gameObject;
            curScore = curScore + ufo.GetComponent<UFOController>().score;
            this.visibleUfos.Remove(ufo);
            ufoFactory.Withdraw(ufo);
          }
        }
        
        // 补充UFO
        for(; visibleUfos.Count < curLevel + 2;)
        {
          GameObject newUfo = ufoFactory.Launch(this.curLevel);
          modifyUfoLevel(newUfo);
          visibleUfos.Add(newUfo);
        }
        
        // 分数足够后自动进入下一关
        if (curScore > curLevel * 10)
        {
          levelUp = true;
          curScore = 0;
          NextRound();
        }
      }
    }
```

