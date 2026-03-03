# GDIM32-Final
## Check-In
The NPC should only chase if the player is close enough and actually visible (no wall between them). Also, chase should stop if line of sight is broken.

This is a line from the NPC’s “eyes” to the player’s “eyes”.
If the first collider hit along that line is the player, then the NPC has line-of-sight (aka LOS).
If the first collider hit is a obstacle, then the player is occluded.

How the code works:

- It chooses an origin/target at head height.

- It then computes the direction: dir = target - origin, then normalizes it for the ray direction.

- It then checks what was hit first: If hit.transform == player, LOS is true. Otherwise, something else blocked it, so LOS is false.

### Jingyi Cheng
#### Contributions
- Lantern pick-up script (on player)
- Quest UI
- Quest UI script (quest completion)
- Lantern skill script
- Lantern skill cooldown UI + script
- Added to monster script: stunned state + event (so the lantern skill can be used with a cooldown)
- Background music
- Post processing

#### How useful is the proposal / breakdown
##### Was your Proposal detailed enough, or did you end up having to address more details as you went?
I think the proposal is pretty useful, it helps me keep track of how much stuff we should implement to the game, such as item pick-up and skills. It was detailed enough, I just implemented it as-is.
##### Has anything changed about your architecture plans?
I have changed "NavMesh agent" of the monster NPC to a script of "randomly patrol around and when the player is close enough and within LOS (with raycasting), it comes closer to the player's transform."
##### What will you improve in your planning process for future games?
Maintain in-group coordination and don't have scene file merge conflicts!!!

### Team Member Name 2
Put your individual check-in Devlog here.
### Team Member Name 3
Put your individual check-in Devlog here.


## Final Submission
### Group Devlog
Put your group Devlog here.


### Team Member Name 1
Put your individual final Devlog here.
### Team Member Name 2
Put your individual final Devlog here.
### Team Member Name 3
Put your individual final Devlog here.

## Open-Source Assets

- Friendly NPC (villager) model + animation: [Mixamo](https://www.mixamo.com/#/)
- Monster model: [Sketchfab](https://sketchfab.com/3d-models/garden-crawler-ae2d35751fb542a9be736bb0ffcd94f1)
- Lantern: [Unity Asset Store](https://assetstore.unity.com/packages/3d/environments/historic/modular-medieval-lanterns-85527)
- Background music: [Pixabay](https://pixabay.com/music/mystery-scary-horror-music-351315/)
