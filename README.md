# old bruteforcer rewrite 5
(open for name suggestions)

This is a WIP rewrite of my old bruteforcer, which, given a starting y position, searches for strats which satisfy specific conditions, such as reaching a specific y position or y position range.
The rewrite aims to improve many aspects of the old version, such as speed, reliability and usability by fixing known issues, making the GUI a lot cleaner and introducing new features.

## Features
- Start/End Y position range
- Rejump/Landing/Stable Y/Y Position solution conditions
- Starting vspeed
- Floor/Ceiling Y
- Singlejump/Djump
- 0f/1f convention

## Notes
- Floor and ceiling y pos assume 1x1 Dotkid hitbox
- Walkoff strats currently cannot djump, starting vspeed strats without rejump have awkward notation
- Upper and lower y positions can be swapped
- Maximum number of solutions shown is 10000
- Starting Y position range bounds may be incorrect by a bit or two for range solutions
- Y position ranges become slower the larger the range used, using the same upper and lower starting Y position seems to be about 50% slower than searching without the starting y position range setting enabled
- Assumes gm8 physics

## Planned improvements
- Make physics parameters adjustable
- Add more result conditions (like minimum peak)
- Add gms/gms yyc engines
- Add controls to bit increment/decrement values in textboxes
- Improve Sorting + add sorting options
- Make a bunch of hardcoded parameters adjustable through the GUI
- Floor/Ceiling Y dependant on origin
- Save results, save config
- Use results as starting states
