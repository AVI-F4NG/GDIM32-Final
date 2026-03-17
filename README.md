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
#### Was your Proposal detailed enough, or did you end up having to address more details as you went?
I think the proposal is pretty useful, it helps me keep track of how much stuff we should implement to the game, such as item pick-up and skills. It was detailed enough, I just implemented parts such as the pick-up as it is. However, the proposal also mentioned some other skills such as a fuel level, which are too complex to implement right now and for now the game is a simplified version of it.
#### Has anything changed about your architecture plans?
I have changed "NavMesh agent" of the monster NPC to a transform-oriented script: it lets the monster randomly patrol around and when the player is close enough and within LOS (with raycasting), it comes closer to the player's transform.
#### What will you improve in your planning process for future games?
Maintain in-group coordination and don't have scene file merge conflicts!!!

### Ke-Chieh Chang
#### Contributions
- Implemented key scene texture and terrain tools
- Constructed terrain, created stamps and terrain layers, implement and add vegetarians
- Built scripts for dialogueManager and interactableNPC. Built scripts for the scriptable dialogues with dialogueNode
- Refined dialogue system, designed dialogue branches and the overall UI during conversation

#### How useful is the proposal / breakdown
#### Was your Proposal detailed enough, or did you end up having to address more details as you went?
Our proposal is very detailed which gives me a clear start-up. The problems would only be that the proposal was too well-elaborated and we might not be able to finish implementing all features (because it's far-beyond the bare minimum XD)

#### Has anything changed about your architecture plans?
I implemented the dialogue features slightly different than I planned and than what we saw in the in-class demo. Instead of pressing keycode to advance dialogue, I require the player to press button and make choice everytime when advancing the dialogue, and also when ending the dialogue. I feel this way of dialogue system is more intuitive. 

#### What will you improve in your planning process for future games?
For the final two weeks, we will really have to communicate to tasks distribution and set effective deadlines for each group member. I found us having problem with deadlines (for me, I was too stressed out for other subjects work QAQ). 

### Jamin Pinson
#### Contributions
- Implemented the falling snow particle system
- Implemented the platform script in which later on will spawn the monster
- Added an incomplete version of the latern which will show the exit
- Added the incomplete version of the fear meter which will increase as the monster sees the player within its field of view.

#### How useful is the proposal / breakdown
#### Was your Proposal detailed enough, or did you end up having to address more details as you went?
The proposal definitely has helped a lot in understanding what I want to add since we were very detailed with the proposal. It helped convey ideas if I didn't remember that such as the platform script where it is supposed to spawn the monster.
#### Has anything changed about your architecture plans?
Something that has changed about my architecture plans is I definitely added more scripts than I previously anticipated and I also need to implement the pathfinder into my architecture plans.
#### What will you improve in your planning process for future games?
What I will improve on is doing more research on certain aspects of design that I don't fully understand and just dedicate more time than I have done now. I hope to dedicate much more time to this game in the next 2 weeks than previously before.

# Final Submission
## Group Devlog

### 1. Finite State Machine

#### Where it is: 
Monster Behavior script enum NpcState and the state field that switches between them, calling ExecutePatrol() vs ExecuteChase().

#### What it did for the game:
The monster’s behavior is mode-based (patrol, chase, stunned). An FSM keeps that logic structured and predictable: each frame, the monster is in exactly one state, and transitions only happen under explicit conditions (player detected - chase; lost player - patrol). Without an FSM, there will be a tangled if/else statements across multiple booleans.

#### Why it was useful here:
There are features like proximity detection, line-of-sight checks, pausing movement, and stun/cooldown. FSM makes those additions safer because we can integrate them as transitions (patrol/chase), state-specific behavior (move differently per state), and global interrupts (stun/pause)

### 2. Event

#### Where it is:
Player Pickup: event Action<PickupEvent> PickedUp; and PickedUp?.Invoke()
Monster Behavior: event Action Stunned; and Stunned?.Invoke()

#### What it did for the game:
This decouples systems that react to something (UI updates, monster stun FX, lantern light pulse, quest progression) from the system that causes it (pickup script, monster script).

#### Why it was useful here:
When the player picks up something, we don't want these things to happen: the pickup script to know about UI scripts, the pickup script to know about dialogue logic, or the pickup script to know about monster stun logic. Events let us plug new listeners in later with zero changes to the pickup code, perfect for the “we’ll use it in UI later” requirement; they also decouple different sets of logic from each other.

### 3. Singleton

#### Where it is: 
PlayerGameplayBlockState script and scripts reading PlayerGameplayBlockState.Instance.

#### What it did for the game:
This provides a single authoritative source of truth for “gameplay blocked” conditions:

- dialogue open (IsTalking)
- settings open (IsSettingsOpen)

#### Why it was useful here:
Multiple systems needed to obey the same pause rules:

- fear meter should stop rising
- monster should stop moving
- cursor lock / input modes change

A singleton makes these flags easy to read anywhere without wiring references everywhere.

## Individual Devlogs
### Jingyi Cheng
#### What have you contributed to the project since the Check-In?
- added a settings panel that allows the player to adjust the mouse sensitivity; settings script
- modified monster behavior so the monster will not move when the game is paused (by settings screen or NPC conversation)
- added the second quest to the game, and refined player pickup logic so it handles two different kinds of object pickup
- added the NPC dialogue associated with the second quest
- added win/lose scenes and added win/lose logic to the corresponding scripts (win: friendly NPC script; lose: fear meter script)
- adjusted fear meter so the post processing's vignette effect intensifies as it grows after fear exceeds 50

### Ke-Chieh Chang
#### What have you contributed to the project since the Check-In?
- massively updated UI prefab by: creating several panel as folderss that split the screen up by zones (such as Top-Left HUDs, Center HUDs, Bottom Center HUDs, etc.); mapping each UI elements into their zones and use anchoring, auto-size, and container to make them self-adjustable and presentable across different screen sizes.
- update the NPC interaction guide so it "floats" on top of the NPC and "looks at" player's camera (logic in the npcUIanchor script)
- alter values through inspectors, including the NPC's interact range, monster detection range, character's speeds, and so on to create a more intuitive and better player experience. 


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
[Procedural Terrain](https://assetstore.unity.com/packages/tools/terrain/procedural-terrain-painter-free-automatic-terrain-texturing-188357), [StampIt](https://assetstore.unity.com/packages/tools/terrain/stampit-collection-free-heightmaps-for-unity-6-microverse-gaia-t-218286)
