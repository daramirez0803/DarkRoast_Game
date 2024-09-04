i. Starting Scene: Main Menu

ii. How to play:
On menu startup start the game.
The player begins in the coffee shop scene, can be exited.
On exiting the coffee shop the player starts in the City scene and can follow the linear level design to reach the end.
Coffee cups are scattered across the world that will heal the player if they are missing health.
Zombies are scattered around the world that can be interacted with and combated with.
Killing zombies give the player "coffee grounds" which can be used to level up the players HP, stamina, and attack through the Stats menu option
The player is able to enter the gas station by walking up to the door and interacting.
The end of the level has the player confront a boss enemy, which on defeat wins the game.

iii. Known Problem Areas
The camera can exhibit a strange behavior if the player is accidentally locks onto an enemy that is behind another object.
The dead zombies can infrequently jitter on the ground.

iv. Manifest
-----------------------------------------------------------
Eddy Gao

SCRIPTS:
PlayerInputHandler
PlayerActionHandler
PlayerLocomotionHandler
WeaponAttackHandler
ZombieAttackHandler
FreeCameraHandler
-Revamped menu/pause related scripts:
MainMenu
MenuEventManager
MenuManager
MenuNavigation

ASSETS:
Barista (the player object)
PlayerAnimator + all animations used by this animator
CoreSystem Prefab
HUD Prefab
-----------------------------------------------------------
Patrick Connor 

SCENES:
MainMenu

SCRIPTS:
BackgroundMusic_Script.cs
BlackWhiteCycle.cs
HealingItem.cs
MainMenu.cs
MovingLight.cs
OptionsMenu.cs
PauseMenu.cs
PersistentValues.cs
Player_Stats.cs [co-authored with Daniel]
RainbowCycle.cs
SoundSettings.cs

GAMEOBJECTS:
All menu screens and their interactivity
-----------------------------------------------------------
Qing Yang

SCENES:
CoffeeShop
City
GasStation

SCRIPTS:
Dialogue
DialogueController
IdleAnimations
LevelController
LevelSpawnController
LevelStateController
PlayerSpawnController

ASSETS:
All assets used for building three scenes:
CoffeeShop
City
GasStation
LevelChange trigger
PopupBox UI for triggers
Dialogue UI for in-game dialogue
Zombie enemies
Audio (steam, BackgroundSound, forest)
Particle effects (steam, smoke, fog, fire)

ZombieController + animations used by this controller
-----------------------------------------------------------
Jiajia Chen

SCRIPTS:
CurrencyManager.cs
HealingItem.cs
PlayerHealth.cs
PlayerStaminaBar.cs
UIManager.cs

ASSETS:
HUD(Stamina bar, healthe bar and currency text)
Pills for healing
Audios(player attack, zombie and healing)
GameManager(handle health bar and currency text)
-----------------------------------------------------------
Daniel Ramirez

SCENES:
Coffee Shop
City
Gas Station

SCRIPTS:
EnemyStatusReporter.cs
GameWinCondition.cs
MenuEventManager.cs
MenuManager.cs
TargetLockHandler.cs
WinLossManager.cs
WinLossUI.cs
ZombieAI.cs
ZombieSoundHandler.cs


ASSETS:
Better Zombie (Prefab)
Better Zombie Boss (Prefab)
Sounds for player and zombies
-----------------------------------------------------------

