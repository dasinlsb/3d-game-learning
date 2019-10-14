using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour {

	private Dictionary<int, Action> actions = new Dictionary<int, Action> ();
	void Update() {
		List<int> willDelete = new List<int>();
		foreach (var item in this.actions) 
		{
			Action action = item.Value;
			if (action.enable) 
			{
				action.Update();
			} 
			else if (action.destroy) 
			{
				willDelete.Add(item.Key);
			}
		}
		foreach (int id in willDelete) 
		{
			var action = this.actions[id];
			this.actions.Remove(id);
			Destroy(action);
		}
	}
	void Start() {

	}
	public void AddAction(Action action) {
		this.actions[action.GetInstanceID()] = action;
		action.Start();
	}

	public void RemoveAction(Action action)
	{
		action.enable = false;
		action.destroy = true;
	}

}