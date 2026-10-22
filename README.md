# Cell Machine simulator
---
## Overview
<p>This repository contains C# biological cell evolution simulator. There are used Pointer genome algorithm for cell decision process. </p>

---
## Demo
![Simulation state example](https://github.com/user-attachments/assets/9f945453-c2e5-48d2-bae4-1698e9787d6a)
|№|genomeMutationChance|Video|
|---|---|---|
|**1**|**0.05**|<video src="https://github.com/user-attachments/assets/8dc75f02-9227-4513-a646-f970cc0435a9" autoplay loop muted playsinline width="100%"></video>|
|**2**|**0.005**|<video src="https://github.com/user-attachments/assets/fb05ff5c-46a2-46cf-9655-04207d10a304" autoplay loop muted playsinline width="100%"></video>|
---
### Mathematical Formulation
At any given simulation tick t, the cell's state is defined by the pointer \(P_t \in [0, N-1]\), where N is the `geneSize`. The genome consists of two arrays of size N:
*   Gene G: Array of target pointer addresses.
*   Trigger T: Array of environmental conditions.

The transition function for the pointer \(P_{t+1}\) and the resulting action \(A_t\) are defined as:

\[P_{t+1} = \begin{cases} G[P_t], & \text{if } \text{Condition}(T[P_t]) \text{ is True} \\ (P_t + 1) \pmod N, & \text{if } \text{Condition}(T[P_t]) \text{ is False} \end{cases}\]

\[A_t = P_{t+1} \pmod{\text{actionSpace}}\]
---
## Technologies
* **C#** (.NET core)
* **SixLabors.ImageSharp** 3.1.6
* Custom integrated **`FFmpeg`** pipeline via standard I/O streaming for real-time video compilation.
---
## Dependencies
* SixLabors.ImageSharp 3.1.6
* SixLabors.ImageSharp.Drawing
---
## Project structure
```text
CellMachine/
├── Config/
|   ├── BoardConfig.cs        # Board configuration parameters 
|   ├── GameConfig.cs         # All evolution and simulation parameters
|   └── VideoConfig.cs        # Parameters for video encoder
├── Models/
|   ├── Base.cs               # Basic class Tile 
|   ├── Cell.cs               # Cell class that extends Tile. It can select actions using its genome
|   ├── Food.cs               # Food class that extends Tile
|   └── Colony.cs             # Contains Cell genome and stores all species-related information and methods
├── Services/
|   └── SimulationEngine.cs   # Simulates all board logic
├── Utils/
|   ├── Genetic.cs            # Mutation, genome processing and other evolution functionality
|   ├── Simulation.cs         # Util function and structures for board processing and simulation
|   ├── Video.cs              # Video encoder class
|   └── Utils.cs              # Other utils
├── Const.cs                  # Constants and services
└── Program.cs                # Project entry point
```

---
## Get started
#### 1. Clone repository:
```bash
git clone https://github.com/Flahhhh/CellMachine
```
#### 2. [Install FFmpeg](https://www.ffmpeg.org/)
#### 3. Install dependencies:
```bash
dotnet restore
```
#### 4. Run project
```bash
dotnet run
``` 
---
## License
MIT License
