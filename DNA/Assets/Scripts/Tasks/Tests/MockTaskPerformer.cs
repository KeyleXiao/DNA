﻿using UnityEngine;
using System.Collections;
using GameInventory;
using DNA.Tasks;

public class MockTaskPerformer : MonoBehaviour, ITaskPerformer, IInventoryHolder {

	PerformableTasks performableTasks = null;
	public PerformableTasks PerformableTasks { 
		get {
			if (performableTasks == null) {
				performableTasks = new PerformableTasks (this);
			}
			return performableTasks;
		}
	}
	
	public Inventory Inventory { get; private set; }

	public MockTaskAcceptor taskAcceptor;

	void Awake () {
		
		InitInventory ();		

		//TestGenerate<MilkshakeHolder> (new GenerateItemTest<MilkshakeHolder> ());
		//TestConsume<CoffeeHolder> (new ConsumeItemTest<CoffeeHolder> ());
		//TestAutoStart (new AutoStartTest ());
		//TestRepeat (new RepeatTest ());
		//TestEnabled ();

		/*TestDeliver (
			new DeliverItemTest<YearHolder> (), 
			taskAcceptor.AcceptableTasks.Get<AcceptDeliverItemTest<YearHolder>> ());*/

		/*TestCollect (
			new CollectItemTest<YearHolder> (), 
			taskAcceptor.AcceptableTasks.Get<AcceptCollectItemTest<YearHolder>> ());*/
	}

	void InitInventory () {
		Inventory = new Inventory (this);
		Inventory.Add (new MilkshakeHolder (5, 0));
		Inventory.Add (new CoffeeHolder (5, 5));
		Inventory.Add (new YearHolder (5, 5));
	}

	public void TestAutoStart (PerformerTask autoStart) {

		PerformableTasks.Add (autoStart);
		
		if (!autoStart.Settings.AutoStart)
			throw new System.Exception ("The task '" + autoStart.GetType () + "' will not auto start because its data model has AutoStart set to false");

		if (autoStart.Settings.Duration == 0)
			throw new System.Exception ("The test '" + autoStart.GetType () + "' will fail because its duratio is 0. Set it to something above 0");

		if (autoStart.Performing) 
			Debug.Log ("Auto Start test succeeded :)");
		else
			Debug.Log ("Auto Start test failed :(");
	}

	public void TestRepeat (PerformerTask repeat) {

		PerformableTasks.Add (repeat);
		repeat.onEnd += () => {
			Coroutine.WaitForFixedUpdate (() => {
				if (repeat.Performing) 
					Debug.Log ("Repeat test succeeded :)");
				else
					Debug.Log ("Repeat test failed :(");
				repeat.Stop ();
			});
		};

		if (!repeat.Settings.Repeat)
			throw new System.Exception ("The task '" + repeat.GetType () + "' will not repeat because its data model has Repeat set to false");

		repeat.Start ();
	}

	public void TestEnabled (PerformerTask task=null) {

		if (task != null) {
			task.Start ();
			if ((task.Enabled && task.Performing) || (!task.Enabled && !task.Performing))
				Debug.Log ("Enabled test succeeded :)");
			else
				Debug.Log ("Enabled test failed because enabled is " + task.Enabled + " but performing is " + task.Performing);
			return;
		}

		EnabledTest enabled = new EnabledTest ();
		enabled.enabled = false;
		bool failed = false;

		enabled.Start ();
		if (enabled.Performing) {
			Debug.Log ("Enabled test failed because the task was started but the task is disabled");
			failed = true;
		}

		enabled.enabled = true;
		enabled.Start ();
		if (!enabled.Performing) {
			Debug.Log ("Enabled test failed because the task is enabled but didn't start");
			failed = true;
		}

		if (!failed)
			Debug.Log ("Enabled test succeeded :)");
	}

	public void TestGenerate<T> (GenerateItem<T> gen) where T : ItemHolder {
		gen.onComplete += () => Debug.Log ("Generate Item test succeeded :)");
		PerformableTasks.Add (gen);
		gen.Start ();
		if (!gen.Performing)
			Debug.Log ("Generate Item test failed because the task did not start");
	}

	public void TestConsume<T> (ConsumeItem<T> cons) where T : ItemHolder {
		cons.onComplete += () => Debug.Log ("Consume Item test succeeded :)");
		PerformableTasks.Add (cons);
		cons.Start ();
		if (!cons.Performing)
			Debug.Log ("Consume Item test failed because the task did not start");
	}

	public void TestDeliver<T> (DeliverItem<T> deliver, AcceptDeliverItem<T> acceptDeliver) where T : ItemHolder {

		T holder = Inventory.Get<T> ();
		holder.Initialize (5);
		PerformableTasks.Add (deliver);

		// Make sure the task doesn't start if the acceptor's inventory is full
		taskAcceptor.FillHolder<T> ();
		deliver.Start (acceptDeliver);
		if (deliver.Performing) {
			Debug.Log ("Deliver Item test failed because the task started but acceptor's inventory is full");
			return;
		}

		taskAcceptor.ClearHolder<T> ();
		deliver.onComplete += () => {
			taskAcceptor.ClearHolder<T> ();
			deliver.Start (acceptDeliver);

			// Make sure the task doesn't start if the performer's inventory is empty
			if (deliver.Performing)
				Debug.Log ("Deliver Item test failed bacause the task started but performer's inventory is empty");
			else
				Debug.Log ("Deliver Item test succeeded :)");
		};

		deliver.Start (acceptDeliver);
		if (!deliver.Performing)
			Debug.Log ("Deliver Item test failed because the task did not start");
	}

	public void TestCollect<T> (CollectItem<T> collect, AcceptCollectItem<T> acceptCollect) where T : ItemHolder {
		
		T holder = Inventory.Get<T> ();
		holder.Clear ();
		PerformableTasks.Add (collect);

		// Make sure task doesn't start if the acceptor's inventory is empty
		taskAcceptor.ClearHolder<T> ();
		collect.Start (acceptCollect);
		if (collect.Performing) {
			Debug.Log ("Collect Item test failed because the task started but acceptor's inventory is empty");
			return;
		}

		taskAcceptor.FillHolder<T> ();
		collect.onComplete += () => {
			taskAcceptor.FillHolder<T> ();
			collect.Start (acceptCollect);

			// Make sure the task doesn't start if the performer's inventory is full
			if (collect.Performing)
				Debug.Log ("Collect Item test failed because the task started but performer's inventory is full");
			else
				Debug.Log ("Collect Item test succeeded :)");
		};

		collect.Start (acceptCollect);
		if (!collect.Performing)
			Debug.Log ("Collect Item test failed because the task did not start");
	}
}
