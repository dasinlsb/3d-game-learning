using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour {

	private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction> ();
	void Update() {
		List<int> willDelete = new List<int>();
		foreach (var item in this.actions) {
			SSAction action = item.Value;
			if (action.enable) {
				action.Update();
			} else if (action.destroy) {
				willDelete.Add(item.Key);
			}
		}
		foreach (int id in willDelete) {
			var action = this.actions[id];
			this.actions.Remove(id);
			Destroy(action);
		}
	}
	void Start() {

	}
	public void AddAction(SSAction action) {
		this.actions[action.GetInstanceID()] = action;
		action.Start();
	}

}