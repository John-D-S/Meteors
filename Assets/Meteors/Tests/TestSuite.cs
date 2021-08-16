using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

using System.Linq;

using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
	private GameObject gameGameObject;
	private Game game;
	private Spawner spawner;
	//private Player player;

	[SetUp]
	public void Setup()
	{
		// Makes the game as a GameObject = gameGameObject.
		gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
		// Gets the GameManager, which is on the game.
		game = gameGameObject.GetComponent<Game>();
		// Gets the AsteroidSpawner, which is a GameObject in the children of the gameGameObject. 
		spawner = game.spawner;
		spawner.isSpawning = false;
		// Gets the Player, which is a GameObject in the children of the gameGameObject. 
		//player = gameGameObject.GetComponentInChildren<Player>();
	}
	
	[TearDown]
	public void Teardown()
	{
		// Destroy the game after the test so nothing is left in the scene.
		Object.Destroy(game.gameObject);
	}
	
	/// <summary>
	/// Passes if the meteors position is different 0.1 seconds after being spawned
	/// </summary>
	[UnityTest]
	public IEnumerator MeteorsMove()
	{
		//spawn meteor
		GameObject meteor = spawner.SpawnMeteor(1);
		//get the initial pos
		Vector2 initialPos = meteor.transform.position;
		//wait a bit for gravity to do its magic
		yield return new WaitForSeconds(0.1f);
		//pass if the meteor's posion has changed
		Assert.AreNotEqual(meteor.transform.position, initialPos);
	}

	[UnityTest]
	public IEnumerator BigMeteorsSplit()
	{
		// Spawns one astroid as asteroid.
		GameObject meteor = spawner.SpawnMeteor(3);
		// Get the Meteor's component.
		Meteor meteorComponent = meteor.GetComponent<Meteor>();
		if(meteorComponent == null)
		{
			Assert.Null(meteorComponent);
		}
		else
		{
			int initialnumberOfMeteors = Object.FindObjectsOfType<Meteor>().Length;
			yield return new WaitForSeconds(0.5f);
			meteorComponent.Hit();
			List<Meteor> newMeteorComponents = Object.FindObjectsOfType<Meteor>().ToList();
			int newNumberOfMeteors = newMeteorComponents.Count;
			bool moreMeteorsThanBefore = newNumberOfMeteors > initialnumberOfMeteors;
			bool newMeteorsAreDifferent = false;
			foreach(Meteor newMeteorComponent in newMeteorComponents)
			{
				if(newMeteorComponent != meteorComponent)
				{
					newMeteorsAreDifferent = true;
				}
			}
			bool meteorsHaveSplit = newMeteorsAreDifferent && moreMeteorsThanBefore;
			Debug.Log($"original number = {initialnumberOfMeteors}, new number = {newNumberOfMeteors}");
			UnityEngine.Assertions.Assert.IsTrue(moreMeteorsThanBefore);
		}
	}
	
	[UnityTest]
	public IEnumerator SmallMeteorsDissappear()
	{
		// Spawns one astroid as asteroid.
		GameObject meteor = spawner.SpawnMeteor(1);
		// Get the Meteor's component.
		Meteor meteorComponent = meteor.GetComponent<Meteor>();
		if(meteorComponent == null)
		{
			Assert.IsNotNull(meteorComponent);
		}
		else
		{
			meteorComponent.Hit();
			yield return new WaitForSeconds(0.5f);
			//Assert.IsNull(meteor); does not work because a destroyed gameobject returns <null> instead of null.
			//GameObjects have built in funcionality for == so that this will return true if the gameobject has been destroyed
			Assert.IsTrue(meteor == null);
		}
	}

	[UnityTest]
	public IEnumerator LifeLostWhenPlayerHit()
	{
		int originalLives = spawner.PlayerSpawns;
		yield return new WaitForSeconds(0.1f);
		Player player = Object.FindObjectOfType<Player>();
		player.Hit();
		yield return new WaitForSeconds(0.1f);
		Assert.Less(spawner.PlayerSpawns, originalLives);
	}

	[UnityTest]
	public IEnumerator LifeLostWhenGroundHit()
	{
		int originalLives = spawner.PlayerSpawns;
		// Spawns a meteor.
		GameObject meteor = spawner.SpawnMeteor(1);
		// Get the meteor's component.
		Meteor meteorComponent = meteor.GetComponent<Meteor>();
		if(meteorComponent == null)
		{
			Assert.IsNotNull(meteorComponent);
		}
		else
		{
			meteorComponent.HitGround();
			yield return new WaitForSeconds(0.5f);
			Assert.Less(spawner.PlayerSpawns, originalLives);
		}
	}
	
	/*[UnityTest]
	public IEnumerator GameOverOccursOnAsteroidCollision()
	{
		// Get the current amount of lives.
		int initialLives = game.lives;
		// Spawns one astroid as asteroid.
		GameObject asteroid = asteroidSpawner.SpawnOneAsteroid();
		// Puts the asteroid ontop of the player.
		asteroid.transform.position = player.gameObject.transform.position;
		// Waits 0.1 Seconds, as yes this is a coroutine.
		yield return new WaitForSeconds(0.1f);
		// Checks if the player has lost lives.
		Assert.Less(game.lives, initialLives);
	}
	
	[UnityTest]
	public IEnumerator NewGameRestartsGame()
	{
		// Makes the isGameOver true.
		game.isGameOver = true;
		// Runs the UIElements 'new game'.
		game.NewGame();
		// Checks if the game over is still true.
		Assert.False(game.isGameOver);
		// Returns after done.
		yield return null;
	}
	*/
}
