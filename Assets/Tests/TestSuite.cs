using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestSuite
    {
        private Game game;



        private void TearDown()
        {
            Object.Destroy(game.gameObject);
        }


        private void Setup()
        {
            GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
            game = gameGameObject.GetComponent<Game>();
        }


        [UnityTest]
        public IEnumerator AsteroidsMoveDown()
        {
            Setup();

            GameObject asteroids = game.GetSpawner().SpawnAsteroid();

            float initialYPos = asteroids.transform.position.y;

            yield return new WaitForSeconds(0.2f);

            Assert.Less(asteroids.transform.position.y, initialYPos);
            TearDown();
        }


        [UnityTest]
        public IEnumerator GameOverOccursOnAsteroidCollision()
        {
            GameObject asteroid = game.GetSpawner().SpawnAsteroid();
            asteroid.transform.position = game.GetShip().transform.position;

            yield return new WaitForSeconds(0.2f);

            Assert.IsTrue(game.isGameOver);
            TearDown();
        }


        [UnityTest]
        public IEnumerator NewGameRestartGame()
        {
            Setup();

            game.isGameOver = true;
            game.NewGame();

            Assert.IsFalse(game.isGameOver);
            yield return new WaitForSeconds(0.2f);
        }


        [UnityTest]
        public IEnumerator LazerMovesUp()
        {
            Setup();

            GameObject Lazer = game.GetShip().SpawnLaser();
            float InitialYPosition = Lazer.transform.position.y;

            yield return new WaitForSeconds(0.2f);

            Assert.Greater(Lazer.transform.position.y, InitialYPosition);
        }


        [UnityTest]
        public IEnumerator LaserDestroysAsteroid()
        {
            Setup();

            GameObject Asteroid = game.GetSpawner().SpawnAsteroid();
            GameObject Lazer = game.GetShip().SpawnLaser();

            Lazer.transform.position = Vector3.zero;
            Asteroid.transform.position = Vector3.zero;

            yield return new WaitForSeconds(0.2f);

            UnityEngine.Assertions.Assert.IsNull(Asteroid);
        }


        [UnityTest]
        public IEnumerator DestroyedAsteroidRaisesScore()
        {
            Setup();

            GameObject Asteroid = game.GetSpawner().SpawnAsteroid();
            GameObject Lazer = game.GetShip().SpawnLaser();

            Lazer.transform.position = Asteroid.transform.position;

            yield return new WaitForSeconds(0.2f);

            Assert.Greater(game.score, 0);
        }


        [UnityTest]
        public IEnumerator GameOverOccursWhenDifferentTypeOfAsteroidCollisionHappen()
        {
            Setup();

            GameObject Asteroid;
            int Dealth = 0;

            for (int i = 0; i < 4; ++i)
            {
                if (i == 0)
                {
                    Asteroid = MonoBehaviour.Instantiate((GameObject)Resources.Load("Prefabs/Asteroid"));
                }
                else
                {
                    Asteroid = MonoBehaviour.Instantiate((GameObject)Resources.Load("Prefabs/Asteroid" + (i + 1)));
                }

                Asteroid.transform.position = game.GetShip().transform.position;
                yield return new WaitForSeconds(0.2f);

                if(game.isGameOver)
                {
                    ++Dealth;
                }

                game.NewGame();
            }

            Assert.AreEqual(4, Dealth);
        }


        [UnityTest]
        public IEnumerator ScoreResetToZeroWhenNewGameStart()
        {
            Setup();

            GameObject Asteroid = game.GetSpawner().SpawnAsteroid();
            GameObject Lazer = game.GetShip().SpawnLaser();

            Lazer.transform.position = Asteroid.transform.position;

            game.NewGame();
            Assert.AreEqual(0, game.score);

            yield return new WaitForSeconds(0.2f);
        }


        [UnityTest]
        public IEnumerator ShipAbleMoveRight()
        {
            float InitialXPosition = game.GetShip().transform.position.x;
            game.GetShip().MoveRight();

            yield return new WaitForSeconds(0.2f);

            Assert.Greater(game.GetShip().transform.position.x, InitialXPosition);
        }
        
        
        [UnityTest]
        public IEnumerator ShipAbleMoveLeft()
        {
            float InitialXPosition = game.GetShip().transform.position.x;
            game.GetShip().MoveLeft();

            yield return new WaitForSeconds(0.2f);

            Assert.Less(game.GetShip().transform.position.x, InitialXPosition);
        }
    }
}
