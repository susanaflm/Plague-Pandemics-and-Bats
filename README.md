# PLAGUE, PANDEMICS & BATS

- [Introduction](#Introduction)
- [Controls](#Controls)
- [Game](#Game)
- [Game Map](#Game-Map)
- [Collision](#Collision)
- [Player](#Player)
- [Projectile](#Projectile)
- [Pick-Ups](#Pick-Ups)
- [Enemies](#Enemies)
- [Cat](#Cat)
- [Dragon](#)
- [Special-Obstacles](#Special-Obstacles)
- [Corona](#FinalBoss)

## Introduction 


Plague, Pandemics & Bats is an original game concept surging from the recent pandemic of the virus COVID-19 on the year 2020, briefly explained, there are two main characters: a female and a male on which the player can choose from; the main objective is to end the plague by finding the big Corona source, on the meantime, gameplay wise, you go through different types of scenarios on which you find 4 types of enemies, the Pink Zombie, the Glass Zombie with the Bat and the Scarf Zombie, with distinct behaviours.


## Controls
The game allows for both types of control movements:

->>**MOVEMENT**

|  |  |  |
| ------ | ------ |------ |
| <kbd>W</kbd> | <kbd>Up</kbd> | Moves Up |
| <kbd>A</kbd> | <kbd>Left</kbd> | Moves Left |
| <kbd>S</kbd> | <kbd>Down</kbd> | Moves Down  |
| <kbd>D</kbd> | <kbd>Right</kbd> | Moves Right |

->>**ATTACKS**

|  |  |  |
| ------ | ------ |------ |
| <kbd>Z</kbd> | <kbd>J</kbd> | Punch |
| <kbd>SPACE</kbd> | | Shoot |
|    <kbd>E</kbd> |  | Dragon Attack  |


## Game

The class _Game_ starts by loading the textures, sounds and create the different components. It also tries to load the _highscore_ from a file, if the file doesn't exist or doesn't have any highscores, the highscore is set to 0.

Each frame, updates and draws the different objects.


## Game Map

The Game Map was created using a external tool called Overlap2D. To load the map into the game it was created the class Scene, that reads the JSON File created by Overlap2D, to create and place the obstacles, player and enemies


## Collision

Collision Manager is a game component that controls every collider and collision in the game. This class is responsible to handle the different collisions according to the collider type and allows other classes to take action according to the object that they are colliding

For instance, it doens't let the player run through obstacles


## Player

The main characters of the game:
**Maria Soto** & **Oliver Buchanan**

The Player starts with 0 points, 0 projectiles, 100 health and a total of 3 lives. Each life is lost by losing all the 100 health point, and the player loses if the lives are all lost. In the early game, the player must punch its enemies to death, until it finds some vaccines laying in the floor

When the player dies, it goes to the last checkpoint, with the ammo and score that the player had when it touched that checkpoint.

The Class Player is responsible for handling its movement. It has some properties such as the player position, direction, collider, score and ammo quantity.

In case the player loses, an event is triggered saving the highscore if the score of the current run is greater than the pre-loaded highscore.


## Projectile

The Player has access to some projectiles, that it can shoot to damage enemies.

The Projectile class is responsible for the projectile's movement and to damage enemies, dealing 10 damage per hit.


## Pick-Ups

During the gameplay, the player will be able to pick up some vaccines that it will add up to its ammo count.

The class handles the collision with the player and conceding it its ammo. Each pick up awards the player with 10 projectiles.


## Enemies

In this Game, exist 4 types of enemies the Pink Zombie, the Spawner Zombie and the Shooter Zombie. The 4th one is spawned by the Spawner Zombie and its a bat.

The abstract class Enemy is responsible for the general instructions, such as collision, update and drawing logic allowing the inherited classes to have its own type of behaviour.

Each Enemy has a different behaviour, the Pink Zombie patrols an area, the Spawner Zombie spawns bats that pursuits the player, and the Shooter Zombie that shoots a projectile towards the player and running when it is too close.


## Cat

Somewhere in the map, the player can find a cat that will follow him around and attack enemies

The class Cat is in charge of handling the movement of the cat, and deciding when to attack a enemy.

To detect an enemy we used an algorithm that filters the game's list of enemies, to the ones that are in the cat's range and then it organizes the filtered list by ordering them from the closest to the furthest enemy, the algorithm will check the enemy that is in the first position and get its position. The cat will now follow the closest enemy only leaving that enemy until he's dead


## Dragon

Special item that the player will be able to find if he crosses some non colliding fences. This dragon is able to perform a single attack, that will insta kill the closest enemy to the player


## Special-Obstacles

The game has its secrets, and with that we implemented some obstacles that the player can cross when meeting a certain condition. For instance, if the player finds the cat, and makes it follow him, the player will now cross the red trees. This can be used to reach areas of the map that couldn't be accessed any other way.

There's also a building that teleports the player to a new position, in which the player will be able to proceed to the rest of the map and find the last house, that it will lead him to the final boss.


## Corona

The final boss of the game, the ultimate and super powerful enemy that is the corona. This final boss is able to sit still and wait for its dead