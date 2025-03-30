## Overview

The idea of the VR Sculpture app is to give the user opportunity to create sculpture in VR. This can be achieved
using controls on both hands:
- Right hand for intrusion
- Left hand for extrusion

Sculptures can be saved between session on the cloud and loaded/updated later.

https://github.com/user-attachments/assets/3eb57891-dd4d-42d9-abbb-ab95cb3aa061

## Controls

When running the application you should be able to move around the room using teleports that are controlled by the
joystick.

Each hand has control for sculpturing. Control has the shape corresponding to the one in the settings.
- Right hand control is used to intrude the sculpture, in other words subtract target shape from it. To intrude
hand control should intersect with the sculpture and trigger on the right controller should be pressed.
- Left hand control is used to extrude the sculpture, in other words add target shape to it. To extrude
hand control should intersect with the sculpture and trigger on the left controller should be pressed.

## UI

There are a few UI panels that are used to configure controls and sculptures.

- **Sign up panel**. This panel is used to sign up the user. It requires email and password. Newly created user is
automatically signed in.
- **Sign in panel**. This panel is used to sign in the user. It requires email and password.
- **Subtraction panel**. This panel allows configuring control for intruding the sculpture. Shape of the subtraction can
be selected from the dropdown. To configure each shape there are individual sliders for each dimension.
- **Addition panel**. This panel allows configuring control for extruding the sculpture. Shape of the addition can
be selected from the dropdown. To configure each shape there are individual sliders for each dimension.
- **Models panel**. Internally this panel loads two types of models: pre-made models and user models. Pre-made models are
loaded from the cloud and can be used as a base for the sculpture, they are read-only. User models are loaded from the cloud
and can be overwritten by the user. User models are saved only for authenticated users. Page also allows creating new model
from current sculpture and saving it to the cloud.

In addition to the panels there is also a header that shows current user and allows to sign out.

