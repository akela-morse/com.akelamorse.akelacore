# AkelaCore

Various tools and components for Unity that I created for my personal use.

[!IMPORTANT]
This toolset was created as a boilerplate for my Unity projects. I'm providing its source as-is in the hope that it is useful to other people or that someone wishes to learn from it. I won't take any responsibility in case of misuse or project corruption and will only provide a brief description of each tool and component. This is merely a personal toolset that I'm making open source, that's all.

### Animation namespace

Animation-related tools and components

- **NoiseConstraint**: A custom constraint for the [Animation Rigging](https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/index.html) package that adds noise to a transform's position and/or rotation.

### Behaviours namespace

Attributes you can add to augment your MonoBehaviours, and special Behaviour types you can inherit from.  
Note that any special Behaviour type or Attribute that uses the AkelaAnalyser for metaprogramming purposes requires your class to be partial. Simply declare your component with the `partial` keyword before the `class` keyword.

- **HideScriptFieldAttribute**: Allows you to hide the “Script” field in any MonoBehaviour's inspector without having to create a custom inspector. Uses the AkelaAnalyser to perform source generation and metaprogramming (see .Analyser folder)
- **INotifySerializedFieldChanged**: Implementing this interface in a MonoBehaviour allows you to be notified when a serialized field (either public or marked with `[SerializeField]`) updates, from anywhere in the engine, even during animation. It requires you to implement GetHashCode(), or to use the `[GenerateHashForEveryField]` attribute to automatically generate it (uses the AkelaAnalyser for source generation and metaprogramming)
- **OptimisedBehaviour**: Special behaviour type that works with `CullingElement` (see below). Allows you to create a component with a variable update frequency, based on cull state and distance from the camera.
- **RoundRobinBehaviour**: Special behaviour type whose components only get updated in a round-robin fashion (each component will be updated one after the other, frame by frame, instead of at the same time in the same frame)
- **SingletonAttribute**: Allows you to mark a MonoBehaviour as being a singleton, meaning it only has one component instance per scene. It generates a `Main` field from which you can access the instance (uses the AkelaAnalyser for source generation and metaprogramming)
- **TickBehaviour**: Special behaviour type which automatically implements an inspector field to select an update method (`Update()`, `LateUpdate()`, `FixedUpdate()`, `OnAnimatorMove()`). You may couple it with a `[TickOptions]` attribute to specify which update methods are allowed. This behaviour type also works well with `[ExecuteInEditMode]` and `[ExecuteAlways]`.
- **UIBehaviour**: Simple behaviour type that provides you with a `transform` property referring to a `RectTransform` component instead of a `Transform` component. Meant to be used for components that are implemented as children of a Canvas gameObject.
- **WithDependenciesAttribute**: Useful attribute that allows you to specify a `[Serializable]` struct type that contains dependencies to other components. You can use `[FromParents]` and `[FromChildren]` on this struct's fields to indicate the source of the component dependencies (if not specified, it is assumed the component is on the gameObject itself). The dependencies will be automatically injected and serialized, and you will be able to access them using a generated `dep` field that will be of the type of your struct (uses the AkelaAnalyser for source generation and metaprogramming)

### Bridges namespace

Serializable wrappers that I use to select a type based on the packages currently in use in the Unity project.

