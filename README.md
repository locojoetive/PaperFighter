# PaperFighter

## Origin
Paper Fighter was initially build as part of my bachelor thesis in about _Model-Driven Game Development with Melanee: Real-Time Simulation of Paper Fighters_.
Melanee, developed by the Software Engineering Group at the University of Mannheim, is an Eclipse-based workbench for creating domain-specific languages, with an arbitrary amount of ontological levels.

As part of this thesis I created a multi-level deep model, including a domain-specific language describing the kind of entities as a meta-meta-model, a meta-model describing all game components, and a model defining the actual levels and stages.
With the corresponding Paper-Fighter-Melanee-plugin, users can build stages (scenes in Unity), connect them to each other, assemble them to levels, and host them on a server to make them accessible to others via a client.

After graduating from the University of Mannheim, I further experimented with Unity, implemented mobile controls, created levels, and build the project as a WebGL build.

## Checkout Project
If you want to checkout on this project, you should consider the following things:

- Use Unity version: 2020.1.17f


## Build
If you want to build this project, you should be aware of the following things

Unity version: 2020.1.17f
Packages: ...
Build target: WebGL
Build options: Wasm + gzipped ...

The project was updated to Unity 2020.1.17f and requires no specific packages to build.
The optimal WebGL build configurations are achieved using the build script integrated as a menu option into the Unity editor and choosing __HTML Export > Wasm release gzipped__.
With this option the game is compiled into WebAssembly using gzip as its compression format.
