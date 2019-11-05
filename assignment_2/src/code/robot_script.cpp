#include "robot_script.h"
using namespace std;

float RobotScript::radians_to_degrees(float angle) {
	return (180*angle)/PI
}
  
RobotScript::RobotScript(int argc,char **argv) {

	ros::NodeHandle nh;

	foundWall = 0;

	state = FIND_WALL;

    std::string robot_cmd_vel = std::string("/") + std::string(argv[1]) + std::string("/cmd_vel");

    std::string laser = std::string("/") + std::string(argv[1]) + std::string("/") + std::string(argv[2]);
      
    laser_subscriber = nh.subscribe(laser.c_str(), 1000, &RobotScript::robotScanner, this);
      
    cmd_vel_advertiser = nh.advertise<geometry_msgs::Twist>(robot_cmd_vel.c_str(), 1000);
  }
  
RobotScript::~RobotScript(void) {
    
}
  
void RobotScript::robotScanner(const sensor_msgs::LaserScan& msg) {
   
	geometry_msgs::Twist cmd;

	float current_angle;

	min_angle = radians_to_degrees(msg.angle_min);

	for (int i = 0; i < msg.ranges.size(); i++) {
		current_angle = min_angle + (radians_to_degrees(msg.angle_increment) * i);

		if (state == FIND_WALL) {
			cmd.linear.x = 1.4;
			cmd.angular.z = 0.0;
			
			//If robot finds wall, then its state changes to FOllowing_wall
			if (current_angle < 0 && current_angle > -30 && msg.ranges[i] <= 1.5 ) {
				state = TURNING_LEFT;
			}
			cmd_vel_advertiser.publish(cmd);
		}

		else if (state == TURNING_LEFT) {
			//if (current_angle < 0 && current_angle > -30 && msg.ranges[i] <= 1.5 && msg.ranges[0] >= (msg.range_max * 1/2)) {
				cmd.linear.x = 0.0;
				cmd.angular.z = 0.4;
				cmd_vel_advertiser.publish(cmd);
			//}
			if (msg.ranges[0] <= (msg.range_max * 1/2) && msg.ranges[90] >= 1) {
				cout << "entreeeeeeeeeei" << endl;
				state = FOLLOWING_WALL;
			}
		}

		else if (state == TURNING_RIGHT) {
			//cout << "turning right" << endl;

			if (current_angle < -70 && current_angle > -90 && msg.ranges[i] >= 3 && msg.ranges[0] >= (msg.range_max * 2/4)) {
					cmd.linear.x = 0.4;
					cmd.angular.z = -0.4;
					cmd_vel_advertiser.publish(cmd);
			}
			if (current_angle < 0 && current_angle > -30 && msg.ranges[i] <= 1) {
				//cout << "entrei" << endl;
				state = FOLLOWING_WALL;
			}
		}

		else if (state == FOLLOWING_WALL) {

			cmd.linear.x = 1.0;
			cmd.angular.z = 0.0;

			//cout << "FOLLOWING_WALL" << endl;

			if (msg.ranges[45] <= 1.0 ) {
				cmd.angular.z = 0.4;
				cmd_vel_advertiser.publish(cmd);
			}

			else if (msg.ranges[45] > 1.0 && msg.ranges[90] >= msg.range_max) {
				cmd.angular.z = -0.1;
				cmd_vel_advertiser.publish(cmd);
			}

			if (current_angle < -70 && current_angle > -90 && msg.ranges[i] > msg.range_max && msg.ranges[0] <= (msg.range_max * 2/4)) {
				state = TURNING_RIGHT;
			}

			if (msg.ranges[90] <= 1.5) {
				cout << "hey hey hey heeeeeeeyyyyyy" << endl;
				state = TURNING_LEFT;
			}

			cmd_vel_advertiser.publish(cmd);
		}

		/*if (current_angle < 0 && current_angle > -30 && msg.ranges[i] <= 2.0 && msg.ranges[0] >= (msg.range_max * 1/4)) {
			cmd.linear.x = 0.0;
			cmd.angular.z = 0.4;
		}

		else if (current_angle <= -60 && current_angle >= - 80 &&msg.ranges[i] >= 3.0) {
			cmd.linear.x = 0.0;
			cmd.angular.z = -0.4;
		}*/
		//else if (current_angle < 90 && msg.ranges[i] <= 3.0) {
		//	cmd.angular.z = 0.1;
		//}
	}
	//cout << msg << endl;

	//cmd.linear.x = 0.4;
	//cmd.angular.z = -0.1;

}
