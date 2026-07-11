# SkyFALL
SkyFALL is a third-person mech shooter game developed in Unity. This repository contains the core scripts used in the project.

## Project Status

This commit marks the completion of the first full playable stage of the game. The core gameplay loop is now in place end-to-end:

- **Training Room** — tutorial flow covering movement, aiming, melee, gun, and mech interactions
- **Outdoor Scene** — open exploration area with elevator traversal and environment interactables
- **Boss Fight** — complete boss encounter with phase logic, missile attacks, respawn/checkpoint handling
- **Combat Systems** — controller aim assist, melee attacks with auto-target-locking
- **Player Systems** — healing, checkpoints/respawn, and a unified Input Device service supporting cursor state switching and localization
- **UI** — animated crosshair, reload hints, interactions hints, pause menu, and fade transitions

## Tech

Built in Unity (HDRP). Core gameplay code lives under `Assets/Scripts`.