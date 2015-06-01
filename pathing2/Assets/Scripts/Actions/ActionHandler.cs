using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameActions {

	public class ActionHandler : MonoBehaviour {
		
		static ActionHandler instanceInternal = null;
		static public ActionHandler instance {
			get {
				if (instanceInternal == null) {
					instanceInternal = Object.FindObjectOfType (typeof (ActionHandler)) as ActionHandler;
					if (instanceInternal == null) {
						GameObject go = new GameObject ("ActionHandler");
						DontDestroyOnLoad (go);
						instanceInternal = go.AddComponent<ActionHandler>();
					}
				}
				return instanceInternal;
			}
		}

		/**
		 * Perform multiple actions when binding to an ActionAcceptor
		 */

		public PerformerAction Bind (IBinder binder) {
			
			if (binder.BoundAcceptor == null) return null;
			
			IActionPerformer performer     = binder as IActionPerformer;
			PerformableActions performable = performer.PerformableActions;
			AcceptableActions acceptable   = binder.BoundAcceptor.AcceptableActions;

			performable.RefreshEnabledActions ();
			acceptable.Bind (performer);
			acceptable.RefreshEnabledActions ();

			PerformerAction matching = null;
			foreach (var action in performable.EnabledActions) {
				AcceptorAction acceptorAction;
				if (acceptable.EnabledActions.TryGetValue (action.Key, out acceptorAction)) {
					PerformerAction performerAction = action.Value;
					matching = performerAction;
					break;
				}
			}

			StartCoroutine (PerformActions (binder, matching));
			return matching;
		}
		
		IEnumerator PerformActions (IBinder binder, PerformerAction action) {
			if (action != null) {
				yield return StartCoroutine (Perform (action));
				Bind (binder);
			} else {
				IActionPerformer performer = binder as IActionPerformer;
				performer.PerformableActions.RefreshEnabledActions ();
				binder.OnEndActions ();
			}
		}

		/**
		 * Perform a single action
		 */

		public void StartAction (PerformerAction action) {
			//Debug.Log ("start action");
			StartCoroutine (Perform (action));
		}

		IEnumerator Perform (PerformerAction action) {
			//Debug.Log ("Perform " + action);
			float time = action.Duration;
			float eTime = 0;

			if (time == 0) {
				action.End ();
				yield break;
			}

			//action.Performing = true;
			//if (action.Performing) yield break;
			action.BindStart ();

			while (eTime < time) {
				eTime += Time.deltaTime;
				action.Progress = eTime / time;
				//action.Perform (eTime / time);
				yield return null;
			}

			//action.End ();
			action.BindEnd ();
		}
	}
}