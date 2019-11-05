#include "robot_script.h"
using namespace std;

float RobotScript::radians_to_degrees(float angle) {
	return (180*angle)/PI
}
  
RobotScript::RobotScript(int argc,char **argv) {

	ros::NodeHandle nh; //Main mechanism

	state = FIND_WALL; //initial state

    std::string robot_cmd_vel = std::string("/") + std::string(argv[1]) + std::string("/cmd_vel");

    std::string laser = std::string("/") + std::string(argv[1]) + std::string("/") + std::string(argv[2]);
      
    laser_subscriber = nh.subscribe(laser.c_str(), 1000, &RobotScript::robotScanner, this); // obtains info from the laser
      
    cmd_vel_advertiser = nh.advertise<geometry_msgs::Twist>(robot_cmd_vel.c_str(), 1000); // advertises info to the robot
  }
  
RobotScript::~RobotScript(void) {
    
}
  
void RobotScript::robotScanner(const sensor_msgs::LaserScan& msg) {
   
	geometry_msgs::Twist cmd;

	float current_angle; //the angle of the sensor currently being analyzed

	min_angle = radians_to_degrees(msg.angle_min); // the minimum angle of the sensors

	for (int i = 0; i < msg.ranges.size(); i++) {
		current_angle = min_angle + (radians_to_degrees(msg.angle_increment) * i);

		//if the robot is attempting to find the first wall
		if (state == FIND_WALL) {
			cmd.linear.x = 1.4;
			cmd.angular.z = 0.0;
			
			//If robot finds wall, then its state changes to TURNING_LEFT
			if (current_angle < 0 && current_angle > -30 && msg.ranges[i] <= 1.5 ) {
				state = TURNING_LEFT;
			}
			cmd_vel_advertiser.publish(cmd);
		}

		//if the robot is turning left
		else if (state == TURNING_LEFT) {
			cmd.linear.x = 0.0;
			cmd.angular.z = 0.4;
			cmd_vel_advertiser.publish(cmd);

			//the robot will follow a wall when its first sensor is detecting a distance of less than half the max range and when its front sensor is detecting a distance greater than 1
			if (msg.ranges[0] <= (msg.range_max * 1/2) && msg.ranges[90] >= 1) {
				state = FOLLOWING_WALL;
			}
		}

		//if the robot is turning right
		else if (state == TURNING_RIGHT) {

			if (current_angle < -70 && current_angle > -90 && msg.ranges[i] >= 3 && msg.ranges[0] >= (msg.range_max * 2/4)) {
					cmd.linear.x = 0.4;
					cmd.angular.z = -0.4;
					cmd_vel_advertiser.publish(cmd);
			}
			if (current_angle < 0 && current_angle > -30 && msg.ranges[i] <= 1) {
				state = FOLLOWING_WALL;
			}
		}

		//if the robot is following a wall
		else if (state == FOLLOWING_WALL) {

			cmd.linear.x = 1.0;
			cmd.angular.z = 0.0;

			//when the sensor with 45 degrees detects a distance smaller than 1 it will adjust its angle in order to not hit the wall
			if (msg.ranges[45] <= 1.0) {
				cmd.angular.z = 0.4;
				cmd_vel_advertiser.publish(cmd);
			}

			//when the sensor with 45 degrees detects a distance greater than 1 and when the front sensor has a distance greater than the max range (usually inf)
			else if (msg.ranges[45] > 1.0 && msg.ranges[90] >= msg.range_max) {
				cmd.angular.z = -0.1;
				cmd_vel_advertiser.publish(cmd);
			}

			//when the sensors to the right no longer detect a wall, it will turn right
			if (current_angle < -70 && current_angle > -90 && msg.ranges[i] > msg.range_max && msg.ranges[0] <= (msg.range_max * 2/4)) {
				state = TURNING_RIGHT;
			}

			//if its front sensor detects a wall, it will turn left
			if (msg.ranges[90] <= 1.5) {
				state = TURNING_LEFT;
			}

			cmd_vel_advertiser.publish(cmd);
		}

	}
}
