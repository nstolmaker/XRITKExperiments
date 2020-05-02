This project is sort of two-in-one, because it's a demo of some stuff I've been working on lately. Kind of a virtual living room, that you could invite friends to.

# Features
- ## Multiplayer
 - Avatars
 - Hand and Mouth Movement
 - Grab and Distance Grab
 - Hand movement animations for local player
- ## TV
 - Synched between users, and can stream video
 - Cool remote control with play/pause/stop and some built-in videos.
- ## Air Hockey table
 - Keeps score!
 - Buttons to reset the puck if you lose it!
 - Reset buttons to play a new game.
 - Semi-realistic materials!

# Limitations
- Video formats are currently limited to what Android supports out-of-the-box. Mp4 works well. MKV doesn't. It doesn't know what to do with AAC audio. 
- No synch correction for TV if it goes out of sync
- Can't fast-forward or jump on the TV right now.
- The room is currently hard-coded, and thus shared with all users.
- Air Hockey uses non-deterministic physics, so you can hit through the walls if you hit hard enough.
- No colliders set on avatars, so you can walk through walls.
