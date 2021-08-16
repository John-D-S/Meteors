using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
	private GameObject gameGameObject;
	private Game game;
	private Spawner spawner;

	[SetUp]
	public void Setup()
	{
		//instantiate the game
		gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
		//sets the game component.
		game = gameGameObject.GetComponent<Game>();
		//set the spawner component.
		spawner = game.spawner;
		//set spawning to false so that it doesn't spawn things automatically.
		spawner.isSpawning = false;
	}
	
	[TearDown]
	public void Teardown()
	{
		//destroy everything in the scene
		GameObject[] everything = GameObject.FindObjectsOfType<GameObject>();
		foreach(GameObject thing in everything)
		{
			if(thing != null)
			{
				Object.Destroy(thing);
			}
		}
	}
	
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
		//spawn a meteor
		GameObject meteor = spawner.SpawnMeteor(3);
		//get the meteor's component.
		Meteor meteorComponent = meteor.GetComponent<Meteor>();
		//make sure the meteor has the meteor component
		if(meteorComponent == null)
		{
			Assert.Null(meteorComponent);
		}
		else
		{
			//get the initial number of meteors and wait a bit
			int initialnumberOfMeteors = Object.FindObjectsOfType<Meteor>().Length;
			yield return new WaitForSeconds(0.5f);
			//call the hit function of the meteor
			meteorComponent.Hit();
			//get all meteor components in the scene now that the meteor has been hit.
			List<Meteor> newMeteorComponents = Object.FindObjectsOfType<Meteor>().ToList();
			//count how many meteors there are
			int newNumberOfMeteors = newMeteorComponents.Count;
			//determine if there are more meteors than before
			bool moreMeteorsThanBefore = newNumberOfMeteors > initialnumberOfMeteors;
			//determine if any of the new meteors are the same as the old one
			bool newMeteorsAreDifferent = false;
			foreach(Meteor newMeteorComponent in newMeteorComponents)
			{
				if(newMeteorComponent != meteorComponent)
				{
					newMeteorsAreDifferent = true;
				}
			}
			//if there are more meteors than before and the meteors are different to the first meteor, the meteor has split.
			bool meteorsHaveSplit = newMeteorsAreDifferent && moreMeteorsThanBefore;
			UnityEngine.Assertions.Assert.IsTrue(meteorsHaveSplit);
		}
	}
	
	[UnityTest]
	public IEnumerator SmallMeteorsDissappear()
	{
		//spawns one small meteor.
		GameObject meteor = spawner.SpawnMeteor(1);
		//get the meteor's component.
		Meteor meteorComponent = meteor.GetComponent<Meteor>();
		if(meteorComponent == null)
		{
			Assert.IsNotNull(meteorComponent);
		}
		else
		{
			//hit the meteor and wait a bit
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
		//get the amount of lives the player starts with
		int originalLives = spawner.PlayerSpawns;
		yield return new WaitForSeconds(0.1f);
		//get the player
		Player player = Object.FindObjectOfType<Player>();
		//hit the player
		player.Hit();
		yield return new WaitForSeconds(0.1f);
		//pass the test if the player lost a life
		Assert.Less(spawner.PlayerSpawns, originalLives);
	}

	[UnityTest]
	public IEnumerator LifeLostWhenGroundHit()
	{
		int originalLives = spawner.PlayerSpawns;
		//spawns a meteor.
		GameObject meteor = spawner.SpawnMeteor(1);
		//get the meteor's component.
		Meteor meteorComponent = meteor.GetComponent<Meteor>();
		//check that the meteor component isn't null
		if(meteorComponent == null)
		{
			Assert.IsNotNull(meteorComponent);
		}
		else
		{
			//hit the ground
			meteorComponent.HitGround();
			yield return new WaitForSeconds(0.5f);
			//check that a life is lost
			Assert.Less(spawner.PlayerSpawns, originalLives);
		}
	}
	
}
