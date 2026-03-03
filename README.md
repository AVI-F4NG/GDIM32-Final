# GDIM32-Final
## Group Devlog - Prompt B

We made use of the insights we gathered from in-class demos to redesign the movement of the monster NPC. For example, we applied raycasting, which is a manifestion of cross-product vector technique, to determine whether the monster should be chasing the player or randomly move around. 
Raycasting is a line from the NPC’s “eyes” to the player’s “eyes”. If the first collider hit along that line is the player, then the monster has line-of-sight (aka LOS) and will lock its target and chase the player. Otherwise, if the first collusive hit is an obstacle, then the monster's chasing movement won't be triggered.

How the code works:
- It chooses an origin/target at head height.

- It then computes the direction: dir = target - origin, then normalizes it for the ray direction.

- It then checks what was hit first: If hit.transform == player, LOS is true. Otherwise, something else blocked it, so LOS is false.

The NPC should only chase if the player is close enough and actually visible (no wall between them). Also, chase should stop if line of sight is broken.

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

### Ke-Chieh Chang
#### Contributions
- Implemented key scene texture and terrain tools
- Constructed terrain, created stamps and terrain layers, implement and add vegetarians
- Built scripts for dialogueManager and interactableNPC. Built scripts for the scriptable dialogues with dialogueNode
- Refined dialogue system, designed dialogue branches and the overall UI during conversation

#### How useful is the proposal / breakdown
##### Was your Proposal detailed enough, or did you end up having to address more details as you went?
Our proposal is very detailed which gives me a clear start-up. The problems would only be that the proposal was too well-elaborated and we might not be able to finish implementing all features (because it's far-beyond the bare minimum XD)

##### Has anything changed about your architecture plans?
I implemented the dialogue features slightly different than I planned and than what we saw in the in-class demo. Instead of pressing keycode to advance dialogue, I require the player to press button and make choice everytime when advancing the dialogue, and also when ending the dialogue. I feel this way of dialogue system is more intuitive. 

##### What will you improve in your planning process for future games?
For the final two weeks, we will really have to communicate to tasks distribution and set effective deadlines for each group member. I found us having problem with deadlines (for me, I was too stressed out for other subjects work QAQ). 

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

- Friendly NPC (villager) model + animation: 
[Mixamo](https://www.mixamo.com/#/)

- Monster model: 
[Sketchfab](https://sketchfab.com/3d-models/garden-crawler-ae2d35751fb542a9be736bb0ffcd94f1)

- Lantern: 
[Unity Asset Store](https://assetstore.unity.com/packages/3d/environments/historic/modular-medieval-lanterns-85527)

- Background music: 
[Pixabay](https://pixabay.com/music/mystery-scary-horror-music-351315/)

- Texture: 
[Horror Texture Pack](https://screamingbrainstudios.itch.io/horror-texture-pack)

- Nature:
[3D Nature Assets](https://assetstore.unity.com/packages/3d/environments/3d-nature-assetspack-215646)

- Terrain Tools: 
[Procedural Terrain](https://assetstore.unity.com/packages/tools/terrain/procedural-terrain-painter-free-automatic-terrain-texturing-188357)

[StampIt](https://assetstore.unity.com/packages/tools/terrain/stampit-collection-free-heightmaps-for-unity-6-microverse-gaia-t-218286)