- **BridgedEvent**: If the [UltEvents](https://kybernetik.com.au/ultevents/) package is installed, then it will be a `UltEvent` wrapper. Otherwise, it's a `UnityEvent` wrapper.

### ExtendedPhysics namespace

Physics-related tools and components

- **Raycaster**: Component that allows you to preview and perform raycasting. Raycasting can be expensive, so I created this component to avoid having two scripts doing the same raycasts several times per frame; they can instead refer to this component. It also allows you to preview a raycast in the editor.
- **TorusCollider**: Custom collider shape that describes a torus. It's not a `MeshCollider` in disguise and can be used as a `Rigidbody`. The higher the resolution the less performant.

### Globals namespace

A collection of ScriptableAssets that allow you to create engine-wide global variables. Right-click in the project view and choose “Globals” to create one, then you can use it in any of your components or one of mine. Use `Var<T>` in your scripts to indicate that you accept a Global as a value.

### Motion namespace

Components that provide tools for moving gameObjects in a controlled manner.

- **ContinuousRotation**: Makes the gameObject rotate continuously, that's self-explanatory.
- **RandomMotion**: Randomly tweaks the gameObject's position, while keeping it close to its starting position.
- **RandomRotation**: Randomly tweaks the gameObject's rotation, while keeping it close to its starting rotation.
- **TransformAnimator**: To be used with `TransformAnimation` assets (Create -> Animation -> Transform Animation). Allows you to move a gameObject according to a defined list of keyframes. It's a dumber animation system that operates on the `Update()` loop and allows you to quickly animate a `Transform`.
- **TransformDriver**: Moves a `Transform`'s property based on another `Transform`'s property. Similar to drivers in Blender.
- **TransformLock**: Locks one or several `Transform` properties to a defined value.
- **TransformShift**: Interpolates one or several `Transform` properties between two states. Useful for moving platforms, for example.

### Optimisations namespace

Tools and components used for Game Logic optimisation.

- **ComponentCull**: Uses the `CullingElement` component to enable or disable other components based on the gameObject's cull state.
- **CullingElement**: Registers as an element in a `CullingSystem`, and sends messages to other components when the gameObject becomes culled or not (works with Occlusion Culling), or when the camera distance changes.
- **CullingSystem**: Manages a [CullingGroup](https://docs.unity3d.com/ScriptReference/CullingGroup.html). Think of it as a multi-purpose LOD system that sends events to CullingElements when the camera distance changes, based on a set of distance bands (LODs).
- **ParticleSystemCull**: Uses the `CullingElement` component to decrease a particle system's quality based on the gameObject's cull state (reduces max particles and emission rate, and can also increase particle size to make up for it)
- **PrefabPool**: Uses the [ObjectPool](https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Pool.ObjectPool_1.html) API to create a pool of a prefab. To turn a prefab into a `PrefabPool`, you need to add the `PooledPrefab` component to the prefab root. Then, right-click a prefab and select “Create -> Prefab Pool”. You can then reference a `PrefabPool` in your scripts and instantiate prefabs that way instead of using `Instantiate()`, which will reduce CPU overhead.
- **ShadowCull**: Uses the `CullingElement` component to decrease a light's shadow quality and resolution based on the gameObject's cull state (works on BiRP and URP, does NOT work on HDRP)

### Signals namespace

Messenging API and Unity event tools

- **ObjectFunctions**: Allows you to create named events that can in turn be called by other events, essentially enabling the ability to create functions for gameObjects.
- **Signal**: ScriptableObject that can be broadcasted from script or an event, and is observed by `SignalRelayer` components.
- **SignalRelayer**: Component that listens for specific Signals, and invokes events when notified.

### Tools namespace

A collection of tools and utilities for scripting purposes.

### Triggers namespace

Event invokers based on specific conditions. Most of them have invokable methods to that are meant to be called from other events (or from script).

- **CameraLookTrigger**: Invokes an event when looked at by the camera, or if the camera is close enough.
- **CameraVolumeTrigger**: Invokes an event when the camera is inside a `TriggerCluster` (see below)
- **CollisionTrigger**: Invokes events on `OnCollision` messages.
- **CombinationTrigger**: Invokes an event when the correct combination (a string) is set.
- **CounterTrigger**: Invokes an event when an internal counter reaches the desired amount.
- **DelayTrigger**: Invokes an event after a time.
- **EntryTrigger**: Invokes events on entry messages such as `Start()`, `Awake()` and `OnEnable()`.
- **ExitTrigger**: Invokes events on exit messages such as `OnDestroy()` and `OnDisable()`.
- **FlipFlopTrigger**: Flip-flop state machine that invokes two events alternatively (`OnFlipped` and `OnUnflipped`)
- **IntervalTrigger**: Invokes an event after a time, then repeatedly at a specified rate.
- **LogicTrigger**: Special trigger that performs bool logic on other triggers.
- **ProxyTrigger**: Special trigger that acts as the proxy of another trigger.
- **TriggerCluster**: Utility component that computes a volume from every collider with `isTrigger` set to true, on this gameObject and any of its children. It is used by `CameraVolumeTrigger`, and may be used by other components in the future.
- **VolumeTrigger**: Invokes events on `OnTrigger` messages.