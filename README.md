# GDIM32-Final
## Check-In
The NPC should only chase if the player is close enough and actually visible (no wall between them). Also, chase should stop if line of sight is broken.

This is a line from the NPC’s “eyes” to the player’s “eyes”.
If the first collider hit along that line is the player, then the NPC has LOS.
If the first collider hit is a obstacle, then the player is occluded.

How the code works:

- It chooses an origin/target at head height.

- It then computes the direction: dir = target - origin, then normalizes it for the ray direction.

- It then checks what was hit first: If hit.transform == player, LOS is true. Otherwise, something else blocked it, so LOS is false.

### Jingyi Cheng
#### Contributions
- Lantern pick-up logic
- Quest UI & pick-up prompt
- Lantern skill logic
- Background music
- Post processing

#### How useful is the proposal / breakdown
##### Was your Proposal detailed enough, or did you end up having to address more details as you went?
I think the proposal is pretty useful, it helps me keep track of how much stuff we should implement to the game, such as item pick-up and skills. It was detailed enough, I just implemented it as-is.
##### Has anything changed about your architecture plans?
I have changed "NavMesh agent" of the monster NPC to a simple "randomly patrol around and come closer to the player's transform" script.
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
Cite any open-source assets here. Put them in a LIST, and use correctly formatted LINKS.

[Background music track](https://pixabay.com/music/mystery-scary-horror-music-351315/)
