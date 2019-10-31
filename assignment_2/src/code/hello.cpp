#include <ros/ros.h>
#include "robot_script.h"

int main(int argc, char **argv) {
	ros::init(argc, argv, "robot_main_node", ros::init_options::AnonymousName);

	RobotScript obj(argc, argv);

	ros::spin();

	return 0;
}
