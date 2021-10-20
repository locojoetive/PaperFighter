# Paper Fighter

### 1. Origin
Paper Fighter was initially built as part of my bachelor thesis in about _Model-Driven Game Development with Melanee: Real-Time Simulation of Paper Fighters_.
Melanee, developed by the Software Engineering Group at the University of Mannheim, is an Eclipse-based workbench for creating domain-specific languages, with an arbitrary number of ontological levels.

As part of this thesis, I created a multi-level deep model, including a domain-specific language describing the kind of entities as a meta-meta-model, a meta-model describing all game components, and a model defining the actual levels and stages.
With the corresponding Paper-Fighter-Melanee-plugin, users can build stages (scenes in Unity), connect them to each other, assemble them to levels, and host them on a server to make them accessible to others via a client.

After graduating from the University of Mannheim, I further experimented with Unity, implemented mobile controls, created levels, and build the project as a WebGL build.

### 2. Checkout & Build Project
If you want to check out and build this project, be aware of the following things:

Unity version: 2020.1.17f
Build target: WebGL
Build options: Wasm + gzipped

The project was updated to Unity 2020.1.17f and requires no specific packages to build.
The best WebGL build configurations are achieved using the build script integrated as a Unity menu option into the Unity editor and choosing __HTML Export > Wasm release gzipped__.
This choice compiles the game into WebAssembly using gzip as its compression format and creates a build into __/<project-root-folder>/Build/webgl/__.


### 3. Unity's Input System
Although Unity's input system can work in WebGL builds, it is not fully supported for mobile input using WebGL.
Pointer events like OnPointerDown(eventData: BasicEventData) do not work as expected.
For instance, while they are only fired once in Android builds when a finger is held down on the UI element of interest, they are fired every frame a finger is held down in WebGL builds.
It seems to be difficult for the application to distinguish when touches occur, when they end, and which finger is still touching the screen.

Therefore, to support mobile browsers, I created an independent touch input system, using parts of Unity's input system and catching the edge cases by simplifying their trigger conditions.

### 4. Future Work

To ensure performance and to allow extensibility the game's architecture, and its loading behavior needs to be addressed.
#### 4.1. Game Architecture

Since this project was my first Unity project in 2015, its structure is kind of convoluted.
To enhance the games performance, its game objects and the code structure needs to be more modular.
For instance, the Game Manager, the HUD, the Camera, and the Audio- and Sound Manager need to be independent objects with their own encapsulated responsibilities.

#### 4.2. Unity WebGL and Mobile Browsers

Unity WebGL builds are not optimized for mobile browsers.
Due to the limited memory mobile browsers allocate to web applications, certain browsers (e.g., Firefox for Android, Safari for iPhone) are not able to execute the built application.
Therefore, it is necessary to down sample audio and image files, load assets only when they are needed, unload them when they are no longer needed, and optimize the game architecture.

Unity's Addressables package allows to load and unload specific assets dynamically.
To prevent memory leaks, and reduce the amount of executed loading procedures, I began experimenting with this package, so I can provide a stable and enjoyable experience with all conventional browsers, like Safari and Firefox for mobile devices.
