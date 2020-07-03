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


Plague, Pandemics & Bats is an original game concept surging from the recent pandemic of the virus COVID-19 on the year 2020, briefly explained, 
there are two main characters: a female and a male on which the player can choose from; 
the main objective is to end the plague by finding the big Corona source, on the meantime, gameplay wise, you go through 
different types of scenarios on which you find 4 types of enemies, the `Pink Zombie`, the `Glass Zombie` with the `Bat` and the `Scarf Zombie`, each having distinct behaviours.

Furthermore, the `Player` can obtain certain friendlies that will help him/her go through the pandemic such as the `Cat` and the Easter Egg `Dragon`. 


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

The class `Game` starts by loading the textures, sounds and create the different components,  also tries to load the _highscore_ from a file, if the file doesn't exist or doesn't have any highscores, the highscore is set to 0.

Each frame updates and draws the different objects.


## Game Map

To create the game map we resorted to an external source called Overlap2D; we load the map through the class `Scene` which reads the JSON File from Overlap2D, generating and placing the obstacles, items and characters.


## Collision

`CollisionManager` is a game component that manages and runs every collider & collision in the game in accordance with the _type_ of collider and collision, therefore, 
actioning in proportion to which object they are colliding with. 

For instance, it restrains the `Player` from running into obstacles.


## Player

The main characters of the game:
**Maria Soto** & **Oliver Buchanan**

The class `Player` is responsible for the moviment of the player and assures the user starts with 0 points, 0 projectiles, 100 health and a total of 3 lives. 

[**LIVES system**]: 

Everytime the `Player` reaches 0 health one live is removed.

[**CHECKPOINT system**]: 

The `Player` has a few checkpoints he can get back to, in case of death the user goes back to the closest checkpoint he/her interacted with along with the items it had upon touching the checkpoint.

[**MOVEMENT // ECONOMY**]: 


The projectiles [[Pick-Ups](#Pick-Ups)] the player is given are scarce reeinforcing a sense of economy of these to later fight the boss [[Corona](#FinalBoss)], however, in order to get through the levels of heavy `Enemy` presence we've implemented a punch ability.
This class benefits from properties such as the player position & direction, collider, score and ammo quantity.

Reaching 0 lives triggers an event upon loss saving the highscore and opening the Game Over screen along with the option to restart.

```cs
Player.OnPlayerLose += () =>
            {
               SaveHighScore(_player.Highscore);
                _gameState = GameState.GameOver;
            };
```

## Projectile


The `Projectile` class is responsible for its movement and damage towards enemies, each projectile deals 10 damage per hit, the `Player` is the only character able to benefit from these.


## Pick-Ups
Class `Ammo`


During the gameplay, the `Player` will be able to pick up only **10** vaccines at a time to further increment its ammo count.

This class handles the collision with the `Player` and conceding it its ammo.


## Enemies
Class `Enemy`
Main class responsable to attribute damage, score, health and certain global methods to all types of enemies in our game; these are categorized and separated in other subclasses inhereting from `Enemy`.

  - Pink Zombie
  - Spawner Zombie
  - Shooter Zombie
  - Bat 


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