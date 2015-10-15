#undef GENERATE_ALL
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DNA.InventorySystem;
using DNA.Tasks;
using DNA.Paths;

namespace DNA.Units {

	public class GivingTreeUnit : StaticUnit, ITaskPerformer {

		public override string Name {
			get { return "Giving Tree"; }
		}

		public override string Description {
			get { return "The Giving Tree gives birth to Laborers and is also a portal to the next dimension."; }
		}

		PerformableTasks performableTasks;
		public PerformableTasks PerformableTasks {
			get {
				if (performableTasks == null) {
					performableTasks = new PerformableTasks (this);
				}
				return performableTasks;
			}
		}

		List<Vector3> createPositions;
		List<Vector3> CreatePositions {
			get {
				if (createPositions == null) {
					createPositions = new List<Vector3> ();
					int positionCount = 8;
					float radius = 2;
					float deg = 360f / (float)positionCount;
					Vector3 center = StaticTransform.Position;
					for (int i = 0; i < positionCount; i ++) {
						float radians = (float)i * deg * Mathf.Deg2Rad;
						createPositions.Add (new Vector3 (
							center.x + radius * Mathf.Sin (radians),
							center.y,
							center.z + radius * Mathf.Cos (radians)
						));
					}
				}
				return createPositions;
			}
		}

		int positionIndex = 4;

		void Awake () {

			Inventory = Player.Instance.Inventory;

			/*AcceptableActions = new AcceptableActions (this);
			//AcceptableActions.Add (new AcceptDeliverItem<CoffeeHolder> ());
			AcceptableActions.Add (new AcceptDeliverToPlayer<MilkshakeHolder> ());
			AcceptableActions.Add (new AcceptCollectItem<MilkshakeHolder> ());
			//AcceptableActions.Add (new AcceptDeliverAllYears ());*/
			AcceptableTasks.Add (new DNA.Tasks.AcceptDeliverItem<MilkshakeHolder> ());
			AcceptableTasks.Add (new DNA.Tasks.AcceptDeliverItem<CoffeeHolder> ());

			PerformableTasks.Add (new DNA.Tasks.GenerateUnit<Distributor> ()).onComplete += OnGenerateDistributor;
			/*PerformableActions = new PerformableActions (this);
			PerformableActions.OnStartAction += OnStartAction;
			PerformableActions.Add (new GenerateUnit<Distributor, CoffeeHolder> (-1, OnUnitGenerated), "Birth Laborer (15C)");
			#if GENERATE_ALL
			PerformableActions.Add (new GenerateUnit<Elder, CoffeeHolder> (0, OnUnitGenerated), "Birth Elder (temp)");
			PerformableActions.Add (new GenerateUnit<Corpse, CoffeeHolder> (0, OnUnitGenerated), "Birth Corpse (temp)");
			#endif*/
		}

		// Deprecate
		void OnStartAction (string id) {
			if (id == "GenerateDistributor") {
				//PerformableActions.SetActive ("GenerateDistributor", false);
			}
		}

		void OnGenerateDistributor (PerformerTask task) {
			Unit unit = ((GenerateUnit<Distributor>)task).GeneratedUnit;
			unit.Position = CreatePositions[positionIndex];
			if (positionIndex >= CreatePositions.Count-1) {
				positionIndex = 0;
			} else {
				positionIndex ++;
			}
			((MobileUnit)unit).SetStartPoint ((GridPoint)Element);
			//((MobileUnit)unit).Init (PathPoint);
		}

		void OnUnitGenerated (Unit unit) {
			unit.Position = CreatePositions[positionIndex];
			if (positionIndex >= CreatePositions.Count-1) {
				positionIndex = 0;
			} else {
				positionIndex ++;
			}
			//((MobileUnit)unit).Init (PathPoint);
			//((MobileUnit)unit).Init (this);
			//PerformableActions.SetActive ("GenerateDistributor", true);
			//RefreshInfoContent ();
		}

		void OnYearsCollected () {
			ChangeUnit<GivingTreeUnit, GivingTreeRipe> ();
		}
	}
}