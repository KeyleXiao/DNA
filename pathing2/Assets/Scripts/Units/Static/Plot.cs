﻿using UnityEngine;
using System.Collections;
using GameInventory;
using GameActions;

namespace Units {
	
	public class Plot : StaticUnit, IActionAcceptor, IActionPerformer {

		public override string Name {
			get { return "Plot"; }
		}

		public AcceptableActions AcceptableActions { get; private set; }
		public PerformableActions PerformableActions { get; private set; }

		void Awake () {

			Inventory = new Inventory ();
			Inventory.Add (new MilkHolder (0, 0));

			AcceptableActions = new AcceptableActions (this);
			AcceptableActions.Add ("DeliverMilk", new AcceptDeliverItem<MilkHolder> ());

		}

		void Start () {
			PerformableActions = new PerformableActions (this);
			PerformableActions.StartAction += OnStartAction;
			PerformableActions.Add ("GenerateHouse", new GenerateUnit<House, MilkHolder> (5, Position, OnUnitGenerated), "Birth House");
			PerformableActions.Add ("GenerateMilkPool", new GenerateUnit<MilkPool, MilkHolder> (5, Position, OnUnitGenerated), "Birth Milk Pool");
			PerformableActions.Add ("GeneratePasture", new GenerateUnit<Pasture, MilkHolder> (10, Position, OnUnitGenerated), "Birth Pasture");
			PerformableActions.Add ("GenerateMilkshakeMaker", new GenerateUnit<MilkshakeMaker, MilkHolder> (10, Position, OnUnitGenerated), "Birth Milkshake Maker");
			PerformableActions.Add ("GenerateHospital", new GenerateUnit<Hospital, MilkHolder> (10, Position, OnUnitGenerated), "Birth Hospital");
		}

		void OnStartAction (string id) {
			PerformableActions.DisableAll ();
		}

		void OnUnitGenerated (Unit unit) {
			StaticUnit staticUnit = unit as StaticUnit;
			staticUnit.PathPoint = PathPoint;
			PathPoint.StaticUnitTransform = staticUnit.UnitTransform as StaticUnitTransform;
			ObjectCreator.Instance.Destroy<Plot> (transform);
		}
	}
}