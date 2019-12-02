Just want to start off by saying that installing ros-stdr on Manjaro is a huge headache and after many unsuccessful attempts of trying to install it, we just ended up with an Ubuntu VM on a Windows 10 host and called it a day.

The project is divided into the following structure
```
+-- src
|   +-- code
|   |   ...
+-- resources
|   +-- launchers
|   |   ...
|   +--maps
|   |   ...
|   +--robots
|   |   ...
```
Software used:
Ubuntu 16.04.6
ROS Kinetic
ROS STDR

Steps to execute program:

* Copy files from the projects resources/maps into the folder /opt/ros/kinetic/share/stdr_resources/maps (default folder)
* Copy files from the projects resources/launchers into the folder /opt/ros/kinetic/share/stdr_launchers/launch (default folder)
* Copy files from the projects resources/robots into the folder /opt/ros/kinetic/share/stdr_resources/resources/robots
* Use the terminal to execute the following command (for example)
```
roslaunch stdr_launchers ass_2_v_launch.launch
```
* Open another terminal and travel to the project directory
* Use
```
source devel/setup.bash
```
* Write the command
```
rosrun code reactive_robot robot0 laser_1 
```
* Observe the final product
