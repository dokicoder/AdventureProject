## TODOs:

- list of useful links (add them to your bookmarks)
- fix rotatablility near the poles (go through math again - there needs to be a simple way to implement this)
- select text and information on objects
- make interactive journal that fills in blanks with information
  - going from unspecific descriptions to better information like "the girl I mat at place x with the weird hair" => to "Marissa Jones" after I learned her name
  - it may be possible to have descriptions of several people like "the guy who stole the golden apple" and "Shady Tom" and link them together (because Shady Tom stole the golden apple)
  - this could become a game mechanic in itself => automatic linking of information is difficult
  - question: how and when does the player know that he made the right deductions (immediately is probably boring) => Obra dinn files this with a "correction count" maybe a "truth percentage" that has to increase to 100%? Then I could upgrade it dynamically
- Cool text animations
- More dialogue, triggered by more stuff
- better dialogue animations
- there has to be a consistent handling for objects and characters name positioning, outlining and other parts that works for 3D as well as sprite objects. Write two versions of the scripts and share parameters or just merge both into one and try to determine if sprite or 3D version?
- There are lots of empty catch cases in my try/catch. At least throw a warning and better yet do additional checks and raise an error if it makes sense
- refactor the name label positioning into another script or utils => learn how to do modular scripts that work together
- sort points by urgency and make a date for it so you do it constantly and not have to read through 200 notes
- add name and descriptions to items (objects) and so on
- It should probably be possible to hack the outline of sprites with The SDF shader of TextMeshPro
- Make a writeup on how TMPJuicer works. And how you integrated it with the UI Update adapter to reset the text and re-initialize the transforms and animation (remember the weird destroyed text)
- we could give different characters different tet speeds, character animations, sentence emphasis (delay between sentences in same line. To be implemented) - Test this. maybe it is cool
- - MainCamera component/layer mask: 'Do not use "Everything" or "Default" as Layer mask...' - fix this

- Currently, NPCController and ItemController know about SelectionLabelController and The Outliners. Maybe they should just trigger events that are connected via GameController
- GameStateManager only manages NPCs as a proof of concept. Make it manage items as well.

## Fixes to know:

If classes or namespaces are not recognized:

https://answers.unity.com/questions/1697513/vs-code-does-not-find-some-classes-cs0246cs0103.html
