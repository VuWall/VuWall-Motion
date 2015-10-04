## Wearhacks 2015
D5_VuWall Motion

## Inspiration
Iron Man.

But really, our team comes from the video wall market where we have customers who would like to collaborate on a video wall in their board room or meeting room using touch screens. But in reality this is unusable! Imagine a 5x5 wall with 55 inch screens in a board room for a total diagonal of 275 inches. First of all if you are touching your video wall at one end you will not be able to clearly see what is at the other end so every time you move a window you will have to move back to a reasonable distance. And then there's the fact that not everybody is 9 feet tall so unless you run a company of giants, your board room is gonna need a ladder.

## What it does
With the Myo armband, you have full gesture and motion control of your desktop windows
- Uses hand poses to interact with available windows
- Uses gyroscope data to detect spacial orientation and associate the cursor on the screen
- Supports sequences of poses
- Supports multiple connected Myo armbands at the same time

## Challenges we ran into
- 3D calculations using quaternions, euler angles and directional vectors.
- Accelerometer data not accurate enough for 3D positioning.

## Accomplishments that we're proud of
We got 40 green, mean, and lean passing tests (integration and unit). Like a boss.
We also achieved multi myo band window control and collaboration.

## What's next for VuWall Motion?
Utilization of more gestures, resizing of windows, transition animations, integration with [VuWall2](http://vuwall.com/products/vuwall2/) (recalling layouts, calling scripts, talking to network devices such as lights or blinds)
The greatest improvement would be using Euler angles only for screen positioning so the calibration would tell the software at what angle the edges of the screen are and this would allow us to get rid of the distance from screen.

