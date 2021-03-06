﻿// using UnityEngine;
using System.Collections;

public class ActPlayerOpenTreasure : Act {
	private Treasure _treasure;

	public ActPlayerOpenTreasure(Player player, Treasure treasure) : base(player) {
		_treasure = treasure;
	}

	protected override IEnumerator Run(MainSystem sys) {
		yield return _treasure.RunAnimation(Actor, sys);
	}

	public override void OnFinished(MainSystem sys) {
		DLog.D("{0} open treasure", Actor);
	}

}
