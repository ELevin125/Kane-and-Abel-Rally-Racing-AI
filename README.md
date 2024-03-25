# Kane and Abel - Rally Racing AI

Welcome to the Kane and Abel - Rally Racing AI project.


## Introduction

This project explores the integration of AI systems into a rally racing game environment. Two AI models were developed and compared: one based on pre-programmed behaviors and another utilising machine learning techniques. Along with the two AI systems, the project includes a custom car controller, 3 rally stages and a basic weather system for each, developed in the Unity Engine.

## Installation

To install the project, follow these steps:

1. Ensure you have Unity 2022.3 installed on your system.
2. Clone the project repository to your local machine.
3. Navigate to the project root directory.
4. Open your file explorer and navigate to `Assets -> Scenes`.
5. Open one of the stage scenes (`.unity` files) using the Unity Editor.

**Note:** Attempting to add the cloned project via the Unity Hub will fail if one of the stage scenes aren't manually opened first, as the required packages are not yet imported and compiled. Manually opening one of the `.unity` files first will start that process. 

## Usage

Once the project is set up, you can navigate between the three stage scenes. In each scene's Hierarchy, navigate to `Environment -> Stage #`, where you'll find two game objects named "Kane" and "Abel". These are the AI racing systems.

By default, Abel is enabled in all scenes. However, you can switch between the two agents by enabling and disabling them in the Inspector

To change the weather condition, navigate to `Environment -> Stage #` in the Hierarchy, and change the "weather" variable of the "Stage Condition" component

To control the car manually:

1. Enable Abel in the Inspector.
2. Navigate to "Behavior Parameters".
3. Change "Behavior Type" to "Heuristic Only".
