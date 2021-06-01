using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Landscaper.Core;
using Landscaper.Pieces;
using Landscaper.Tiles;
using Landscaper.Chunks;

public class EncounterController : MonoBehaviour
{
	public LandscaperBoard LandscaperBoard;

	public List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

	public void TakeTurn(Action callback)
	{
		StopAllCoroutines();
		StartCoroutine(CoTakeTurn(callback));
	}

	public void AddPiece(EnemyBehaviour enemyBehaviour)
	{
		enemies.Add(enemyBehaviour);
	}

	IEnumerator CoTakeTurn(Action callback)
	{
		yield return new WaitForSeconds(2.0f);

		// Loop through each enemy
		foreach (EnemyBehaviour enemy in enemies)
		{
			yield return enemy.Act();
		}

		foreach (EnemyBehaviour enemy in EnemyBehaviour.NewEnemies) {
			enemies.Add(enemy);
		}

		EnemyBehaviour.NewEnemies.Clear();

		yield return new WaitForSeconds(2.0f);

		callback.Invoke();

		yield return null;
	}
}
