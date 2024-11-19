## Overview

The idea of the VR Sculpture app is to give the user opportunity to create sculpture in VR. This can be achieved
using controls on both hands:
- Right hand for intrusion
- Left hand for extrusion

Sculptures can be saved between session using.

## Controls

When running the application you should be able to move around the room using teleports that are controlled by the
joystick.

Each hand has control for sculpturing. Control has the shape corresponding to the one in the settings.
- Right hand control is used to intrude the sculpture, in other words subtract target shape from it. To intrude
hand control should intersect with the sculpture and trigger on the right controller should be pressed.
- Left hand control is used to extrude the sculpture, in other words add target shape to it. To extrude
hand control should intersect with the sculpture and trigger on the left controller should be pressed.

## UI

There are three UI panels that are used to configure controls and sculptures.

- **Subtraction panel**. This panel allows configuring control for intruding the sculpture. Shape of the subtraction can
be selected from the dropdown. To configure each shape there are individual sliders for each dimension.
- **Addition panel**. This panel allows configuring control for extruding the sculpture. Shape of the addition can
be selected from the dropdown. To configure each shape there are individual sliders for each dimension.
- Models panel. This panel allows saving sculptures between sessions. Button in the right-bottom corner saves new model
from currently applied to the sculpture. All sculptures and stored in the list. There are two buttons on each item:
    - **Load button**. Applied selected model to the sculpture in the scene.
    - **Save button**. Overrides saved model with the model currently applied to the sculpture.