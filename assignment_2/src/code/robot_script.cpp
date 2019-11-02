#include "robot_script.h"
using namespace std;

float RobotScript::degrees_to_radians(float angle) {
	return (180*angle)/PI
}
  
RobotScript::RobotScript(int argc,char **argv) {

	ros::NodeHandle nh;

	foundWall = 0;

    std::string robot_cmd_vel = std::string("/") + std::string(argv[1]) + std::string("/cmd_vel");

    std::string laser = std::string("/") + std::string(argv[1]) + std::string("/") + std::string(argv[2]);
      
    laser_subscriber = nh.subscribe(laser.c_str(), 1000, &RobotScript::robotScanner, this);
      
    cmd_vel_advertiser = nh.advertise<geometry_msgs::Twist>(robot_cmd_vel.c_str(), 1000);
  }
  
RobotScript::~RobotScript(void) {
    
}
  
void RobotScript::robotScanner(const sensor_msgs::LaserScan& msg) {
   
	geometry_msgs::Twist cmd;

	cmd.linear.x = 1.4;
	cmd.angular.z = 0.0;

	float min_angle = degrees_to_radians(msg.angle_min);

	float current_angle;

	for (int i = 0; i < msg.ranges.size(); i++) {
		current_angle = min_angle + (degrees_to_radians(msg.angle_increment) * i);

		if (current_angle < 0 && current_angle > -30 && msg.ranges[i] <= 2.0 && msg.ranges[0] >= (msg.range_max * 1/4)) {
			cmd.linear.x = 0.0;
			cmd.angular.z = 0.4;
		}

		else if (current_angle <= -60 && current_angle >= - 80 &&msg.ranges[i] >= 3.0) {
			cmd.linear.x = 0.0;
			cmd.angular.z = -0.4;
		}
		//else if (current_angle < 90 && msg.ranges[i] <= 3.0) {
		//	cmd.angular.z = 0.1;
		//}
	}

	cout << msg << endl;

	//cout << msg << endl;

	//cmd.linear.x = 0.4;
	//cmd.angular.z = -0.1;

	cmd_vel_advertiser.publish(cmd);

}
